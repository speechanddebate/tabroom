var db = require("../../models");
var utils = require("../../utils");

module.exports = function(req, res, Tourn) { 

	if (Tourn === null) { 
		res.json({ error: "No such tournament"});
	} else { 

		// Do not show potentially sensitive information except to someone 
		// authorized to see the tournament

		if (req.session.siteAdmin) { 

			res.json(Tourn);

		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 
				where: 	{ tourn_id: Tourn.id, person_id: req.session.user }
			}).then(function(Permission) { 

				if (
					(Permission && Permission.tag === "owner") ||
					(Permission && Permission.tag === "full_admin") ) { 
					Tourn.full = true;
				} 

				res.json({
					success  : true,
					messages : [ { text : "Tourn "+Tourn.name+" found", severity : "success" } ],
					model    : Tourn
				});
		
			}, function errorCallBack(response) { 
				res.json({ 
					messages: [ { text: "Tourn "+req.params.id+" not found", severity: "failure" } ] 
				});
			});

		}
	}
};

