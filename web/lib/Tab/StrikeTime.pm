package Tab::Bin;
use base 'Tab::DBI';
Tab::Bin->table('bin');
Tab::Bin->columns(Primary => qw/id/);
Tab::Bin->columns(Essential => qw/judge_group start end name fine timestamp/);
Tab::Bin->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Bin->has_many(strikes => 'Tab::Strike', "bin");
__PACKAGE__->_register_datetimes( qw/start/ );
__PACKAGE__->_register_datetimes( qw/end/);


sub strike {

	my ($self, $judge) = @_;

	my @cons = Tab::Strike->search(	
				bin => $self->id, 
				judge => $judge->id );

	return shift @cons;
}

