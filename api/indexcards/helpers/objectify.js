export const objectify = (array) => {
	const dest = {};
	array.forEach( (item) => {
		dest[item.id] = item;
		delete item.id;
	});
	return dest;
};

export const arrayify = (destroyMe, key) => {
	const dest = [];
	destroyMe.forEach( (individual) => {
		dest.push(individual[key]);
	});
	return dest;
};

export const objectifyGroupSettings = (array, targetKey) => {

	const dest = {};

	array.forEach( (setting) => {
		const target = setting[targetKey];
		if (!dest[target]) {
			dest[target] = {};
		}
		if (setting.value === 'json') {
			dest[target][setting.tag] = {};
			dest[target][setting.tag].json = JSON.parse(setting.value_text);
		} else if (setting.value === 'date') {
			dest[target][setting.tag] = { date: setting.value_date };
		} else if (setting.value === 'text') {
			dest[target][setting.tag] = { text: setting.value_text };
		} else {
			dest[target][setting.tag] = { value: setting.value };
		}
	});
	return dest;
};

export const objectifySettings = (array) => {

	const dest = {};

	array.forEach( (setting) => {
		if (setting.value === 'json') {
			dest[setting.tag] = {};
			dest[setting.tag].json = JSON.parse(setting.value_text);
		} else if (setting.value === 'date') {
			dest[setting.tag] = { date: setting.value_date };
		} else if (setting.value === 'text') {
			dest[setting.tag] = { text: setting.value_text };
		} else {
			dest[setting.tag] = { value: setting.value };
		}
	});
	return dest;
};

export const objectStrip = (target, stripme) => {

	stripme?.forEach( (strip) => {
		delete target[strip];
	});

	Object.keys(target).forEach( (tag) => {
		if (target[tag] === null) {
			delete target[tag];
		}
	});

	return target;
};

export default objectify;
