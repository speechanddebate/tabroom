/* istanbul ignore file */
import Cookies from 'js-cookie';
import { fetch } from '@speechanddebate/nsda-js-utils';

export const fetchBase = async (path, options = {}, body = {}) => {
	const base = process.env.REACT_APP_API_BASE;
	const fetchOptions = {
		method: options.method ? options.method : 'GET',
		body: body instanceof FormData ? body : JSON.stringify(body),
		maxRetries: (options.method && options.method !== 'GET') ? 0 : 3,
		retryDelay: process.env.NODE_ENV === 'test' ? 10 : 100,
		credentials: 'include',
		headers: body instanceof FormData ?
			{
				'X-Tabroom-Cookie': Cookies.get('TabroomToken'),
			}
			:
			{
				'Content-Type': 'application/json',
				'X-Tabroom-Cookie': Cookies.get('TabroomToken'),
			},
		...options,
	};

	if (fetchOptions.method === 'GET') {
		delete fetchOptions.body;
		delete fetchOptions.headers?.['Content-Type'];
	}

	try {
		const response = await fetch(`${base}/${path}`, fetchOptions);
		return options.raw ? response : response.json();
	} catch (err) {
		console.log(err);
		throw err;
	}
};

export const getProfile = async () => {
	return fetchBase(`user/profile`);
};
export const postProfile = async (personId, profile) => {
	return fetchBase(`user/profile/${personId}`, { method: 'POST', body: profile });
};

export const tournSearch = async (searchString, mode) => {

	if (mode === 'all') {
		return fetchBase(`/public/search/all/${searchString}`);
	}
	if (mode === 'past') {
		return fetchBase(`/public/search/past/${searchString}`);
	}
	return fetchBase(`/public/search/future/${searchString}`);
};

export const getTournList = async (state) => {
	return fetchBase(`/invite/upcoming?state=${state}`);
};
