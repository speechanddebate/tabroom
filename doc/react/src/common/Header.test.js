import React from 'react';
import { assert } from 'chai';

import { customRender as render, waitFor, screen } from '../setupTests';

import Header from './Header';

describe('<Header />', () => {
	it('should render the top of page header', async () => {
		render(<Header />);
		await waitFor(() => assert.isOk(document.querySelector('div'), 'Core element is displayed'));
		// await waitFor(() => assert.isOk(screen.queryByText(/test@test.com/), 'email is displayed'));
		assert.isOk(document.querySelector('#logo'), 'Logo element exists');
		assert.isOk(screen.queryByAltText(/Tabroom.com by the National Speech and Debate Association/), 'Tabroom logo alt text displays');
		assert.equal(document.querySelectorAll('div > span').length, 2, 'Two top level spans');
	});
});
