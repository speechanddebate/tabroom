package Tab::Tourn;
use base 'Tab::DBI';
Tab::Tourn->table('tourn');
Tab::Tourn->columns(Primary => qw/id/);
Tab::Tourn->columns(Essential => qw/name start end director approved method/);

Tab::Tourn->columns(Others => qw/timestamp hidden reg_start reg_end webname
							judge_deadline drop_deadline freeze_deadline
							fine_deadline judge_policy invitename inviteurl
							results bill_packet disclaimer invoice_message
							ballot_message chair_ballot_message web_message
							vlabel hlabel vcorner hcorner/);

Tab::Tourn->has_many(admins => 'Tab::TournAdmin', 'tourn');

Tab::Tourn->has_many(files => 'Tab::File', 'tourn');
Tab::Tourn->has_many(events => 'Tab::Event', 'tourn');
Tab::Tourn->has_many(emails => 'Tab::Email', 'tourn');
Tab::Tourn->has_many(judges => 'Tab::Judge', 'tourn');
Tab::Tourn->has_many(groups => 'Tab::JudgeGroup', 'tourn');
Tab::Tourn->has_many(ratings => 'Tab::Rating', 'tourn');
Tab::Tourn->has_many(entries => 'Tab::Entry', 'tourn');
Tab::Tourn->has_many(strikes => 'Tab::Strike', 'tourn');
Tab::Tourn->has_many(schools => 'Tab::School', 'tourn');
Tab::Tourn->has_many(timeslots => 'Tab::Timeslot', 'tourn');
Tab::Tourn->has_many(concessions => 'Tab::Concession', 'tourn');
Tab::Tourn->has_many(rating_tiers => 'Tab::RatingTier', 'tourn');
Tab::Tourn->has_many(room_strikes => 'Tab::RoomStrike', 'tourn');
Tab::Tourn->has_many(school_fines => 'Tab::SchoolFine', 'tourn');
Tab::Tourn->has_many(tourn_changes => 'Tab::TournChange', 'tourn');
Tab::Tourn->has_many(event_doubles => 'Tab::EventDouble', 'tourn');
Tab::Tourn->has_many(tiebreak_sets => 'Tab::TiebreakSet', 'tourn');
Tab::Tourn->has_many(housing_students => 'Tab::HousingStudent', 'tourn');

__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);
__PACKAGE__->_register_datetimes( qw/reg_start/);
__PACKAGE__->_register_datetimes( qw/reg_end/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline/);
__PACKAGE__->_register_datetimes( qw/drop_deadline/);
__PACKAGE__->_register_datetimes( qw/judge_deadline/);
__PACKAGE__->_register_datetimes( qw/fine_deadline/);

Tab::Tourn->set_sql(by_admin => "
					select distinct tourn.*
					from tourn,tourn_admin 
					where tourn.id = tourn_admin.tourn 
					and tourn_admin.account = ?
					order by tourn.start");

Tab::Tourn->set_sql(by_site => "
		select distinct tourn.id 
		from tourn,tourn_site 
		where tourn.id = tourn_site.tourn 
		and tourn_site.site = ?");

Tab::Tourn->set_sql(open_by_chapter => "
		select distinct tourn.*
		from tourn,circuit,chapter_circuit
		where chapter_circuit.chapter = ?
		and tourn.approved = 1
		and tourn.hidden != 1
		and chapter_circuit.active = 1
		and chapter_circuit.circuit = circuit.id
		and circuit.active = 1
		and circuit.approved = 1
		and circuit.diocese_based != 1
		and circuit.id = tourn.circuit
		and tourn.reg_start < NOW()
		and tourn.reg_end > NOW()
			and not exists (
				select id from school where 
				school.chapter = chapter_circuit.chapter
				and school.tourn = tourn.id
			)
		order by tourn.reg_end DESC;
	");

Tab::Tourn->set_sql(entered_by_chapter => "
		select distinct tourn.*
		from tourn,school
		where school.chapter = ?
		and school.tourn = tourn.id
		and tourn.end > NOW()
		order by tourn.start DESC;
	");

Tab::Tourn->set_sql(results_by_chapter => "
		select distinct tourn.*
		from tourn,school
		where school.chapter = ?
		and school.tourn = tourn.id
		and tourn.results = 1
		order by tourn.end DESC
	");

sub setting {

	my ($self, $tag, $value, $text) = @_;

	my @existing = Tab::TournSetting->search(  
		tourn => $self->id,
		tag => $tag
	);

	if ($value &! $value == 0) { 

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->text($text);
			$exists->update;

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			Tab::JudgeGroupSetting->create({
				judge_group => $self->id,
				tag => $tag,
				value => $value,
				text => $text
			});

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		foreach my $other (@existing) { 
			$other->delete;
		}

		return $setting->text if $setting->value eq "text";
		return $setting->value;

	}

}

