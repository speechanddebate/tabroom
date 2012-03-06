package Tab::Qual;
use base 'Tab::DBI';
Tab::Qual->table('qual');
Tab::Qual->columns(Primary => qw/id/);
Tab::Qual->columns(Essential => qw/name tourn judge_group/);
Tab::Qual->columns(Others => qw/timestamp description strike type max min conflict/);
Tab::Qual->has_a(tourn => 'Tab::Tourn');
Tab::Qual->has_a(judge_group => 'Tab::JudgeGroup');

Tab::Qual->set_sql(subset_qual => "select qual.* from qual,rating
						where rating.judge = ?
						and rating.subset = ? 
						and rating.type = \"coach\"
						and rating.qual = qual.id");

Tab::Qual->set_sql(qual => "select qual.* from qual,rating
						where rating.judge = ?
						and rating.qual = qual.id
						and rating.type = \"coach\"");
