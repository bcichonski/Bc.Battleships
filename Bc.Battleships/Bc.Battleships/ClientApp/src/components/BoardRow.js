import React from 'react';
import { convertNumberToLetter } from '../logic/logic';
import BoardField from './BoardField'

import '../custom.css'

const BoardRow = props => {
    var columns = props.row.map((x, idx) => (<BoardField key={(''+props.rowNumber+idx)} state={x} rowNumber={props.rowNumber} columnNumber={idx} onShot={props.onShot}></BoardField>))

    return (
        <tr key={props.rowNumber} className="board-row">
            <th className="board-field board-header">{convertNumberToLetter(props.rowNumber)}</th>
            {columns}
        </tr>
    )
}

export default BoardRow;