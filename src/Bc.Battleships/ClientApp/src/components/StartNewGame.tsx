interface Props {
  onClick: () => void
  disabled?: boolean
}

const StartNewGame = ({ onClick, disabled = false }: Props) => (
  <button type="button" className="btn btn-secondary" onClick={onClick} disabled={disabled}>
    Start new game
  </button>
)

export default StartNewGame
