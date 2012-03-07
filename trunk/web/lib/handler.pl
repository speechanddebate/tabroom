package Tab::Mason;
use lib ("/www/itab/web/lib");
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";

use strict;
use DBI;
use POSIX;
use Class::DBI::AbstractSearch;
use HTML::Mason::ApacheHandler;
use Digest::SHA1;
use Data::Dumper;
use DateTime;
use Lingua::EN::Numbers::Ordinate;
use MIME::Lite;
use HTML::FromText;
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
use Tab::Account;
use Tab::ChapterAdmin;
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
use Tab::AccountIgnore;
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
use Tab::Uber;

my $ah;

sub handler {

    my $r = shift;  # Apache request object;

    if ($r->hostname =~ /www.tabroom.com/) {
    
		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root => '/www/itab/web',
			data_dir  => '/www/itab/web/mason',
			error_mode => 'fatal'
		); 

	} else { 

		$ah = HTML::Mason::ApacheHandler->new(
			args_method => 'mod_perl',
			comp_root => '/www/itab/web',
			data_dir  => '/www/itab/web/mason',
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
