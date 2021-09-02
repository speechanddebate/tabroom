/// This file directly taken from Hardy's work in the NSDA API.
/// So blame him if it's screwy.

import moment from 'moment-timezone';

const now = new Date();
export const currentMonth = now.getMonth();
export const currentYear = now.getFullYear();
export const previousYear = (now.getFullYear() - 1);
export const nextYear = (now.getFullYear() + 1);
export const startOfYear = currentMonth < 6 ? previousYear : currentYear;
export const endOfYear = currentMonth < 6 ? currentYear : nextYear;

// I have modified this one to be something more like what I use in Tabroom --
// CLP

export const academicYear = (targetDate) => {

	// Without a passed object just assume we are here & now
	if (typeof targetDate !== 'object' || Object.prototype.toString.call(targetDate) !== '[object Date]') {
		targetDate = new Date();
	}

	let startYear = targetDate.getFullYear();

	if (targetDate.getMonth() < 7) {
		startYear--;
	}

	const endYear = startYear + 1;

	return `${startYear}-${endYear}`;
};

export const ordinalize = (n) => {
	const s = ['th', 'st', 'nd', 'rd'];
	const v = n % 100;
	return n + (s[(v - 20) % 10] || s[v] || s[0]);
};

export const escapeCSV = (string, excludeComma) => {
	if (!string) { return `\\N${!excludeComma ? ',' : ''}`; }
	return `"${string
		.replace(new RegExp('"', 'g'), '')
		.replace(new RegExp(/\\/, 'g'), '')
		.trim()}"${!excludeComma ? ',' : ''}`;
};

export const emailValidator = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const condenseDateRange = (start, end) => {
	const startMoment = moment(start || null);
	const endMoment = moment(end || null);

	let dates = '';
	if (startMoment.isValid() && endMoment.isValid()) {
		if (startMoment.isSame(endMoment, 'day')) {
			dates = `${startMoment.format('l')}`;
		} else {
			dates = `${startMoment.local().format('l')} - ${endMoment.local().format('l')}`;
		}
	}
	return dates;
};

export const showDateTime = (
	sqlDT,
	options = { locale: 'en-us', tz: 'UTC' }
) => {

	// Split timestamp into [ Y, M, D, h, m, s ]
	let dt;

	if (typeof sqlDT === 'string') {
		const dtString = sqlDT.split(/[- :]/);
		// Apply each element to the Date function
		dt = new Date(Date.UTC(
			dtString[0],
			dtString[1] - 1,
			dtString[2],
			dtString[3],
			dtString[4],
			dtString[5]
		));
	} else {
		dt = sqlDT;
	}

	let dateFormat = {};
	let chopped = {};

	switch (options.format) {

	case 'full':
		dateFormat = {
			timeZone     : options.tz,
			weekday      : 'long',
			year         : 'numeric',
			month        : 'long',
			day          : 'numeric',
			hour         : 'numeric',
			minute       : 'numeric',
			timeZoneName : 'short',
		};
		break;

	case 'long':
		dateFormat = {
			timeZone     : options.tz,
			weekday      : 'short',
			month        : 'long',
			year         : 'numeric',
			day          : 'numeric',
			hour         : 'numeric',
			minute       : 'numeric',
			timeZoneName : 'short',
		};
		break;

	case 'medium':
		dateFormat = {
			timeZone     : options.tz,
			month        : 'short',
			day          : 'numeric',
			hour         : 'numeric',
			minute       : 'numeric',
			timeZoneName : 'short',
		};
		break;

	case 'short':
		dateFormat = {
			timeZone     : options.tz,
			month        : 'numeric',
			day          : 'numeric',
			hour         : 'numeric',
			minute       : 'numeric',
		};
		break;

	case 'daytime':
		dateFormat = {
			timeZone : options.tz,
			weekday  : 'short',
			hour     : 'numeric',
			minute   : 'numeric',
		};
		break;

	case 'sortable':
		chopped = new Intl.DateTimeFormat(
			options.locale,
			{
				timeZone     : options.tz,
				month        : 'numeric',
				day          : 'numeric',
				hour         : 'numeric',
				minute       : 'numeric',
				hour12 		 : true,
			}
		)
			.formatToParts(dt)
			.map(component => {
				return component.reduce( (dtString, comp) => {
					if (comp.type !== 'literal') {
						dtString[comp.type] = comp.value;
					}
					return comp.value;
				});
			});

		return `${chopped.year}-${chopped.month}-${chopped.day} ${chopped.hour}-${chopped.minute}`;

	default:
		return dt;
	}

	if (options.dateOnly) {
		delete dateFormat.hour;
		delete dateFormat.minute;
		delete dateFormat.timeZoneName;
	}

	if (options.timeOnly) {
		delete dateFormat.year;
		delete dateFormat.month;
		delete dateFormat.day;
		delete dateFormat.timeZoneName;
	}

	return new Intl.DateTimeFormat(
		options.locale,
		dateFormat
	).format(dt);
};
