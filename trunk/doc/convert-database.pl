#!/opt/local/bin/perl

use lib ("/www/itab/web/lib");
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";

use strict;
use DBI;
use Class::DBI::AbstractSearch;
use DateTime::Span;
use DateTime::Format::MySQL;

use Tab::General;
use Tab::Account;
use Tab::AccountAccess;
use Tab::Ballot;
use Tab::Bill;
use Tab::Change;
use Tab::Chapter;
use Tab::ChapterAccess;
use Tab::ChapterLeague;
use Tab::Class;
use Tab::Coach;
use Tab::Comp;
use Tab::DBI;
use Tab::Dues;
use Tab::ElimAssign;
use Tab::Email;
use Tab::Event;
use Tab::File;
use Tab::Fine;
use Tab::Flight;
use Tab::FollowComp;
use Tab::FollowJudge;
use Tab::General;
use Tab::Housing;
use Tab::HousingSlots;
use Tab::Item;
use Tab::Judge;
use Tab::JudgeHire;
use Tab::JudgeGroup;
use Tab::League;
use Tab::LeagueAdmin;
use Tab::Link;
use Tab::Membership;
use Tab::Method;
use Tab::News;
use Tab::NoInterest;
use Tab::Panel;
use Tab::Pool;
use Tab::PoolGroup;
use Tab::PoolJudge;
use Tab::Purchase;
use Tab::Qual;
use Tab::QualSubset;
use Tab::Rating;
use Tab::Region;
use Tab::ResultFile;
use Tab::Room;
use Tab::RoomBlock;
use Tab::RoomPool;
use Tab::Round;
use Tab::Schedule;
use Tab::School;
use Tab::Session;
use Tab::Site;
use Tab::Strike;
use Tab::Sweep;
use Tab::Student;
use Tab::StudentResult;
use Tab::TeamMember;
use Tab::Tiebreak;
use Tab::TiebreakSet;
use Tab::Timeslot;
use Tab::Tournament;
use Tab::TournSite;
use Tab::TournSetting;
use Tab::EventSetting;
use Tab::JudgeSetting;
use Tab::CircuitSetting;
use Tab::JudgeGroupSetting;
use Tab::Uber;

print "Loading all tournaments\n";

my @tourns = Tab::Tournament->retrieve_all;

print "Done.  Loading all methods\n";

my @methods = Tab::Tournament->retrieve_all;

print "Done.  Hashing the methods by id\n";

my %methods_by_id = ();

foreach my $method (@methods) { 
	$methods_by_id{$method->id} = $method;
}

print "Done.  Converting tournament methods to settings\n";

