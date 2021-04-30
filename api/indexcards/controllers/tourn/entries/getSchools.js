import db from '../../../models';

const getSchools = {
    GET: async (req, res) => {
		console.log("Getting schools for "+req.params.tourn_id);

        const result = await res.locals.db.permission.findAll({
			where: {tourn: req.params.tourn_id, person: req.session.person}
		});

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
