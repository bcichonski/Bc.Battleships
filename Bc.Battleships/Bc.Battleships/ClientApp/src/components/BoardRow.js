import React, { Fragment } from 'react';
import BoardField from './BoardField'

const BoardRow = props => {
    var columns = props.row.map((x, idx) => (<BoardField state={x} rowNumber={props.rowNumber} columnNumber={idx}></BoardField>))
    return (
        <Fragment key={props.rowNumber} className="board-row">
            {columns}
        </Fragment>
    )
}

export default BoardRow;