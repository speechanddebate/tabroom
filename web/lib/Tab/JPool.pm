package Tab::JPool;
use base 'Tab::DBI';
Tab::JPool->table('jpool');
Tab::JPool->columns(Primary => qw/id/);
Tab::JPool->columns(Essential => qw/name category site parent timestamp/);

Tab::JPool->has_a(site     => 'Tab::Site');
Tab::JPool->has_a(parent   => 'Tab::JPool');
Tab::JPool->has_a(category => "Tab::Category");

Tab::JPool->has_many(children    => 'Tab::JPool', 'parent');
Tab::JPool->has_many(settings    => 'Tab::JPoolSetting', 'jpool');
Tab::JPool->has_many(pool_judges => 'Tab::JPoolJudge', 'jpool');

Tab::JPool->has_many(judges => [Tab::JPoolJudge => 'judge']);
Tab::JPool->has_many(rounds => [Tab::JPoolRound => 'round']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::JPoolSetting->search(
		jpool => $self->id,
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

			my $existing = Tab::JPoolSetting->create({
				jpool => $self->id,
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
			from jpool_setting setting
		where setting.jpool = ?
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
