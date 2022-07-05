import React, { useEffect, useState } from 'react';
import { Container, Row } from 'reactstrap';
import CurrentHitPoints from './CurrentHitPoints';
import Board from './Board'
import { handleShot } from '../logic/logic';
import { fetchGameData } from '../logic/backend';

import '../custom.css'

const Game = () => {
    const [state, setState] = useState({
        state: 'loading'
    })

    const onShot = (result) => setState(handleShot(result, state))
    
    useEffect(() => {
        fetchGameData(setState)
    }, [])

    if (state.state === 'ok') {

        if(state.data.isGameActive) {
            return (
                <Container className="center">
                    <Row><CurrentHitPoints hp={state.data.currentHitPoints}></CurrentHitPoints></Row>
                    <Row><Board rows={state.data.board.boardRows} onShot={onShot}></Board></Row>
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
