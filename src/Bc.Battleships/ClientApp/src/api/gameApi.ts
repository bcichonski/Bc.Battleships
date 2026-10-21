import type { GameDto, ShootCommand, ShootResult } from '../logic/types'

const GAME_PATH = '/game'

export class ApiError extends Error {
  readonly status: number

  constructor(method: string, status: number) {
    super(`${method} ${GAME_PATH} failed with status ${status}`)
    this.name = 'ApiError'
    this.status = status
  }
}

const ensureOk = (method: string, response: Response) => {
  if (!response.ok) throw new ApiError(method, response.status)
}

export async function getGame(): Promise<GameDto> {
  const response = await fetch(GAME_PATH)
  ensureOk('GET', response)
  return (await response.json()) as GameDto
}

export async function createGame(): Promise<void> {
  const response = await fetch(GAME_PATH, { method: 'POST' })
  ensureOk('POST', response)
}

export async function shoot(command: ShootCommand): Promise<ShootResult> {
  const response = await fetch(GAME_PATH, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(command),
  })
  ensureOk('PUT', response)
  return (await response.json()) as ShootResult
}
