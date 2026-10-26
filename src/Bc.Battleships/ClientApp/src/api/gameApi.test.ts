import { afterEach, describe, expect, it, vi } from 'vitest'
import { ApiError, createGame, getGame, shoot } from './gameApi'

const stubFetch = (response: Response) => {
  const fetchMock = vi.fn().mockResolvedValue(response)
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

const dto = { id: 'abc', isGameActive: true, currentHitPoints: 3, board: null }

describe('gameApi', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('createGame sends POST /games and returns the game with its id', async () => {
    const fetchMock = stubFetch(new Response(JSON.stringify(dto), { status: 201 }))

    await expect(createGame()).resolves.toEqual(dto)
    expect(fetchMock).toHaveBeenCalledWith('/games', { method: 'POST' })
  })

  it('getGame sends GET /games/{id} and parses the body', async () => {
    const fetchMock = stubFetch(new Response(JSON.stringify(dto), { status: 200 }))

    await expect(getGame('abc')).resolves.toEqual(dto)
    expect(fetchMock).toHaveBeenCalledWith('/games/abc')
  })

  it('shoot sends POST /games/{id}/shots with the column and row', async () => {
    const fetchMock = stubFetch(new Response(JSON.stringify({ hit: true, currentHitPoints: 4 }), { status: 200 }))

    await expect(shoot('abc', { column: 2, row: 7 })).resolves.toEqual({ hit: true, currentHitPoints: 4 })
    expect(fetchMock).toHaveBeenCalledWith('/games/abc/shots', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ column: 2, row: 7 }),
    })
  })

  it('rejects with the status and the actual path on a non-2xx response', async () => {
    stubFetch(new Response('boom', { status: 500 }))
    const error = await getGame('abc').catch((e: unknown) => e)
    expect(error).toBeInstanceOf(ApiError)
    expect((error as ApiError).status).toBe(500)
    expect((error as ApiError).message).toBe('GET /games/abc failed with status 500')

    stubFetch(new Response(null, { status: 500 }))
    await expect(createGame()).rejects.toMatchObject({ status: 500, message: 'POST /games failed with status 500' })
    stubFetch(new Response(null, { status: 409 }))
    await expect(shoot('abc', { column: 0, row: 0 })).rejects.toMatchObject({
      status: 409,
      message: 'POST /games/abc/shots failed with status 409',
    })
  })
})
