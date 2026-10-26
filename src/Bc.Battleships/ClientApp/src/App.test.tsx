import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import App from './App'
import type { GameDto } from './logic/types'

const emptyBoard = (): GameDto['board'] => ({
  boardSize: 10,
  boardRows: Array.from({ length: 10 }, () => ({ row: Array<0 | 1 | 2>(10).fill(0) })),
})

const ID = '11111111-1111-1111-1111-111111111111'
const activeGame = (hp = 5): GameDto => ({ id: ID, isGameActive: true, currentHitPoints: hp, board: emptyBoard() })
const wonGame = (): GameDto => ({ ...activeGame(0), isGameActive: false })

const json = (body: unknown, status = 200) => new Response(JSON.stringify(body), { status })

type Route = (init?: RequestInit) => Response | Promise<Response>

// Routes are keyed by "METHOD /path"; an unlisted request fails the test.
const stubFetch = (routes: Record<string, Route>) => {
  const fetchMock = vi.fn(async (url: string, init?: RequestInit) => {
    const key = `${init?.method ?? 'GET'} ${url}`
    const route = routes[key]
    if (!route) throw new Error(`unexpected request ${key}`)
    return route(init)
  })
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

const requests = (fetchMock: ReturnType<typeof stubFetch>) =>
  fetchMock.mock.calls.map(([url, init]) => `${init?.method ?? 'GET'} ${url}`)

const GAME_PATH = `/games/${ID}`
const KEY = 'battleships.gameId'

describe('App', () => {
  beforeEach(() => {
    localStorage.clear()
  })

  afterEach(() => {
    vi.unstubAllGlobals()
    localStorage.clear()
  })

  it('shows the no-game view and sends no request without a stored id', async () => {
    const fetchMock = stubFetch({})
    render(<App />)

    expect(await screen.findByText(/No game in progress/)).toBeInTheDocument()
    expect(screen.getAllByRole('button', { name: 'Start new game' })).toHaveLength(2)
    expect(fetchMock).not.toHaveBeenCalled()
  })

  it('shows loading and then the board of the stored game', async () => {
    localStorage.setItem(KEY, ID)
    const fetchMock = stubFetch({ [`GET ${GAME_PATH}`]: () => json(activeGame()) })
    render(<App />)

    expect(screen.getByText('Loading...')).toBeInTheDocument()
    expect(await screen.findByText('Left to hit: 5')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'A1' })).toBeInTheDocument()
    expect(requests(fetchMock)).toEqual([`GET ${GAME_PATH}`])
  })

  it('clears the stored id and shows the no-game view when the game is gone (404)', async () => {
    localStorage.setItem(KEY, ID)
    stubFetch({ [`GET ${GAME_PATH}`]: () => json({}, 404) })
    render(<App />)

    expect(await screen.findByText(/No game in progress/)).toBeInTheDocument()
    expect(screen.queryByRole('alert')).not.toBeInTheDocument()
    expect(localStorage.getItem(KEY)).toBeNull()
  })

  it('shows a readable error and keeps the id when loading fails otherwise', async () => {
    localStorage.setItem(KEY, ID)
    stubFetch({ [`GET ${GAME_PATH}`]: () => json({}, 500) })
    render(<App />)

    expect(await screen.findByRole('alert')).toHaveTextContent(`GET ${GAME_PATH} failed with status 500`)
    expect(localStorage.getItem(KEY)).toBe(ID)
  })

  it('shows "You win!" when a finished game is reloaded', async () => {
    localStorage.setItem(KEY, ID)
    stubFetch({ [`GET ${GAME_PATH}`]: () => json(wonGame()) })
    render(<App />)

    expect(await screen.findByText('You win!')).toBeInTheDocument()
  })

  it('starts a new game with exactly one POST /games, stores the id and sends no GET', async () => {
    const user = userEvent.setup()
    const fetchMock = stubFetch({ 'POST /games': () => json(activeGame(7), 201) })
    render(<App />)
    await screen.findByText(/No game in progress/)

    await user.click(screen.getAllByRole('button', { name: 'Start new game' })[1])

    expect(await screen.findByText('Left to hit: 7')).toBeInTheDocument()
    expect(requests(fetchMock)).toEqual(['POST /games'])
    expect(localStorage.getItem(KEY)).toBe(ID)
  })

  it('sends a single POST on a double click and disables Start while in flight', async () => {
    const user = userEvent.setup()
    let resolvePost: (response: Response) => void = () => {}
    const fetchMock = stubFetch({
      'POST /games': () => new Promise<Response>((resolve) => (resolvePost = resolve)),
    })
    render(<App />)
    await screen.findByText(/No game in progress/)

    const [navButton, viewButton] = screen.getAllByRole('button', { name: 'Start new game' })
    await user.dblClick(viewButton)

    await waitFor(() => expect(navButton).toBeDisabled())
    expect(viewButton).toBeDisabled()
    resolvePost(json(activeGame(), 201))

    expect(await screen.findByText('Left to hit: 5')).toBeInTheDocument()
    expect(requests(fetchMock)).toEqual(['POST /games'])
  })

  it('sends the clicked cell to the game shots route, renders a hit and decrements the hit points', async () => {
    const user = userEvent.setup()
    localStorage.setItem(KEY, ID)
    const fetchMock = stubFetch({
      [`GET ${GAME_PATH}`]: () => json(activeGame(5)),
      [`POST ${GAME_PATH}/shots`]: () => json({ hit: true, currentHitPoints: 4 }),
    })
    render(<App />)
    const cell = await screen.findByRole('button', { name: 'C4' })

    await user.click(cell)

    await waitFor(() => expect(cell).toHaveTextContent('●'))
    expect(screen.getByText('Left to hit: 4')).toBeInTheDocument()
    expect(cell).toBeDisabled()
    const [, init] = fetchMock.mock.calls[1]
    expect(JSON.parse(init!.body as string)).toEqual({ column: 3, row: 2 })
  })

  it('renders a miss without changing the hit points', async () => {
    const user = userEvent.setup()
    localStorage.setItem(KEY, ID)
    stubFetch({
      [`GET ${GAME_PATH}`]: () => json(activeGame(5)),
      [`POST ${GAME_PATH}/shots`]: () => json({ hit: false, currentHitPoints: 5 }),
    })
    render(<App />)
    const cell = await screen.findByRole('button', { name: 'A1' })

    await user.click(cell)

    await waitFor(() => expect(cell).toHaveTextContent('○'))
    expect(screen.getByText('Left to hit: 5')).toBeInTheDocument()
    expect(cell).toBeDisabled()
  })

  it('shows an error message when a shot fails', async () => {
    const user = userEvent.setup()
    localStorage.setItem(KEY, ID)
    stubFetch({ [`GET ${GAME_PATH}`]: () => json(activeGame()), [`POST ${GAME_PATH}/shots`]: () => json({}, 500) })
    render(<App />)

    await user.click(await screen.findByRole('button', { name: 'A1' }))

    expect(await screen.findByRole('alert')).toHaveTextContent(`POST ${GAME_PATH}/shots failed with status 500`)
  })

  it('shows "You win!" when the hit points reach zero', async () => {
    const user = userEvent.setup()
    localStorage.setItem(KEY, ID)
    stubFetch({
      [`GET ${GAME_PATH}`]: () => json(activeGame(1)),
      [`POST ${GAME_PATH}/shots`]: () => json({ hit: true, currentHitPoints: 0 }),
    })
    render(<App />)

    await user.click(await screen.findByRole('button', { name: 'B2' }))

    expect(await screen.findByText('You win!')).toBeInTheDocument()
  })
})
