import React from 'react';
import { assert } from 'chai';
import { render } from '@testing-library/react';

import App from './App';

describe('App', () => {
	it('Renders the app without crashing', () => {
		const { container } = render(<App />);
		assert.strictEqual(container.tagName.toLowerCase(), 'div', 'Renders the root div');
	});
});
