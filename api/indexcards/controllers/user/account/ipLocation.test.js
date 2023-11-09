import request from 'supertest';
import { assert } from 'chai';
import { vi } from 'vitest';
import server from '../../../../app';

vi.mock('@maxmind/geoip2-node', () => ({
	Reader: {
		open: vi.fn().mockResolvedValue({
			city: vi.fn().mockResolvedValue({
				country: { names: { en: 'Mock Country' }, isoCode: 'MC' },
				continent: { names: { en: 'Mock Continent' }, isoCode: 'MC' },
				city: { names: { en: 'Mock City' } },
				registeredCountry: { isInEuropeanUnion: false },
				location: { latitude: 0, longitude: 0, timeZone: 'Mock/TimeZone' },
				postal: { code: 'M0C K0D' },
				subdivisions: [{ isoCode: 'MC' }],
			}),
			isp: vi.fn().mockResolvedValue({
				isp: 'Mock ISP',
				organization: 'Mock Organization',
			}),
		}),
	},
}));

describe('IP Location Results', () => {
	it('Returns correct location data', async () => {
		const res = await request(server)
			.get(`/v1/user/iplocation/1.1.1.1`)
			.set('Accept', 'application/json')
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body.country,
			'Mock Country',
			'Correct location'
		);

		assert.isFalse(
			res.body.isEU,
			'Correct EU status'
		);
	});
});
