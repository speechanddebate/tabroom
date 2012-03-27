package Tab::Account;
use base 'Tab::DBI';
Tab::Account->table('account');

Tab::Account->columns(Primary =>   qw/id/);
Tab::Account->columns(Essential => qw/email passhash site_admin multiple/);
Tab::Account->columns(Others =>    qw/first last phone street city state zip provider paradigm started gender
								   timestamp no_email change_pass_key password_timestamp tz/);

__PACKAGE__->_register_datetimes( qw/password_timestamp/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

Tab::Account->has_many(sessions => 'Tab::Session', 'account');
Tab::Account->has_many(sites => 'Tab::Site', 'host');

Tab::Account->has_many(tourn_admins => 'Tab::TournAdmin', 'account');
Tab::Account->has_many(chapter_admins => 'Tab::ChapterAdmin', 'account');
Tab::Account->has_many(circuit_admins => 'Tab::CircuitAdmin', 'account');
Tab::Account->has_many(region_admins => 'Tab::RegionAdmin', 'account');

Tab::Account->has_many(followers => 'Tab::FollowAccount', 'account');
Tab::Account->has_many(follow_accounts => 'Tab::FollowAccount', 'follower');
Tab::Account->has_many(chapter_judges => 'Tab::ChapterJudge', 'account');

Tab::Account->has_many(judges => 'Tab::Judge', 'account');
Tab::Account->has_many(follow_judge => 'Tab::FollowJudge', 'account');

Tab::Account->has_many(entries => 'Tab::Entry', 'account');
Tab::Account->has_many(follow_entry => 'Tab::FollowEntry', 'entry');

sub tourns {
    my $self = shift;
    return Tab::Tourn->search_by_admin($self->id);
}

sub chapters {
    my $self = shift;
    return Tab::Chapter->search_by_admin($self->id);
}

sub circuits {
    my $self = shift;
    return Tab::Circuit->search_by_admin($self->id);
}

sub regions { 
	my $self = shift;
	return Tab::Region->search_by_admin($self->id);
}

sub account_ignore {
	my ($self, $tourn)  = @_;
	return unless $tourn;
	my @nis = Tab::AccountIgnore->search( tourn => $tourn->id, account => $self->id );
	return @nis;
}

sub can_alter {

	my ($self,$school) = @_;

	return if $self->site_admin;
	return if Tab::ChapterAdmin->search( account => $self->id, chapter => $school->chapter->id );
	return if Tab::CircuitAdmin->search( account => $self->id, circuit => $school->tourn->circuit->id );

	$m->print("<p class=\"err\">You are not authorized to make changes to that school's entry.  Hit back and try again.</p>");
	$m->abort();
}

Tab::Account->set_sql(by_circuit_coach => 
				"select distinct account.*
					from account,chapter_admin,chapter,chapter_circuit
						where account.id = chapter_admin.account
						and chapter.id = chapter_admin.chapter
						and chapter.id = chapter_circuit.chapter
						and chapter_circuit.circuit = ?
						and chapter_circuit.full_member = 1");

Tab::Account->set_sql(by_circuit_admin =>
					"select distinct account.*
						from account,circuit_admin
						where account.id = circuit_admin.account
						and circuit_admin.circuit = ?");

Tab::Account->set_sql(by_chapter_admin => 
					"select distinct account.*
						from account,chapter_admin 
						where account.id = chapter_admin.account
						and chapter_admin.chapter= ?");

