import { getProfile } from '../../api/api';

export const LOAD_PROFILE_REQUEST = 'LOAD_PROFILE_REQUEST';
export const LOAD_PROFILE_SUCCESS = 'LOAD_PROFILE_SUCCESS';
export const LOAD_PROFILE_FAILURE = 'LOAD_PROFILE_FAILURE';

const reducer = (state = {}, action) => {
	switch (action.type) {
	case LOAD_PROFILE_SUCCESS:
		return action.response;
	default:
		return state;
	}
};

export default reducer;

export const loadProfile = () => {
	return async (dispatch) => {
		return dispatch({
			types: [
				LOAD_PROFILE_REQUEST,
				LOAD_PROFILE_SUCCESS,
				LOAD_PROFILE_FAILURE,
			],
			callAPI: async () => getProfile(),
			payload: { slice: 'profile' },
		});
	};
};
