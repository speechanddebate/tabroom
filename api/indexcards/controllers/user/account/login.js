import { b64_sha512crypt as crypt } from 'sha512crypt-node';

// This name is currently a misnomer, because this doesn't actually create a session, it just validates the username and password
// Eventually, this should be expanded to create a session and return it
const login = {
	POST: async (req, res) => {
		const db = req.db;

		const person = await db.person.findOne({
			where: { email: req.body.username },
			include : [
				{ model: db.personSetting, as: 'Settings' },
			],
		});

		if (!person || typeof person !== 'object' || !person.id) {
			return res.status(400).json({ error: 'No user found for username' });
		}

		const hash = crypt(req.body.password, person.password);

		if (hash !== person.password) {
			return res.status(400).json({ error: 'Incorrect password' });
		}

		return res.status(200).json({ person_id: person.id, name: `${person.first} ${person.last}` });
	},
};

login.POST.apiDoc = {
	summary: 'Logs in and returns a session object',
	operationId: 'login',
	requestBody: {
		description: 'The username and password to login',
		required: true,
		content: { '*/*': { schema: { $ref: '#/components/schemas/LoginRequest' } } },
	},
	responses: {
		200: {
			description: 'Session',
			content: { '*/*': { schema: { $ref: '#/components/schemas/Session' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['accounts'],
};

export default login;
