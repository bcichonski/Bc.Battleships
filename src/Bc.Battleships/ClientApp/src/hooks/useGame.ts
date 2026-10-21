import { useCallback, useEffect, useRef, useState } from 'react'
import { createGame, getGame, shoot as shootApi } from '../api/gameApi'
import { applyShot } from '../logic/logic'
import type { GameDto } from '../logic/types'

export type GameView = 'loading' | 'error' | 'win' | 'active' | 'none'

interface State {
  loading: boolean
  error: string | null
  game: GameDto | null
  // true once a board has been loaded in this session, i.e. a game existed
  hadBoard: boolean
  startingNewGame: boolean
}

const toMessage = (error: unknown) => (error instanceof Error ? error.message : String(error))

const withGame = (current: State, game: GameDto): State => ({
  ...current,
  loading: false,
  error: null,
  game,
  hadBoard: current.hadBoard || game.board !== null,
})

export const selectView = (state: Pick<State, 'loading' | 'error' | 'game' | 'hadBoard'>): GameView => {
  if (state.loading) return 'loading'
  if (state.error !== null) return 'error'
  const game = state.game
  if (game && state.hadBoard && game.currentHitPoints <= 0) return 'win'
  if (game && game.isGameActive && game.board) return 'active'
  return 'none'
}

export function useGame() {
  const [state, setState] = useState<State>({
    loading: true,
    error: null,
    game: null,
    hadBoard: false,
    startingNewGame: false,
  })
  const startingRef = useRef(false)

  useEffect(() => {
    let cancelled = false
    getGame().then(
      (game) => {
        if (!cancelled) setState((current) => withGame(current, game))
      },
      (error: unknown) => {
        if (!cancelled) setState((current) => ({ ...current, loading: false, error: toMessage(error) }))
      },
    )
    return () => {
      cancelled = true
    }
  }, [])

  const startNewGame = useCallback(async () => {
    if (startingRef.current) return
    startingRef.current = true
    setState((current) => ({ ...current, startingNewGame: true }))
    try {
      await createGame()
      const game = await getGame()
      setState((current) => ({ ...withGame(current, game), startingNewGame: false }))
    } catch (error) {
      setState((current) => ({ ...current, loading: false, error: toMessage(error), startingNewGame: false }))
    } finally {
      startingRef.current = false
    }
  }, [])

  const shoot = useCallback(async (column: number, row: number) => {
    try {
      const result = await shootApi({ column, row })
      setState((current) =>
        current.game ? { ...current, error: null, game: applyShot(current.game, column, row, result) } : current,
      )
    } catch (error) {
      setState((current) => ({ ...current, error: toMessage(error) }))
    }
  }, [])

  return {
    view: selectView(state),
    game: state.game,
    error: state.error,
    startingNewGame: state.startingNewGame,
    startNewGame,
    shoot,
  }
}
