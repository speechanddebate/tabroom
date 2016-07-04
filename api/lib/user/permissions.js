
// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and chapter records are all covered and returned as a
// json object to the caller.

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
		"region.circuit_id as regionCircuit, region.archdiocese regionArch, " + 
		"chapter.id as chapterID, chapter.name chapterName, " + 
		"tourn.id as tournID, tourn.name tournName, tourn.start, tourn.end " + 
		"from permission "+
		"left join chapter on chapter.id = permission.chapter_id " + 
		"left join circuit on circuit.id = permission.circuit_id " +
		"left join region on region.id = permission.region_id " +
		"left join tourn on tourn.id = permission.tourn_id "+
			" and tourn.start > utc_timestamp() "+
		"where permission.person_id = ";

	judgesQuery = "select judge.id, judge.first, judge.last, " +
		"category.name categoryName, "+
		"tourn.name tournName, tourn.start tournStart "+
		"from judge, category, tourn "+
		"where tourn.end > utc_timestamp() "+
		"and tourn.id = category.tourn_id "+
		"and category.id = judge.category_id "+
		"and judge.person_id = ";

	sjQuery = "select chapter_judge.id, chapter_judge.first, chapter_judge.last, "+
		"chapter.name chapterName "+
		"from chapter_judge, chapter "+
		"where chapter.id = chapter_judge.chapter_id "+
		"and chapter_judge.person_id = ";

	studentsQuery = "select student.id, student.first, student.last, "+
		"chapter.name chapterName "+
		"from student, chapter "+
		"where chapter.id = student.chapter_id "+
		"and student.person_id = ";

	module.exports = function(userID, locals) { 

		return new BluePromise ( function(resolve, reject) { 

			console.log(locals);

			db.sequelize.query(kingdomsKeys + userID).spread(
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
								if (taken[row.tournID] === undefined) {
									taken[row.tournID] = true;
								}
							}
						break;
					}
				});
				
				db.sequelize.query(judgesQuery + userID).spread(
					function(judgeRows, metadata) { 

					perms.judges = judgeRows;

					db.sequelize.query(sjQuery + userID).spread(
						function(sjRows, metadata) { 

						perms.chapterJudges = sjRows;

						db.sequelize.query(studentsQuery + userID).spread(
							function(studentRows, metadata) { 

							perms.students = studentRows;
							resolve(perms);
						});
					});
				});
			});
		});
	};
