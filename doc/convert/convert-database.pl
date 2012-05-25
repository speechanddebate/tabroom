#!/usr/bin/perl

use lib "/www/itab/doc/convert/lib";
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3";
use lib "/opt/local/lib/perl5/site_perl/5.12.3";

use strict;
use DBI;
use Class::DBI::AbstractSearch;
use DateTime::Span;
use DateTime::Format::MySQL;

# Old table structures required for conversion to the new.
use Tab::General;
use Tab::DBI;

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
use Tab::Qualifier;
use Tab::QualSubset;
use Tab::Rating;
use Tab::Region;
use Tab::RegionAdmin;
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
use Tab::Uber;

# New tables
use Tab::TournFee;
use Tab::TournCircuit;
use Tab::TournSetting;
use Tab::EventSetting;
use Tab::JudgeSetting;
use Tab::CircuitSetting;
use Tab::JudgeGroupSetting;
use Tab::Result;

print "Loading all results files\n";

my @results = Tab::ResultFile->retrieve_all;

foreach my $result (@results) { 

	Tab::File->create({
		tournament => $result->tournament->id,
		label => $result->name,
		name => $result->filename,
		result => 1
	});

}

print "Loading all regions and diocese\n";

my @regions = Tab::Region->retrieve_all;

foreach my $region (@regions) { 

    Tab::RegionAdmin->create({
        region => $region->id,
        account => $region->director->id
    }) if $region->director;

}

print "Loading all fines\n";

my @fines = Tab::Fine->retrieve_all;

foreach my $fine (@fines) { 

    next unless $fine->tournament;

    Tab::TournFee->create({
        tourn => $fine->tournament->id,
        start => $fine->start,
        reason => $fine->reason,
        end => $fine->end,
        amount => $fine->amount,
    });
 
}

print "Converting the sweeps table\n";

my @sweeps = Tab::Sweep->retrieve_all;

foreach my $sweep (@sweeps) { 

	if ($sweep->place) { 

		Tab::TournSetting->create({
			tourn => $sweep->tournament->id,
			tag => "sweep_place_".$sweep->place,
			value => $sweep->value
		});

	}

	if ($sweep->prelim_cume) { 

		Tab::TournSetting->create({
			tourn => $sweep->tournament->id,
			tag => "sweep_prelim_cume_".$sweep->prelim_cume,
			value => $sweep->value
		});

	}

}


print "Loading all tournaments\n";

my @tourns = Tab::Tournament->retrieve_all;

print "Done.  Loading all methods\n";

my @methods = Tab::Method->retrieve_all;

print "Done.  Hashing the methods by id\n";

my %methods_by_id = ();

foreach my $method (@methods) { 
	$methods_by_id{$method->id} = $method;
}

print "Done.  Converting tournament methods to settings\n";

