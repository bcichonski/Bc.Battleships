import { describe, expect, it } from 'vitest'
import { convertNumberToLetter } from './logic'

describe('convertNumberToLetter', () => {
  it('converts indexes to letters', () => {
    expect(convertNumberToLetter(0)).toBe('A')
    expect(convertNumberToLetter(9)).toBe('J')
  })

  it('skips the letter O', () => {
    expect(convertNumberToLetter(13)).toBe('N')
    expect(convertNumberToLetter(14)).toBe('P')
  })

  it('throws when out of the alphabet', () => {
    expect(() => convertNumberToLetter(25)).toThrow('Alphabet is to small')
    expect(() => convertNumberToLetter(-1)).toThrow()
  })
})
