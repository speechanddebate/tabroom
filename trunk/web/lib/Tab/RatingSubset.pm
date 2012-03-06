package Tab::QualSubset;
use base 'Tab::DBI';
Tab::QualSubset->table('qual_subset');
Tab::QualSubset->columns(Primary => qw/id/);
Tab::QualSubset->columns(Essential => qw/name judge_group/);
Tab::QualSubset->has_a(judge_group => 'Tab::JudgeGroup');

Tab::QualSubset->has_many(events => 'Tab::Event', "qual_subset");
