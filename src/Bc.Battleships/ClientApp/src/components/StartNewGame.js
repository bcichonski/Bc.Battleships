import { Button } from 'reactstrap';
import React from 'react';
import { useHistory } from 'react-router'
import { newGame } from '../logic/backend';

const StartNewGame = (props) => {
    const history = useHistory()

    const onClickFn = () => newGame(history)

    return (
        <Button onClick={onClickFn}>Start new game</Button>
    )
}

export default StartNewGame;