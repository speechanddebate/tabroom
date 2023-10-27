export const enablePushNotifications = {
	POST: async (req, res) => {
		const db = req.db;
		const oneSignalData = req.body;

		if (!oneSignalData.currentSubscription) {
			res.status(200).json({
				error   : false,
				message : 'No current subscription was found or registered',
			});
			return;
		}

		const currentSubscription = {
			id      : oneSignalData.currentSubscription?.id,
			enabled : oneSignalData.currentSubscription?.optIn,
		};

		// Update this session with the active push notification signal
		console.log(currentSubscription);

		if (!req.session.push_notify
			|| req.session.push_notify?.id !== currentSubscription.id
		) {
			await db.session.update(
				{ push_notify : currentSubscription },
				{ where: { id : req.session.id } }
			);
		}

		let pushNotify = await db.personSetting.findOne({
			where: {
				person : req.session.person,
				tag    : 'push_notify',
			},
		});

		if (pushNotify) {
			pushNotify = await pushNotify.update({
				value: oneSignalData.identity.onesignal_id,
			});
		} else {
			console.log(`Push Notify does not exist.  Creating`);
			pushNotify = await db.personSetting.create({
				person : req.session.person,
				tag    : 'push_notify',
				value  : oneSignalData.identity.onesignal_id,
			});
		}

		res.status(200).json({
			error   : false,
			message : 'You are subscribed to push notifications.  SMS texting is disabled',
		});
	},
};

export const disablePushNotifications = {
	GET: async (req, res) => {
		const db = req.db;

		await db.personSetting.destroy({
			where: {
				person : req.session.person,
				tag    : 'push_notify',
			},
		});

		res.status(200).json({
			error   : false,
			message : 'You are unsubscribed to push notifications.  SMS texting is re-enabled if you had it set up before',
		});
	},
};

export default enablePushNotifications;
