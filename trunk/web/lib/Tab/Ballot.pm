package Tab::Ballot;
use base 'Tab::DBI';
Tab::Ballot->table('ballot');
Tab::Ballot->columns(Primary => qw/id/);
Tab::Ballot->columns(Essential => qw/judge panel entry speakerorder tv audit bye side/);
Tab::Ballot->columns(Others => qw/noshow timestamp chair speechnumber collected
								  collected_by cat_id account seed pullup/);
Tab::Ballot->columns(TEMP => qw/roundid entryid panelid judgename/);

Tab::Ballot->has_a(judge => 'Tab::Judge');
Tab::Ballot->has_a(panel => 'Tab::Panel');
Tab::Ballot->has_a(entry => 'Tab::Entry');
Tab::Ballot->has_a(account => 'Tab::Account');
Tab::Ballot->has_a(collected_by => 'Tab::Account');

Tab::Ballot->has_many(values => 'Tab::BallotValue');
Tab::Ballot->has_many(ballot_values => 'Tab::BallotValue');

__PACKAGE__->_register_datetimes( qw/timestamp collected/);

sub value {

	my ($self, $tag, $value, $student) = @_;

	my @existing = Tab::BallotValue->search(  
		ballot => $self->id,
		tag => $tag
	);

	@existing = Tab::BallotValue->search(  
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

			my $exists = Tab::BallotValue->create({
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
