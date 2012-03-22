package Tab::Account;
use base 'Tab::DBI';
Tab::Account->table('account');

Tab::Account->columns(Primary => qw/id/);
Tab::Account->columns(Essential => qw/email passhash site_admin multiple/);
Tab::Account->columns(Others => qw/password first last phone paradigm provider started
									street city state zip active is_cell gender
									hotel saw_judgewarn timestamp noemail
									change_pass_key change_deadline tz/);

Tab::Account->columns(TEMP => qw/chname chid/);

Tab::Account->has_many(tourns => 'Tab::AccountAccess', 'account');
Tab::Account->has_many(sessions => 'Tab::Session', 'account');
Tab::Account->has_many(sites => 'Tab::Site', 'host');
Tab::Account->has_many(dioceses => 'Tab::Region', 'director');
Tab::Account->has_many(coach_joins => 'Tab::ChapterAccess', 'account');

Tab::Account->set_sql(tabbers => "select distinct account.* 
									from account,account_access 
									where account.id = account_access.account 
									and account_access.entry != 1
									and account_access.tournament = ?");

Tab::Account->set_sql(coaches => " select distinct account.id 
									from account,chapter_access 
									where chapter_access.chapter= ? 
									and chapter_access.account = account.id");

__PACKAGE__->_register_datetimes( qw/change_deadline/);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub no_interest {
	my ($self, $tourn)  = @_;
	return unless $tourn;
	my @nis = Tab::NoInterest->search( tournament => $tourn->id, account => $self->id );
	return @nis;
}

sub can_alter {

	#Peform a check of a school against an account
	my ($self,$school) = @_;

	#OK if the account is a site administrator
	return if $self->site_admin;

	#OK if the account is a league administrator
	my $league = $school->tournament->league;
	return if Tab::LeagueAdmin->search( account => $self->id, league => $league_id );

	#OK if the account is a coach of the school
	return if Tab::ChapterAccess->search( account => $self->id, chapter => $school->chapter->id );

	$m->print("<p class=\"err\">You are not authorized to make changes to that school's entry");
	$m->abort();

}

sub tournaments {
    my $self = shift;
    my @tourns;
    push (@tourns, Tab::Tournament->search( director => $self->id));
    push (@tourns, Tab::Tournament->search_my_tourns($self->id));
    return @tourns;
}

sub chapters {
    my $self = shift;
    return sort {$a->name cmp $b->name} Tab::Chapter->search_accounts($self->id);
}

sub leagues {
    my $self = shift;
    return sort {$a->name cmp $b->name} Tab::League->search_by_account($self->id);
}

sub league_members { 
	my $self = shift;

	my @members;

	push (@members, Tab::Account->search_by_chapter_and_league($self->id));

	push (@members, Tab::Account->search_by_league_admin($self->id));

	return @members;

}

Tab::Account->set_sql(by_tournament => "select distinct account.*, chapter.name as chname
						from account,chapter_access,school,chapter
						where account.id = chapter_access.account
						and chapter_access.chapter = school.chapter
						and chapter.id = school.chapter
						and school.tournament = ?");

Tab::Account->set_sql(by_league_coach => 
			"select distinct account.*,chapter.name as chname, chapter.id as chid
			from account,chapter_access,chapter,chapter_league
			where account.id = chapter_access.account
			and chapter.id = chapter_access.chapter
			and chapter.id = chapter_league.chapter
			and chapter_league.league = ?
			and chapter_league.full_member = 1");

Tab::Account->set_sql(by_league_admin =>
			"select distinct account.*,\"Admin\" as chname
			from account,league_admin
			where account.id = league_admin.account
			and league_admin.league = ?");


Tab::Account->set_sql(league_and_last => 
			"select distinct account.* 
			from account,league,chapter_access,chapter_league,chapter
			where account.id = chapter_access.account
            and chapter.id = chapter_access.chapter
            and chapter.id = chapter_league.chapter
            and chapter_league.league = ?
			and account.last like ?");

Tab::Account->set_sql(league_and_first => 
			"select distinct account.* 
			from account,league,chapter_access,chapter_league,chapter
			where account.id = chapter_access.account
            and chapter.id = chapter_access.chapter
            and chapter.id = chapter_league.chapter
            and chapter_league.league = ?
			and account.first like ?");

Tab::Account->set_sql(league_and_email => 
			"select distinct account.* 
			from account,league,chapter_access,chapter_league,chapter
			where account.id = chapter_access.account
            and chapter.id = chapter_access.chapter
            and chapter.id = chapter_league.chapter
            and chapter_league.league = ?
			and account.email like ?");

