package Tab::Panel;
use base 'Tab::DBI';
Tab::Panel->table('panel');
Tab::Panel->columns(Primary   => qw/id/);
Tab::Panel->columns(Essential => qw/letter round room flight bye/);
Tab::Panel->columns(Others    => qw/bracket publish started timestamp/);

Tab::Panel->columns(TEMP => qw/opp pos side entryid judge audit
	timeslotid roomname eventname judgenum panelsize ada speakerorder
/);

Tab::Panel->has_a(room => 'Tab::Room');
Tab::Panel->has_a(round => 'Tab::Round');

Tab::Panel->has_many(logs          => 'Tab::ChangeLog'   , 'panel');
Tab::Panel->has_many(ballots       => 'Tab::Ballot'      , 'panel');
Tab::Panel->has_many(student_votes => 'Tab::StudentVote' , 'panel');

__PACKAGE__->_register_datetimes( qw/started timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;
	$/ = ""; #Remove all trailing newlines

	chomp $blob;

	my @existing = Tab::PanelSetting->search(
		panel => $self->id,
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
				my $json = eval{
					return JSON::encode_json($blob);
				};
				$existing->value_text($json);
			}

			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::PanelSetting->create({
				panel => $self->id,
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

			$existing->update;
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
			from panel_setting setting
		where setting.panel = ?
			order by setting.tag
    ");

    $sth->execute($self->id);

    while( my ($tag, $value, $value_date, $value_text)  = $sth->fetchrow_array() ) {

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

