import { access } from "fs";

export const changeAccess = {

	// A POST creates a new tournament permission for the user
    POST: async (req, res) => {
		const db = req.db;
		const adminId = req.body.target_id;
		const tournId = req.params.tourn_id;
		let accessLevel = "checker";

		if (req.body.other_value) { 
			accessLevel = req.body.other_value;
		};

		if (
			accessLevel === "checker"
			|| accessLevel === "tabber"
			|| accessLevel === "by_event"
			|| (accessLevel === "owner" && req.session[tournId].level === "owner")
		) { 
			// everything is a-ok
		} else {
			return res.status(200).json({ 
				error: true,
				message: `Invalid access level requested: ${accessLevel}`
			});
		}

		let already = await db.permission.findAll({
			where : { tourn: tournId, person: adminId}
		});

		parsePerms(already).then(async function(existing) { 

			if (existing.level) { 

				return res.status(200).json({ 
					error: true,
					message: 'User already has access'
				});

			} else { 

				let newAdmin = await db.person.findByPk(adminId);
				
				const newPerm =  db.permission.create({
					person : newAdmin,
					tourn  : tournId,
					tag    : accessLevel
				});

				const changeLog = await db.changeLog.create({
					tag         : "access",
					tourn       : tournId,
					person      : req.session.person,
					description : `Added ${newAdmin.email} with ${accessLevel} level permissions`,
					created_at  : Date()
				});

				return res.status(200).json({ 
					error: false,
					message: changeLog.description
				});
			}
		});

		return;
    },

	// A PUT alters an existing permission level or contact status
	PUT: async (req, res) => { 

		const db = req.db;
		const adminId = req.body.target_id;
		const tournId = req.params.tourn_id;
		let accessLevel = req.body.property_value;
		const newAdmin = await db.person.findByPk(adminId);

		if (
			req.body.property_name === "contact"
			&& ( req.session[tournId].level === "owner"
				 || adminId == req.session.person 
				 || req.session[tournId].contact
			)
		) { 

			let already = await db.permission.findByPk(28285536);

			if (already === null) {

				if (accessLevel > 0) {

					// Checkbox checked so create it
					const newPerm =  await db.permission.create({
						person : newAdmin.id,
						tourn  : tournId,
						tag    : "contact"
					});

					const changeLog = await db.changeLog.create({
						tag         : "access",
						tourn       : tournId,
						person      : req.session.person,
						description : `Added ${newAdmin.email} as a contact`,
						created_at  : Date()
					});

					return res.status(200).json({ 
						error: false,
						message: changeLog.description
					});

				} else {
				
					// Nope wasn't then isn't now
					return res.status(200).json({
						error: false,
						message: `${newAdmin.email} was already not a contact for this tournament`
					 });
				}
				
			} else { 
				
				if (accessLevel > 0) { 

					// Checkbox checked so keep it
					return res.status(200).json({
						error   : false,
						message : newAdmin.email+' is already a contact for this tournament'
					 });

				} else {
			
					// Checkbox is unchecked so TERMINATE. TERMINATE. 
					await already.destroy();

					const changeLog = await db.changeLog.create({
						tag         : "access",
						tourn       : tournId,
						person      : req.session.person,
						description : `${newAdmin.email} is no longer a tournament contact`,
						created_at  : Date()
					});

					return res.status(200).json({ 
						error: false,
						message: changeLog.description
					});
				}
				 		
			}

		} else if (
			accessLevel === "checker"
			|| accessLevel === "tabber"
			|| accessLevel === "by_event"
			|| (accessLevel === "owner" && req.session[tournId].level === "owner")
		) { 

			let already = await db.permission.findAll({
				where:{ 
					tourn  : tournId,
					person : adminId
				},
				include : [
					{ model: db.person, as: 'Person' },
				]
			});

			let existing = await parsePerms(already);
			console.log(existing.permObject.toJSON);
			await existing.permObject.update({tag: accessLevel});
			console.log(existing.permObject.toJSON);
				
			const changeLog = await db.changeLog.create({
				tag         : "access",
				tourn       : tournId,
				person      : req.session.person,
				created_at  : Date(),
				description : `Changed ${existing.person.email} access level to ${accessLevel}`
			});

			return res.status(200).json({
				error: false,
				message: changeLog.description
			 });
		}
	},

	// A DELETE removes access from the user
	DELETE: async (req, res) => { 

		const db = req.db;
		const adminId = req.body.target_id;
		const tournId = req.params.tourn_id;

		let already = await db.permission.findAll({
			where : {
				tourn  : tournId,
				person : adminId
			}, include : [
				{ model: db.person, as: 'Person' },
			]
		});

		let existing = await parsePerms(already)

		if (existing.person === undefined) {
			return res.status(200).json({ 
				error: true,
				message: 'That account has already lost access to this tournament.  You may want to refresh the page.'
			});
		}

		const reply = { 
			error: false,
			message: `${existing.person.first} ${existing.person.last}'s access to this tournament has been revoked.`,
			destroy: existing.person.id
		};

		const checkOwner = async (target) => {
			if (target.level === "owner") { 
				if (req.session[tournId].level != "owner") { 
					return res.status(200).json({ 
						error: true,
						message: 'Only a tournament owner may revoke privileges from another'
					});
				}
			}
			return;
		};

		const destroyPerms = async (target) => {
			if (target.contactObject) { 
				await target.contactObject.destroy();
			}
					
			if (target.permObject) { 
				await target.permObject.destroy();
			}
			return;
		}

		await checkOwner(existing);
		await destroyPerms(existing);
		
		const changeLog = await db.changeLog.create({
			tag         : "access",
			Tourn       : tournId,
			person      : req.session.person,
			created_at  : Date(),
			description : reply.message
		});
			
		return res.status(200).json(reply);
	},
};

