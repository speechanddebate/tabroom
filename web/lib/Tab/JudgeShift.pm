package Tab::JudgeShift;
use base 'Tab::DBI';
Tab::JudgeShift->table('shift');
Tab::JudgeShift->columns(Primary   => qw/id/);
Tab::JudgeShift->columns(Essential => qw/name type fine start end category no_hires/);
Tab::JudgeShift->columns(Others    => qw/timestamp/);

Tab::JudgeShift->has_a(category => 'Tab::Category');
Tab::JudgeShift->has_many(strikes => 'Tab::Strike', "shift");

__PACKAGE__->_register_datetimes( qw/start end/);

sub strike {

	my ($self, $judge) = @_;

	return unless $self;
	return unless $judge; 

	my @cons = Tab::Strike->search(	
		shift => $self->id,
		judge => $judge->id
	);

    my $con = shift @cons if @cons;
    foreach (@cons) {$_->delete;} #rm spares
	return $con;
}

