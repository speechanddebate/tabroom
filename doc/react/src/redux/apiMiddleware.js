// With inspiration from https://redux.js.org/recipes/reducing-boilerplate
const api = ({ dispatch, getState }) => {
	return next => action => {
		const { types, callAPI, shouldCallAPI = () => true, payload = {}, meta = {} } = action;

		if (!types) {
			// Normal action without a types parameter, just pass it on
			return next(action);
		}

		if (
			!Array.isArray(types) ||
			types.length !== 3 ||
			!types.every(type => typeof type === 'string')
		) {
			throw new Error('Expected an array of three string types.');
		}
		if (typeof callAPI !== 'function') {
			throw new Error('Expected callAPI to be a function.');
		}
		if (!shouldCallAPI(getState())) {
			return;
		}

		const [requestType, successType, failureType] = types;

		// Dispatch a request action
		dispatch({ ...payload, type: requestType });

		// Call the provided API function and dispatch success or error actions
		return callAPI().then(response => {
			dispatch({ ...payload, response, type: successType });
			if (meta.successMessage) {
				// dispatch(Notifications.success({ title: meta.successMessage }));
			}
			return response;
		}).catch(err => {
			// log(err);
			dispatch({ ...payload, err, type: failureType });
			if (meta.failureMessage) {
				// dispatch(Notifications.error({
				//     title: meta.failureMessage,
				//     message: `Error ${err.statusCode}: ${err.message}`,
				// }));
			}
		});
	};
};

export default api;
