#!/usr/bin/perl -d
package Tab::Mason;

use lib "/www/tabroom/web/lib";
#Alternative locations
use lib "/www/testing.tabroom.com/web/lib";
#use lib "/www/backup.tabroom.com/web/lib";
use lib "/www/staging.tabroom.com/web/lib";

use strict;
no warnings "uninitialized";
no warnings "redefine";

use DBI;
use POSIX;
use Class::DBI::AbstractSearch;
use HTML::Mason::ApacheHandler;
use Digest::SHA;
use Data::Dumper;
use DateTime;
use JSON;
use Lingua::EN::Numbers::Ordinate;
use MIME::Lite;
use HTML::FromText;
use HTML::Strip;
use HTML::Scrubber;
use Email::Valid;
use DateTime::Span;
use DateTime::Format::MySQL;
use Crypt::PasswdMD5;
use Apache2::Cookie;
use Apache2::Request;
use Apache2::Upload;
use Switch;
use Sys::Syslog;

use Tab::General;
use Tab::DBI;
use Tab::Ad;
use Tab::Person;
use Tab::PersonSetting;
use Tab::Conflict;
use Tab::Ballot;
use Tab::Score;
use Tab::Calendar;
use Tab::Category;
use Tab::CategorySetting;
use Tab::Chapter;
use Tab::ChapterCircuit;
use Tab::ChapterJudge;
use Tab::ChapterSetting;
use Tab::Circuit;
use Tab::CircuitSetting;
use Tab::CircuitMembership;
use Tab::Concession;
use Tab::ConcessionPurchase;
use Tab::Email;
use Tab::Entry;
use Tab::EntrySetting;
use Tab::EntryStudent;
use Tab::Event;
use Tab::EventSetting;
use Tab::Pattern;
use Tab::File;
use Tab::Fine;
use Tab::Follower;
use Tab::GoogleCalendar;
use Tab::Hotel;
use Tab::Housing;
use Tab::HousingSlots;
use Tab::Judge;
use Tab::JudgeSetting;
use Tab::JudgeHire;
use Tab::JudgeShift;
use Tab::Login;
use Tab::Panel;
use Tab::Permission;
#use Tab::Practice;
#use Tab::PracticeStudent;
use Tab::Permission;
use Tab::JPool;
use Tab::JPoolSetting;
use Tab::JPoolJudge;
use Tab::JPoolRound;
use Tab::Qualifier;
use Tab::Rating;
use Tab::RatingSubset;
use Tab::RatingTier;
use Tab::Region;
use Tab::RegionFine;
use Tab::RegionSetting;
use Tab::Result;
use Tab::ResultSet;
use Tab::ResultValue;
use Tab::Room;
use Tab::RPool;
use Tab::RPoolSetting;
use Tab::RPoolRoom;
use Tab::RPoolRound;
use Tab::RoomStrike;
use Tab::Round;
use Tab::RoundSetting;
use Tab::School;
use Tab::SchoolSetting;
use Tab::Session;
use Tab::Setting;
use Tab::SettingLabel;
use Tab::Site;
use Tab::Stats;
use Tab::Strike;
use Tab::Student;
use Tab::StudentBallot;
use Tab::StudentSetting;
use Tab::SweepInclude;
use Tab::SweepSet;
use Tab::SweepRule;
use Tab::SweepEvent;
use Tab::Tiebreak;
use Tab::TiebreakSetSetting;
use Tab::TiebreakSet;
use Tab::Timeslot;
use Tab::Tourn;
use Tab::TournSetting;
use Tab::Weekend;
use Tab::WeekendSetting;
use Tab::ChangeLog;
use Tab::TournCircuit;
use Tab::TournIgnore;
use Tab::TournSite;
use Tab::Webpage;

use Tab::NSDA::PointsDBI;

my $ah;

sub handler {

    my $r = shift;  # Apache request object;

    if ($r->hostname =~ /www.tabroom.com/) {
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root   => $Tab::file_root,
			data_dir    => $Tab::file_root."mason",
			error_mode  => 'fatal'
		); 
	} else { 
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root   => $Tab::file_root,
			data_dir    => $Tab::file_root."mason",
		); 
 	}
	
	my $return = eval { 
		$ah->handle_request($r) 
	};

	if ( my $err = $@ ) {

		$r->pnotes( error => $err );
		$r->filename( $r->document_root . '/index/oh_crap.mhtml' );
		return $ah->handle_request($r);
	
	}
	return $return;
}

1;
