import type { GameDto, ShootCommand, ShootResult } from '../logic/types'

const GAMES_PATH = '/games'

export class ApiError extends Error {
  readonly status: number

  constructor(method: string, path: string, status: number) {
    super(`${method} ${path} failed with status ${status}`)
    this.name = 'ApiError'
    this.status = status
  }
}

const ensureOk = (method: string, path: string, response: Response) => {
  if (!response.ok) throw new ApiError(method, path, response.status)
}

export async function createGame(): Promise<GameDto> {
  const response = await fetch(GAMES_PATH, { method: 'POST' })
  ensureOk('POST', GAMES_PATH, response)
  return (await response.json()) as GameDto
}

export async function getGame(id: string): Promise<GameDto> {
  const path = `${GAMES_PATH}/${id}`
  const response = await fetch(path)
  ensureOk('GET', path, response)
  return (await response.json()) as GameDto
}

export async function shoot(id: string, command: ShootCommand): Promise<ShootResult> {
  const path = `${GAMES_PATH}/${id}/shots`
  const response = await fetch(path, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(command),
  })
  ensureOk('POST', path, response)
  return (await response.json()) as ShootResult
}
