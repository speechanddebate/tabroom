// gruntfile to build CSS and launch nodemon for Treo

module.exports = function(grunt) {

	grunt.initConfig({
		env : {
			options : {
				//Shared Options Hash
			},
			dev  : { NODE_ENV : 'development', },
			prod : { NODE_ENV : 'production', },
		},

		jslint: {
			// lint the server code
			server: {
				// files to lint
				src: [
					'app.js',
					'routes/*.js'
				],
				// files to exclude
				exclude: [
					'config/config.js',
				],
				// lint options
				directives: {
					node    : true, 	// node environment
					browser : false, 	// browser environment
					nomen   : true, 	// allow dangling underscores
					todo    : true, 	// allow todo statements
					unparam : true, 	// allow unused parameters
					sloppy  : true,		// don't require strict pragma
					white   : true 		// allow whitespace discrepencies
				}
			},			
		},			

		jshint: {
			all: [ ] 
		},

		// Launch nodemon script for the Express instance
		nodemon: {
			dev: { script: 'bin/tabroom' }
		},

		concurrent: {
			options: { logConcurrentOutput: true },
			tasks: ['nodemon']
		}	 
	});

	// load nodemon
	grunt.loadNpmTasks('grunt-jslint');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-env');
	grunt.loadNpmTasks('grunt-nodemon');
	grunt.loadNpmTasks('grunt-concurrent');

	// register the nodemon task when we run grunt
	grunt.registerTask('default', ['concurrent']);

};

