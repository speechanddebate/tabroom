package Tab::StrikeTime;
use base 'Tab::DBI';
Tab::StrikeTime->table('strike_time');
Tab::StrikeTime->columns(Primary => qw/id/);
Tab::StrikeTime->columns(Essential => qw/category name fine start end timestamp/);
Tab::StrikeTime->has_a(category => 'Tab::Category');
Tab::StrikeTime->has_many(strikes => 'Tab::Strike', "strike_time");
__PACKAGE__->_register_datetimes( qw/start/ );
__PACKAGE__->_register_datetimes( qw/end/);

sub strike {

	my ($self, $judge) = @_;

	return unless $self;
	return unless $judge; 

	my @cons = Tab::Strike->search(	
		strike_time => $self->id, 
		judge       => $judge->id 
	);

    my $con = shift @cons if @cons;
    foreach (@cons) {$_->delete;} #rm spares
	return $con;
}

