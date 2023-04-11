// Various ways of changing text that are repetitive and best stashed in one
// place.

export const wudcPosition = (n, options) => {
	const input = parseInt(n);

	if (options === 'long') {
		if (input === 1) { return 'Opening Government'; }
		if (input === 2) { return 'Opening Opposition'; }
		if (input === 3) { return 'Closing Government'; }
		if (input === 4) { return 'Closing Opposition'; }
	}

	if (options === 'medium') {
		if (input === 1) { return '1st Gov'; }
		if (input === 2) { return '1st Opp'; }
		if (input === 3) { return '2nd Gov'; }
		if (input === 4) { return '2nd Opp'; }
	}

	if (input === 1) { return '1G'; }
	if (input === 2) { return '1O'; }
	if (input === 3) { return '2G'; }
	if (input === 4) { return '2O'; }
};

export default wudcPosition;
