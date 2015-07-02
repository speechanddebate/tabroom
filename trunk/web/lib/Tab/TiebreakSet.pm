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

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::Setting->search(  
		tiebreak_set => $self->id,
		tag          => $tag,
		type         => "tiebreak_set"
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			$existing->value($value);
			$existing->value_text($blob) if $value eq "text";
			$existing->value_date($blob) if $value eq "date";
			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::Setting->create({
				tiebreak_set => $self->id,
				tag          => $tag,
				value        => $value,
				type         => "tiebreak_set"
			});

			if ($value eq "text") { 
				$existing->value_text($blob);
			}

			if ($value eq "date") { 
				$existing->value_date($blob);
			}

			$existing->update;

		}

	} else {

		return unless $existing;
		return $existing->value_text if $existing->value eq "text";
		return $existing->value_date if $existing->value eq "date";
		return $existing->value;

	}

}

