import React from 'react';
import BoardRow from './BoardRow';

const Board = props => {
    const rowElements = props.rows.map((x, idx) => (
        <BoardRow key={idx} row={x.row} rowNumber={idx} onShot={props.onShot}>
        </BoardRow>
    ))

    return (
        <div className="board">
            {rowElements}
        </div>
    )
}

export default Board;