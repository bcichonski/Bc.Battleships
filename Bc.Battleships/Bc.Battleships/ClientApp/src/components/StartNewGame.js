import { Button } from 'reactstrap';
import React from 'react';

const StartNewGame = (props) => {
    const onClickFn = () => {
        async function NewGame() {
            await fetch('/game',{
                method: 'POST',
                body: ''
            })
        }

        NewGame()
    }

    return (
        <Button onClick={onClickFn}>Start new game</Button>
    )
}

export default StartNewGame;