foreach my $tourn (@tourns) { 

	print "Converting ".$tourn->id." ".$tourn->name." ".$tourn->start->year."\n";

	Tab::TournCircuit->create({
		tourn => $tourn->id,
		circuit => $tourn->league->id,
		approved => 1
	});

	if ($tourn->director && $tourn->director->id) { 
	
		Tab::AccountAccess->create({
			tournament => $tourn->id,
			account => $tourn->director->id,
			contact => 1
		});

	}

	my $method = $methods_by_id{$tourn->method->id} if $tourn->method;

	if ($method) { 

		my @tiebreaks = $method->tiebreaks;

		my %sets_by_name = ();

		foreach my $tb (@tiebreaks) { 

			my $set = $sets_by_name{$tb->covers};

			unless ($set) { 

				$set = Tab::TiebreakSet->create({
					tournament => $tourn->id,
					name => $tb->covers
				});

			}

			$tb->tb_set($set->id);
			$tb->update;
		}

		$tourn->setting("mfl_time_violation", $method->mfl_time_violation);
		$tourn->setting("truncate_to_smallest", $method->truncate_to_smallest);
		$tourn->setting("drop_worst_rank", $method->drop_worst_rank);
		$tourn->setting("drop_best_rank", $method->drop_best_rank);
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
		$tourn->setting("min_panel_size", $method->min_panel_size);
		$tourn->setting("default_panel_size", $method->default_panel_size);
		$tourn->setting("max_panel_size", $method->max_panel_size);
		$tourn->setting("min_chamber_size", $method->min_chamber_size);
		$tourn->setting("default_chamber_size", $method->default_chamber_size);
		$tourn->setting("max_chamber_size", $method->max_chamber_size);
		$tourn->setting("default_qualification", $method->default_qualification);
		$tourn->setting("sweep_per_event", $method->sweep_per_event);
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
		$tourn->setting("housing", $method->housing);
		$tourn->setting("housing_message", "text", $method->housing_message);
		$tourn->setting("points_per_finalist", $method->points_per_finalist);
		$tourn->setting("bid_min_round_type", $method->bid_min_round_type);
		$tourn->setting("points_per_elim", $method->points_per_elim);
		$tourn->setting("hide_codes", $method->hide_codes);
		$tourn->setting("bid_min_number", $method->bid_min_number);
		$tourn->setting("elim_method", $method->elim_method);
		$tourn->setting("allow_neutral_judges", $method->allow_neutral_judges);
		$tourn->setting("elim_method_basis", $method->elim_method_basis);
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
		$tourn->setting("ask_quals", "1") if $method->ask_qualifying_tournament;
		$tourn->setting("ask_quals", "2") if $method->ask_two_quals;
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
	$tourn->setting("drop_deadline", "date", $tourn->drop_deadline) if $tourn->drop_deadline;
	$tourn->setting("fine_deadline", "date", $tourn->fine_deadline) if $tourn->fine_deadline;
	$tourn->setting("judge_deadline", "date", $tourn->judge_deadline) if $tourn->judge_deadline;
	$tourn->setting("freeze_deadline", "date", $tourn->freeze_deadline) if $tourn->freeze_deadline;
	$tourn->setting("vcorner", $tourn->vcorner);
	$tourn->setting("hcorner", $tourn->hcorner);
	$tourn->setting("vlabel", $tourn->vlabel);
	$tourn->setting("hlabel", $tourn->hlabel);
	$tourn->setting("hlabel", $tourn->hlabel);
	$tourn->setting("results", $tourn->results);

}

print "Done.  Retrieving all judges:\n";
my @judges;
my @judges = Tab::Judge->retrieve_all;

print "Done. Converting judges to the new settings\n";

foreach my $judge (@judges) { 

	$judge->setting("special", $judge->special);
	$judge->setting("notes", $judge->notes);
	$judge->setting("neutral", $judge->neutral);
	$judge->setting("cfl_tab_first", $judge->cfl_tab_first);
	$judge->setting("cfl_tab_second", $judge->cfl_tab_second);
	$judge->setting("cfl_tab_third", $judge->cfl_tab_third);
	$judge->setting("alt_group", $judge->alt_group);
	$judge->setting("spare_pool", $judge->spare_pool);
	$judge->setting("prelim_pool", $judge->prelim_pool);
	$judge->setting("first_year", $judge->first_year);
	$judge->setting("novice", $judge->novice);

}

print "Done.  Converting Uberjudges to the new settings and creating accounts:\n";

foreach my $judge (Tab::Uber->retrieve_all) {

    print "Converting judge ".$judge->first." ".$judge->last." \n";

    my $started;

    if ($judge->started && $judge->started > 0) {
        $started = $started."-07-01";
    }

    my $account = Tab::Account->create({
        first => $judge->first,
        last => $judge->last,
        gender => $judge->gender,
        started => $started,
        site_admin => 0,
        paradigm => $judge->paradigm,
    });

    $judge->account($account->id);
    $judge->update;

}

print "Done.  Retrieving all judge groups:\n";
my @judge_groups = Tab::JudgeGroup->retrieve_all;

print "Done.  Converting judge groups to the new settings\n";

foreach my $group (@judge_groups) { 

	$group->setting("free", $group->free);
	$group->setting("alt_max", $group->alt_max);
	$group->setting("dio_min", $group->dio_min);
	$group->setting("ask_alts", $group->ask_alts);
	$group->setting("missing_judge_fee", $group->missing_judge_fee);
	$group->setting("uncovered_entry_fee", $group->uncovered_entry_fee);
	$group->setting("fee_missing", $group->fee_missing);
	$group->setting("tab_room", $group->tab_room);
	$group->setting("track_judge_hires", $group->track_judge_hires);
	$group->setting("hired_pool", $group->hired_pool);
	$group->setting("hired_fee", $group->hired_fee);
	$group->setting("track_by_pools", $group->track_by_pools);
	$group->setting("ask_paradigm", $group->ask_paradigm);
	$group->setting("elim_special", $group->elim_special);
	$group->setting("special", $group->special);
	$group->setting("school_strikes", $group->school_strikes);
	$group->setting("strike_reg_opens", $group->strike_reg_opens);
	$group->setting("strike_reg_closes", $group->strike_reg_closes);
	$group->setting("min_burden", $group->min_burden);
	$group->setting("ratings_need_paradigms", $group->ratings_need_paradigms);
	$group->setting("strikes_need_paradigms", $group->strikes_need_paradigms);
	$group->setting("comp_strikes", $group->comp_strikes);
	$group->setting("coach_ratings", $group->coach_ratings);
	$group->setting("school_ratings", $group->school_ratings);
	$group->setting("comp_ratings", $group->comp_ratings);
	$group->setting("max_burden", $group->max_burden);
	$group->setting("pub_assigns", $group->pub_assigns);
	$group->setting("obligation_before_strikes", $group->obligation_before_strikes);
	$group->setting("conflicts", $group->conflicts);
	$group->setting("live_updates", $group->live_updates);
	$group->setting("deadline", "date", $group->deadline) if $group->deadline;

}

print "Done.  Retrieving all circuits:\n";
my @circuits = Tab::League->retrieve_all;

print "Done.  Converting circuits to the new settings\n";

foreach my $circuit (@circuits) { 

	$circuit->setting("url", $circuit->url);
	$circuit->setting("public_email", $circuit->public_email);
	$circuit->setting("short_name", $circuit->short_name);
	$circuit->setting("dues_to", $circuit->dues_to);
	$circuit->setting("timezone", $circuit->timezone);
	$circuit->setting("dues_amount", $circuit->dues_amount);
	$circuit->setting("hosted_site", $circuit->hosted_site);
	$circuit->setting("apda_seeds", $circuit->apda_seeds);
	$circuit->setting("logo_file", $circuit->logo_file);
	$circuit->setting("site_theme", $circuit->site_theme);
	$circuit->setting("public_results", $circuit->public_results);
	$circuit->setting("header_file", $circuit->header_file);
	$circuit->setting("invoice_message", "text", $circuit->invoice_message);
	$circuit->setting("track_bids", $circuit->track_bids);
	$circuit->setting("last_change", $circuit->last_change);
	$circuit->setting("approved", $circuit->approved);
	$circuit->setting("tourn_only", $circuit->tourn_only);
	$circuit->setting("full_members", $circuit->full_members);

}

print "Done.  Retrieving all events:\n";
my @events = Tab::Event->retrieve_all;

print "Done.  Converting events to the new settings\n";

foreach my $event (@events) { 

	$event->setting("alumni", $event->alumni);
	$event->setting("allow_judge_own", $event->allow_judge_own);
	$event->setting("ballot", $event->ballot);
	$event->setting("omit_sweeps", $event->omit_sweeps);
	$event->setting("no_judge_burden", $event->no_judge_burden);
	$event->setting("ballot_type", $event->ballot_type);
	$event->setting("deadline", "date",$event->deadline) if $event->deadline;
	$event->setting("ask_for_titles", $event->ask_for_titles);
	$event->setting("supp", $event->supp);
	$event->setting("code_style", "numbers") if $event->code;
	$event->setting("code_style", "initials") if $event->initial_codes;
	$event->setting("code_style", "register") if $event->reg_codes;
	$event->setting("code_start", $event->code);
	$event->setting("code_hide", $event->no_code);
	$event->setting("field_report", $event->field_report);
	$event->setting("live_updates", $event->live_updates);
	$event->setting("waitlist_all", $event->waitlist_all);
	$event->setting("judge_group", $event->judge_group);
	$event->setting("bids", $event->bids);
	$event->setting("waitlist", $event->waitlist);
	$event->setting("cap", $event->cap);
	$event->setting("school_cap", $event->school_cap);
	$event->setting("min_entry", $event->team);
	$event->setting("max_entry", $event->team);
	$event->setting("max_entry", 8) if $event->team == 3;
	$event->setting("description", "text", $event->blurb);

}

print "Retrieving all competitors to remove the stupid partner student mistake:\n";
my @comps = Tab::Comp->retrieve_all;

print "Converting the tables to a true many to many comp student relation:\n";

my $count;
my $total;

foreach my $comp (@comps) { 

	if ($count > 500) { 
		$total += $count;
		undef($count);
		print "$total done\n";
	}

	$count++;

	if ($comp->student && $comp->student->id) { 

		Tab::TeamMember->create({
			student => $comp->student->id,
			comp => $comp->id
		});

	}

	if ($comp->sweeps_points > 0) { 

		Tab::Result->create({
			entry => $comp->id,
			sweeps => $comp->sweeps_points,
			label => "Sweepstakes"
		})
	}

	if ($comp->partner && $comp->partner->id) { 

		Tab::TeamMember->create({
			student => $comp->partner->id,
			comp => $comp->id
		});

	}

	if ($comp->qualifier) { 

		Tab::Qualifier->create({
			entry => $comp->id,
			name => $comp->qualifier,
			result => $comp->qualexp
		});

	}

	if ($comp->qual2) {
		Tab::Qualifier->create({
			entry => $comp->id,
			name => $comp->qual2,
			result => $comp->qual2exp
		});
	}

}

print "Retrieving all chapters to update coach credit records:\n";

my @coaches = Tab::Coach->retrieve_all;

print "Updating coach records into chapter text fields:\n";

foreach my $coach (@coaches) { 
	my $chapter = $coach->chapter;
	$chapter->coaches( $coach->chapter->coaches." ".$coach->name );
	$chapter->update;
}

print "Finis!\n";