foreach my $tourn (@tourns) { 

	print "Converting ".$tourn->id." ".$tourn->name." ".$tourn->start->year."\n";

	my $method = $methods_by_id{$tourn->method->id} if $tourn->method;

	if ($method) { 

		$tourn->setting("mfl_time_violation", $method->mfl_time_violation);
		$tourn->setting("truncate_to_smallest", $method->truncate_to_smallest);
		$tourn->setting("drop_worst_rank", $method->drop_worst_rank);
		$tourn->setting("drop_best_rank", $method->drop_best_rank);
		$tourn->setting("snake_elims", $method->snake_elims);
		$tourn->setting("honorable_mentions", $method->honorable_mentions);
		$tourn->setting("mfl_flex_finals", $method->mfl_flex_finals);
		$tourn->setting("noshows_never_break", $method->noshows_never_break);
		$tourn->setting("sweep_method", $method->sweep_method);
		$tourn->setting("sweep_wildcards", $method->sweep_wildcards);
		$tourn->setting("sweep_event_total", $method->sweep_event_total);
		$tourn->setting("sweep_class_based", $method->sweep_class_based);
		$tourn->setting("truncate_ranks_to", $method->truncate_ranks_to);
		$tourn->setting("double_entry", $method->double_entry);
		$tourn->setting("judge_event_twice", $method->judge_event_twice);
		$tourn->setting("judge_quality_system", $method->judge_quality_system);
		$tourn->setting("school_bump_penalty", $method->school_bump_penalty);
		$tourn->setting("region_bump_penalty", $method->region_bump_penalty);
		$tourn->setting("second_bump_penalty", $method->second_bump_penalty);
		$tourn->setting("full_panel_penalty", $method->full_panel_penalty);
		$tourn->setting("min_panel_size", $method->min_panel_size);
		$tourn->setting("default_panel_size", $method->default_panel_size);
		$tourn->setting("max_panel_size", $method->max_panel_size);
		$tourn->setting("min_chamber_size", $method->min_chamber_size);
		$tourn->setting("default_chamber_size", $method->default_chamber_size);
		$tourn->setting("max_chamber_size", $method->max_chamber_size);
		$tourn->setting("default_qualification", $method->default_qualification);
		$tourn->setting("sweep_per_event", $method->sweep_per_event);
		$tourn->setting("large_schools_first", $method->large_schools_first);
				$tourn->setting("allow_school_panels", $method->allow_school_panels);
		$tourn->setting("add_fine", $method->add_fine);
		$tourn->setting("drop_fine", $method->drop_fine);
		$tourn->setting("sweep_count_elims", $method->sweep_count_elims);
		$tourn->setting("sweep_count_prelims", $method->sweep_count_prelims);
		$tourn->setting("sweep_rank_in_elims", $method->sweep_rank_in_elims);
		$tourn->setting("sweep_final_rank", $method->sweep_final_rank);
		$tourn->setting("sweep_count_finals", $method->sweep_count_finals);
		$tourn->setting("allow_judge_own", $method->allow_judge_own);
		$tourn->setting("noshow_judge_fine", $method->noshow_judge_fine);
		$tourn->setting("bid_min_cume", $method->bid_min_cume);
		$tourn->setting("bid_percent", $method->bid_percent);
		$tourn->setting("bid_round_to_rank", $method->bid_round_to_rank);
		$tourn->setting("bid_min_round", $method->bid_min_round);
		$tourn->setting("circuit", $method->circuit);
		$tourn->setting("housing", $method->housing);
		$tourn->setting("housing_message", $method->housing_message);
		$tourn->setting("points_per_finalist", $method->points_per_finalist);
		$tourn->setting("bid_min_round_type", $method->bid_min_round_type);
		$tourn->setting("points_per_elim", $method->points_per_elim);
		$tourn->setting("hide_codes", $method->hide_codes);
		$tourn->setting("bid_min_number", $method->bid_min_number);
		$tourn->setting("elim_method", $method->elim_method);
		$tourn->setting("allow_neutral_judges", $method->allow_neutral_judges);
		$tourn->setting("ask_qualifying_tourn", $method->ask_qualifying_tourn);
		$tourn->setting("elim_method_basis", $method->elim_method_basis);
		$tourn->setting("timestamp", $method->timestamp);
		$tourn->setting("judge_cells", $method->judge_cells);
		$tourn->setting("track_first_year", $method->track_first_year);
				$tourn->setting("publish_schools", $method->publish_schools);
		$tourn->setting("incremental_school_codes", $method->incremental_school_codes);
		$tourn->setting("first_school_code", $method->first_school_code);
		$tourn->setting("schemat_school_code", $method->schemat_school_code);
		$tourn->setting("schemat_display", $method->schemat_display);
		$tourn->setting("per_student_fee", $method->per_student_fee);
		$tourn->setting("publish_paradigms", $method->publish_paradigms);
		$tourn->setting("must_pay_dues", $method->must_pay_dues);
		$tourn->setting("no_back_to_back", $method->no_back_to_back);
		$tourn->setting("master_printouts", $method->master_printouts);
		$tourn->setting("ask_two_quals", $method->ask_two_quals);
		$tourn->setting("audit_method", $method->audit_method);
		$tourn->setting("require_adult_contact", $method->require_adult_contact);
		$tourn->setting("novices", $method->novices);
		$tourn->setting("points_per_entry", $method->points_per_entry);
		$tourn->setting("sweep_only_place_final", $method->sweep_only_place_final);
		$tourn->setting("track_reg_changes", $method->track_reg_changes);
		$tourn->setting("drop_worst_elim", $method->drop_worst_elim);
		$tourn->setting("drop_best_elim", $method->drop_best_elim);
		$tourn->setting("drop_worst_final", $method->drop_worst_final);
		$tourn->setting("drop_best_final", $method->drop_best_final);
		$tourn->setting("rating_system", $method->rating_system);
		$tourn->setting("rating_covers", $method->rating_covers);
		$tourn->setting("hide_debate_codes", $method->hide_debate_codes);
		$tourn->setting("noshow_judge_fine_elim", $method->noshow_judge_fine_elim);
		$tourn->setting("concession_name", $method->concession_name);
		$tourn->setting("track_novice_judges", $method->track_novice_judges);
		$tourn->setting("judge_sheet_notice", $method->judge_sheet_notice);
		$tourn->setting("num_judges", $method->num_judges);
		$tourn->setting("disable_region_strikes", $method->disable_region_strikes);
		$tourn->setting("double_max", $method->double_max);

	}

	$tourn->setting("invite", $tourn->invitename);
	$tourn->setting("bills", $tourn->bill_packet);
	$tourn->setting("judge_policy", "text", $tourn->judge_policy);
	$tourn->setting("disclaimer", "text", $tourn->disclaimer);
	$tourn->setting("invoice_message", "text", $tourn->invoice_message);
	$tourn->setting("ballot_message", "text", $tourn->ballot_message);
	$tourn->setting("web_message", "text", $tourn->web_message);
	$tourn->setting("chair_ballot_message", "text", $tourn->chair_ballot_message);
	$tourn->setting("drop_deadline", $tourn->drop_deadline);
	$tourn->setting("fine_deadline", $tourn->fine_deadline);
	$tourn->setting("judge_deadline", $tourn->judge_deadline);
	$tourn->setting("freeze_deadline", $tourn->freeze_deadline);
	$tourn->setting("vcorner", $tourn->vcorner);
	$tourn->setting("hcorner", $tourn->hcorner);
	$tourn->setting("vlabel", $tourn->vlabel);
	$tourn->setting("hlabel", $tourn->hlabel);


}

