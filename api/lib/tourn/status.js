
// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and chapter records are all covered and returned as
// a json object to the caller.

// Because I'm learning node LIKE A BOSS, I'm going to return a promise instead
// of dealing with callback hell.  Or die trying.  

var db = require('../../models');
var BluePromise = require('bluebird');

	const currentRound = "",
		unstarted    = "",
		unvoted      = "",
		unconfirmed  = "";

	currentRound = ` select distinct round.* 
            from round
                where round.event = ? 
                    and exists (
                        select ballot.id 
                            from ballot, entry, panel
                            where ballot.entry = entry.id
                            and entry.active = 1
                            and entry.dq != 1
                            and ballot.audit != 1
                            and ballot.panel = panel.id
                            and panel.bye != 1
                            and panel.round = round.id
                    )
                order by round.name limit 1
	`;

	unstarted = ` select distinct judge.*
		from judge, panel, ballot
		where panel.round = ? 
		and panel.id = ballot.panel
		and ballot.judge = judge.id
		and ballot.judge_started is null
	`;

	unvoted = ` select distinct judge.*
		from judge, panel, ballot
		where panel.round = ? 
		and panel.id = ballot.panel
		and ballot.judge = judge.id
		and ballot.judge_started is not null
		and not exists ( 
			select score.*
			from score 
			where score.ballot = ballot.id
		)
	`;

	unconfirmed = ` select distinct judge.*
		from judge, panel, ballot
		where panel.round = ? 
		and panel.id = ballot.panel
		and ballot.judge = judge.id
		and ballot.judge_started is not null
		and exists ( 
			select score.*
			from score 
			where score.ballot = ballot.id
		)
		and ballot.audit != 1
	`;

	module.exports = function(userId, locals, tournId) { 

		// Check if the inputs are valid and sane 

		if (tournId) { 

			return new BluePromise ( function(resolve, reject) { 

				tournQuery = ` select permission.id, permission.tag, 
				 permission.category_id from permission 
				 where permission.tourn_id = ? 
				 and permission.person_id = ? `;

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

	module.exports = function(userId, locals, tournId) { 

		// Check if the inputs are valid and sane 

		if (tournId) { 

			return new BluePromise ( function(resolve, reject) { 

				tournQuery = " select permission.id, permission.tag, "+
				" permission.category_id from permission "+
				" where permission.tourn_id = "+tournId +
				" and permission.person_id = "+userId;

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
