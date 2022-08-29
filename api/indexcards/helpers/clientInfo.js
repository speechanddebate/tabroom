import { Reader } from '@maxmind/geoip2-node';
import config from '../../config/config';

export const findLocation = async (ipAddress) => {

	if (
		!ipAddress
		|| ipAddress.startsWith('192.168')
		|| ipAddress.startsWith('172.16')
		|| ipAddress.startsWith('172.17')
		|| ipAddress.startsWith('10')
		|| ipAddress.startsWith('127')
	) {
		return;
	}

	const options = {
		// you can use options like `cache` or `watchForUpdates`
	};

	const reader = await Reader.open(config.IPLOCATION, options);
	const locationDB = await reader.city(ipAddress);

	const ispLocation = {
		country       : locationDB.country?.names?.en,
		countryCode   : locationDB.country.isoCode,
		continent     : locationDB.continent?.names?.en,
		continentCode : locationDB.continent.isoCode,
		city          : locationDB.city?.names?.en,
		isEU          : locationDB.registeredCountry?.isInEuropeanUnion,
		latitude      : locationDB.location?.latitude,
		longitude     : locationDB.location?.longitude,
		timeZone      : locationDB.location?.timeZone,
		postal        : locationDB.postal?.code,
	};

	if (locationDB.subdivisions) {
		ispLocation.state = locationDB.subdivisions[0].isoCode;
	}

	return ispLocation;
};

export const findISP = async (ipAddress) => {

	if (
		!ipAddress
		|| ipAddress.startsWith('192.168')
		|| ipAddress.startsWith('172.16')
		|| ipAddress.startsWith('172.17')
		|| ipAddress.startsWith('10')
		|| ipAddress.startsWith('127')
	) {
		return;
	}

	const options = {
		// you can use options like `cache` or `watchForUpdates`
	};

	const reader = await Reader.open(config.IPISP, options);
	const ispDB = await reader.isp(ipAddress);

	const ispData = {
		isp          : ispDB.isp,
		organization : ispDB.organization,
	};

	return ispData;
};
