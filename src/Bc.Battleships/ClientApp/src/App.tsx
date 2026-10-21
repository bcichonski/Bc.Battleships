import Layout from './components/Layout'
import Game from './components/Game'
import { useGame } from './hooks/useGame'

import './custom.css'

const App = () => {
  const { view, game, error, startingNewGame, startNewGame, shoot } = useGame()

  return (
    <Layout onStartNewGame={() => void startNewGame()} startDisabled={startingNewGame}>
      <Game
        view={view}
        game={game}
        error={error}
        startingNewGame={startingNewGame}
        onStartNewGame={() => void startNewGame()}
        onShot={(column, row) => void shoot(column, row)}
      />
    </Layout>
  )
}

export default App
