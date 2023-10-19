import sendToSalesforce from '../../../helpers/naudl';
import config from '../../../../config/config';

export const postNAUDLStudents = {
	GET: async (req, res) => {

		const unpostedStudentsQuery = `
			select
				student.id, student.first, student.middle, student.last,
				student.grad_year,
				race.value race,
				school_sid.value schoolSid,
				student.chapter chapterId
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

		const naudlPostPromises = unpostedStudents.map( async (student) => {
			const studentRecord = {
				tabroomid                : `TR${student.id}`,
				teamid                   : `TR${student.chapterId}`,
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

			return studentRecord;
		});

		const naudlPost = await Promise.all(naudlPostPromises);
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
		} else {
			res.status(200).json(response.data);
		}
	},
};

export default postNAUDLStudents;
