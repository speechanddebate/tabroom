package Tab::JPool;
use base 'Tab::DBI';
Tab::JPool->table('jpool');
Tab::JPool->columns(Primary => qw/id/);
Tab::JPool->columns(Essential => qw/name tourn judge_group standby standby_timeslot timestamp/);
Tab::JPool->columns(Others => qw/site publish registrant burden event_based/);

Tab::JPool->has_a(standby_timeslot => 'Tab::Timeslot');

Tab::JPool->has_a(site => 'Tab::Site');
Tab::JPool->has_a(tourn => 'Tab::Tourn');
Tab::JPool->has_a(judge_group => "Tab::JudgeGroup");

Tab::JPool->has_many(rounds => 'Tab::Round', 'pool');
Tab::JPool->has_many(pool_judges => 'Tab::JPoolJudge', 'pool');

Tab::JPool->has_many(judges => [Tab::JPoolJudge => 'judge']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::Setting->search(  
		jpool => $self->id,
		tag   => $tag,
		type  => "jpool"
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
				jpool => $self->id,
				tag   => $tag,
				value => $value,
				type  => "jpool"
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

