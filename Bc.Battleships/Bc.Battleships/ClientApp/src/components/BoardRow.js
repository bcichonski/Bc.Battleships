import React from 'react';
import BoardField from './BoardField'

const BoardRow = props => {
    var columns = props.row.map((x, idx) => (<BoardField key={(''+props.rowNumber+idx)} state={x} rowNumber={props.rowNumber} columnNumber={idx} onShot={props.onShot}></BoardField>))
    return (
        <div key={props.rowNumber} className="board-row">
            {columns}
        </div>
    )
}

export default BoardRow;