package Tab::Pool;
use base 'Tab::DBI';
Tab::Pool->table('pool');
Tab::Pool->columns(Primary => qw/id/);
Tab::Pool->columns(Essential => qw/name judge_group standby timestamp/);
Tab::Pool->columns(Others => qw/site publish registrant burden event_based/);

Tab::Pool->has_a(site => 'Tab::Site');
Tab::Pool->has_a(judge_group => "Tab::JudgeGroup");

Tab::Pool->has_many(rounds => 'Tab::Round', 'pool');
Tab::Pool->has_many(pool_judges => 'Tab::PoolJudge', 'pool');
Tab::Pool->has_many(judges => [Tab::PoolJudge => 'judge']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub judges {
    my $self = shift;
	my @judges = Tab::Judge->search_by_pool($self->id);
	return @judges;
}

