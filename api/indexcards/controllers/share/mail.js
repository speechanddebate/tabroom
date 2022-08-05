import nodemailer from 'nodemailer';
import config from '../../../config/config.js';

export const transporter = nodemailer.createTransport({
	host: config.SHARE_SMTP_HOST,
	port: 25,
	secure: false, // Still allows STARTTLS
	pool: true,
	maxConnections: 20,
	auth: {
		user: config.SHARE_SMTP_USER,
		pass: config.SHARE_SMTP_PASS,
	},
});

const sendMail = async (from = 'share@share.tabroom.com', to, subject, text, html, attachment) => {
	// Configure email options
	const mailOptions = {
		from: `"Tabroom Share" <share@share.tabroom.com>`,
		to: 'share@share.tabroom.com',
		replyTo: from,
		bcc: to,
		subject,
		text,
		html,
	};
	if (attachment) {
		mailOptions.attachments = [{ filename: attachment.filename, content: attachment.file, encoding: 'base64' }];
	}

	// Send mail
	try {
		const result = await transporter.sendMail(mailOptions);
		return result;
	} catch (err) {
		return new Error(`Failed to send mail: ${err.message}`);
	}
};

export default sendMail;
