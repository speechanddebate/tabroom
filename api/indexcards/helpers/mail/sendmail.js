import nodemailer from 'nodemailer';
import config from '../../../config/config';
import { debugLogger } from '../logger';

async function sendMail(mailData) {

	const transporter = nodemailer.createTransport({
		host   : config.MAIL_SERVER,
		port   : config.MAIL_PORT,
		secure : false,
	});

	if (!mailData.text && !mailData.html) {
		return 'No message body; not sending';
	}

	if (!mailData.to && !mailData.bcc) {
		return 'No desination addresses provided, not sent';
	}

	// BCC only emails require a single non deliverable TO
	if (!mailData.to) {
		mailData.to = config.MAIL_FROM;
	}

	if (!mailData.subject) {
		mailData.subject = 'Tabroom.com Notification';
	}

	if (!mailData.from) {
		mailData.from = config.MAIL_FROM;
	}

	if (mailData.text) {
		mailData.html += '\n\n----------------------------\n\n';
		mailData.html +=  'You signed up for this email by registering for an account on ';
		mailData.html += 'https://www.tabroom.com ';
		mailData.html += 'and following entry or judge records.\n';
		mailData.html += 'If you do not want further emails from Tabroom, ';
		mailData.html += 'login to your Tabroom account, and click the Profile icon on the top right of the screen.\n';
		mailData.html += 'Once there, either check off "No Emails", and save your profile, ';
		mailData.html += 'or you can also delete your Tabroom account entirely using the button on the right sidebar.';
	}

	if (mailData.html) {
		mailData.html += '<p>-----------------------------</p>';
		mailData.html += '<p>You signed up for this email by registering for an account on ';
		mailData.html += '<a href="https://www.tabroom.com">https://www.tabroom.com</a> ';
		mailData.html += 'and following entry or judge records.</p>';
		mailData.html += '<p>If you do not want further emails from Tabroom, access ';
		mailData.html += '<a href="https://www.tabroom.com/user/login/profile.mhtml">Your Profile</a>, ';
		mailData.html += ' or login to your Tabroom account, and click the Profile icon on the top right of the screen</p>';
		mailData.html += '<p>Once there, either check off "No Emails", and save your profile, ';
		mailData.html += 'or you can also delete your Tabroom account entirely using the button on the right sidebar.</p>';
	}

	if (process.env.NODE_ENV === 'production') {
		const info = await transporter.sendMail(mailData);
		debugLogger.info(`Sent email id ${info.messageId} for ${mailData.from} to ${mailData.to} bcc ${mailData.bcc} textlength ${mailData.text.length} html ${mailData.html.length}`);
	} else {
		console.log(`Local: not sending email for ${mailData.from} to ${mailData.to} bcc ${mailData.bcc} text ${mailData.text} html ${mailData.html}`);
	}

	return `Email message sent to ${mailData.to.length + mailData.bcc.length} recipients`;
}

export default sendMail;
