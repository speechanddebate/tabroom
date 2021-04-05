import selectProfile from '../../models/person/selectPerson';

const getProfile = {
    GET: async (req, res) => {
        const result = await selectProfile(req.query.userId);
        if (result.count < 1) {
            return res.status(400).json({ message: 'User does not exist' });
        }
        return res.status(200).json({ message: 'User exists' });
    },
};

getUsername.GET.apiDoc = {
    summary: 'Load the profile data of the logged in user',
    operationId: 'getProfile',
    parameters: false,
    responses: {
        200: {
            description: 'Username exists',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['logins'],
};

export default getUsername;
