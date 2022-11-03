import { tournSearch } from '../../api/api';

export const LOAD_TOURNSEARCH_REQUEST = 'LOAD_TOURNSEARCH_REQUEST';
export const LOAD_TOURNSEARCH_SUCCESS = 'LOAD_TOURNSEARCH_SUCCESS';
export const LOAD_TOURNSEARCH_FAILURE = 'LOAD_TOURNSEARCH_FAILURE';

export const LOAD_CLEARSEARCH_REQUEST = 'LOAD_TOURNSEARCH_REQUEST';
export const LOAD_CLEARSEARCH_SUCCESS = 'LOAD_TOURNSEARCH_SUCCESS';
export const LOAD_CLEARSEARCH_FAILURE = 'LOAD_TOURNSEARCH_FAILURE';

const reducer = (state = {}, action) => {
	switch (action.type) {
	case LOAD_TOURNSEARCH_SUCCESS:
		return action.response;
	default:
		return state;
	}
};

export default reducer;

export const loadTournSearch = (string, mode) => {
	return async (dispatch) => {
		return dispatch({
			types: [
				LOAD_TOURNSEARCH_REQUEST,
				LOAD_TOURNSEARCH_SUCCESS,
				LOAD_TOURNSEARCH_FAILURE,
			],
			callAPI: async () => tournSearch(string, mode),
			payload: { slice: 'searchResults' },
		});
	};
};

export const clearTournSearch = () => {
	return async (dispatch) => {
		return dispatch({
			types: [
				LOAD_CLEARSEARCH_REQUEST,
				LOAD_CLEARSEARCH_SUCCESS,
				LOAD_CLEARSEARCH_FAILURE,
			],
			callAPI: async () => { return false; },
			payload: { slice: 'searchResults' },
		});
	};
};
