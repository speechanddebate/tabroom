import { tourns } from './stores';

/** @type {import('./$types').PageLoad} */
export const load = async () => {

    const response = await fetch(`${import.meta.env.VITE_API_URL}/invite/upcoming`);
    const rawData = await response.json();

	interface Tourn {
		url       : string,
		webname   : string,
		id        : number,
		dates     : string,
		state     : string,
		regStatus : string,
		location  : string,
		tz        : string,
		tzHover   : string,
		start     : Date,
		end       : Date,
		reg_start : Date,
		reg_end   : Date,
	};

	const data = rawData.flatMap( (tourn: Tourn) => {

		if (tourn.webname) {
			tourn.url = `${import.meta.env.VITE_WEB_URL}/t/${tourn.webname}`;
		} else {
			tourn.url = `${import.meta.env.VITE_WEB_URL}/tid/${tourn.id}`;
		}

		tourn.start = new Date(tourn.start);
		tourn.end = new Date(tourn.end);

		tourn.dates = `${tourn.start.toLocaleDateString('en-us', {month: "numeric", day:"numeric"})}`;

		if (tourn.start.getDay() !== tourn.end.getDay()) {
			tourn.dates += `-${tourn.end.toLocaleDateString('en-us', {month: "numeric", day:"numeric"})}`;
		}

		if (
			tourn.location.toLowerCase() === 'online'
			|| tourn.location.toLowerCase() === 'nsda campus'
		) {

			tourn.tzHover = tourn.tz;

			const tz = new Date().toLocaleDateString('en-US', {
				timeZoneName: 'short',
				timeZone: tourn.tz,
			}).slice(10);

			tourn.state = tz;
		}

		return tourn;
	});

    tourns.update(() => data);
    return {
        tourns: data,
    };
};
