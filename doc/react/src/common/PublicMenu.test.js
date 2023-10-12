import React from 'react';
import { render, waitFor, screen } from '@testing-library/react';
import { assert } from 'chai';

import PublicMenu from './PublicMenu';

describe('<PublicMenu />', () => {
	it('should render a list of tabs', async () => {
		render(<PublicMenu />);

		await waitFor(() => assert.isOk(document.querySelector('ul'), 'UL exists'));

		assert.isOk(document.querySelector('#navmenu'), 'Nav Menu dom element');
		assert.isOk(screen.queryByText(/Results/), 'Results tab');
		assert.equal(document.querySelectorAll('ul > li').length, 6, 'Six children menus exist');
		assert.equal(document.querySelectorAll('.top_link').length, 6, 'Six menu links exist');
		assert.isOk(document.querySelector(`a[href="${process.env.REACT_APP_LEGACY_URL}/index/paradigm.mhtml"]`), 'Paradigm link exists');
	});
});
