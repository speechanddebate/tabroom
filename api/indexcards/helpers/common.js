
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
	if (typeof targetDate != "object" || Object.prototype.toString.call(targetDate) != '[object Date]') {
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
