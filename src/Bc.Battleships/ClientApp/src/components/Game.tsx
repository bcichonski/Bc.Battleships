import CurrentHitPoints from './CurrentHitPoints'
import Board from './Board'
import StartNewGame from './StartNewGame'
import type { GameView } from '../hooks/useGame'
import type { GameDto } from '../logic/types'

import '../custom.css'

interface Props {
  view: GameView
  game: GameDto | null
  error: string | null
  startingNewGame: boolean
  onStartNewGame: () => void
  onShot: (column: number, row: number) => void
}

const Game = ({ view, game, error, startingNewGame, onStartNewGame, onShot }: Props) => {
  if (view === 'loading') return <div>Loading...</div>
  if (view === 'error') return <div role="alert">{error}</div>
  if (view === 'win') return <div className="big-message">You win!</div>

  if (view === 'active' && game?.board) {
    return (
      <div>
        <div className="row">
          <CurrentHitPoints hp={game.currentHitPoints} />
        </div>
        <div className="row">
          <Board rows={game.board.boardRows} onShot={onShot} />
        </div>
      </div>
    )
  }

  return (
    <div>
      <p>No game in progress.</p>
      <StartNewGame onClick={onStartNewGame} disabled={startingNewGame} />
    </div>
  )
}

export default Game
