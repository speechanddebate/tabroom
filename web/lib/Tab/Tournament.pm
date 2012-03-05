package Tab::Tournament;
use base 'Tab::DBI';
Tab::Tournament->table('tournament');
Tab::Tournament->columns(Primary => qw/id/);
Tab::Tournament->columns(Essential => qw/name start end league director approved method/);

Tab::Tournament->columns(Others => qw/timestamp hidden reg_start reg_end webname
							judge_deadline drop_deadline freeze_deadline
							fine_deadline judge_policy invitename inviteurl
							results bill_packet disclaimer invoice_message
							ballot_message chair_ballot_message web_message
							vlabel hlabel vcorner hcorner/);

Tab::Tournament->has_a(director => 'Tab::Account');
Tab::Tournament->has_a(league => 'Tab::League');
Tab::Tournament->has_a(method => 'Tab::Method');
Tab::Tournament->has_many(events => 'Tab::Event', 'tournament');
Tab::Tournament->has_many(items => 'Tab::Item', 'tournament');
Tab::Tournament->has_many(news => 'Tab::News', 'tournament');
Tab::Tournament->has_many(emails => 'Tab::Email', 'tournament');
Tab::Tournament->has_many(resultfiles => 'Tab::ResultFile', 'tournament');
Tab::Tournament->has_many(comps => 'Tab::Comp', 'tournament');
Tab::Tournament->has_many(strikes => 'Tab::Strike', 'tournament');
Tab::Tournament->has_many(judges => 'Tab::Judge', 'tournament');
Tab::Tournament->has_many(timeslots => 'Tab::Timeslot', 'tournament');
Tab::Tournament->has_many(changes => 'Tab::Change', 'tournament');
Tab::Tournament->has_many(accounts => 'Tab::AccountAccess', 'tournament');
Tab::Tournament->has_many(classes => 'Tab::Class', 'tournament');
Tab::Tournament->has_many(schools => 'Tab::School', 'tournament');
Tab::Tournament->has_many(tiebreak_sets => 'Tab::TiebreakSet', 'tournament');
Tab::Tournament->has_many(files => 'Tab::File', 'tournament');
Tab::Tournament->has_many(ratings => 'Tab::Rating', 'tournament');
Tab::Tournament->has_many(groups => 'Tab::JudgeGroup', 'tournament');
Tab::Tournament->has_many(quals => 'Tab::Qual', 'tournament');
Tab::Tournament->has_many(roomblocks => 'Tab::RoomBlock', 'tournament');
Tab::Tournament->has_many(bills => 'Tab::Bill', 'tournament');
Tab::Tournament->has_many(fines => 'Tab::Fine', 'tournament');
Tab::Tournament->has_many(sweeps => 'Tab::Sweep', 'tournament');
Tab::Tournament->has_many(housing_requests => 'Tab::Housing', 'tournament');
__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);
__PACKAGE__->_register_datetimes( qw/reg_start/);
__PACKAGE__->_register_datetimes( qw/reg_end/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline/);
__PACKAGE__->_register_datetimes( qw/drop_deadline/);
__PACKAGE__->_register_datetimes( qw/judge_deadline/);
__PACKAGE__->_register_datetimes( qw/fine_deadline/);

Tab::Tournament->set_sql(my_tourns => "
		select distinct tournament.id 
		from tournament,account_access 
		where tournament.id = account_access.tournament 
		and account_access.account = ?");

Tab::Tournament->set_sql(by_site => "
		select distinct tournament.id 
		from tournament,tournament_site 
		where tournament.id = tournament_site.tournament 
		and tournament_site.site = ?");

Tab::Tournament->set_sql(open_by_chapter => "
		select distinct tournament.*
		from tournament,league,chapter_league
		where chapter_league.chapter = ?
		and tournament.approved = 1
		and tournament.hidden != 1
		and chapter_league.active = 1
		and chapter_league.league = league.id
		and league.active = 1
		and league.approved = 1
		and league.diocese_based != 1
		and league.id = tournament.league
		and tournament.reg_start < NOW()
		and tournament.reg_end > NOW()
			and not exists (
				select id from school where 
				school.chapter = chapter_league.chapter
				and school.tournament = tournament.id
			)
		order by tournament.reg_end DESC;
	");

Tab::Tournament->set_sql(entered_by_chapter => "
		select distinct tournament.*
		from tournament,school
		where school.chapter = ?
		and school.tournament = tournament.id
		and tournament.end > NOW()
		order by tournament.start DESC;
	");

Tab::Tournament->set_sql(results_by_chapter => "
		select distinct tournament.*
		from tournament,school
		where school.chapter = ?
		and school.tournament = tournament.id
		and tournament.results = 1
		order by tournament.end DESC
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

