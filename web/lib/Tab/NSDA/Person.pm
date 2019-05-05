package Tab::NSDA::Person;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Person->table('NEW_USERS');
Tab::NSDA::Person->columns(Essential => qw/
				user_id ualt_id ufname umname ulname utype
				uemail cell grad_yr graduated paid_status ustatus 
				uapproved total_pts
			/);

Tab::NSDA::Person->columns(TEMP => qw/middle_joined high_joined school_id advisor_type demo login/);

sub nsda { 
	my $self = shift;
	return $self->user_id;
}


