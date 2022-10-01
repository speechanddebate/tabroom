import db from '../indexcards/helpers/db';
import userData from './testFixtures';

const globalTestTeardown = async () => {
	const testUser = await db.person.findByPk(userData.testUser.id);
	await testUser.destroy();

	const testAdmin = await db.person.findByPk(userData.testAdmin.id);
	await testAdmin.destroy();

	const testUserSession = await db.session.findByPk(userData.testUserSession.id);
	await testUserSession.destroy();

	const testAdminSession = await db.session.findByPk(userData.testAdminSession.id);
	await testAdminSession.destroy();
	await db.sequelize.query('delete from campus_log where person > 3 and person < 100');
};

export default globalTestTeardown;
