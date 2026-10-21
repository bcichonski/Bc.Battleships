import type { ReactNode } from 'react'
import NavMenu from './NavMenu'

import '../custom.css'

interface Props {
  children: ReactNode
  onStartNewGame: () => void
  startDisabled: boolean
}

const Layout = ({ children, onStartNewGame, startDisabled }: Props) => (
  <div>
    <NavMenu onStartNewGame={onStartNewGame} startDisabled={startDisabled} />
    <div className="container center">{children}</div>
  </div>
)

export default Layout
