import React, { useEffect, useState } from 'react';
import { Container, Row } from 'reactstrap';
import CurrentHitPoints from './CurrentHitPoints';
import Board from './Board'
import { handleShot } from '../logic/logic';
import { fetchGameData, newGame } from '../logic/backend';
import { useHistory } from 'react-router-dom';

import '../custom.css'

const Game = () => {
    const history = useHistory()

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
                </Container>
            )
        }

        newGame(history);

        return (
            <div>
                Creating a new game...
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
