export const objectify = (array, key) => {
	const dest = {};
	if (!key) {
		key = 'id';
	}
	array.forEach( (item) => {
		if (item.dataValues) {
			const output = item.get({ plain:true });
			const tag = output[key];
			dest[tag] = stripNull(output);
			delete dest[tag][key];
		} else {
			const tag = item[key];
			dest[tag] = stripNull(item);
			delete dest[tag][key];
		}

		Object.keys(item).forEach( (tag) => {
			if (item[tag] === null) {
				delete item[tag];
			}
		});
	});

	return dest;
};

export const multiObjectify = (array) => {

	const dest = {};
	array.forEach( (item) => {
		if (item.dataValues) {
			const output = item.get({ plain:true });
			if (!dest[output.id]) {
				dest[output.id] = [];
			}
			dest[output.id].push(stripNull(output));
		} else {
			if (!dest[item.id]) {
				dest[item.id] = [];
			}
			dest[item.id].push(stripNull(item));
		}
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

export const objectifyGroupSettings = (array, targetKey, dest) => {

	array.forEach( (setting) => {
		const target = setting[targetKey];
		if (!dest[target].settings) {
			dest[target].settings = {};
		}
		if (setting.value === 'json') {
			if (setting.value_text) {
				dest[target].settings[setting.tag] = {};
				dest[target].settings[setting.tag].json = JSON.parse(setting.value_text);
			}
		} else if (setting.value === 'date') {
			if (setting.value_date) {
				dest[target].settings[setting.tag] = { date: setting.value_date };
			}
		} else if (setting.value === 'text') {
			if (setting.value_text) {
				dest[target].settings[setting.tag] = { text: setting.value_text };
			}
		} else {
			if (setting.value) {
				dest[target].settings[setting.tag] = { value: setting.value };
			}
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

export const objectStrip = (target, stripme, booleans) => {

	stripme?.forEach( (strip) => {
		delete target[strip];
	});

	booleans?.forEach( (boolean) => {
		if (
			!target[boolean]
			|| target[boolean] === 0
			|| target[boolean] === false
		) {
			delete target[boolean];
		} else {
			target[boolean] = true;
		}
	});

	return stripNull(target);
};

const stripNull = (target) => {
	Object.keys(target).forEach( (tag) => {
		if (
			target[tag] === null
			|| target[tag] === 0
		) {
			delete target[tag];
		}
	});
	return target;
};

export default objectify;
