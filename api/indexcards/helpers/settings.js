import db from './db';
import { errorLogger } from './logger';

export const getSettings = async (model, id, options = {} ) => {

	const replacements = { id };

	let queryLimits = '';

	if (options.skip) {
		queryLimits = ' and setting.tag NOT IN (:tagArray) ';
		replacements.tagArray = options.skip;
	}

	const results = await db.sequelize.query(`
		select
			setting.id, setting.tag, setting.value, setting.value_date, setting.value_text
		from ${model}_setting setting
		where setting.${model} = :id
			${queryLimits}
		order by setting.tag
	`, {
		replacements,
		type: db.sequelize.QueryTypes.SELECT,
	});

	const settings = {};

	for (const setting of results) {
		if (setting.value === 'json') {
			let jsonValue = '';
			try {
				jsonValue = JSON.parse(setting.value_text);
			} catch (err) {
				errorLogger.info(`Unable to parse ${setting.tag} as valid JSON to an object`);
				errorLogger.info(setting.value_text);
			}
			if (jsonValue) {
				settings[setting.tag] = jsonValue;
			}
		} else if (setting.value === 'text') {
			settings[setting.tag] = setting.value_text;
		} else if (setting.value === 'date') {
			settings[setting.tag] = new Date(setting.value_text);
		} else {
			settings[setting.tag] = setting.value;
		}
	}

	return settings;
};

export default getSettings;
