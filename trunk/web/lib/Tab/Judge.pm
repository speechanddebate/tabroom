package Tab::Judge;
use base 'Tab::DBI';
Tab::Judge->table('judge');
Tab::Judge->columns(Primary => qw/id/);
Tab::Judge->columns(Essential => qw/school first last code active ada
									special notes judge_group alt_group gender
									timestamp covers chapter_judge obligation 
									account acct_request dropped drop_time 
									reg_time drop_by hired score tmp standby
									hire_offer hire_approved tab_rating cat_id diverse/);

Tab::Judge->columns(TEMP => qw/tier pref panelid chair tourn avg diet ballotid accountid 
							   cjid schoolname schoolcode regname regcode region/);

Tab::Judge->has_a(judge_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(alt_group => 'Tab::JudgeGroup');
Tab::Judge->has_a(covers => 'Tab::JudgeGroup');
Tab::Judge->has_a(school => 'Tab::School');
Tab::Judge->has_a(drop_by => 'Tab::Account');
Tab::Judge->has_a(account => 'Tab::Account');
Tab::Judge->has_a(acct_request => 'Tab::Account');
Tab::Judge->has_a(chapter_judge => 'Tab::ChapterJudge');

Tab::Judge->has_many(ratings => 'Tab::Rating', 'judge');
Tab::Judge->has_many(strikes => 'Tab::Strike', 'judge');
Tab::Judge->has_many(ballots => 'Tab::Ballot', 'judge');
Tab::Judge->has_many(settings => "Tab::Setting", "judge");
Tab::Judge->has_many(hires => "Tab::JudgeHire", "judge");

Tab::Judge->has_many(jpools => [Tab::JPoolJudge => 'jpool']);

Tab::Judge->set_sql(highest_code => "select MAX(code) from judge where judge_group = ?");
Tab::Judge->set_sql(lowest_code => "select MIN(code) from judge where judge_group = ?");

__PACKAGE__->_register_datetimes( qw/drop_time reg_time/);

Tab::Judge->set_sql( by_group => "
        		select distinct judge.*
            	from judge
            	where judge.judge_group = ?
				and judge.active = 1");

Tab::Judge->set_sql( hired_by_group => "
        		select distinct judge.*
            		from judge 
				where judge.judge_group = ?
				and spare_pool = 1");

sub print_ratings {

	my ($self, $subset) = @_;

	my @ratings = Tab::Rating->search( judge => $self->id, subset => $subset->id, type => "coach" ) if $subset;
	@ratings = Tab::Rating->search( judge => $self->id, type => "coach") unless $subset;

	my $string;

	foreach my $rating (sort {$a->id cmp $b->id} @ratings) { 
		$string .= " ".$rating->rating_tier->name if $rating->rating_tier;
	}

	return $string;
}

sub rating { 

	my ($self, $subset) = @_;

	if ($subset) { 
		my @ratings = Tab::Rating->search( judge => $self->id, subset => $subset->id, type => "coach" );
		return shift @ratings if @ratings;
		return;
	} else { 
		my @ratings = Tab::Rating->search( judge => $self->id, type => "coach");
		return shift @ratings if @ratings;
		return;
	} 

}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	$/ = "";			#Remove all trailing newlines
	chomp $blob;

	my $existing = Tab::Setting->search(  
		judge => $self->id,
		tag    => $tag,
		type   => "judge"
	)->first;

	if (defined $value) { 
			
		if ($existing) {

			$existing->value($value);
			$existing->value_text($blob) if $value eq "text";
			$existing->value_date($blob) if $value eq "date";
			$existing->update;

			if ($value eq "delete" || $value eq "" || $value eq "0") { 
				$existing->delete;
			}

			return;

		} elsif ($value ne "delete" && $value && $value ne "0") {

			my $existing = Tab::Setting->create({
				judge => $self->id,
				tag    => $tag,
				value  => $value,
				type   => "judge"
			});

			if ($value eq "text") { 
				$existing->value_text($blob);
			}

			if ($value eq "date") { 
				$existing->value_date($blob);
			}

			$existing->update;

		}

	} else {

		return unless $existing;
		return $existing->value_text if $existing->value eq "text";
		return $existing->value_date if $existing->value eq "date";
		return $existing->value;

	}

}

