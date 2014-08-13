package Tab::Calendar;
use base 'Tab::DBI';
Tab::Calendar->table('calendar');
Tab::Calendar->columns(Primary => qw/calendar_id/);
Tab::Calendar->columns(Essential => qw/start_date end_date title reg_start reg_end state country timezone status_code hidden location contact url tabroom_id source/);
Tab::Calendar->has_a(tabroom_id => "Tab::Tourn");

__PACKAGE__->_register_dates( qw/start_date end_date/);
__PACKAGE__->_register_datetimes( qw/reg_start reg_end/);

sub tourn {
	my $self = shift;
	return $self->tabroom_id if $self;
	return;
}

