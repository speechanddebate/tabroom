/* istanbul disable file */
import React from 'react';
import { Provider } from 'react-redux';
import { MemoryRouter, Route } from 'react-router-dom';
import '@testing-library/jest-dom';
import { render } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import api from './redux/apiMiddleware';

const mockStore = configureMockStore([thunk, api]);

jest.mock('./api/api');

global.window.scrollTo = () => true;
global.window.URL.createObjectURL = () => '';
global.navigator.clipboard = {};
global.navigator.clipboard.writeText = jest.fn();

export const customRender = (
	ui,
	options = {
		route: '/',
		initialEntries: ['/'],
		store: {},
	},
) => {
	const store = mockStore({
		profile: {},
		tournSearch: [],
		...options.store,
	});
	const Wrapper = ({ children }) => (
		<Provider store={store}>
			<MemoryRouter initialEntries={options.initialEntries}>
				<Route path={options.route}>
					<div>
						{children}
					</div>
				</Route>
			</MemoryRouter>
		</Provider>
	);
	return render(ui, { wrapper: Wrapper, ...options });
};

export * from '@testing-library/react';

export { customRender as render };

export default null;
