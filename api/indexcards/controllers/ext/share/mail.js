import nodemailer from 'nodemailer';
import config from '../../../../config/config';

export const transporter = nodemailer.createTransport({
	host: config.SHARE_SMTP_HOST,
	port: 25,
	secure: false, // Still allows STARTTLS
	pool: true,
	maxConnections: 30,
	tls: {
		secure: false,
		ignoreTLS: true,
		rejectUnauthorized: false,
	},
	auth: {
		user: config.SHARE_SMTP_USER,
		pass: config.SHARE_SMTP_PASS,
	},
});

const sendMail = async (from = 'share@share.tabroom.com', to, subject, text, html, attachments) => {
	// Configure email options
	const mailOptions = {
		from: `"Tabroom Share" <share@share.tabroom.com>`,
		to: 'noreply@share.tabroom.com',
		replyTo: from,
		bcc: to,
		subject,
		text,
		html,
	};
	if (attachments && attachments.length > 0) {
		mailOptions.attachments = [];
		attachments.forEach(a => {
			mailOptions.attachments.push({ filename: a.filename, content: a.file, encoding: 'base64' });
		});
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
