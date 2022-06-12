import obj from 'objection-swagger';
import models from './models.js';

const { saveSchema } = obj;
const main = async () => {
    saveSchema(models, './schemas');
}

main();
