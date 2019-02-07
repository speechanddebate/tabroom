package Tab::Ballot;
use base 'Tab::DBI';
Tab::Ballot->table('ballot');
Tab::Ballot->columns(Primary => qw/id/);
Tab::Ballot->columns(Essential => qw/judge panel entry speakerorder side audit bye forfeit tv/);
Tab::Ballot->columns(Others => qw/chair seat entered_by audited_by collected collected_by 
								  cat_id seed pullup hangout_admin judge_started timestamp/);
Tab::Ballot->columns(TEMP => qw/roundid entryid panelid judgename/);

Tab::Ballot->has_a(judge => 'Tab::Judge');
Tab::Ballot->has_a(panel => 'Tab::Panel');
Tab::Ballot->has_a(entry => 'Tab::Entry');

Tab::Ballot->has_a(entered_by => 'Tab::Person');
Tab::Ballot->has_a(audited_by => 'Tab::Person');
Tab::Ballot->has_a(collected_by => 'Tab::Person');

Tab::Ballot->has_a(hangout_admin => 'Tab::Person');

Tab::Ballot->has_many(values => 'Tab::Score');
Tab::Ballot->has_many(scores => 'Tab::Score');

__PACKAGE__->_register_datetimes( qw/timestamp collected judge_started/);

sub account { 
	my $self = shift;
	return $self->entered_by;
}

sub value {

	my ($self, $tag, $value, $student) = @_;

	my @existing = Tab::Score->search(  
		ballot => $self->id,
		tag => $tag
	);

	@existing = Tab::Score->search(  
		ballot => $self->id,
		student => $student->id,
		tag => $tag
	) if $student;

    if ($value) { 

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->update;
		
			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} elsif ($value ne "0") {

			my $exists = Tab::Score->create({
				ballot => $self->id,
				tag => $tag,
				value => $value,
			});

			$exists->update;
		}

	} else {

		return unless @existing;
		my $value = shift @existing;
		return $value->value;

	}

}
