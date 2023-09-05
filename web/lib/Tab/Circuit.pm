package Tab::Circuit;
use base 'Tab::DBI';
Tab::Circuit->table('circuit');
Tab::Circuit->columns(Primary   => qw/id/);
Tab::Circuit->columns(Essential => qw/name abbr tz active state country webname/);
Tab::Circuit->columns(Others    => qw/timestamp/);

Tab::Circuit->has_many(sites       => "Tab::Site");
Tab::Circuit->has_many(regions     => "Tab::Region");
Tab::Circuit->has_many(webpages    => "Tab::Webpage");
Tab::Circuit->has_many(permissions => "Tab::Permission");
Tab::Circuit->has_many(quizzes     => "Tab::Quiz");

Tab::Circuit->has_many(settings            => "Tab::CircuitSetting", "circuit");
Tab::Circuit->has_many(tourn_circuits      => "Tab::TournCircuit");
Tab::Circuit->has_many(chapter_circuits    => "Tab::ChapterCircuit");
Tab::Circuit->has_many(awards              => "Tab::SweepAward");
Tab::Circuit->has_many(sweep_awards        => "Tab::SweepAward");

Tab::Circuit->has_many(tourns   => [ Tab::TournCircuit   => 'tourn' ]);
Tab::Circuit->has_many(chapters => [ Tab::ChapterCircuit => 'chapter' ]);
Tab::Circuit->has_many(admins   => [ Tab::Permission     => 'person' ]);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub location {
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;
	$/ = ""; #Remove all trailing newlines

	chomp $blob;

	my @existing = Tab::CircuitSetting->search(
		circuit => $self->id,
		tag   => $tag
	);

	my $existing = shift @existing if @existing;

	foreach (@existing) { $_->delete(); }

	if (defined $value) {

		if ($existing) {

			$existing->value($value);
			$existing->value_text($blob) if $value eq "text";
			$existing->value_date($blob) if $value eq "date";

			if ($value eq "json") {
				eval{
					$existing->value_text(JSON::encode_json($blob));
				};
			}

			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::CircuitSetting->create({
				circuit => $self->id,
				tag   => $tag,
				value => $value,
			});

			if ($value eq "text") {
				$existing->value_text($blob);
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {
				eval{
					$existing->value_text(JSON::encode_json($blob));
				};
			}
			$existing->update();
		}

	} else {

		return unless $existing;
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
		from circuit_setting setting
		where setting.circuit = ?
        order by setting.tag
    ");

    $sth->execute($self->id);

    while(
		my (
			$tag, $value, $value_date, $value_text
		)  = $sth->fetchrow_array()
	) {

		if ($value eq "date") {

			my $dt = Tab::DBI::dateparse($value_date);
			$all_settings{$tag} = $dt if $dt;

		} elsif ($value eq "text") {

			$all_settings{$tag} = $value_text;

		} elsif ($value eq "json") {

			$all_settings{$tag} = eval {
				return JSON::decode_json($value_text);
			};

		} else {
			$all_settings{$tag} = $value;
		}
	}
	return %all_settings;
}
