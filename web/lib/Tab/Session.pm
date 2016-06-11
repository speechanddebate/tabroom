package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All => qw/id person authkey userkey timestamp ip tourn event judge_group su/);
Tab::Session->has_a(person => 'Tab::Account');
Tab::Session->has_a(su => 'Tab::Account');
Tab::Session->has_a(tourn => 'Tab::Tourn');
Tab::Session->has_a(event => 'Tab::Event');
Tab::Session->has_a(judge_group => 'Tab::JudgeGroup');

sub account {
	my $self = shift;
	return $self->person;
}

__PACKAGE__->_register_datetimes( qw/timestamp/);
