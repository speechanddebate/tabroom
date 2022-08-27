import { combineReducers } from 'redux';
import profile from './ducks/profile';

const rootReducer = combineReducers({
	profile,
});

export default rootReducer;
