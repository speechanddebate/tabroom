import nodemailer from 'nodemailer';
import convert from 'html-to-text';
import config from '../../../config/config';
import { debugLogger } from '../logger';

async function mailBlast(mailData) {

	const transporter = nodemailer.createTransport({
		host   : config.MAIL_SERVER,
		port   : config.MAIL_PORT,
		secure : false,
	});

	if (!mailData.text && !mailData.html) {
		return 'No message body; not sending';
	}

	if (mailData.html && !mailData.text) {
		mailData.text = convert(mailData.html);
	}

	if (!mailData.to) {
		return 'No desination addresses provided, not sent';
	}

	// Only send BCC emails so folks do not reply all or see student contact etc.
	// And then add the sender as the To as well so it will not deliver.
	mailData.bcc = mailData.to;
	mailData.to = config.MAIL_FROM;

	if (!mailData.subject) {
		mailData.subject = 'Tabroom.com Notification';
	}

	if (!mailData.from) {
		mailData.from = config.MAIL_FROM;
	}

	if (mailData.text) {
		mailData.text += '\n\n----------------------------\n\n';
		mailData.text +=  'You signed up for this email by registering for an account on ';
		mailData.text += 'https://www.tabroom.com ';
		mailData.text += 'and following entry or judge records.\n';
		mailData.text += 'If you do not want further emails from Tabroom, ';
		mailData.text += 'login to your Tabroom account, and click the Profile icon on the top right of the screen.\n';
		mailData.text += 'Once there, either check off "No Emails", and save your profile, ';
		mailData.text += 'or you can also delete your Tabroom account entirely using the button on the right sidebar.';
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

async function textBlast(mailData) {

	const transporter = nodemailer.createTransport({
		host   : config.MAIL_SERVER,
		port   : config.MAIL_PORT,
		secure : false,
	});

	if (!mailData.text && !mailData.html) {
		return 'No message body; not sending';
	}

	if (!mailData.to) {
		return 'No desination addresses provided, not sent';
	}

	if (mailData.html && !mailData.text) {
		mailData.text = convert(mailData.html);
	} else {
		// Let us just be sure
		mailData.text = convert(mailData.text);
	}

	// Only send BCC emails so folks do not reply all or see student contact
	// etc. And then add the sender as the To as well so it will not deliver.

	mailData.bcc = mailData.to;
	mailData.to = config.MAIL_FROM;

	if (!mailData.subject) {
		mailData.subject = 'Tabroom Update';
	}

	if (!mailData.from) {
		mailData.from = config.MAIL_FROM;
	}

	mailData.text += '\n';
	mailData.text += 'You signed up for this text on https://www.tabroom.com ';
	mailData.text += 'To stop, log into Tabroom, click the Profile icon at top, select No Emails.\n';

	if (process.env.NODE_ENV === 'production') {
		const info = await transporter.sendMail(mailData);
		debugLogger.info(`Sent email id ${info.messageId} for ${mailData.from} to ${mailData.to} bcc ${mailData.bcc} textlength ${mailData.text.length} html ${mailData.html.length}`);
	} else {
		console.log(`Local: not sending email for ${mailData.from} to ${mailData.to} bcc ${mailData.bcc} text ${mailData.text} html ${mailData.html}`);
	}

	return `Email message sent to ${mailData.to.length + mailData.bcc.length} recipients`;

}

export default mailBlast;
export textBlast;
