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
use Tab::BallotValue;
use Tab::TournCircuit;
use Tab::TournSetting;
use Tab::EventSetting;
use Tab::JudgeSetting;
use Tab::CircuitSetting;
use Tab::JudgeGroupSetting;
use Tab::Result;

print "Retrieving all the ballots.  This may take a while...\n";
my @ballots = Tab::Ballot->retrieve_all;
print "Done. Here we go.\n";

my $count;
my $total;

foreach my $ballot (@ballots) { 
	$ballot->value("rank", $ballot->real_rank);
	$ballot->value("points", $ballot->real_points);

	if ($count > 1000) { 
		$total += $count;
		undef($count);
		print "$total done\n";
	}
}

print "Finis!\n";

