package Tab::JudgeHire;
use base 'Tab::DBI';
Tab::JudgeHire->table('judge_hire');
Tab::JudgeHire->columns(Primary => qw/id/);
Tab::JudgeHire->columns(Others => qw/judge tournament timestamp school covers
									judge_group accepted request_made/);
Tab::JudgeHire->has_a(tournament => "Tab::Tournament");
Tab::JudgeHire->has_a(school => "Tab::School");
Tab::JudgeHire->has_a(judge_group => "Tab::JudgeGroup");

sub events {
	my $self = shift;
	return Tab::Event->search_by_hire($self->id);
}

__PACKAGE__->_register_datetimes( qw/request_made/ );




