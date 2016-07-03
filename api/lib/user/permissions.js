
// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and school records are all covered and returned as a
// json object to the caller.

// Because I'm learning node LIKE A BOSS, I'm going to return a promise instead
// of dealing with callback hell.  Or die trying.  

var db = require('../../models');
var BluePromise = require('bluebird');

	var kingdomsKeys = "", 	
		judgesQuery = "", 
		sjQuery = "", 
		studentsQuery = "";

	kingdomsKeys = "select permissions.id, permissions.tag, "+
		"circuits.id circuitID, circuits.name circuitName, " +
		"regions.id regionID, regions.name regionName, "+
		"regions.circuit_id as regionCircuit, regions.ncfl_archdiocese regionArch, " + 
		"schools.id as schoolID, schools.name schoolName, " + 
		"tourns.id as tournID, tourns.name tournName, tourns.start, tourns.end " + 
		"from permissions "+
		"left join schools on schools.id = permissions.school_id " + 
		"left join circuits on circuits.id = permissions.circuit_id " +
		"left join regions on regions.id = permissions.region_id " +
		"left join tourns on tourns.id = permissions.tourn_id and tourns.start > utc_timestamp() "+
		"where permissions.person_id = ";

	judgesQuery = "select judge.id, judge.first, judge.last, " +
		"class.name className, "+
		"tourn.name tournName, tourn.start tournStart "+
		"from judges judge, classes class, tourns tourn "+
		"where tourn.end > utc_timestamp() "+
		"and tourn.id = class.tourn_id "+
		"and class.id = judge.class_id "+
		"and judge.person_id = ";

	sjQuery = "select school_judge.id, school_judge.first, school_judge.last, "+
		"school.name schoolName "+
		"from school_judges school_judge, schools school "+
		"where school.id = school_judge.school_id "+
		"and school_judge.person_id = ";

	studentsQuery = "select student.id, student.first, student.last, "+
		"school.name schoolName "+
		"from students student, schools school "+
		"where school.id = student.school_id "+
		"and student.person_id = ";


	module.exports = function (userID) { 

		return new BluePromise ( function(resolve, reject) { 
	
			db.sequelize.query(kingdomsKeys + userID).spread(function(permissionRows, metadata) { 

				var perms = {
					circuit      : {},
					region       : {},
					school       : {},
					tourn        : {},
					judge        : {},
					schoolJudge  : {},
					student      : {}
				},

				taken = [];

				permissionRows.forEach( function(row) { 

					switch (row.tag) { 

						case "circuit":
							perms.circuit[row.circuitID] = row;
							break;
						case "region":
							perms.region[row.regionID] = row;
							break;
						case "chapter":
							perms.school[row.schoolID] = row;
							break;
						case "chapter_prefs":
							perms.school[row.schoolID] = row;
							break;
						case "prefs":
							perms.school[row.schoolID] = row;
							break;
						default: 
							if (row.tournName) { 
								if (taken[row.tournID] === undefined) {
									taken[row.tournID] = true;
								}
							}
						break;
					}
				});
				
				db.sequelize.query(judgesQuery + userID).spread(function(judgeRows, metadata) { 

					perms.judges = judgeRows;

					db.sequelize.query(sjQuery + userID).spread(function(sjRows, metadata) { 

						perms.schoolJudges = sjRows;

						db.sequelize.query(studentsQuery + userID).spread(function(studentRows, metadata) { 

							perms.students = studentRows;
							resolve(perms);
						});
					});
				});
			});
		});
	};
