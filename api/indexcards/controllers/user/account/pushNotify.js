export const enablePushNotifications = {
	GET: async (req, res) => {
		const db = req.db;
		const oneSignalData = req.body;

		const currentSubscription = {
			id      : oneSignalData.currentSubscription.id,
			enabled : oneSignalData.currentSubscription.enabled,
		};

		// Update this session with the active push notification signal

		if (!req.session.push_notify
			|| req.session.push_notify?.id !== currentSubscription.id
		) {
			await db.session.update(
				{ push_notify : JSON.stringify(currentSubscription) },
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
				value: req.params.onesignal,
			});
		} else {
			pushNotify = await db.personSetting.create({
				person : req.session.person,
				tag    : 'push_notify',
				value  : req.params.onesignal,
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
