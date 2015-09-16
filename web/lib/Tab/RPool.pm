package Tab::RPool;
use base 'Tab::DBI';
Tab::RPool->table('rpool');
Tab::RPool->columns(All => qw/id name tourn timestamp/);
Tab::RPool->has_many(rpools => 'Tab::RPoolRoom','rpool');
Tab::RPool->has_many(round_links => 'Tab::RPoolRound','rpool');
Tab::RPool->has_many(room_links => 'Tab::RPoolRoom','rpool');

Tab::RPool->has_many(rounds => [ Tab::RPoolRound => 'round'], 'round');
Tab::RPool->has_many(rooms => [ Tab::RPoolRoom => 'room'], 'room');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::RPoolSetting->search(  
		rpool => $self->id,
		tag   => $tag,
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

			my $existing = Tab::RPoolSetting->create({
				rpool => $self->id,
				tag   => $tag,
				value => $value,
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

