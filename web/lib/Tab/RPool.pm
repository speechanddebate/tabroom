package Tab::RPool;
use base 'Tab::DBI';
Tab::RPool->table('rpool');
Tab::RPool->columns(All => qw/id name tourn timestamp/);

Tab::RPool->has_a(tourn => 'Tab::Tourn');
Tab::RPool->has_many(rpools => 'Tab::RPoolRoom','rpool');
Tab::RPool->has_many(round_links => 'Tab::RPoolRound','rpool');
Tab::RPool->has_many(room_links => 'Tab::RPoolRoom','rpool');

Tab::RPool->has_many(rounds => [Tab::RPoolRound => 'round']);
Tab::RPool->has_many(rooms => [Tab::RPoolRoom => 'room']);

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

			if ($value eq "text") {
				$existing->value_text($blob)
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {
				my $json = eval{
					return JSON::encode_json($blob);
				};
				$existing->value_text($json);
			}

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete();
			} else {
				$existing->update();
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
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {
				my $json = eval{
					return JSON::encode_json($blob);
				};
				$existing->value_text($json);
			}

			$existing->update();
		}

	} elsif ($existing) {

		if ($existing->value eq "text") {
			return $existing->value_text
		} elsif ($existing->value eq "date") {
			return $existing->value_date
		} elsif ($existing->value eq "json") {
			return eval {
				return JSON::decode_json($existing->value_text);
			};
		}

		return $existing->value;
	}
}

sub all_settings {

	my $self = shift;
	my %all_settings;

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select setting.tag, setting.value, setting.value_date, setting.value_text
			from rpool_setting setting
		where setting.rpool = ?
			order by setting.tag
    ");

    $sth->execute($self->id);

	my $results = $sth->fetchall_hash();

	foreach my $result (@{$results}) {

		if ($result->{value} eq "date") {
			$all_settings{$tag} = eval {
				return Tab::DBI::dateparse($result->{value_date});
			};
		} elsif ($result->{value} eq "text") {
			$all_settings{$tag} = $result->{value_text};
		} elsif ($result->{value} eq "json") {
			$all_settings{$tag} = eval {
				return JSON::decode_json($value_text);
			};
		} else {
			$all_settings{$tag} = $value;
		}
	}

	return %all_settings;

}
