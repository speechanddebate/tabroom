var db = require("../../models");
var utils = require("../../lib/utils");

module.exports = function(req, res, Squads) { 

	if (Squads.length === 0) { 
		res.json({ error: "No squads found"});
	} else { 

		// Do not show potentially sensitive information except to someone 
		// authorized to see the school. 

		if (req.session.siteAdmin) { 
			res.json(Squads);
		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 
				where: 	{ school_id: Squads[0].school_id, person_id: req.session.user }
			}).then(function(Permission) { 

				Squads.forEach(function(oneSquad) { 

					if (Permission && Permission.tag === "chapter") { 

						oneSquad.full = true;
					
					} else { 
						oneSquad.onsite = undefined;
						oneSquad.onsite_by_id = undefined;
						oneSquad.registered_by_id = undefined;
						oneSquad.created_at = undefined;
						oneSquad.updated_at = undefined;
					}
				});

				res.json({
					success: true,
					messages: [{ 
						text: Squads.length()+" squads found", 
						severity: "success" }],
					model: Squads
				});
		
			}, function errorCallBack(response) { 

				res.json({ 
					messages: [{ 
						text: "Squads "+req.params.id+" not found", 
						severity: "failure" 
					}] 
				});
			});

		}
	}
};

