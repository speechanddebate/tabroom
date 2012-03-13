package Tab::ChapterJudge;
use base 'Tab::DBI';
Tab::ChapterJudge->table('chapter_judge');
Tab::ChapterJudge->columns(Primary => qw/id/);
Tab::ChapterJudge->columns(Essential => qw/chapter account notes created timestamp/);

Tab::ChapterJudge->has_a(chapter => 'Tab::Chapter');
Tab::ChapterJudge->has_many(judges => 'Tab::Judge', 'chapter_judge');

Tab::ChapterJudge->set_sql(free_by_school => "
				select distinct chapter_judge.* from
				school,chapter_judge
				where school.id = ?
				and chapter_judge.chapter = school.chapter
				and chapter_judge.retired != 1
				and not exists (
					select judge.id from judge
					where judge.chapter_judge = chapter_judge.id
					and judge.school = school.id);");

sub judge {
	my ($self, $tourn) = @_;
	my @judges = Tab::Judge->search_by_chapter_judge_and_tourn($self->id, $tourn->id);
	return shift @judges;
}
