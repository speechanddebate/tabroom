export const enablePushNotifications = {
	GET: async (req, res) => {
		const db = req.db;

		let pushNotify = await db.personSetting.findOne({
			where: {
				person : req.session.person,
				tag    : 'push_notify',
			},
		});

		console.log(pushNotify);
		console.log(req.params.onesignal);

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
