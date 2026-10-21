import './NavMenu.css'
import StartNewGame from './StartNewGame'

interface Props {
  onStartNewGame: () => void
  startDisabled: boolean
}

const NavMenu = ({ onStartNewGame, startDisabled }: Props) => (
  <header>
    <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light ng-white border-bottom box-shadow mb-3">
      <div className="container">
        <a className="navbar-brand" href="/">
          Bc.Battleships
        </a>
        <StartNewGame onClick={onStartNewGame} disabled={startDisabled} />
      </div>
    </nav>
  </header>
)

export default NavMenu
