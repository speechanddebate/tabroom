import db from '../../../models/';

const getInviteByWebname = {

    GET: async (req, res) => {
        const invite = await db.getInvite(req.params.webname);
    	return res.status(200).json(invite);
    },
};

getInviteByWebname.GET.apiDoc = {
    summary: 'Returns the public pages for a tournament',
    operationId: 'getInviteByWebname',
    parameters: [
        {
            in: 'path',
            name: 'webname',
            description: 'Public webname of the tournament to return',
            required: true,
            schema: { type: 'string', minimum: 1 },
        },
    ],
    responses: {
        200: {
            description: 'Invitationl & General Tournament Info',
            content: { '*/*': { schema: { $ref: '#/components/schemas/Invite' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['invite', 'public'],
};

export default getInviteByWebname;
