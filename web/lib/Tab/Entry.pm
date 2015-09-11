package Tab::Entry;
use base 'Tab::DBI';
Tab::Entry->table('entry');
Tab::Entry->columns(Primary => qw/id/);
Tab::Entry->columns(Essential => qw/code dropped name school tourn event waitlist dq/);
Tab::Entry->columns(Others => qw/ada created_at timestamp tba/);
Tab::Entry->columns(TEMP => qw/panelid speaks side ballot othername schname regname regcode region pullup bracketseed won lost/);

Tab::Entry->has_a(school => 'Tab::School');
Tab::Entry->has_a(tourn => 'Tab::Tourn');
Tab::Entry->has_a(event => 'Tab::Event');

Tab::Entry->has_many(strikes => 'Tab::Strike', 'entry');
Tab::Entry->has_many(ballots => 'Tab::Ballot', 'entry');
Tab::Entry->has_many(changes => 'Tab::TournChange', 'entry');
Tab::Entry->has_many(ratings => 'Tab::Rating', 'entry');
Tab::Entry->has_many(qualifiers => 'Tab::Qualifier', 'entry');
Tab::Entry->has_many(entry_students => 'Tab::EntryStudent', 'entry');
Tab::Entry->has_many(students => [Tab::EntryStudent => 'student']);

__PACKAGE__->_register_datetimes( qw/created_at timestamp/);

sub add_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student, entry => $self->id );
	Tab::EntryStudent->create({ student => $student, entry => $self->id }) unless @existing;
	return;
}

sub rm_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student, entry => $self->id );
	foreach (@existing) { $_->delete; }
	return;
}


sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::EntrySetting->search(  
		entry => $self->id,
		tag    => $tag,
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

			my $existing = Tab::EntrySetting->create({
				entry => $self->id,
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

