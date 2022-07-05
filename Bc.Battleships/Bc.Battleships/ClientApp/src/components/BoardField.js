import React from 'react';
import { shotAt } from '../logic/backend';
import { Button } from 'reactstrap';
import { FIELDSTATEMAP } from '../logic/constants';

import '../custom.css'

const BoardField = props => {
    const key = '' + props.rowNumber + '|' + props.columnNumber
    const onClick = () => {
        shotAt(props.columnNumber, props.rowNumber, props.onShot)
    }

    return (
        <td>
            <Button key={key} onClick={onClick} className="board-field">{FIELDSTATEMAP[props.state]}
            </Button>
        </td>
    )
}

export default BoardField;