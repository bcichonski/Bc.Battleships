import React from 'react';
import { shotAt } from '../backend';
import { Button } from 'reactstrap';
import { FIELDSTATEMAP } from '../constants';

import '../custom.css'

const BoardField = props => {
    const key = '' + props.rowNumber + '|' + props.columnNumber
    const onClick = () => {
        shotAt(props.columnNumber + 1, props.rowNumber + 1, props.onShot)
    }

    return <Button key={key} onClick={onClick} className="board-field">{FIELDSTATEMAP[props.state]}</Button>
}

export default BoardField;