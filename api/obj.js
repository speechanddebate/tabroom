require('dotenv').config();
const models = require('objection-model-generator/src/dbToModel');

const main = async () => {
  const connection = {
    host: process.env.DB_HOST || 'localhost',
    port: process.env.DB_PORT || '3306',
    user: process.env.DB_USER || 'root',
    password: process.env.DB_PASS || ''
  };
  const modelsPromise = models('tabroom', connection, 'knex', './models.js');
  const all = await Promise.all([modelsPromise]);
}

main();
