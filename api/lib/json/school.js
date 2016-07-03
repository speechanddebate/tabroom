var db = require("../../models");

module.exports = function(req, res, School) { 

	if (School === null) { 
		res.json({ error: "No such school"});
	} else { 

		// Do not show potentially sensitive information except to someone 
		// authorized to see the school. 

		if (req.session.siteAdmin) { 
			console.log("Site admin is "+req.session.siteAdmin);
			res.json(School);

		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 
				where: 	{ school_id: School.id, person_id: req.session.user }
			}).then(function(Permission) { 

				if (Permission && Permission.tag === "chapter") { 

					School.full = true;
					
				} else { 
					School.coaches = undefined;
					School.naudl = undefined;
					School.nces = undefined;
					School.ipeds = undefined;
				}

				res.json({
					success  : true,
					messages : [ { text : "School "+School.name+" found", severity : "success" } ],
					model    : School
				});
		
			}, function errorCallBack(response) { 
				res.json({ 
					messages: [ { text: "School "+req.params.id+" not found", severity: "failure" } ] 
				});
			});

		}
	}
};

