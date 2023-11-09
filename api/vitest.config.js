// eslint-disable-next-line import/no-unresolved
import { defineConfig } from 'vitest/config';

export default defineConfig({
	test: {
		threads: true,
		globals: true,
		globalSetup: './tests/globalTestSetup.js',
		coverage: {
			reporter: ['text', 'html'],
			exclude: [
				'node_modules/',
			],
		},
	},
});
