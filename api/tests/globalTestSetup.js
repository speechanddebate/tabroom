import db from '../indexcards/helpers/db';
import userData from './testFixtures';

const globalTestSetup = async () => {

	await db.sequelize.query( `delete from session where person > 3 and person < 100 ` );
	await db.sequelize.query( `delete from campus_log where id < 100`);
	await db.sequelize.query( `delete from campus_log where person > 3 and person < 100` );
	await db.sequelize.query( `delete from person where id > 3 and id < 100` );

	await db.person.create(userData.testUser);
	await db.session.create(userData.testUserSession);
	await db.person.create(userData.testAdmin);
	await db.session.create(userData.testAdminSession);
	await db.person.bulkCreate(userData.testCampusUsers);

	await db.campusLog.bulkCreate(userData.testCampusLogs);
	await db.permission.create(userData.testUserTournPerm);
};

export default globalTestSetup;
