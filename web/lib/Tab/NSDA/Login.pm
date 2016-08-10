package Tab::NSDA::Login;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Login->table('nsda.logins');
Tab::NSDA::Login->columns(Essential => qw/login_id username password person_id source sha512/);

sub person {
	my $self = shift; 
	return Tab::NSDA::Person->retrieve($self->person_id);
}

