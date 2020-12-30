const ErrorResponse = {
    description: 'Error',
    content: { '*/*': { schema: { $ref: '#/components/schemas/Err' } } },
};

export default ErrorResponse;
