package Tab::Strike;
use base 'Tab::DBI';
Tab::Strike->table('strike');
Tab::Strike->columns(Primary => qw/id/);
Tab::Strike->columns(Essential => qw/tourn judge type start end 
										event entry school region bin 
										timestamp strike/);

Tab::Strike->has_a(judge => 'Tab::Judge');
Tab::Strike->has_a(tourn => 'Tab::Tourn');
Tab::Strike->has_a(event => 'Tab::Event');
Tab::Strike->has_a(entry => 'Tab::Entry');
Tab::Strike->has_a(bin => 'Tab::Bin');
Tab::Strike->has_a(school => 'Tab::School');
Tab::Strike->has_a(region => 'Tab::Region');
__PACKAGE__->_register_datetimes( qw/start/ );
__PACKAGE__->_register_datetimes( qw/end/);

Tab::Strike->set_sql(by_group => " 
			select distinct strike.* from strike,judge
				where strike.judge = judge.id
				and judge.judge_group = ? 
			");

sub name { 

	my $self = shift;
	my $name;

	$name = "No rounds in ".$self->event->name if $self->type eq "event";

	$name = "No prelims in ".$self->event->name if $self->type eq "elim";

	$name = "No one from ".$self->school->short_name if $self->type eq "school";

	$name = "No one from ".$self->region->name." (".$self->region->code.")" if $self->type eq "region";

	$name = "Speaker ".$self->entry->code." ".$self->entry->team_name if $self->type eq "entry";

	$name = "Unavailable between ".  Tab::niceshortdt($self->start->set_time_zone($self->tourn->league->timezone))
			." and ".  Tab::niceshortdt($self->end->set_time_zone($self->tourn->league->timezone)) if $self->type eq "time";

	return $name;

}


