// Functions to establish access parameters
import getNSDA from '../../../helpers/nsda';
import { multiObjectify } from '../../../helpers/objectify';

export const syncNatsAppearances = {

	GET: async (req, res) => {

		const chapterNats = await getNSDA('/reports/nats-appearances');

		const existingChapters = multiObjectify(await req.db.sequelize.query(`
			select chapter.id chapter, chapter.nsda id, cs.id csid, cs.value
			from chapter
				left join chapter_setting cs
					on cs.chapter = chapter.id
					and cs.tag = 'nats_appearances'
			where chapter.nsda > 0
			order by chapter.id, chapter.nsda
		`, {
			type : req.db.sequelize.QueryTypes.SELECT,
		}));

		const updateChapter = `update chapter_setting set value = :value where id = :csid`;
		const createChapter = `insert into chapter_setting (tag, chapter, value) VALUES ('nats_appearances', :chapter, :value)`;

		chapterNats?.data.forEach( async (chapter) => {

			existingChapters[chapter.school_id]?.forEach( async (existing) => {

				if (existing.csid) {
					if (parseInt(existing.value) !== parseInt(chapter.Appearances)) {
						await req.db.sequelize.query(
							updateChapter, {
								replacements : {
									value    : chapter.Appearances,
									csid     : existing.csid,
								},
								type : req.db.sequelize.QueryTypes.UPDATE,
							}
						);
					}
				} else {
					await req.db.sequelize.query(
						createChapter, {
							replacements : {
								value    : chapter.Appearances,
								chapter  : existing.chapter,
							},
							type : req.db.sequelize.QueryTypes.INSERT,
						}
					);
				}
			});
		});

		const studentNats = await getNSDA('/reports/member-nats-appearances');

		const existingStudents = multiObjectify(await req.db.sequelize.query(`
			select student.id student, student.nsda id, ss.id ssid, ss.value
			from student
				left join student_setting ss
					on ss.student = student.id
					and ss.tag = 'nats_appearances'
			where student.nsda > 0
				and student.retired != 1
			order by student.nsda
		`, {
			type : req.db.sequelize.QueryTypes.SELECT,
		}));

		// And then the individual students

		const updateStudent = `update student_setting set value = :value where id = :ssid`;
		const createStudent = `insert into student_setting (tag, student, value) VALUES ('nats_appearances', :student, :value)`;

		studentNats?.data.forEach( async (student) => {

			existingStudents[student.person_id]?.forEach( async (existing) => {

				if (existing.ssid) {

					if (parseInt(existing.value) !== parseInt(student.appearances)) {

						await req.db.sequelize.query(
							updateStudent, {
								replacements : {
									value    : student.appearances,
									ssid     : existing.ssid,
								},
								type : req.db.sequelize.QueryTypes.UPDATE,
							}
						);
					}
				} else {
					await req.db.sequelize.query(
						createStudent, {
							replacements : {
								value    : student.appearances,
								student  : existing.student,
							},
							type : req.db.sequelize.QueryTypes.INSERT,
						}
					);
				}
			});
		});

		res.status(200).json({
			error   : false,
			message : `Chapters and students nats appearances updated`,
		});
	},
};

export default syncNatsAppearances;
