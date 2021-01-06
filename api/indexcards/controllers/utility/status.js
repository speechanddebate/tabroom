const status = {
    GET: async (req, res) => {
        return res.status(200).json({ message: 'OK' });
    },
    POST: async (req, res) => {
        return res.status(200).json({ message: 'OK' });
    },
    PUT: async (req, res) => {
        return res.status(200).json({ message: 'OK' });
    },
    PATCH: async (req, res) => {
        return res.status(200).json({ message: 'OK' });
    },
    DELETE: async (req, res) => {
        return res.status(200).json({ message: 'OK' });
    },
};

status.GET.apiDoc = {
    summary: 'Responds with a 200 if up',
    operationId: 'getStatus',
    responses: {
        200: {
            description: 'Server is up',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['status'],
};

status.POST.apiDoc = {
    summary: 'Responds with a 200 if up',
    operationId: 'postStatus',
    responses: {
        200: {
            description: 'Server is up',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['status'],
};

status.PUT.apiDoc = {
    summary: 'Responds with a 200 if up',
    operationId: 'putStatus',
    responses: {
        200: {
            description: 'Server is up',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['status'],
};

status.PATCH.apiDoc = {
    summary: 'Responds with a 200 if up',
    operationId: 'patchStatus',
    responses: {
        200: {
            description: 'Server is up',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['status'],
};

status.DELETE.apiDoc = {
    summary: 'Responds with a 200 if up',
    operationId: 'deleteStatus',
    responses: {
        200: {
            description: 'Server is up',
            content: { '*/*': { schema: { type: 'string' } } },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['status'],
};

export default status;
