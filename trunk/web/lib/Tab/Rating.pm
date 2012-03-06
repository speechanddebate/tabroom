package Tab::Rating;
use base 'Tab::DBI';
Tab::Rating->table('rating');
Tab::Rating->columns(Primary => qw/id/);
Tab::Rating->columns(Essential => qw/tourn judge type qual subset comp school entered name/);
Tab::Rating->columns(TEMP => qw/qual_val/);

Tab::Rating->has_a(judge => 'Tab::Judge');
Tab::Rating->has_a(tourn => 'Tab::Tourn');
Tab::Rating->has_a(comp => 'Tab::Comp');
Tab::Rating->has_a(school => 'Tab::School');
Tab::Rating->has_a(qual => 'Tab::Qual');

__PACKAGE__->_register_datetimes( qw/entered/ );

Tab::Rating->set_sql(school_ratings_by_group => "
						select distinct rating.*,qual.name as qual_val from rating,judge,qual
						where rating.judge = judge.id
						and judge.judge_group = ?
						and rating.qual = qual.id
						and qual.type = \"qual\"");

Tab::Rating->set_sql(comp_ratings_by_group => "
						select distinct rating.*,qual.name as qual_val from rating,judge,qual
						where rating.judge = judge.id
						and judge.judge_group = ?
						and rating.qual = qual.id
						and qual.type = \"mpj\"");

