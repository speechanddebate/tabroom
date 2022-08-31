// node --experimental-specifier-resolution=node -e 'import("./indexcards/controllers/share/generateTestEmails").then(m => m.init())' 10
import fs from 'fs';
import { randomPhrase } from '@speechanddebate/nsda-js-utils';
import sendMail from './mail';
import { debugLogger } from '../../helpers/logger';

const generateTestEmails = async (numberOfEmails = parseInt(process.argv[1]) || 10) => {
	const buffer = fs.readFileSync('./indexcards/controllers/share/test.docx');
	const base64 = buffer.toString('base64');
	try {
		for (let i = 0; i < numberOfEmails; i++) {
			const phrase = randomPhrase();
			// eslint-disable-next-line no-await-in-loop
			await sendMail(
				`noreply@share.tabroom.com`,
				`${phrase}@share.tabroom.com`,
				`Test email ${phrase}`,
				`Test email ${phrase}`,
				null,
				[{ filename: 'Test.docx', file: base64 }],
			);
		}
	} catch (err) {
		debugLogger.error(err);
	}
	return true;
};

export const init = () => {
	return generateTestEmails(parseInt(process.argv[1]) || 10);
};

export default generateTestEmails;
