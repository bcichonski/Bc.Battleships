import React, { useEffect, useState } from 'react';
import { Container, Row } from 'reactstrap';
import CurrentHitPoints from './CurrentHitPoints';
import Board from './Board'

const Game = () => {
    const [state, setState] = useState({
        state: 'loading'
    })

    useEffect(() => {
        async function fetchData() {
            try {
                const result = await (await fetch("/game")).json();
                setState({
                    state: 'ok',
                    data: result
                })
            } catch (error) {
                setState({
                    state: 'error',
                    error: error
                })
            }
        }

        fetchData()
    }, [])

    if (state.state === 'ok') {

        if(state.data.isGameActive) {
            return (
                <Container>
                    <Row><CurrentHitPoints hp={state.data.currentHitPoints}></CurrentHitPoints></Row>
                    <Row><Board rows={state.data.board.boardRows}></Board></Row>
                    <Row><pre>{JSON.stringify(state.data, null, 2)}</pre></Row>
                </Container>
            )
        }

        return (
            <div>
                CreateNewGame
            </div>
        )
    }

    if (state.state === 'error') {
        return (
            <div>
                {JSON.stringify(state.error)}
            </div>
        )
    }

    return (<div>Loading...</div>)
}

export default Game;
