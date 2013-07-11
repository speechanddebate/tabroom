package Tab::Event;
use base 'Tab::DBI';
Tab::Event->table('event');
Tab::Event->columns(Primary => qw/id/);
Tab::Event->columns(Essential => qw/tourn name abbr judge_group type fee rating_subset event_double timestamp/);

__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Event->has_a(tourn => 'Tab::Tourn');
Tab::Event->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Event->has_a(event_double => 'Tab::EventDouble');
Tab::Event->has_a(rating_subset => 'Tab::RatingSubset');

Tab::Event->has_many(files => 'Tab::File', 'event');

Tab::Event->has_many(room_pools => 'Tab::RoomPool', 'event');
Tab::Event->has_many(settings => "Tab::EventSetting", "event");

Tab::Event->has_many(result_sets => "Tab::ResultSet", "event");

Tab::Event->has_many(entries => 'Tab::Entry', 'event' => { order_by => 'code'} );
Tab::Event->has_many(rounds => 'Tab::Round', 'event' => { order_by => 'name'}  );


sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing;

	if ($tag eq "tab_rating_priority") { 

		@existing = Tab::EventSetting->search(  
			event => $self->id,
			tag => $tag,
			value => $value
		);

	} else { 
	
		@existing = Tab::EventSetting->search(  
			event => $self->id,
			tag => $tag
		);

	}

    if (defined $value && ($tag ne "tab_rating_priority" || $blob)) {

		if (@existing) {
			
			my $exists = shift @existing;

			if ($value eq "delete" || $value eq "" || $value eq 0) { 

				$exists->delete;

			} else { 

				if ($value eq "text") { 
					$exists->value_text($blob);
				} elsif ($value eq "date") { 
					$exists->value_date($blob);
				} elsif ($tag eq "tab_rating_priority") { 
					$exists->value($value);
					$exists->value_text($blob);
				} else { 
					$exists->value($value);
				}

				$exists->update;
			}

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $exists = Tab::EventSetting->create({
				event => $self->id,
				tag => $tag,
				value => $value,
			});

			if ($blob) { 

				if ($value eq "text") { 
					$exists->value_text($blob);
					$exists->update;
				}

				if ($value eq "date") { 
					$exists->value_date($blob);
					$exists->update;
				}

				if ($tag eq "tab_rating_priority") { 
					$exists->value_text($blob);
					$exists->update;
				}

			} else { 
	
				$exists->update;
			}

		}


	} else {

		return unless @existing;

		my $setting = shift @existing;
		return $setting->value_text if $setting->value eq "text";
		return $setting->value_date if $setting->value eq "date";
		return $setting->value_text if $setting->tag eq "tab_rating_priority";
		return $setting->value;

	}

}
