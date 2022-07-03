import React, { useEffect, useState } from 'react';

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
        return (
            <div>
                {JSON.stringify(state.data)}
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
