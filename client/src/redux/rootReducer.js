import { combineReducers } from 'redux';
import profile from './ducks/profile';
import tournSearch from './ducks/search';

const rootReducer = combineReducers({
	profile,
	tournSearch,
});

export default rootReducer;
