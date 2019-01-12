
// This might be a horrible idea but for now grunt seems to play way better for
// development with the linter etc while pm2 is better for production and
// getting them to talk with each other is so arcane and undocumented that I'm
// just going to run grunt for dev and pm2 for prod by default because fuck it.
// -CLP

module.exports = {
  apps : [{
    name               : 'tabroom',
    script             : 'bin/tabroom',
	instances          : max,
	autorestart        : true,
	watch              : true,
	max_memory_restart : '3G',
    env: {
   		NODE_ENV: 'production'
    },
    env_production: {
     	NODE_ENV           : 'development',
    }
  }],

  deploy : {
    production : {
      user : 'node',
      host : '',
      ref  : 'origin/master',
      repo : 'git@github.com:repo.git',
      path : '/www/tabroom/api',
      	'post-deploy' : 'npm install && pm2 reload ecosystem.config.js --env production'
    }
  }
};
