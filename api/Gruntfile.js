// gruntfile to build CSS and launch nodemon for Treo

module.exports = function(grunt) {

	grunt.initConfig({

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
			all: [
			] 
		},

		uglify: { 

			options: {
				sourceMap: true 
			},

			build: { 
				files : {
					'static/js/treo.min.js': [
					], 
					'source/vendor/vendor.min.js' : [
					]
				}
			}
		},

		concat: { 
			js : { 
				src : [ 
				],
				dest: "static/js/treo-vendor.js"
			},

			debugjs : { 
				src : [ 
				],
				dest: "static/js/treo-debug.js"
			}
		},

		sass: {
			dist: {
				files : [{ 
				}],
			}
		},

		cssmin: { 
			build: {
				files: { 
					'static/css/treo-vendor.css': [ 
					],
					'static/css/treo-local.css': [ 
					]
				}
			}
		},

		watch: {
			css: {
				files: ['source/css/*.css'],
				tasks: ['cssmin']
			},
			js: {
				files: ['source/js/*.js'],
				tasks: ['jshint', 'jslint', 'uglify', 'concat:debugjs']
			}
		},


		// Launch nodemon script for the Express instance
		nodemon: {
			dev: { script: 'bin/www' }
		},

		concurrent: {
			options: {
				logConcurrentOutput: true
			},
			tasks: ['nodemon', 'watch']
		}	 

	});

	// load nodemon
	grunt.loadNpmTasks('grunt-jslint');
	grunt.loadNpmTasks('grunt-contrib-concat');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-contrib-uglify');
	grunt.loadNpmTasks('grunt-contrib-cssmin');
	grunt.loadNpmTasks('grunt-contrib-sass');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-nodemon');
	grunt.loadNpmTasks('grunt-concurrent');

	// register the nodemon task when we run grunt
	grunt.registerTask('default', ['jslint', 'jshint', 'uglify', 'concat', 'concurrent']);

};

