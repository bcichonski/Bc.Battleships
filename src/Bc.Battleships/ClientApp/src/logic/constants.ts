export const ALPHABET = 'ABCDEFGHIJKLMNPQRSTUVWXYZ'

export const BOARDFIELD = {
  UNKNOWN: 0,
  MISS: 1,
  HIT: 2,
} as const

export type BoardFieldState = (typeof BOARDFIELD)[keyof typeof BOARDFIELD]

export const FIELDSTATEMAP: Record<BoardFieldState, string> = {
  [BOARDFIELD.UNKNOWN]: '',
  [BOARDFIELD.HIT]: '●',
  [BOARDFIELD.MISS]: '○',
}
