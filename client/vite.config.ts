import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vitest/config';

export default defineConfig({
	plugins: [sveltekit()],
	test: {
		include: ['src/**/*.{test,spec}.{js,ts}']
	},
	headers: {
       'Access-Control-Allow-Origin': 'http://local.tabroom.com',
       'Access-Control-Allow-Headers': 'http://local.tabroom.com:3000',
   }

});
