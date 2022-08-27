import db from '../indexcards/models/index.cjs';
import userData from './testFixtures.js';

const globalTestSetup = async () => {
	await db.person.create(userData.testUser);
	await db.session.create(userData.testUserSession);
	await db.person.create(userData.testAdmin);
	await db.session.create(userData.testAdminSession);

	userData.testCampusLogs.forEach(async (cl) => {
		await db.campusLog.create(cl);
	});
	//	await db.campusLog.bulkCreate(userData.testCampusLogs);
	await db.permission.create(userData.testUserTournPerm);
};

export default globalTestSetup;
