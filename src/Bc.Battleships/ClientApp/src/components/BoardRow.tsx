import { convertNumberToLetter } from '../logic/logic'
import type { BoardFieldState } from '../logic/constants'
import BoardField from './BoardField'

interface Props {
  row: BoardFieldState[]
  rowNumber: number
  onShot: (column: number, row: number) => void
}

const BoardRow = ({ row, rowNumber, onShot }: Props) => (
  <tr className="board-row">
    <th className="board-field board-header">{convertNumberToLetter(rowNumber)}</th>
    {row.map((state, idx) => (
      <BoardField key={idx} state={state} rowNumber={rowNumber} columnNumber={idx} onShot={onShot} />
    ))}
  </tr>
)

export default BoardRow
