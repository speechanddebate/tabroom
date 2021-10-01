
const Reader = require('@maxmind/geoip2-node').Reader;
import config from '../../../../config/config';

const ipLocation = {

	GET: async (req, res) => {

		const options = {
			// you can use options like `cache` or `watchForUpdates`
		};

		Reader.open(config.IPLOCATION, options).then(async reader => {
			const locationData = await reader.city(req.params.ip_address);

			let returnData = { 
				country       : locationData.country.names.en,
				countryCode   : locationData.country.isoCode,
				countryCode   : locationData.country.isoCode,
				continent     : locationData.continent.names.en,
				continentCode : locationData.continent.isoCode,
				city          : locationData.city.names.en,
				isEU          : locationData.registeredCountry.isInEuropeanUnion,
				latitude      : locationData.location.latitude,
				longitude     : locationData.location.longitude,
				timeZone      : locationData.location.timeZone,
				postal        : locationData.postal.code
			};

			if (locationData.subdivisions) { 
				returnData.state = locationData.subdivisions[0].isoCode;
			}
			res.json(returnData);
		});
	},
};

export default ipLocation;