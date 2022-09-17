// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { backupTourn, restoreTourn } from '../../../controllers/tourn/data/exportTourn';

export default [
	{ path : '/tourn/{tourn_id}/data/backup', module : backupTourn },
	{ path : '/tourn/{tourn_id}/data/restore', module : restoreTourn },
];
