var db = require("../../models");
var utils = require("../../lib/utils");

module.exports = function(req, res, Students, authorized) { 

	if (Students.length === 0 || Students[0] === null) { 

		res.json({ error: "No students found"});

	} else { 

		// Do not show potentially sensitive information except to someone 
		// authorized to see the school. 

		if (req.session.siteAdmin) { 
			res.json(Students);
		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 
				where: 	{ 
					school_id: Students[0].school_id, 
					person_id: req.session.user 
				}
			}).then(function(Permission) { 

				Students.forEach(function(oneStudent) { 

					if (Permission && Permission.tag === "chapter") { 

						oneStudent.full = true;
					
					} else { 
						oneStudent.person = undefined;
						oneStudent.gender = undefined;
						oneStudent.phonetic = undefined;
						oneStudent.person_id = undefined;
						oneStudent.ualt_id = undefined;
					}
				});

				res.json({
					success: true,
					messages: [ { text: Students.length + " students found", severity: "success" } ],
					model: Students
				});
		
			}, function errorCallBack(response) { 
				res.json({ 
					messages: [ { text: "Students " + req.params.id + " not found", 
								  severity: "failure" } ] 
				});
			});

		}
	}
};

