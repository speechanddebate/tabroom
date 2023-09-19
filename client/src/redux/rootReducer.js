import { combineReducers } from 'redux';
import profile from './ducks/profile';
import tournSearch from './ducks/search';
import tournList from './ducks/tournList';

const rootReducer = combineReducers({
	profile,
	tournSearch,
	tournList,
});

export default rootReducer;
