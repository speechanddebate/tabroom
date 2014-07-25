#!/usr/bin/perl

use lib "/www/itab/web/lib";
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.16.1/darwin-multi-2level";
use lib "/opt/local/lib/perl5/vendor_perl/5.16.1";
use lib "/opt/local/lib/perl5/site_perl/5.16.1";

use strict;
use DBI;
use Class::DBI::AbstractSearch;
use DateTime::Span;
use DateTime::Format::MySQL;

# Old table structures required for conversion to the new.
use Tab::General;
use Tab::DBI;

use Tab::Account;
use Tab::AccountConflict;
use Tab::AccountSetting;
use Tab::Ballot;
use Tab::BallotValue;
use Tab::Chapter;
use Tab::ChapterAdmin;
use Tab::ChapterCircuit;
use Tab::ChapterJudge;
use Tab::Circuit;
use Tab::CircuitAdmin;
use Tab::CircuitDues;
use Tab::CircuitMembership;
use Tab::CircuitSetting;
use Tab::Concession;
use Tab::ConcessionPurchase;
use Tab::Email;
use Tab::Entry;
use Tab::EntryStudent;
use Tab::Event;
use Tab::EventDouble;
use Tab::EventSetting;
use Tab::File;
use Tab::FollowAccount;
use Tab::FollowTourn;
use Tab::FollowEntry;
use Tab::FollowJudge;
use Tab::FollowSchool;
use Tab::Hotel;
use Tab::Housing;
use Tab::HousingSlots;
use Tab::Judge;
use Tab::JudgeGroup;
use Tab::JudgeGroupSetting;
use Tab::JudgeHire;
use Tab::JudgeSetting;
use Tab::Login;
use Tab::Panel;
use Tab::Person;
use Tab::Pool;
use Tab::PoolJudge;
use Tab::Qualifier;
use Tab::Rating;
use Tab::RatingSubset;
use Tab::RatingTier;
use Tab::Region;
use Tab::RegionAdmin;
use Tab::RegionFine;
use Tab::Result;
use Tab::ResultSet;
use Tab::ResultValue;
use Tab::Room;
use Tab::RoomGroup;
use Tab::RoomGroupRoom;
use Tab::RoomGroupRound;
use Tab::RoomPool;
use Tab::RoomStrike;
use Tab::Round;
use Tab::School;
use Tab::SchoolFine;
use Tab::Session;
use Tab::Site;
use Tab::Stats;
use Tab::Strike;
use Tab::StrikeTime;
use Tab::Student;
use Tab::SweepInclude;
use Tab::SweepSet;
use Tab::SweepRule;
use Tab::SweepEvent;
use Tab::Tiebreak;
use Tab::TiebreakSet;
use Tab::TiebreakSetting;
use Tab::Timeslot;
use Tab::Tourn;
use Tab::TournAdmin;
use Tab::TournChange;
use Tab::TournCircuit;
use Tab::TournIgnore;
use Tab::TournSetting;
use Tab::TournSite;
use Tab::Webpage;

my @accounts = Tab::Account->retrieve_all;

foreach my $account (@accounts) { 

	print "Creating account for ".$account->id." ".$account->email."\n";

	my $person = Tab::Person->create({
		id              => $account->id,
		email           => $account->email,
		first           => $account->first,
		last            => $account->last,
		phone           => $account->phone,
		provider        => $account->provider,
		site_admin      => $account->site_admin,
		multiple        => $account->multiple,
		gender          => $account->gender,
		no_email        => $account->no_email,
		tz              => $account->tz,
		started_judging => $account->started_judging,
		diversity       => $account->diversity,
		timestamp       => $account->timestamp,
	});

	Tab::Login->create({ 
		id       => $account->id,
		username => $account->email,
		password => $account->passhash,
		salt     => "3EZjdkNB9k92a4qG4Q61",
		name     => $account->first." ".$account->last,
		source   => "tabroom",
		person   => $account->id
	});

	$person->setting("hotel", $account->hotel) if $account->hotel;
	$person->setting("paradigm_timestamp", $account->paradigm_timestamp) if $account->paradigm_timestamp;

}

