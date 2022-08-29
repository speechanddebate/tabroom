import React from 'react';
import { assert } from 'chai';
import { wrappedRender as render } from './setupTests';

import App from './App';

describe('App', () => {
	it('Renders the app without crashing', () => {
		const { container } = render(<App />);
		assert.strictEqual(container.tagName.toLowerCase(), 'div', 'Renders the root div');
	});
});
