import type { BoardRowDto } from '../logic/types'
import BoardRow from './BoardRow'

interface Props {
  rows: BoardRowDto[]
  onShot: (column: number, row: number) => void
}

const Board = ({ rows, onShot }: Props) => (
  <table className="board">
    <thead>
      <tr>
        <th className="board-field board-header"></th>
        {rows[0].row.map((_, idx) => (
          <th key={idx} className="board-field board-header">
            {idx + 1}
          </th>
        ))}
      </tr>
    </thead>
    <tbody>
      {rows.map((x, idx) => (
        <BoardRow key={idx} row={x.row} rowNumber={idx} onShot={onShot} />
      ))}
    </tbody>
  </table>
)

export default Board
