package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/person first middle last chapter novice
									 grad_year retired gender person_request/);
Tab::Student->columns(Other => qw/timestamp phonetic nsda/);
Tab::Student->columns(TEMP => qw/code entry event school region status/);

Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_a(person => 'Tab::Person');
Tab::Student->has_a(person_request => 'Tab::Person');

Tab::Student->has_many(followers => 'Tab::Follower', 'student');

Tab::Student->has_many(settings => 'Tab::StudentSetting', 'student');
Tab::Student->has_many(entries => [ Tab::EntryStudent => 'entry']);
Tab::Student->has_many(entry_students => 'Tab::EntryStudent', 'student');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub fullname {
	my $self = shift;
	return $self->first." ".$self->middle." ".$self->last if $self->middle;
	return $self->first." ".$self->last;
}


sub setting {

	my ($self, $tag, $value, $blob) = @_;
	$/ = ""; #Remove all trailing newlines

	chomp $blob;

	my @existing = Tab::StudentSetting->search(
		student => $self->id,
		tag     => $tag
	);

	my $existing;
	$existing = shift @existing if @existing;

	foreach (@existing) { $_->delete(); }

	if (defined $value) {

		if (
			(not defined $existing)
			&& $value ne "delete" && $value && $value ne "0"
		) {
			$existing = Tab::StudentSetting->create({
				student => $self->id,
				tag     => $tag,
				value   => $value,
			});
		}

		if ($existing) {

			$existing->value($value);

			if ($value eq "text") {
				$existing->value_text($blob)
			} elsif ($value eq "date") {
				$existing->value_date($blob);
			} elsif ($value eq "json") {
				my $json = eval{
					return Tab::Utils::compress(JSON::encode_json($blob));
				};
				$existing->value_text($json);
			}

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete();
			} else {
				$existing->update();
				undef $existing;
			}
		}

		return;

	} else {

		return unless $existing;

		if ($existing->value eq "text") {
			return $existing->value_text
		} elsif ($existing->value eq "date") {
			return $existing->value_date
		} elsif ($existing->value eq "json") {
			return eval {
				return JSON::decode_json(Tab::Utils::decompress($existing->value_text));
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
		from student_setting setting
		where setting.student = ?
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
				return JSON::decode_json(Tab::Utils::decompress($value_text));
			};

		} else {
			$all_settings{$tag} = $value;
		}
	}
	return %all_settings;
}

