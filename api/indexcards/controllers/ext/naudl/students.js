import { sendToSalesforce, getSalesforceTeams } from '../../../helpers/naudl';
import emailBlast from '../../../helpers/mail';
import { errorLogger } from '../../../helpers/logger';
import config from '../../../../config/config';

export const postNAUDLStudents = {
	GET: async (req, res) => {

		const unpostedStudentsQuery = `
			select
				student.id, student.first, student.middle, student.last,
				student.grad_year,
				race.value race,
				school_sid.value schoolSid,
				student.chapter chapterId,
				chapter.name chapterName,
				chapter.state chapterState
			from (student, chapter_setting naudl, chapter)

				left join student_setting race on race.student = student.id and race.tag = 'race'
				left join student_setting school_sid on school_sid.student = student.id and school_sid.tag = 'school_sid'

			where naudl.tag = 'naudl'
				and naudl.chapter = student.chapter
				and naudl.chapter = chapter.id
				and student.retired = 0

			and not exists (
				select nu.id
				from student_setting nu
				where nu.student = student.id
				and nu.tag = 'naudl_updated'
				and nu.value_date > student.timestamp
			)
			order by student.id DESC
			limit 50
		`;

		const unpostedStudents = await req.db.sequelize.query(unpostedStudentsQuery, {
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		const raceEncoding = {
			asian      : 'Asian',
			latino     : 'Latino',
			black      : 'Black_African_American',
			amerindian : 'American_Indian_Native_American',
			pacific    : 'Native_Hawaiian_Pacific_Islander',
			white      : 'White',
			dual       : 'Two_or_more_races',
			other      : 'Other',
		};

		const teams = await getSalesforceTeams();
		const postedChapters = {};

		teams.records.forEach( (school) => {
			postedChapters[school.Tabroom_teamid__c] = true;
		});

		const missedChapters = {};

		const naudlPost = unpostedStudents.flatMap( (student) => {

			const chapterKey = `TR${student.chapterId}`;

			if (postedChapters[chapterKey]) {
				const studentRecord = {
					tabroomid                : `TR${student.id}`,
					teamid                   : chapterKey,
					First_Name               : student.first,
					Middle_Name              : student.middle ? student.middle : ' ',
					Last_Name                : student.last,
					Expected_graduation_year : student.grad_year,
				};

				if (student.race && raceEncoding[student.race]) {
					studentRecord[raceEncoding[student.race]] = true;
				}

				if (student.schoolSid) {
					studentRecord.studentschoolid = student.schoolSid;
				}
				return [studentRecord];
			}
			missedChapters[chapterKey] = `${student.chapterName} in ${student.chapterState}`;
			return [];
		});

		const response = await sendToSalesforce(
			{ students_from_tabroom: naudlPost },
			config.NAUDL.STUDENT_ENDPOINT
		);

		if (response.data?.success === 'true') {
			unpostedStudents.map(async (student) => {
				await req.db.studentSetting.create({
					student    : student.id,
					tag        : 'naudl_updated',
					value      : 'date',
					value_date : new Date(),
				});
				return student.id;
			});
		}

		if (Object.keys(missedChapters).length > 0) {

			let messageBody = 'Response from posted data:\n';
			messageBody += JSON.stringify(response.data.message);
			messageBody += '\n\nChapters marked as NAUDL schools, but not in Salesforce: \n';

			Object.keys(missedChapters).forEach( (chapterId) => {
				messageBody += `Chapter ID ${chapterId}: ${missedChapters[chapterId]} \n`;
			});

			// replace this with an ID based sender later.
			const emails = await req.db.sequelize.query(`
				select
					person.email
				from person, person_setting ps
				where person.id = ps.person
					and person.no_email != 1
					and ps.tag = :tag
			`, {
				replacements: { tag: 'naudl_admin' },
				type: req.db.sequelize.QueryTypes.SELECT,
			});

			console.log(emails);

			if (emails) {
				const emailResponse = await emailBlast({
					email   : emails,
					from    : 'naudldata@www.tabroom.com',
					subject : `Tabroom Students Record Post: ${response.data.success ? 'SUCCESS' : 'ERRORS'}`,
					text    : messageBody,
				});

				errorLogger.info(messageBody);
				errorLogger.info(emailResponse);

			} else {
				errorLogger.info(`No NAUDL email addresses found, message not sent`);
			}

			res.status(200).json(response.data);
		}
	},
};

export const getNAUDLChapters = {
	GET: async (req, res) => {
		const teams = await getSalesforceTeams();
		res.status(200).json(teams);
	},
};

export default postNAUDLStudents;
