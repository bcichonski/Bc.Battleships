import cloneDeep from 'lodash/cloneDeep';
import { BOARDFIELD } from './constants';

const handleShot = (result, state) => {
    if (result.state === 'error') {
        console.error(result.error)
        return
    }

    let newState = cloneDeep(state)

    newState.data.currentHitPoints = result.data.currentHitPoints

    newState.data.board.boardRows[result.row-1].row[result.column-1] = result.data.hit ? BOARDFIELD.HIT : BOARDFIELD.MISS
    
    return newState 
}

export { handleShot }