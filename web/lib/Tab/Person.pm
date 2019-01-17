package Tab::Person;
use base 'Tab::DBI';

Tab::Person->table('person');
Tab::Person->columns(Primary   => qw/id/);

Tab::Person->columns(Essential => qw/email first middle last phone provider site_admin /);

Tab::Person->columns(Others    => qw/street city state zip country postal
                                   gender pronoun no_email tz diversity 
									ualt_id nsda timestamp googleplus/);

Tab::Person->columns(TEMP => qw/prefs student_id judge_id/);

Tab::Person->has_many(logins => 'Tab::Login', 'person');

Tab::Person->has_many(settings => 'Tab::PersonSetting', 'person');
Tab::Person->has_many(sessions => 'Tab::Session', 'person');

Tab::Person->has_many(sites => 'Tab::Site', 'host');
Tab::Person->has_many(conflicts => 'Tab::Conflict', 'person');
Tab::Person->has_many(conflicteds => 'Tab::Conflict', 'conflicted');

Tab::Person->has_many(followers => 'Tab::Follower', 'person');
Tab::Person->has_many(follow_persons => 'Tab::Follower', 'follower');

Tab::Person->has_many(chapter_judges => 'Tab::ChapterJudge', 'person');
Tab::Person->has_many(judges => 'Tab::Judge', 'person' => { order_by => 'id DESC'} );

Tab::Person->has_many(students => 'Tab::Student', 'person');
Tab::Person->has_many(ignores => [ Tab::TournIgnore => 'tourn']);

Tab::Person->has_many(permissions => 'Tab::Permission', 'person');
Tab::Person->has_many(circuits => [ Tab::Permission => 'circuit']);
Tab::Person->has_many(tourns => [ Tab::Permission => 'tourn']);
Tab::Person->has_many(chapters => [ Tab::Permission => 'chapter']);
Tab::Person->has_many(regions => [ Tab::Permission => 'region']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::PersonSetting->search(  
		person => $self->id,
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

			my $existing = Tab::PersonSetting->create({
				person => $self->id,
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

sub all_permissions {

	my $self = shift;

	return unless $self;

	my $tourn = shift;

	my %perms;

	if ($tourn) { 

		Tab::Permission->set_sql( tourn_perms => "
			select permission.*
			from permission
			where permission.person = ? 
			and permission.tourn = ? 
		");

		my @tourn_perms =  Tab::Permission->search_tourn_perms( 
			$self->id,
			$tourn->id
		);

		foreach my $perm (@tourn_perms) { 

			my $tag = $perm->tag;

			if ($tag eq "detailed") { 

				$perms{$tag} = $perm;
				$perms{"tourn"}{$tourn->id} = $tag;
				$perms{"details"} = eval { 
					return JSON::decode_json($perm->details);
				};

			} else { 
				$perms{"tourn"}{$tourn->id} = $tag;
				$perms{"undetailed"}{$tourn->id}++;
				$perms{$tag} = $perm;
			}
		}

		if ($self && $self->site_admin) { 
			$perms{"owner"}++;
			delete $perms{"entry_only"};
			delete $perms{"details"};
		}   
				
		if ($perms{"undetailed"}{$tourn->id}) { 
			# Universal perms override specific ones
			delete $perms{"details"};
			delete $perms{"detailed"};
		}
	}

	Tab::Permission->set_sql( other_perms => "
		select permission.*
		from permission
		where permission.person = ? 
		and (permission.tourn is null or permission.tourn = 0)
	");

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select permission.tag, permission.region, permission.circuit, 
			permission.chapter, permission.district
		from permission
		where permission.person = ? 
			and (permission.tourn is null or permission.tourn = 0)
    ");
    
    $sth->execute($self->id);
    
    while( 
		my ($tag, $region, $circuit, $chapter, $district)  = $sth->fetchrow_array() 
	) { 

		if ($district) { 
			$perms{"district"}{$district} = $tag;
		}

		if ($region) {
			$perms{"region"}{$region} = $tag;
		}

		if ($circuit) {
			$perms{"circuit"}{$circuit} = $tag;
		}

		if ($chapter) { 
			$perms{"chapter"}{$chapter} = $tag;
		}
	}

	$perms{"owner"}++ if $self->site_admin;

	return %perms;

}

sub all_settings { 

	my $self = shift;

	my %all_settings;

	my $dbh = Tab::DBI->db_Main();

    my $sth = $dbh->prepare("
		select setting.tag, setting.value, setting.value_date, setting.value_text
		from person_setting setting
		where setting.person = ? 
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

