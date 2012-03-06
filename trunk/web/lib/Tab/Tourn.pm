package Tab::Tourn;
use base 'Tab::DBI';
Tab::Tourn->table('tourn');
Tab::Tourn->columns(Primary => qw/id/);
Tab::Tourn->columns(Essential => qw/name start end league director approved method/);

Tab::Tourn->columns(Others => qw/timestamp hidden reg_start reg_end webname
							judge_deadline drop_deadline freeze_deadline
							fine_deadline judge_policy invitename inviteurl
							results bill_packet disclaimer invoice_message
							ballot_message chair_ballot_message web_message
							vlabel hlabel vcorner hcorner/);

Tab::Tourn->has_a(director => 'Tab::Account');
Tab::Tourn->has_a(league => 'Tab::League');
Tab::Tourn->has_a(method => 'Tab::Method');
Tab::Tourn->has_many(events => 'Tab::Event', 'tourn');
Tab::Tourn->has_many(items => 'Tab::Item', 'tourn');
Tab::Tourn->has_many(news => 'Tab::News', 'tourn');
Tab::Tourn->has_many(emails => 'Tab::Email', 'tourn');
Tab::Tourn->has_many(resultfiles => 'Tab::ResultFile', 'tourn');
Tab::Tourn->has_many(entrys => 'Tab::Entry', 'tourn');
Tab::Tourn->has_many(strikes => 'Tab::Strike', 'tourn');
Tab::Tourn->has_many(judges => 'Tab::Judge', 'tourn');
Tab::Tourn->has_many(timeslots => 'Tab::Timeslot', 'tourn');
Tab::Tourn->has_many(changes => 'Tab::Change', 'tourn');
Tab::Tourn->has_many(accounts => 'Tab::AccountAccess', 'tourn');
Tab::Tourn->has_many(classes => 'Tab::Class', 'tourn');
Tab::Tourn->has_many(schools => 'Tab::School', 'tourn');
Tab::Tourn->has_many(tiebreak_sets => 'Tab::TiebreakSet', 'tourn');
Tab::Tourn->has_many(files => 'Tab::File', 'tourn');
Tab::Tourn->has_many(ratings => 'Tab::Rating', 'tourn');
Tab::Tourn->has_many(groups => 'Tab::JudgeGroup', 'tourn');
Tab::Tourn->has_many(quals => 'Tab::Qual', 'tourn');
Tab::Tourn->has_many(roomblocks => 'Tab::RoomBlock', 'tourn');
Tab::Tourn->has_many(bills => 'Tab::Bill', 'tourn');
Tab::Tourn->has_many(fines => 'Tab::Fine', 'tourn');
Tab::Tourn->has_many(sweeps => 'Tab::Sweep', 'tourn');
Tab::Tourn->has_many(housing_requests => 'Tab::Housing', 'tourn');
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
		from tourn,league,chapter_league
		where chapter_league.chapter = ?
		and tourn.approved = 1
		and tourn.hidden != 1
		and chapter_league.active = 1
		and chapter_league.league = league.id
		and league.active = 1
		and league.approved = 1
		and league.diocese_based != 1
		and league.id = tourn.league
		and tourn.reg_start < NOW()
		and tourn.reg_end > NOW()
			and not exists (
				select id from school where 
				school.chapter = chapter_league.chapter
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

