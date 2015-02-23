package Tab::Tourn;
use base 'Tab::DBI';
Tab::Tourn->table('tourn');
Tab::Tourn->columns(Primary => qw/id/);
Tab::Tourn->columns(Essential => qw/name start end webname reg_start reg_end created_by
									tz state country hidden timestamp foreign_site foreign_id/);
Tab::Tourn->columns(TEMP => qw/schoolid/);

Tab::Tourn->has_many(files => 'Tab::File', 'tourn');
Tab::Tourn->has_many(pools => 'Tab::Pool', 'tourn');
Tab::Tourn->has_many(emails => 'Tab::Email', 'tourn');
Tab::Tourn->has_many(hotels => 'Tab::Hotel', 'tourn' => {order_by => 'name'} );
Tab::Tourn->has_many(events => 'Tab::Event', 'tourn' => { order_by => 'name'} );
Tab::Tourn->has_many(entries => 'Tab::Entry', 'tourn');
Tab::Tourn->has_many(ratings => 'Tab::Rating', 'tourn');
Tab::Tourn->has_many(regions => 'Tab::Region', 'tourn');
Tab::Tourn->has_many(strikes => 'Tab::Strike', 'tourn');
Tab::Tourn->has_many(schools => 'Tab::School', 'tourn' => { order_by => 'name'} );
Tab::Tourn->has_many(housings => 'Tab::Housing', 'tourn');
Tab::Tourn->has_many(webpages => 'Tab::Webpage', 'tourn');
Tab::Tourn->has_many(groups => 'Tab::JudgeGroup', 'tourn' => { order_by => 'name'} );
Tab::Tourn->has_many(timeslots => 'Tab::Timeslot', 'tourn' => { order_by => 'start'} );
Tab::Tourn->has_many(sweep_sets => 'Tab::SweepSet', 'tourn' => {order_by => 'name'} );
Tab::Tourn->has_many(tourn_fees => 'Tab::TournFee', 'tourn');
Tab::Tourn->has_many(settings => 'Tab::TournSetting', 'tourn');
Tab::Tourn->has_many(result_sets => "Tab::ResultSet", 'tourn');
Tab::Tourn->has_many(tourn_sites => 'Tab::TournSite', 'tourn');
Tab::Tourn->has_many(room_groups => 'Tab::RoomGroup', 'tourn');
Tab::Tourn->has_many(concessions => 'Tab::Concession', 'tourn' => { order_by => 'name'} );
Tab::Tourn->has_many(tourn_admins => 'Tab::TournAdmin', 'tourn');
Tab::Tourn->has_many(room_strikes => 'Tab::RoomStrike', 'tourn');
Tab::Tourn->has_many(school_fines => 'Tab::SchoolFine', 'tourn');
Tab::Tourn->has_many(judge_groups => 'Tab::JudgeGroup', 'tourn');
Tab::Tourn->has_many(follow_tourns => 'Tab::FollowTourn', 'tourn');
Tab::Tourn->has_many(tourn_changes => 'Tab::TournChange', 'tourn');
Tab::Tourn->has_many(event_doubles => 'Tab::EventDouble', 'tourn');
Tab::Tourn->has_many(tiebreak_sets => 'Tab::TiebreakSet', 'tourn');
Tab::Tourn->has_many(housing_slots => 'Tab::HousingSlots', 'tourn');
Tab::Tourn->has_many(tourn_circuits => 'Tab::TournCircuit', 'tourn');

Tab::Tourn->has_many(admins => [ Tab::TournAdmin => 'account']);
Tab::Tourn->has_many(sites => [Tab::TournSite => 'site']);
Tab::Tourn->has_many(circuits => [Tab::TournCircuit => 'circuit']);

__PACKAGE__->_register_datetimes( qw/start end reg_start reg_end timestamp/);


sub location { 
	my $self = shift;
	my $location = $self->state."/" if $self->state;
	return $location.$self->country;
}

sub location_name { 
	my $self = shift;
	my $state = $m->comp("/funclib/state_translate.mas", state => $self->state) if $self->state;
	my $country = $m->comp("/funclib/country_translate.mas", country => $self->country) if $self->country;
	$country = $state.", ".$country if $state;
	return $country;
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my @existing = Tab::TournSetting->search(  
		tourn => $self->id,
		tag => $tag
	);

	if (defined $value) { 
			
		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->value_text($blob) if $value eq "text";
			$exists->value_date($blob) if $value eq "date";
			$exists->update;


			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$exists->delete;
			}

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $exists = Tab::TournSetting->create({
				tourn => $self->id,
				tag => $tag,
				value => $value,
			});

			if ($value eq "text") { 
				$exists->value_text($blob);
			}

			if ($value eq "date") { 
				$exists->value_date($blob);
			}

			$exists->update;

		}

	} else {

		return unless @existing;
		my $setting = shift @existing;
		return $setting->value_text if $setting->value eq "text";
		return $setting->value_date if $setting->value eq "date";
		return $setting->value;

	}

}

