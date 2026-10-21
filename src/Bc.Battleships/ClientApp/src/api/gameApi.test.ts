import { afterEach, describe, expect, it, vi } from 'vitest'
import { ApiError, createGame, getGame, shoot } from './gameApi'

const stubFetch = (response: Response) => {
  const fetchMock = vi.fn().mockResolvedValue(response)
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

describe('gameApi', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('getGame sends GET /game and parses the body', async () => {
    const dto = { isGameActive: true, currentHitPoints: 3, board: null }
    const fetchMock = stubFetch(new Response(JSON.stringify(dto), { status: 200 }))

    await expect(getGame()).resolves.toEqual(dto)
    expect(fetchMock).toHaveBeenCalledWith('/game')
  })

  it('createGame sends POST /game and does not parse the empty body', async () => {
    const response = new Response(null, { status: 200 })
    const json = vi.spyOn(response, 'json')
    const fetchMock = stubFetch(response)

    await expect(createGame()).resolves.toBeUndefined()
    expect(fetchMock).toHaveBeenCalledWith('/game', { method: 'POST' })
    expect(json).not.toHaveBeenCalled()
  })

  it('shoot sends PUT /game with the column and row', async () => {
    const fetchMock = stubFetch(new Response(JSON.stringify({ hit: true, currentHitPoints: 4 }), { status: 200 }))

    await expect(shoot({ column: 2, row: 7 })).resolves.toEqual({ hit: true, currentHitPoints: 4 })
    expect(fetchMock).toHaveBeenCalledWith('/game', {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ column: 2, row: 7 }),
    })
  })

  it('rejects with the status on a non-2xx response', async () => {
    stubFetch(new Response('boom', { status: 500 }))

    const error = await getGame().catch((e: unknown) => e)
    expect(error).toBeInstanceOf(ApiError)
    expect((error as ApiError).status).toBe(500)

    stubFetch(new Response(null, { status: 404 }))
    await expect(createGame()).rejects.toMatchObject({ status: 404 })
    stubFetch(new Response(null, { status: 400 }))
    await expect(shoot({ column: 0, row: 0 })).rejects.toMatchObject({ status: 400 })
  })
})
