package Tab::Chapter;
use base 'Tab::DBI';
Tab::Chapter->table('chapter');
Tab::Chapter->columns(Primary => qw/id/);
Tab::Chapter->columns(Essential => qw/name formal street city state zip postal country/);
Tab::Chapter->columns(Others => qw/level naudl nsda district timestamp/);
Tab::Chapter->columns(TEMP => qw/count prefs code member schoolid/);

Tab::Chapter->has_a(district => 'Tab::District');

Tab::Chapter->has_many(schools => 'Tab::School', 'chapter');
Tab::Chapter->has_many(students => 'Tab::Student', 'chapter' => { order_by => 'last'});
Tab::Chapter->has_many(chapter_judges => 'Tab::ChapterJudge', 'chapter' => { order_by => 'last'});
Tab::Chapter->has_many(chapter_circuits => 'Tab::ChapterCircuit', 'chapter');
Tab::Chapter->has_many(chapter_settings => 'Tab::ChapterSetting', 'chapter');
Tab::Chapter->has_many(settings => 'Tab::ChapterSetting', 'chapter');

Tab::Chapter->has_many(admins => [ Tab::Permission => 'person']);
Tab::Chapter->has_many(persons => [ Tab::Permission => 'person']);
Tab::Chapter->has_many(circuits => [ Tab::ChapterCircuit => 'circuit']);
Tab::Chapter->has_many(permissions => 'Tab::Permission', 'chapter');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub location {
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub full_member {
    my ($self, $circuit) = @_;
	my @membership = Tab::ChapterCircuit->search(
		chapter => $self->id,
		circuit => $circuit->id
	);

    return $membership[0]->full_member if @membership;
    return;
}

sub circuit_code {
    my ($self, $circuit, $code) = @_;
    my @membership = Tab::ChapterCircuit->search(
		chapter => $self->id,
		circuit => $circuit->id
	);

	if ($code) {
		$membership[0]->code($code);
		$membership[0]->update;
		return;
	}

    return $membership[0]->code if @membership;
    return;
}

sub region {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search(
		chapter => $self->id,
		circuit => $circuit->id
	);
    return $membership[0]->region if @membership;
    return;
}

sub circuit_membership {
    my ($self, $circuit) = @_;
    my @membership = Tab::ChapterCircuit->search(
		chapter => $self->id,
		circuit => $circuit->id
	);
    return $membership[0] if @membership;
    return;
}

sub short_name {
	my ($self, $limit) = @_;
	return &Tab::short_name($self->name, $limit);
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::ChapterSetting->search(
		chapter => $self->id,
		tag     => $tag,
	)->first;

	if (defined $value) {

		if ($existing) {

			if ($value eq "delete" || $value eq "" || $value eq "0") {
				$existing->delete;
			} else {
				$existing->value($value);
				$existing->value_text($blob) if $value eq "text";
				$existing->value_date($blob) if $value eq "date";
				$existing->update;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::ChapterSetting->create({
				chapter => $self->id,
				tag    => $tag,
				value  => $value,
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

sub all_settings {

	my $self = shift;

	my %all_settings;

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select setting.tag, setting.value, setting.value_date, setting.value_text
		from chapter_setting setting
		where setting.chapter = ?
        order by setting.tag
    ");

    $sth->execute($self->id);

    while( my ($tag, $value, $value_date, $value_text)  = $sth->fetchrow_array() ) {

		if ($value eq "date") {

			my $dt = Tab::DBI::dateparse($value_date);
			$all_settings{$tag} = $dt if $dt;

		} elsif ($value eq "text") {

			$all_settings{$tag} = $value_text;

		} else {

			$all_settings{$tag} = $value;

		}

	}

	return %all_settings;

}
