#!/usr/bin/perl -w

package Tab::Mason;
use lib "/www/tabroom/web/lib";
use lib "/opt/local/lib/perl5/vendor_perl/5.16.3/darwin-multi-2level";
use lib "/opt/local/lib/perl5/vendor_perl/5.16.3";
#use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";
#use lib "/opt/local/lib/perl5";

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
use Lingua::EN::Numbers::Ordinate;
use MIME::Lite;
use HTML::FromText;
use HTML::Strip;
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
use Tab::Account;
use Tab::AccountConflict;
use Tab::AccountSetting;
use Tab::Ballot;
use Tab::BallotValue;
use Tab::Calendar;
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

use Tab::NSDA::PointsDBI;

my $ah;

sub handler {

    my $r = shift;  # Apache request object;

    if ($r->hostname =~ /www.tabroom.com/) {
    
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root  => '/www/tabroom/web',
			data_dir    => '/www/tabroom/web/mason',
			error_mode  => 'fatal'
		); 

	} else { 

		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root  => '/www/tabroom/web',
			data_dir    => '/www/tabroom/web/mason',
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
