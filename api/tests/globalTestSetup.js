import db from '../indexcards/helpers/db';
import testData from './testFixtures';

export const setup = async () => {
	await db.sequelize.query( `delete from session where person > 3 and person < 100 ` );
	await db.sequelize.query( `delete from campus_log where id < 100`);
	await db.sequelize.query( `delete from campus_log where person > 3 and person < 100` );
	await db.sequelize.query( `delete from person where id > 3 and id < 100` );

	await db.person.create(testData.testUser);
	await db.session.create(testData.testUserSession);
	await db.person.create(testData.testAdmin);
	await db.session.create(testData.testAdminSession);
	await db.person.bulkCreate(testData.testCampusUsers);

	// Campus logs test data is failing because of foreign key constraints with missing tournament events
	// Will likely not matter if we plan to use a real production database for tests,
	// so just disabling associated tests of the status board for now
	// await db.campusLog.bulkCreate(testData.testCampusLogs);
	await db.permission.create(testData.testUserTournPerm);
};

export const teardown = async () => {
	try {
		const testUser = await db.person.findByPk(testData.testUser.id);
		if (testUser) {
			await testUser.destroy();
		}

		const testAdmin = await db.person.findByPk(testData.testAdmin.id);
		if (testAdmin) {
			await testAdmin.destroy();
		}

		const testUserSession = await db.session.findByPk(testData.testUserSession.id);
		if (testUserSession) {
			await testUserSession.destroy();
		}

		const testAdminSession = await db.session.findByPk(testData.testAdminSession.id);
		if (testAdminSession) {
			await testAdminSession.destroy();
		}
		await db.sequelize.query('delete from campus_log where person > 3 and person < 100');

		await db.sequelize.close();
	} catch (err) {
		console.log(err);
	}
};
