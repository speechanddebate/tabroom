package Tab::Event;
use base 'Tab::DBI';
Tab::Event->table('event');
Tab::Event->columns(Primary => qw/id/);
Tab::Event->columns(Essential => qw/tourn name abbr category 
									type fee rating_subset pattern 
									timestamp/);
Tab::Event->columns(TEMP => qw/panelid supp conn/);

__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Event->has_a(tourn => 'Tab::Tourn');
Tab::Event->has_a(category => 'Tab::Category');
Tab::Event->has_a(pattern => 'Tab::Pattern');
Tab::Event->has_a(rating_subset => 'Tab::RatingSubset');

Tab::Event->has_many(files => 'Tab::File', 'event');
Tab::Event->has_many(settings => "Tab::EventSetting", "event");
Tab::Event->has_many(result_sets => "Tab::ResultSet", "event");

Tab::Event->has_many(entries => 'Tab::Entry', 
						'event' => { order_by => 'code'} );

Tab::Event->has_many(rounds => 'Tab::Round',
						'event' => { order_by => 'name'}  );

sub setting {

	my ($self, $tag, $value, $blob) = @_;
	$/ = ""; #Remove all trailing newlines

	chomp $blob;

	my @existing = Tab::EventSetting->search(
		event => $self->id,
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

			my $existing = Tab::EventSetting->create({
				event => $self->id,
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
				$existing->value_text($jsonref);
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
			my $ref = eval { 
				return JSON::decode_json($existing->value_text);
			};
			return $ref;
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
		from event_setting setting
		where setting.event = ? 
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

			my $jsonref = eval { 
				JSON::decode_json($value_text);
			};
			$all_settings{$tag} = $jsonref;

		} else { 
			$all_settings{$tag} = $value;
		}
	}
	return %all_settings;
}

