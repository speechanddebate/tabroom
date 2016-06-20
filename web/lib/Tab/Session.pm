package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All => qw/id person userkey timestamp ip tourn event category su/);
Tab::Session->has_a(person => 'Tab::Person');
Tab::Session->has_a(su => 'Tab::Person');
Tab::Session->has_a(tourn => 'Tab::Tourn');
Tab::Session->has_a(event => 'Tab::Event');
Tab::Session->has_a(category => 'Tab::Category');

sub account {
	my $self = shift;
	return $self->person;
}

__PACKAGE__->_register_datetimes( qw/timestamp/);
