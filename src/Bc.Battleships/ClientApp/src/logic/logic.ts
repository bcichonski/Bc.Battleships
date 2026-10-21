import { ALPHABET, BOARDFIELD } from './constants'
import type { GameDto, ShootResult } from './types'

export const applyShot = (game: GameDto, column: number, row: number, result: ShootResult): GameDto => {
  if (!game.board) return game
  const board = game.board
  return {
    ...game,
    currentHitPoints: result.currentHitPoints,
    board: {
      ...board,
      boardRows: board.boardRows.map((boardRow, r) =>
        r === row
          ? { row: boardRow.row.map((state, c) => (c === column ? (result.hit ? BOARDFIELD.HIT : BOARDFIELD.MISS) : state)) }
          : boardRow,
      ),
    },
  }
}

export const convertNumberToLetter = (index: number): string => {
  if (index < 0 || index >= ALPHABET.length) throw new Error('Alphabet is to small')
  return ALPHABET[index]
}
