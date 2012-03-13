package Tab::StrikeTime;
use base 'Tab::DBI';
Tab::StrikeTime->table('strike_time');
Tab::StrikeTime->columns(Primary => qw/id/);
Tab::StrikeTime->columns(Essential => qw/judge_group name fine start end timestamp/);
Tab::StrikeTime->has_a(judge_group => 'Tab::JudgeGroup');
Tab::StrikeTime->has_many(strikes => 'Tab::Strike', "strike_time");
__PACKAGE__->_register_datetimes( qw/start/ );
__PACKAGE__->_register_datetimes( qw/end/);

sub strike {
	my ($self, $judge) = @_;
	my @cons = Tab::Strike->search(	
				strike_time => $self->id, 
				judge => $judge->id );
    my $con = shift @cons if @cons;
    foreach (@cons) {$_->delete;} #rm spares
	return $con;
}

