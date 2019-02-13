package Tab::Panel;
use base 'Tab::DBI';
Tab::Panel->table('panel');
Tab::Panel->columns(Primary => qw/id/);
Tab::Panel->columns(Essential => qw/letter round room flight bye bracket started/);
Tab::Panel->columns(Others => qw/timestamp confirmed publish label
								room_ext_id g_event invites_sent
								cat_id score /);
Tab::Panel->columns(TEMP => qw/opp pos side entryid judge audit 
								timeslotid roomname eventname judgenum 
								panelsize ada speakerorder
/);

Tab::Panel->has_a(room => 'Tab::Room');
Tab::Panel->has_a(round => 'Tab::Round');

Tab::Panel->has_many(ballots => 'Tab::Ballot', 'panel');
Tab::Panel->has_many(student_votes => 'Tab::StudentVote', 'panel');

Tab::Panel->has_many(student_ballots => 'Tab::StudentBallot', 'panel' => { order_by => 'value'});

__PACKAGE__->_register_datetimes( qw/started timestamp confirmed/);

__PACKAGE__->add_trigger(after_set_room => \&new_hangout);

# Set appropriate "room ID" value depending on what room was selected
sub new_hangout() {
    my $self = shift;
    if ($self->room == -1) {
        # Hangout room selected
        if (!$self->g_event) {
            # No GCal event? Means we may have just switched to a Hangout room
            # So clear out HOA link and invitation record
            $self->room_ext_id(undef);
            $self->invites_sent(0);
        }
    } elsif ($self->room == -2) {
        # Hangout On Air room selected
        if ($self->g_event) {
            # We just switched to a HOA room from a Hangout so...
            # Delete Hangout event
            Tab::GoogleCalendar->deleteEvent($self->g_event);
            $self->g_event(undef);
            $self->room_ext_id(undef);
            $self->invites_sent(0);
	} elsif (!$self->room_ext_id) {
            # We may have just switched to a HOA room
            # Clear out invitation record
            $self->invites_sent(0);
        }
    } else {
        # If the room isn't a Hangout of either kind, Hangout info should be blank
        if ($self->g_event) {
            # We just switched from a Hangout so...
            # Delete Hangout event
            Tab::GoogleCalendar->deleteEvent($self->g_event);
            $self->g_event(undef);
        }
        $self->room_ext_id(undef);
        $self->invites_sent(0);
    }
}

