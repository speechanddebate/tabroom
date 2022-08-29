import db from '../indexcards/helpers/db';

// eslint-disable-next-line no-undef
afterAll(async () => {
	// Kill any existing database handles here if they don't close automagically
	await db.sequelize.close();
});
