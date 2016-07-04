var db = require("../../models");

module.exports = function(req, res, SchoolJudges, authorized) { 

	if (SchoolJudges.length === 0) { 
		res.json({ error: "No school judges found"});
	} else { 

		// Do not show potentially sensitive information except to someone 
		// authorized to see the school. 

		if (req.session.siteAdmin) { 
			res.json(SchoolJudges);
		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 

				where: 	{ school_id: SchoolJudges[0].school_id, person_id: req.session.user }

			}).then(function(Permission) { 

				if (Permission && Permission.tag === "chapter") { 

					Permission.full = true;	

				} else { 

					SchoolJudges.forEach(function(oneSchoolJudge) { 

						if (req.session.user === oneSchoolJudge.person_id) { 

							oneSchoolJudge.full = true;	

						} else { 
							oneSchoolJudge.gender = undefined;
							oneSchoolJudge.person_id = undefined;
							oneSchoolJudge.person = undefined;
							oneSchoolJudge.request_person_id = undefined;
							oneSchoolJudge.request_person = undefined;
							oneSchoolJudge.cell = undefined;
							oneSchoolJudge.email = undefined;
							oneSchoolJudge.diet = undefined;
							oneSchoolJudge.ada = undefined;
						}
					});

				}

				res.json(SchoolJudges);
		
			}, function errorCallBack(response) { 
				res.json({ 
					messages: [ { text: "Judge Roster for school "+req.params.id+" not found", severity: "failure" } ] 
				});
			});

		}
	}
};

