import React from 'react';
import { Container, Row } from 'reactstrap';
import BoardRow from './BoardRow';

const Board = props => {
    const rowElements = props.rows.map((x, idx) => (
        <Row>
            <BoardRow row={x.row} rowNumber={idx}>
            </BoardRow>
        </Row>
    ))

    return (
        <Container>
            {rowElements}
        </Container>
    )
}

export default Board;