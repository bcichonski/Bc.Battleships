import cloneDeep from 'lodash/cloneDeep';
import { ALPHABET, BOARDFIELD } from './constants';

const throwIt = (err) => {
    throw err;
}

const handleShot = (result, state) => {
    if (result.state === 'error') {
        throwIt(result.error)
        return
    }

    let newState = cloneDeep(state)

    newState.data.currentHitPoints = result.data.currentHitPoints

    newState.data.board.boardRows[result.row-1].row[result.column-1] = result.data.hit ? BOARDFIELD.HIT : BOARDFIELD.MISS
    
    return newState 
}

const convertNumberToLetter = (number) => number < ALPHABET.length ? ALPHABET.at(number) : throwIt(new Error('Alphabet is to small'))

export { handleShot, convertNumberToLetter }