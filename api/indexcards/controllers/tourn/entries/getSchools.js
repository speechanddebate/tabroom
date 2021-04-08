import db from '../../../models';

const getSchools = {
    GET: async (req, res) => {
        const result = await db.Person.findAll({ where: {tourn: req.params.tourn_id}});

        if (result.count < 1) {
            return res.status(400).json({ message: 'No schools found' });
        }
        return res.status(200).json(result);
    },
};

getSchools.GET.apiDoc = {
    summary: 'Listing of schools in the tournament',
    operationId: 'getSchools',
    parameters: [
        {
            in          : 'path',
            name        : 'tourn_id',
            description : 'Tournament ID',
            required    : true,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },
    ],
    responses: {
        200: {
            description: 'School Data',
            content: {
                '*/*': {
                    schema: {
                        type: 'array',
                        items: { $ref: '#/components/schemas/School' },
                    },
                },
            },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['tournament/entries'],
};

export default getSchools;
