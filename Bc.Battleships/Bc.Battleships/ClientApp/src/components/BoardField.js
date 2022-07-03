import React, { Fragment } from 'react';

const fieldStateMap = {
    0 : '▢',
    1 : '○',
    2 : '●'
}

const BoardField = props => {
    const key = props.rowNumber+'|'+props.columnNumber

    return <span key={key}>{fieldStateMap[props.state]}</span>
}

export default BoardField;