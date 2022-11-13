import React from 'react';
import { assert } from 'chai';
import { customRender as render, screen, fireEvent, waitFor } from '../setupTests';
// eslint-disable-next-line import/named
import SearchBar from './SearchBar';

describe('SearchBar', () => {
	it('Renders a sidebar', async () => {
		render(<SearchBar />);

		await waitFor(() => screen.queryByText(/Search/));
		assert.isOk(document.querySelector('button'), 'Button exists');
		const button = document.querySelector('button');
		fireEvent.click(button);
	});

	// it.skip('finds a search thing', async () => {
	// 	render(<SearchBar />, { store: { tournSearch: [] } });
	// 	assert.isOk(document.querySelector('results'));
	// });

	// it.skip('Runs a search when clicking the button', async () => {
	// 	render(<SearchBar />);

	// 	await waitFor(() => screen.queryByText(/Search/));
	// 	const button = screen.queryByText(/Search/);
	// 	assert.isNotOk(document.querySelector('.results'));
	// 	fireEvent.click(button);
	// 	assert.isOk(document.querySelector('.results'));
	// });
});
