import { BOARDFIELD, FIELDSTATEMAP, type BoardFieldState } from '../logic/constants'
import { convertNumberToLetter } from '../logic/logic'

interface Props {
  state: BoardFieldState
  rowNumber: number
  columnNumber: number
  onShot: (column: number, row: number) => void
}

const BoardField = ({ state, rowNumber, columnNumber, onShot }: Props) => (
  <td>
    <button
      type="button"
      className="btn btn-secondary board-field"
      aria-label={`${convertNumberToLetter(rowNumber)}${columnNumber + 1}`}
      disabled={state !== BOARDFIELD.UNKNOWN}
      onClick={() => onShot(columnNumber, rowNumber)}
    >
      {FIELDSTATEMAP[state]}
    </button>
  </td>
)

export default BoardField
