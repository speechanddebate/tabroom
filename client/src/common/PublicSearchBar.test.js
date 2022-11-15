import React from 'react';
import { assert } from 'chai';
import { customRender as render, screen, fireEvent, waitFor } from '../setupTests';
// eslint-disable-next-line import/named
import PublicSearchBar from './PublicSearchBar';

describe('PublicSearchBar', () => {
	it('Renders a searchbar', async () => {
		render(<PublicSearchBar />);

		await waitFor(() => screen.queryByText(/Search/));
		await waitFor(() => screen.queryByPlaceholderText(/Search Tournaments/));
		assert.isOk(document.querySelector('button'), 'Button exists');
	});

	it('displays empty results if searching for not found', async () => {
		render(<PublicSearchBar />);
		await waitFor(() => screen.queryByPlaceholderText(/Search Tournaments/));
		fireEvent.change(screen.getByPlaceholderText('Search Tournaments'), { target: { value: 'missing' } });
		const button = document.querySelector('button');
		fireEvent.click(button);

		// This is how you test keydown
		// fireEvent.keyDown(document, { key: 'Escape', code: 'Escape', charCode: 27 });
		// assert.equal(document.querySelector('#searchtext').value, '', 'Clears the input');
	});
});
