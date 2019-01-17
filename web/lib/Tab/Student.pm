package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/person first middle last chapter novice 
									 grad_year retired gender person_request diet/);
Tab::Student->columns(Other => qw/timestamp phonetic race birthdate school_sid ualt_id nsda/);
Tab::Student->columns(TEMP => qw/code entry event school region status/);

Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_a(person => 'Tab::Person');
Tab::Student->has_a(person_request => 'Tab::Person');

Tab::Student->has_many(followers => 'Tab::Follower', 'student');

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

		} else { 

			$all_settings{$tag} = $value;

		}

	}

	return %all_settings;

}

