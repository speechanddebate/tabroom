
// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and chapter records are all covered and returned as
// a json object to the caller.

// Because I'm learning node LIKE A BOSS, I'm going to return a promise instead
// of dealing with callback hell.  Or die trying.  

const db = require('../../models');
const Promise = require('bluebird');

	const kingdomsKeys = `select permission.id, permission.tag, 
			circuit.id circuitID, circuit.name circuitName, 
			region.id regionID, region.name regionName, 
			region.circuit as regionCircuit, region.arch regionArch,  
			chapter.id as chapterID, chapter.name chapterName,  
			tourn.id as tournID, tourn.name tournName, tourn.start, tourn.end  
		from permission 
			left join chapter on chapter.id = permission.chapter  
			left join circuit on circuit.id = permission.circuit 
			left join region on region.id = permission.region 
			left join tourn on tourn.id = permission.tourn 
		    and tourn.start > utc_timestamp() 
		where permission.person = ? `;

	const judgesQuery = `select judge.id, judge.first, judge.last, 
		category.name categoryName, 
		tourn.name tournName, tourn.start tournStart 
		from judge, category, tourn 
		where tourn.end > utc_timestamp() 
		and tourn.id = category.tourn 
		and category.id = judge.category 
		and judge.person = ? `;

	const sjQuery = `select chapter_judge.id, chapter_judge.first, chapter_judge.last, 
		chapter.name chapterName 
		from chapter_judge, chapter 
		where chapter.id = chapter_judge.chapter 
		and chapter_judge.person = ? `;

	const studentsQuery = 
		` select student.id, student.first, student.last, 
		chapter.name chapterName 
		from student, chapter 
		where chapter.id = student.chapter 
		and student.person = ?`;

	module.exports = function(userID, locals, tournID) { 

		// Check if the inputs are valid and sane 

		if (userID) { 

			if (tournID) { 

				return new Promise ( function(resolve, reject) { 

					db.tourn.findById(tournID, 
						{include: ["Events"]}
					).then(function(Tourn) { 

						db.permission.findAll({ 
							where: { person: userID, tourn: tournID },
							include: ["Category", "Person"] 
						}).then(function(Permissions) { 

							let tournPerms = {};

							Tourn.Events.forEach(function(Event) { 
								allEvents[Event.id] = "admin";
								categoryEvents[Event.category] = {} 
								categoryEvents[Event.category][Event.id] = "admin";
							});

							if (Permissions.length == 0) { 

							} else { 

								Permissions.forEach(function(perm) {
										
									let allEvents = {};
									let categoryEvents = {};
									tournPerms.events = {};


									console.log("perm object is");
									console.log(perm);
									console.log("Person object is");
									console.log(perm.Person);

									if (perm.Person.site_admin) { 

										tournPerms.owner        = true;
										tournPerms.settings     = true;
										tournPerms.registration = true;
										tournPerms.tabbing      = true;

										Object.keys(allEvents).forEach(function(eventID) { 
											tournPerms.events[eventID] = "admin";
										});

										resolve(tournPerms);
									} 
										
									if (perm.tag === "owner") { 

										tournPerms.owner        = true;
										tournPerms.settings     = true;
										tournPerms.registration = true;
										tournPerms.tabbing      = true;
										Object.keys(allEvents).forEach(function(eventID) { 
											tournPerms.events[eventID] = "admin";
										});

									} else if (perm.tag === "full_admin") { 

										tournPerms.settings     = true;
										tournPerms.registration = true;
										tournPerms.tabbing      = true;

										Object.keys(allEvents).forEach(function(eventID) { 
											tournPerms.events[eventID] = "admin";
										});

									} else if (perm.tag === "entry_only") { 

										tournPerms.entry = true;

									} else if (perm.tag === "registration") { 

										tournPerms.registration = true;

									} else if (perm.tag === "tabulation") { 

										tournPerms.tabbing = true;

										Object.keys(allEvents).forEach(function(eventID) { 
											tournPerms.events[eventID] = "admin";
										});

									} else if (perm.tag === "category_tab") {

										Object.keys(categoryEvents[perm.category]).forEach(function(eventID) { 
											tournPerms.events[eventID] = "admin";
										});

										tournPerms.tabbing = true;

									} else if (perm.tag === "detailed") { 
										tournPerms.events = JSON.parse(perm.details);
									}

								});

								resolve(tournPerms);
							}
						});
					});
				});

			} else { 
			
				return new Promise ( function(resolve, reject) { 

					db.sequelize.query(
						kingdomsKeys,
						{replacements: [userID]}
					).spread(
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
						
						db.sequelize.query(
							judgesQuery,
							{replacements: [userID]}
						).spread(
							function(judgeRows, metadata) { 

							perms.judges = judgeRows;

							db.sequelize.query(
								sjQuery,
								{replacements: [userID]}
							).spread(function(sjRows, metadata) { 

								perms.chapterJudges = sjRows;

								db.sequelize.query(
									studentsQuery,
									{replacements: [userID]}
								).spread(function(studentRows, metadata) { 
									perms.students = studentRows;
									resolve(perms);
								});
							});
						});
					});
				});
			}
		}
	};
