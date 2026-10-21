import React from 'react';
import BoardRow from './BoardRow';

import '../custom.css'

const Board = props => {
    const rowElements = props.rows.map((x, idx) => (
        <BoardRow key={idx} row={x.row} rowNumber={idx} onShot={props.onShot}>
        </BoardRow>
    ))

    const header = [<th className='board-field board-header'></th>, ...props.rows[0].row.map((_, idx) => (<th className='board-field board-header'>{idx + 1}</th>))]

    return (
        <table className="board">
            <thead>
                {header}
            </thead>
            <tbody>
                {rowElements}
            </tbody>
        </table>
    )
}

export default Board;