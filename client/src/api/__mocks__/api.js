// Jest will only use mock implementations defined here if resetMocks: false is set in package.json
// Otherwise, you have to copy the mockResolveValue to each test and define the response on the imported mock
// https://github.com/facebook/jest/issues/10894
// https://github.com/facebook/create-react-app/issues/9935
export const getProfile = jest.fn().mockResolvedValue({ id: 1, email: 'test@test.com' });
export const postProfile = jest.fn().mockResolvedValue({ message: 'Successfully updated profile' });
export const tournSearch = jest.fn().mockResolvedValue([{ id: 1 }]);

export default null;
