#!/usr/bin/perl -d
package Tab::Mason;

use lib "/www/tabroom/web/lib";
#Alternative locations
#use lib "/www/testing.tabroom.com/web/lib";
#use lib "/www/backup.tabroom.com/web/lib";
#use lib "/www/staging.tabroom.com/web/lib";

use strict;
no warnings "uninitialized";
no warnings "redefine";

use DBI;
use POSIX;
use Class::DBI::AbstractSearch;
use Compress::Zlib;

use MIME::Base64;
use HTML::Mason::ApacheHandler;
use Digest::SHA;
use Data::Dumper;
use DateTime;

use JSON::XS;
use JSON -convert_blessed_universally;

use Lingua::EN::Numbers::Ordinate;
use MIME::Lite;
use HTML::FromText;
use HTML::Strip;
use HTML::Scrubber;
use Email::Valid;
use DateTime::Span;
use DateTime::Format::MySQL;
use Crypt::JWT;
use Crypt::PasswdMD5;
use Apache2::Cookie;
use Apache2::Request;
use Apache2::Upload;
use Switch;
use Math::Round;
use Sys::Syslog;

no warnings "uninitialized";

use Tab::General;
use Tab::Utils;
use Tab::DBI;
use Tab::Ad;
use Tab::Autoqueue;
use Tab::Person;
use Tab::PersonSetting;
use Tab::Conflict;
use Tab::Ballot;
use Tab::Score;
use Tab::CampusLog;
use Tab::Category;
use Tab::CategorySetting;
use Tab::Chapter;
use Tab::ChapterCircuit;
use Tab::ChapterJudge;
use Tab::ChapterSetting;
use Tab::Circuit;
use Tab::CircuitSetting;
use Tab::Concession;
use Tab::ConcessionOption;
use Tab::ConcessionPurchase;
use Tab::ConcessionPurchaseOption;
use Tab::Default;
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
use Tab::Hotel;
use Tab::Judge;
use Tab::JudgeSetting;
use Tab::JudgeHire;
use Tab::JudgeShift;
use Tab::JPool;
use Tab::JPoolSetting;
use Tab::JPoolJudge;
use Tab::JPoolRound;
use Tab::Panel;
use Tab::PanelSetting;
use Tab::Permission;
use Tab::Practice;
use Tab::PracticeStudent;
use Tab::Quiz;
use Tab::Rating;
use Tab::RatingSubset;
use Tab::RatingTier;
use Tab::Region;
use Tab::RegionSetting;
use Tab::Result;
use Tab::ResultSet;
use Tab::ResultKey;
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
use Tab::Strike;
use Tab::Student;
use Tab::StudentVote;
use Tab::StudentSetting;
use Tab::SweepAward;
use Tab::SweepAwardEvent;
use Tab::SweepInclude;
use Tab::SweepSet;
use Tab::SweepRule;
use Tab::SweepEvent;
use Tab::TabroomSetting;
use Tab::Tiebreak;
use Tab::ProtocolSetting;
use Tab::Protocol;
use Tab::Timeslot;
use Tab::Topic;
use Tab::Tourn;
use Tab::TournFee;
use Tab::TournSetting;
use Tab::Weekend;
use Tab::ChangeLog;
use Tab::TournCircuit;
use Tab::TournIgnore;
use Tab::TournSite;
use Tab::Webpage;

no warnings "uninitialized";

my $ah;

sub handler {

    my $r = shift;  # Apache request object;

    if ($r->hostname =~ /www.tabroom.com/) {
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root   => $Tab::file_root,
			data_dir    => $Tab::data_dir,
			error_mode  => 'fatal'
		);
	} else {
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root   => $Tab::file_root,
			data_dir    => $Tab::data_dir,
			error_mode  => 'fatal'
		);
 	}

	my $return = eval {
		return $ah->handle_request($r);
	};

	if ( my $err = $@ ) {
		$r->pnotes( error => $err );
		$r->filename( $r->document_root . '/index/oh_crap.mhtml' );
		return $ah->handle_request($r);
	}

	return $return;
}

1;
