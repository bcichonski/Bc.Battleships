async function fetchGameData(setState) {
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

async function newGame(history) {
    try {
        await fetch('/game',{
            method: 'POST',
            body: ''
        })

        history.go(0)
    } catch (error) {
        console.error(error)
    }
}

async function shotAt(column, row, process) {
    try {
        const result = await (await fetch("/game", {
            method: 'PUT',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
              },
            body:JSON.stringify({
                column,
                row
            })
        })).json();

        process({
            state: 'ok',
            column,
            row,
            data: result
        })
    } catch (error) {
        process({
            state: 'error',
            error
        })
    }
}

export { fetchGameData, shotAt, newGame }