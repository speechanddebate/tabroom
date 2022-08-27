/* istanbul disable file */
import React from 'react';
import { Provider } from 'react-redux';
import '@testing-library/jest-dom';
import { render } from '@testing-library/react';
import store from './redux/store';
// import { MemoryRouter } from 'react-router-dom';
// import { ToastContainer } from 'react-toastify';

// import { ProvideAuth } from './helpers/auth';
// import { ProvideStore } from './helpers/store';

// jest.mock('./helpers/auth');
// jest.mock('./helpers/store');
// jest.mock('./helpers/api');
// jest.mock('./helpers/useScript');

global.window.scrollTo = () => true;
global.window.URL.createObjectURL = () => '';
global.navigator.clipboard = {};
global.navigator.clipboard.writeText = jest.fn();

export const wrappedRender = (component) => {
	return render(
		// <ProvideAuth>
		// <MemoryRouter initialEntries={['/?q=test']}>
		<Provider store={store}>
			{component}
		</Provider>
		// </MemoryRouter>
		// <ToastContainer />
		// </ProvideAuth>
	);
};

export * from '@testing-library/react';

export default null;
