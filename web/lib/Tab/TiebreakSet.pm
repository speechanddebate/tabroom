package Tab::TiebreakSet;
use base 'Tab::DBI';
Tab::TiebreakSet->table('tiebreak_set');
Tab::TiebreakSet->columns(All => qw/id tourn name timestamp type elim/);
Tab::TiebreakSet->has_a(tourn => 'Tab::Tourn');
Tab::TiebreakSet->has_many(tiebreaks => 'Tab::Tiebreak', 'tb_set');
Tab::TiebreakSet->has_many(rounds => 'Tab::Tiebreak', 'tb_set');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::TiebreakSetting->search(  
		tiebreak_set => $self->id,
		tag => $tag
	);

    if (defined $value) { 

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->update;
		
			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$exists->delete;
			}

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $exists = Tab::TiebreakSetting->create({
				tiebreak_set => $self->id,
				tag => $tag,
				value => $value,
			});

			$exists->update;

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		return $setting->value;

	}

}

