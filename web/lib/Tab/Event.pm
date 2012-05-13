package Tab::Event;
use base 'Tab::DBI';
Tab::Event->table('event');
Tab::Event->columns(Primary => qw/id/);
Tab::Event->columns(Essential => qw/tourn name abbr judge_group type/);
Tab::Event->columns(Others =>  qw/fee rating_subset event_double timestamp/);

__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Event->has_a(tourn => 'Tab::Tourn');
Tab::Event->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Event->has_a(event_double => 'Tab::EventDouble');
Tab::Event->has_a(rating_subset => 'Tab::RatingSubset');

Tab::Event->has_many(room_pools => 'Tab::RoomPool', 'event');
Tab::Event->has_many(settings => "Tab::EventSetting", "event");

Tab::Event->has_many(entries => 'Tab::Entry', 'event' => { order_by => 'code'} );
Tab::Event->has_many(rounds => 'Tab::Round', 'event' => { order_by => 'name'}  );
Tab::Event->has_many(panels => 'Tab::Panel', 'event'  => { order_by => 'letter'} );

sub sites {
	my $self = shift;
	return Tab::Site->search_by_event($self->id);
}

sub active { 
	my $self = shift;
	return Tab::Entry->search_active_by_event($self->id);
}

sub waitlisted { 
	my $self = shift;
	return Tab::Entry->search_waitlist_by_event($self->id);
}

sub judges {
    my $self = shift;
    my @judges = Tab::Judge->search_by_event($self->id);
    return @judges;
}

sub schools {
    my $self = shift;
    return Tab::School->search_by_event($self->id);
}

sub dioceses {
	my $self = shift;
    return Tab::Region->search_by_event($self->id);
}

sub ballots { 
	my $self = shift;
	return Tab::Ballot->search_by_event($self->id);
}

sub next_code {

    my $self = shift;
    my @existing_entries = Tab::Entry->search( event => $self->id, {order_by => "code DESC"} );
	my $code = $self->code; 
	
	while (defined $self->tourn->entry_with_code($code)) {
		$code++;
		$code++ if $code == 666;
		$code++ if $code == 69;
	}

	return $code;

}

sub students { 
	my $self = shift;
    return Tab::Student->search_by_event($self->id);
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::EventSetting->search(  
		event => $self->id,
		tag => $tag
	);

    if ($value && $value ne 0) {

		if (@existing) {

			my $exists = shift @existing;

			$exists->value($value);


			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			Tab::EventSetting->create({
				event => $self->id,
				tag => $tag,
				value => $value,
			});

		}

		if ($value eq "text") { 
			$exists->value_text($blob);
		}

		if ($value eq "date") { 
			$exists->value_date($blob);
		}

		$exists->update;

	} else {

		return unless @existing;

		my $setting = shift @existing;
		return $setting->value_text if $setting->value eq "text";
		return $setting->value_date if $setting->value eq "date";
		return $setting->value;

	}

}