export const changeEventAccess = {

	// A post will add access level to a user

    POST: async (req, res) => {

		const db          = req.db;
		const adminId     = req.body.target_id;
		const tournId     = req.params.tourn_id;
		const accessLevel = req.body.setting_name;

		const accessKey = req.body.property_value;
		const [accessType, accessId] = accessKey.split('_');

		let events;

		if (accessType === "event") { 
			events = await db.event.findAll({
				where : {id: accessId}
			});
		} else if (accessType === "category") { 
			events = await db.event.findAll({
				where : {category: accessId}
			});
		} else if (accessType === "type") { 
			events = await db.event.findAll({
				where : {
					type: accessId,
					tourn: tournId
				}
			});
		}

		let already = await db.permission.findAll({
			where : {
				tourn: tournId, 
				person: adminId
			}
		});

		parsePerms(already).then(async function(existing) { 

			let logString = " ";
			let replyButtons = " ";
			let newAdmin = await db.person.findByPk(adminId);

			console.log(existing.permObject.details);
			
			if (existing.permObject.details == null) {
				existing.permObject.details = {};
			}

			for (let event of events) { 

				if (existing.permObject.details[event.id] == accessLevel) { 
					continue;
				}

				existing.permObject.details[event.id] = accessLevel;

				if (logString) {
					logString += " ";
				}
				
				logString += event.abbr;

				// Need to add the button to the listing.  God this will be so
				// much easier with React.  This is basically why react/angular
				// etc were created I guess.  Anguish.
				
				replyButtons += `<div
					class = "third padvertless semibold greentext yellowhover centeralign nospace smaller"
					id    = "${ event.id }_${ adminId }"
					title = "Click event to remove access"
					target_id    = "${ adminId }"
					title        = "Click event to remove access"
					post_method  = "delete"
					setting_name = "${ event.id }"
					onClick      = "postSwitch(this, '/v1/tourn/${tournId}/tab/setup/eventaccess');"
				>${event.abbr}</div>`;
			}

			existing.permObject.changed("details", true);
			existing.permObject.save();

			const changeLog = await db.changeLog.create({
				tag         : "access",
				tourn       : tournId,
				person      : req.session.person,
				description : `Added ${newAdmin.email} with ${accessLevel} level permissions to ${logString}`,
				created_at  : Date()
			});

			console.log(changeLog.toJSON);

			return res.status(200).json({ 
				error   : false,
				message : changeLog.description,
				reply   : replyButtons
			});
		});
    },

	// A delete will revoke access to that event

    DELETE: async (req, res) => {
		
		const db = req.db;
		const adminId = req.body.target_id;
		const tournId = req.params.tourn_id;
		const eventId = req.body.setting_name;
		
		const already = await db.permission.findAll({
			where : {tourn: tournId, person: adminId}
		});

		const newAdmin = await db.person.findByPk(adminId);
		const targetEvent = await db.event.findByPk(eventId);

		parsePerms(already).then(async function(existing) { 

			delete existing.permObject.details[eventId];
			existing.permObject.changed("details", true);
			await existing.permObject.save();

			const changeLog = await db.changeLog.create({
				tag         : "access",
				tourn       : tournId,
				person      : req.session.person,
				description : `Revoked ${newAdmin.email} permissions to ${targetEvent.name}`,
				created_at  : Date()
			});
			
			return res.status(200).json({ 
				error   : false,
				message : changeLog.description,
				destroy : `${eventId}_${adminId}`
			});
		});

		return;
		
    },
}

changeAccess.POST.apiDoc = {
    summary     : 'Change, delete and add tournament permissions for user accounts',
    operationId : 'listSchools',
    parameters: [
        {
            in          : 'parameters',
            name        : 'target_id',
            description : 'Person ID',
            required    : true,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },
        {
            in          : 'parameters',
            name        : 'property_name',
            description : 'Access Level',
            required    : false,
            schema      : {
				type    : 'string',
				enum    : ['owner','tabber','checker','by_event']
			},
        },
    ],
    responses: {
        200: {
            description: 'Success! Messages included',
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['tournament/setup'],
};
		
const parsePerms = async (permsArray) => {

	let permsOutput = {};

	for (let perm of permsArray) { 

		if (permsOutput.person === undefined) { 
			permsOutput.person = perm.Person;
		}

		if (perm.tag === "contact") { 
			permsOutput.contact = true;
			permsOutput.contactObject = perm;
		} else if (permsOutput.level) { 
			// duplicates are the bane of all
			await perm.destroy();
		} else { 
			permsOutput.level = perm.tag;
			permsOutput.permObject = perm;
		}
	}

	return permsOutput;
}