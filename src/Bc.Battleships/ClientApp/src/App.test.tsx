import { afterEach, describe, expect, it, vi } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import App from './App'
import type { GameDto } from './logic/types'

const emptyBoard = (): GameDto['board'] => ({
  boardSize: 10,
  boardRows: Array.from({ length: 10 }, () => ({ row: Array<0 | 1 | 2>(10).fill(0) })),
})

const activeGame = (hp = 5): GameDto => ({ isGameActive: true, currentHitPoints: hp, board: emptyBoard() })
const noGame: GameDto = { isGameActive: false, currentHitPoints: 0, board: null }

const json = (body: unknown, status = 200) => new Response(JSON.stringify(body), { status })

type Handler = (init?: RequestInit) => Response | Promise<Response>

const stubFetch = (handlers: { GET: Handler[] | Handler; POST?: Handler; PUT?: Handler }) => {
  const gets = Array.isArray(handlers.GET) ? [...handlers.GET] : null
  const fetchMock = vi.fn(async (_url: string, init?: RequestInit) => {
    const method = init?.method ?? 'GET'
    if (method === 'GET') {
      const handler = gets ? (gets.length > 1 ? gets.shift()! : gets[0]) : (handlers.GET as Handler)
      return handler(init)
    }
    const handler = method === 'POST' ? handlers.POST : handlers.PUT
    if (!handler) throw new Error(`unexpected ${method}`)
    return handler(init)
  })
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

const callsOf = (fetchMock: ReturnType<typeof stubFetch>, method: string) =>
  fetchMock.mock.calls.filter(([, init]) => (init?.method ?? 'GET') === method)

describe('App', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('shows loading and then the board of an active game', async () => {
    const fetchMock = stubFetch({ GET: () => json(activeGame()) })
    render(<App />)

    expect(screen.getByText('Loading...')).toBeInTheDocument()
    expect(await screen.findByText('Left to hit: 5')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'A1' })).toBeInTheDocument()
    expect(callsOf(fetchMock, 'GET')).toHaveLength(1)
    expect(callsOf(fetchMock, 'POST')).toHaveLength(0)
  })

  it('shows the no-game view without sending a POST', async () => {
    const fetchMock = stubFetch({ GET: () => json(noGame) })
    render(<App />)

    expect(await screen.findByText(/No game in progress/)).toBeInTheDocument()
    expect(screen.getAllByRole('button', { name: 'Start new game' })).toHaveLength(2)
    expect(callsOf(fetchMock, 'POST')).toHaveLength(0)
  })

  it('shows a readable error when loading fails', async () => {
    stubFetch({ GET: () => json({}, 500) })
    render(<App />)

    expect(await screen.findByRole('alert')).toHaveTextContent('GET /game failed with status 500')
  })

  it('starts a new game with one POST then a GET, without reload', async () => {
    const user = userEvent.setup()
    const fetchMock = stubFetch({
      GET: [() => json(noGame), () => json(activeGame(7))],
      POST: () => new Response(null, { status: 200 }),
    })
    render(<App />)
    await screen.findByText(/No game in progress/)

    await user.click(screen.getAllByRole('button', { name: 'Start new game' })[1])

    expect(await screen.findByText('Left to hit: 7')).toBeInTheDocument()
    expect(callsOf(fetchMock, 'POST')).toHaveLength(1)
    expect(callsOf(fetchMock, 'GET')).toHaveLength(2)
    const methods = fetchMock.mock.calls.map(([, init]) => init?.method ?? 'GET')
    expect(methods).toEqual(['GET', 'POST', 'GET'])
  })

  it('sends a single POST on a double click and disables Start while in flight', async () => {
    const user = userEvent.setup()
    let resolvePost: (response: Response) => void = () => {}
    const fetchMock = stubFetch({
      GET: [() => json(noGame), () => json(activeGame())],
      POST: () => new Promise<Response>((resolve) => (resolvePost = resolve)),
    })
    render(<App />)
    await screen.findByText(/No game in progress/)

    const [navButton, viewButton] = screen.getAllByRole('button', { name: 'Start new game' })
    await user.dblClick(viewButton)

    await waitFor(() => expect(navButton).toBeDisabled())
    expect(viewButton).toBeDisabled()
    resolvePost(new Response(null, { status: 200 }))

    expect(await screen.findByText('Left to hit: 5')).toBeInTheDocument()
    expect(callsOf(fetchMock, 'POST')).toHaveLength(1)
  })

  it('sends the clicked cell, renders a hit and decrements the hit points', async () => {
    const user = userEvent.setup()
    const fetchMock = stubFetch({
      GET: () => json(activeGame(5)),
      PUT: () => json({ hit: true, currentHitPoints: 4 }),
    })
    render(<App />)
    const cell = await screen.findByRole('button', { name: 'C4' })

    await user.click(cell)

    await waitFor(() => expect(cell).toHaveTextContent('●'))
    expect(screen.getByText('Left to hit: 4')).toBeInTheDocument()
    expect(cell).toBeDisabled()
    const [[url, init]] = callsOf(fetchMock, 'PUT')
    expect(url).toBe('/game')
    expect(JSON.parse(init!.body as string)).toEqual({ column: 3, row: 2 })
  })

  it('renders a miss without changing the hit points', async () => {
    const user = userEvent.setup()
    stubFetch({ GET: () => json(activeGame(5)), PUT: () => json({ hit: false, currentHitPoints: 5 }) })
    render(<App />)
    const cell = await screen.findByRole('button', { name: 'A1' })

    await user.click(cell)

    await waitFor(() => expect(cell).toHaveTextContent('○'))
    expect(screen.getByText('Left to hit: 5')).toBeInTheDocument()
    expect(cell).toBeDisabled()
  })

  it('shows an error message when a shot fails', async () => {
    const user = userEvent.setup()
    stubFetch({ GET: () => json(activeGame()), PUT: () => json({}, 500) })
    render(<App />)

    await user.click(await screen.findByRole('button', { name: 'A1' }))

    expect(await screen.findByRole('alert')).toHaveTextContent('PUT /game failed with status 500')
  })

  it('shows "You win!" when the hit points reach zero', async () => {
    const user = userEvent.setup()
    stubFetch({ GET: () => json(activeGame(1)), PUT: () => json({ hit: true, currentHitPoints: 0 }) })
    render(<App />)

    await user.click(await screen.findByRole('button', { name: 'B2' }))

    expect(await screen.findByText('You win!')).toBeInTheDocument()
  })
})
