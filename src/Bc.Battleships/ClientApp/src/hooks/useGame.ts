import { useCallback, useEffect, useRef, useState } from 'react'
import { ApiError, createGame, getGame, shoot as shootApi } from '../api/gameApi'
import { applyShot } from '../logic/logic'
import type { GameDto } from '../logic/types'

export type GameView = 'loading' | 'error' | 'win' | 'active' | 'none'

interface State {
  loading: boolean
  error: string | null
  game: GameDto | null
  startingNewGame: boolean
}

export const GAME_ID_KEY = 'battleships.gameId'

const readStoredGameId = (): string | null => {
  try {
    return localStorage.getItem(GAME_ID_KEY)
  } catch {
    return null
  }
}

const storeGameId = (id: string | null) => {
  try {
    if (id === null) localStorage.removeItem(GAME_ID_KEY)
    else localStorage.setItem(GAME_ID_KEY, id)
  } catch {
    // storage unavailable: the game just will not survive a reload
  }
}

const toMessage = (error: unknown) => (error instanceof Error ? error.message : String(error))

const withGame = (current: State, game: GameDto): State => ({
  ...current,
  loading: false,
  error: null,
  game,
})

export const selectView = (state: Pick<State, 'loading' | 'error' | 'game'>): GameView => {
  if (state.loading) return 'loading'
  if (state.error !== null) return 'error'
  const game = state.game
  if (game && game.board && game.currentHitPoints <= 0) return 'win'
  if (game && game.isGameActive && game.board) return 'active'
  return 'none'
}

export function useGame() {
  const [initialId] = useState(readStoredGameId)
  const gameIdRef = useRef<string | null>(initialId)
  const [state, setState] = useState<State>({
    loading: initialId !== null,
    error: null,
    game: null,
    startingNewGame: false,
  })
  const startingRef = useRef(false)

  useEffect(() => {
    const id = gameIdRef.current
    if (id === null) return
    let cancelled = false
    getGame(id).then(
      (game) => {
        if (!cancelled) setState((current) => withGame(current, game))
      },
      (error: unknown) => {
        if (cancelled) return
        if (error instanceof ApiError && error.status === 404) {
          gameIdRef.current = null
          storeGameId(null)
          setState((current) => ({ ...current, loading: false, error: null, game: null }))
          return
        }
        setState((current) => ({ ...current, loading: false, error: toMessage(error) }))
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
      const game = await createGame()
      gameIdRef.current = game.id
      storeGameId(game.id)
      setState((current) => ({ ...withGame(current, game), startingNewGame: false }))
    } catch (error) {
      setState((current) => ({ ...current, loading: false, error: toMessage(error), startingNewGame: false }))
    } finally {
      startingRef.current = false
    }
  }, [])

  const shoot = useCallback(async (column: number, row: number) => {
    const id = gameIdRef.current
    if (id === null) return
    try {
      const result = await shootApi(id, { column, row })
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
