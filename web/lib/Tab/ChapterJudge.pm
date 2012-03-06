package Tab::Uber;
use base 'Tab::DBI';
Tab::Uber->table('uber');
Tab::Uber->columns(Primary => qw/id/);
Tab::Uber->columns(Essential => qw/chapter retired first last account/);
Tab::Uber->columns(Other => qw/gender started created last_judged notes cell paradigm/);

Tab::Uber->has_a(chapter => 'Tab::Chapter');
Tab::Uber->has_many(judges => 'Tab::Judge', 'uber');

Tab::Uber->set_sql(free_by_school => "
				select distinct uber.* from
				school,uber
				where school.id = ?
				and uber.chapter = school.chapter
				and uber.retired != 1
				and not exists (
					select judge.id from judge
					where judge.uber = uber.id
					and judge.school = school.id);");

sub judge {
	my ($self, $tourn) = @_;
	my @judges = Tab::Judge->search_by_uber_and_tourn($self->id, $tourn->id);
	return shift @judges;
}
