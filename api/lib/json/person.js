var db = require("../../models");

module.exports = function(req, res, Person) { 

	if (Person === null) { 
		res.json({ error: "No such person"});
	} else { 

		// Do not show potentially sensitive information except to someone
		// authorized to see the person's details

		if (req.session.siteAdmin) { 

			res.json(Person);

		} else { 

			if (req.session.user === undefined) { 
				req.session.user = 0;
			}

			db.permission.findOne({ 
				where: 	{ school_id: Person.id, person_id: req.session.user }
			}).then(function(Permission) { 

				if (Permission && Permission.tag === "chapter") { 
					Person.full = true;
				} else { 
					Person.coaches = undefined;
					Person.naudl   = undefined;
					Person.nces    = undefined;
					Person.ipeds   = undefined;
				}

				res.json({
					success  : true,
					messages : [ { text : "Person "+Person.name+" found", severity : "success" } ],
					model    : Person
				});
		
			}, function errorCallBack(response) { 
				res.json({ 
					messages: [ { text: "Person "+req.params.id+" not found", severity: "failure" } ] 
				});
			});
		}
	}
};

