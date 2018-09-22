
// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and chapter records are all covered and returned as
// a json object to the caller.

// Because I'm learning node LIKE A BOSS, I'm going to return a promise instead
// of dealing with callback hell.  Or die trying.  

var db = require('../../models');
var BluePromise = require('bluebird');

	var kingdomsKeys = "", 	
		judgesQuery = "", 
		sjQuery = "", 
		studentsQuery = "";

	kingdomsKeys = "select permission.id, permission.tag, "+
		"circuit.id circuitID, circuit.name circuitName, " +
		"region.id regionID, region.name regionName, "+
		"region.circuit as regionCircuit, region.arch regionArch, " + 
		"chapter.id as chapterID, chapter.name chapterName, " + 
		"tourn.id as tournId, tourn.name tournName, tourn.start, tourn.end " + 
		"from permission "+
		"left join chapter on chapter.id = permission.chapter " + 
		"left join circuit on circuit.id = permission.circuit " +
		"left join region on region.id = permission.region " +
		"left join tourn on tourn.id = permission.tourn "+
			" and tourn.start > utc_timestamp() "+
		"where permission.person = ";

	judgesQuery = "select judge.id, judge.first, judge.last, " +
		"category.name categoryName, "+
		"tourn.name tournName, tourn.start tournStart "+
		"from judge, category, tourn "+
		"where tourn.end > utc_timestamp() "+
		"and tourn.id = category.tourn "+
		"and category.id = judge.category "+
		"and judge.person = ";

	sjQuery = "select chapter_judge.id, chapter_judge.first, chapter_judge.last, "+
		"chapter.name chapterName "+
		"from chapter_judge, chapter "+
		"where chapter.id = chapter_judge.chapter "+
		"and chapter_judge.person = ";

	studentsQuery = "select student.id, student.first, student.last, "+
		"chapter.name chapterName "+
		"from student, chapter "+
		"where chapter.id = student.chapter "+
		"and student.person = ";

	module.exports = function(userId, locals, tournId) { 

		// Check if the inputs are valid and sane 

		if (tournId) { 

			return new BluePromise ( function(resolve, reject) { 

				tournQuery = " select permission.id, permission.tag, "+
				" permission.category from permission "+
				" where permission.tourn = "+tournId +
				" and permission.person = "+userId;

				db.sequelize.query(tournQuery).spread(
					function (permissionRows, metadata) { 
						
						var tournPerms;
					
						permissionRows.forEach( function(row) { 
							tournPerms[row.tag] = row;
						});

						resolve(tournPerms);
					}
				);

			});
				

		} else { 
		
			return new BluePromise ( function(resolve, reject) { 

				db.sequelize.query(kingdomsKeys + userId).spread(
					function(permissionRows, metadata) { 

					var perms = {
						circuit       : {},
						region        : {},
						chapter       : {},
						tourn         : {},
						judge         : {},
						chapter_judge : {},
						student       : {}
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
								perms.chapter[row.chapterID] = row;
								break;
							case "chapter_prefs":
								perms.chapter[row.chapterID] = row;
								break;
							case "prefs":
								perms.chapter[row.chapterID] = row;
								break;
							default: 
								if (row.tournName) { 
									if (taken[row.tournId] === undefined) {
										taken[row.tournId] = true;
									}
								}
							break;
						}
					});
					
					db.sequelize.query(judgesQuery + userId).spread(
						function(judgeRows, metadata) { 

						perms.judges = judgeRows;

						db.sequelize.query(sjQuery + userId).spread(
							function(sjRows, metadata) { 

							perms.chapterJudges = sjRows;

							db.sequelize.query(studentsQuery + userId).spread(
								function(studentRows, metadata) { 

								perms.students = studentRows;
								resolve(perms);
							});
						});
					});
				});
			});
		}
	};
