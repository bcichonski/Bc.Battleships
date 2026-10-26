import type { BoardFieldState } from './constants'

export interface BoardRowDto {
  row: BoardFieldState[]
}

export interface BoardDto {
  boardSize: number
  boardRows: BoardRowDto[]
}

export interface GameDto {
  id: string
  isGameActive: boolean
  currentHitPoints: number
  board: BoardDto | null
}

export interface ShootResult {
  hit: boolean
  currentHitPoints: number
}

export interface ShootCommand {
  column: number
  row: number
}
