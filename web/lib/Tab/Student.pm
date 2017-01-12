package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/person first middle last chapter novice 
									 grad_year retired gender person_request diet/);
Tab::Student->columns(Other => qw/timestamp phonetic race birthdate school_sid ualt_id/);
Tab::Student->columns(TEMP => qw/code entry event school region/);

Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_a(person => 'Tab::Person');
Tab::Student->has_a(person_request => 'Tab::Person');

Tab::Student->has_many(settings => 'Tab::StudentSetting', 'student');
Tab::Student->has_many(entries => [ Tab::EntryStudent => 'entry']);
Tab::Student->has_many(entry_students => 'Tab::EntryStudent', 'student');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_dates( qw/birthdate/);


sub housing { 
	my ($self, $tourn, $day) = @_;
	my @housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id, night => $day->ymd ) if $day;
	@housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id ) unless $day;
	return shift @housings if $day;
	return @housings;
}

sub fullname { 
	my $self = shift;
	return $self->first." ".$self->middle." ".$self->last if $self->middle;
	return $self->first." ".$self->last;
}


sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = ""; # Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::StudentSetting->search(  
		student => $self->id,
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

			my $existing = Tab::StudentSetting->create({
				student => $self->id,
				tag     => $tag,
				value   => $value,
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

	my @settings = $self->settings;

	my %all_settings;

	foreach my $setting (@settings) { 
		$all_settings{$setting->tag} = $setting->value;
		$all_settings{$setting->tag} = $setting->value_text if $all_settings{$setting->tag} eq "text";
		$all_settings{$setting->tag} = $setting->value_date if $all_settings{$setting->tag} eq "date";
	}

	return %all_settings;

}


