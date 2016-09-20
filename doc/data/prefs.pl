#!/usr/bin/perl

use lib "/www/tabroom/web/lib";

use strict;
no warnings "uninitialized";
no warnings "redefine";

use DBI;
use POSIX;
use Class::DBI::AbstractSearch;
use Data::Dumper;
use DateTime;
use DateTime::Span;
use DateTime::Format::MySQL;

use Tab::General;
use Tab::DBI;

	my $dbh = Tab::DBI->db_Main();

	my $sth = $dbh->prepare('

		select tourn.id, tourn.name, tourn.start, event.id, event.name, count(entry.id)

        from (tourn, tourn_circuit, event) 

        left join entry on event.id = entry.event

        where tourn_circuit.circuit = 43
        and tourn_circuit.tourn = tourn.id
        and tourn.id = event.tourn
        group by event.id
        order by tourn.start
	');

	$sth->execute();

	my $compare_start = DateTime::Format::MySQL->parse_datetime("2013-07-01 00:00:00");
	my $compare_end = DateTime::Format::MySQL->parse_datetime("2015-07-01 00:00:00");

	my $tourn_ids;

    my $filename = "Tournaments.csv";
	open (CSVOUT, ">>$filename");

	print CSVOUT "TournID, Tourn Name, Tourn Start, Event ID, Event Name, Field Size\n";

	my $tourn_id_string;

	while( my (
    	$tourn_id, $tourn_name, $tourn_start, $event_id, $event_name, $count
			) = $sth->fetchrow_array() ) {

		my $start = DateTime::Format::MySQL->parse_datetime($tourn_start);
		
		next if $start->epoch > $compare_end->epoch;
		next if $start->epoch < $compare_start->epoch;

		next unless $count > 70;

		$tourn_id_string .= ", " if $tourn_id_string;
		$tourn_id_string .= $tourn_id;

		print CSVOUT '"'.$tourn_id.'", "'. $tourn_name.'", "'.$tourn_start.'", "';
		print CSVOUT $event_id.'", "'. $event_name.'", "'.$count.'"'."\n";

	}

	close CSVOUT;

	my $student_sth = $dbh->prepare('
		select 
			student.id, student.first, student.last, student.account, student.grad_year, student.gender,
			chapter.id, chapter.name, chapter.level

			from student, chapter

			where student.first = ? 
			and student.last = ? 
			and student.chapter = chapter.id
	');

	my $student_prefs_sth = $dbh->prepare('
		select 

			tourn.id, tourn.name, tourn.start,
			entry.id, entry.code, entry.name,
			rating.ordinal, rating.percentile, rating_tier.name,
			judge.id, judge.first, judge.last, (judge.obligation + judge.hired),
			diverse.value, gender.value

			from (entry_student, entry, rating, judge,tourn)
			left join rating_tier on rating.rating_tier = rating_tier.id
			left join judge_setting diverse 
				on diverse.judge = judge.id
				and diverse.tag = "diverse"

			left join judge_setting gender 
				on gender.judge = judge.id
				and gender.tag = "gender"

			where entry_student.student = ?
			and entry_student.entry = entry.id
			and entry.id = rating.entry
			and judge.id = rating.judge
			and rating.tourn = tourn.id
			and tourn.id in ('.$tourn_id_string.')
			group by rating.id
			order by tourn.start, rating.ordinal, rating.rating_tier
	');

	my @names = (
		"Alan Apthorp",
		"Austin Oliver ",
		"Cameron Henderson",
		"Cody Crunkilton",
		"DM Woodward",
		"Teddy Dillon ",
		"Ellie Miller",
		"Erica Jalbuena",
		"Erin Carley",
		"Isaac Brown",
		"Jacob Bosley",
		"Kailyn Revenew",
		"Katie Marshall",
		"Khalil Lee",
		"Kristen Lowe",
		"Kyle Klucas",
		"Luke Melton",
		"Mac Cronin",
		"Mary Bobbitt",
		"Megan Niermann",
		"Michael Mays",
		"Miranda Ehrlich",
		"Natalie Bennie",
		"Nick Vail",
		"Nick Lepp",
		"Nouran Ghanem",
		"Paul Banks",
		"Rich Kaye",
		"Richard Min",
		"Robert Mitchell",
		"Ryan Marcus",
		"Simon Sheaff",
		"Young Kwon"
	);

    my $filename = "Competitors.csv";
	open (CSVOUT, ">>$filename");
	print CSVOUT "TournID, Tourn Name, Tourn Start, Event ID, Event Name, Field Size\n";

	my @student_ids;

	foreach my $name (@names) { 

		my ($first, $last) = split(/\ /, $name);

		print ("\nDoing $first $last:\n");

		$student_sth->execute($first, $last);

		my @ids;

		while( my (
				$student_id, $student_first, $student_last, $student_person, 
					$student_grad_year, $student_gender,
				$chapter_id, $chapter_name, $chapter_level
			) = $student_sth->fetchrow_array() ) {

			print CSVOUT '"'.$student_id.'", "'. $student_first.'", "'. $student_last.'", "';
			print CSVOUT $student_person.'", "'.$student_grad_year.'", "'.$student_gender.'", "';
			print CSVOUT $chapter_id.'", "'. $chapter_name.'", "'. $chapter_level.'"'."\n";

			push @ids, $student_id;
		}

		my $student_filename = $first."-".$last.".csv";
		open (PREFSOUT, ">>$student_filename");


		print PREFSOUT "Tourn ID, Tourn Name, Tourn Date,";
		print PREFSOUT "Entry ID, Entry Code, Entry Name,";
		print PREFSOUT "Ordinal, Percentile, Rating Tier,";
		print PREFSOUT "Judge ID, Judge First, Judge Last,";
		print PREFSOUT "Round Obligation,Diverse Opt-In, Gender\n";

		my $counter;

		foreach my $id (@ids) { 

			$student_prefs_sth->execute($id);

			while (my (
				$tourn_id, $tourn_name, $tourn_date,
				$entry_id, $entry_code, $entry_name,
				$rating_ordinal, $rating_percentile, $rating_tier_name,
				$judge_id, $judge_first, $judge_last, 
				$judge_rounds, $diverse_value, $gender_value
			) = $student_prefs_sth->fetchrow_array() ) { 

				$counter++;

				print PREFSOUT '"'.$tourn_id.'", "'. $tourn_name.'", "'.$tourn_date.'", "';
				print PREFSOUT $entry_id.'", "'. $entry_code.'", "'. $entry_name.'", "';
				print PREFSOUT $rating_ordinal.'", "'. $rating_percentile.'", "'.$rating_tier_name.'", "';
				print PREFSOUT $judge_id.'", "'. $judge_first.'", "'. $judge_last.'", "';
				print PREFSOUT $judge_rounds.'", "'. $diverse_value.'"'."\n";

			}

		}

		close PREFSOUT; 

		print "Found $counter prefs for $student_filename\n";

	}



