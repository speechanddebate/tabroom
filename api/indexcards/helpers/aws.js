import {
	DeleteObjectCommand,
	GetObjectCommand,
	PutObjectCommand,
	CopyObjectCommand,
	S3Client } from '@aws-sdk/client-s3';
import config from '../../config/config';
import { errorLogger } from './logger';

const client = new S3Client(config.AWS_CREDENTIALS);

const s3Client = {

	client,

	rm: async (filepath) => {
		const rmCommand = new DeleteObjectCommand({
			Bucket     : config.AWS_CREDENTIALS.Bucket,
			Key        : filepath,
		});

		let response = '';

		try {
			response = await client.send(rmCommand);
		} catch (err) {
			errorLogger.error(`Error on deleting AWS ${filepath}`);
			errorLogger.error(err);
			return err;
		}

		return response;
	},

	cp: async (filepath, dest) => {

		const cpCommand = new CopyObjectCommand({
			CopySource : `${config.AWS_CREDENTIALS.Bucket}/${filepath}`,
			Bucket     : config.AWS_CREDENTIALS.Bucket,
			Key        : dest,
		});

		let response = '';
		try {
			response = await client.send(cpCommand);
		} catch (err) {
			errorLogger.error(`Error on copying AWS ${filepath} to ${dest}`);
			errorLogger.error(err);
			return err;
		}
		return response;
	},

	mv: async (filepath, dest) => {

		console.log(config.AWS_CREDENTIALS.Bucket);

		const cpCommand = new CopyObjectCommand({
			CopySource : `${config.AWS_CREDENTIALS.Bucket}/${filepath}`,
			Bucket     : config.AWS_CREDENTIALS.Bucket,
			Key        : dest,
		});

		const rmCommand = new DeleteObjectCommand({
			Bucket     : config.AWS_CREDENTIALS.Bucket,
			Key        : filepath,
		});

		try {
			await client.send(cpCommand);
		} catch (err) {
			errorLogger.error(`Error on moving AWS ${filepath} to ${dest} on the copy`);
			errorLogger.error(err);
			return err;
		}

		let response = '';

		try {
			response = await client.send(rmCommand);
		} catch (err) {
			errorLogger.error(`Error on moving AWS ${filepath} to ${dest} on the deletion`);
			errorLogger.error(err);
			return err;
		}

		return response;
	},

	get : async (filepath) => {
		const getCommand = new GetObjectCommand({
			Bucket : config.AWS_CREDENTIALS.Bucket,
			Key    : filepath,
		});

		let stream;

		try {
			const response = await client.send(getCommand);
			stream = await response.Body.transformToWebStream();
		} catch (err) {
			console.error(err);
		}
		return stream;
	},

	put : async (filepath, data) => {
		const putCommand = new PutObjectCommand({
			Bucket : config.AWS_CREDENTIALS.Bucket,
			Key    : filepath,
			Body   : data,
		});

		let response = '';

		try {
			response = await client.send(putCommand);
		} catch (err) {
			console.error(err);
		}
		return response;
	},
};

export default s3Client;
