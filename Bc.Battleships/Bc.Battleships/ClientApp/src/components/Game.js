import React, { useEffect, useState, Fragment } from 'react';
import { Row } from 'reactstrap';
import CurrentHitPoints from './CurrentHitPoints';
import Board from './Board'
import { handleShot } from '../logic/logic';
import { fetchGameData, newGame } from '../logic/backend';
import { useHistory } from 'react-router-dom';

import '../custom.css'

const winMessage = (
    <div className="big-message">
        You win!
    </div>
)

const creatingMessage = (
    <div>
        Creating a new game...
    </div>
)

const activeGame = (state, onShot) => (
    <div>
        <Row><CurrentHitPoints hp={state.data.currentHitPoints}></CurrentHitPoints></Row>
        <Row><Board rows={state.data.board.boardRows} onShot={onShot}></Board></Row>
    </div>
)

const errorMessage = state => (
    <div>
        {JSON.stringify(state.error)}
    </div>
)

const Game = () => {
    const history = useHistory()

    const [state, setState] = useState({
        state: 'loading'
    })

    const onShot = (result) => setState(handleShot(result, state))

    useEffect(() => {
        fetchGameData(setState)
    }, [])

    if (state.state === 'loading') {
        return (<div>Loading...</div>)
    }

    if (state.state === 'error') {
        return errorMessage(state)
    }

    if (state.data.currentHitPoints <= 0) {
        return winMessage
    }

    if (state.data.isGameActive) {
        return activeGame(state, onShot)
    }

    newGame(history);

    return creatingMessage
}

export default Game;
