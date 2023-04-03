import nodemailer from 'nodemailer';
import { convert } from 'html-to-text';
import config from '../../config/config';
import { debugLogger } from './logger';

export const emailBlast = async (inputData) => {

	const messageData = { ...inputData };

	const transporter = nodemailer.createTransport({
		host   : config.MAIL_SERVER,
		port   : config.MAIL_PORT,
		secure : false,
	});

	if (!messageData.text && !messageData.html) {
		return { error: true, count: 0, message: 'No message body; not sending' };
	}

	if (messageData.html && !messageData.text) {
		messageData.text = convert(messageData.html);
	}

	if (!messageData.email) {
		return { error: true, count: 0, message: 'No desination addresses provided, not sent' };
	}

	// Only send BCC emails so folks do not reply all or see student contact
	// etc. And then add the sender as the To as well so it will not deliver.
	messageData.bcc = messageData.email;
	messageData.to = config.MAIL_FROM;

	if (!messageData.subject) {
		messageData.subject = 'Tabroom.com Notification';
	}

	if (!messageData.from) {
		messageData.from = config.MAIL_FROM;
	}

	if (messageData.text) {
		messageData.text += '\n\n----------------------------\n';
		messageData.text +=  'You received this email because you registered for an account on https://www.tabroom.com\n';
		messageData.text += 'To stop them, login to your Tabroom account, click the Profile icon at top right, and ';
		messageData.text += 'check off "No Emails", then save your profile. ';
		messageData.text += 'You can also delete your Tabroom account entirely on that profile screen.';
	}

	if (messageData.html) {
		messageData.html += '<p>-----------------------------</p>';
		messageData.html += '<p>You received this email because you registered for an account on ';
		messageData.html += '<a href="https://www.tabroom.com">https://www.tabroom.com</a></p>';
		messageData.html += '<p>To stop them, visit ';
		messageData.html += '<a href="https://www.tabroom.com/user/login/profile.mhtml">Your Profile</a>, ';
		messageData.html += '<p>and check off "No Emails", and save your profile.';
		messageData.html += 'You can also delete your Tabroom account entirely on your profile.</p>';
	}

	let info = {};

	if (process.env.NODE_ENV === 'production') {
		info = await transporter.sendMail(messageData);
		debugLogger.info(`Sent email id ${info.messageId} for ${messageData.from} to ${messageData.to} bcc ${messageData.bcc} textlength ${messageData.text?.length} html ${messageData.html?.length}`);
	} else {
		console.log(`Local: not sending email for ${messageData.from} to ${messageData.to} bcc ${messageData.bcc} text ${messageData.text} html ${messageData.html}`);
	}

	return {
		error   : false,
		count   : messageData.bcc?.length,
		message : `Email message sent to ${messageData.to?.length + messageData.bcc?.length} recipients`,
		info,
	};
};

export const phoneBlast = async (inputData) => {

	const messageData = { ...inputData };

	const transporter = nodemailer.createTransport({
		host   : config.MAIL_SERVER,
		port   : config.MAIL_PORT,
		secure : false,
	});

	if (!messageData.text && !messageData.html) {
		return { error: true, count: 0, message: 'No message body; not sending' };
	}

	if (!messageData.phone) {
		return { error: true, count: 0, message: 'No desination addresses provided, not sent' };
	}

	if (messageData.html && !messageData.text) {
		messageData.text = convert(messageData.html);
	} else {
		// Let us just be sure
		messageData.text = convert(messageData.text);
	}

	// Only send BCC emails so folks do not reply all or see student contact
	// etc. And then add the sender as the To as well so it will not deliver.

	messageData.bcc = messageData.phone;
	messageData.phone = config.MAIL_FROM;

	if (!messageData.subject) {
		messageData.subject = 'Tabroom Update';
	}

	if (!messageData.from) {
		messageData.from = config.MAIL_FROM;
	}

	messageData.text += '\n';
	messageData.text += 'You registered for this text on https://www.tabroom.com ';
	messageData.text += 'To stop, log into Tabroom, click the Profile icon at top, select No Emails.\n';

	let info = {};

	if (process.env.NODE_ENV === 'production') {
		info = await transporter.sendMail(messageData);
		console.log(`Sent text blast id ${info.messageId} for ${messageData.from} to ${messageData.to} bcc ${messageData.bcc} textlength ${messageData.text?.length} html ${messageData.html?.length}`);
	} else {
		console.log(`Local: not sending email for ${messageData.from} to ${messageData.to} bcc ${messageData.bcc} text ${messageData.text} html ${messageData.html}`);
	}

	return {
		error   : false,
		count   : messageData.bcc?.length,
		message : `Email message sent to ${messageData.to?.length + messageData.bcc?.length} recipients`,
		info,
	};

};

export default emailBlast;
