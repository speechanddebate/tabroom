import { getTournList } from '../../api/api';

export const LOAD_TOURN_LIST_REQUEST = 'LOAD_TOURN_LIST_REQUEST';
export const LOAD_TOURN_LIST_SUCCESS = 'LOAD_TOURN_LIST_SUCCESS';
export const LOAD_TOURN_LIST_FAILURE = 'LOAD_TOURN_LIST_FAILURE';

const reducer = (state = {}, action) => {
	switch (action.type) {
	case LOAD_TOURN_LIST_SUCCESS:
		return action.response;
	default:
		return state;
	}
};

export default reducer;

export const loadTournList = (stateCode) => {
	return async (dispatch) => {
		return dispatch({
			types: [
				LOAD_TOURN_LIST_REQUEST,
				LOAD_TOURN_LIST_SUCCESS,
				LOAD_TOURN_LIST_FAILURE,
			],
			callAPI: async () => getTournList(stateCode),
			payload: { slice: 'tournList' },
		});
	};
};
