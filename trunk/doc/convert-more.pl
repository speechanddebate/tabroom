#!/usr/bin/perl

use lib "/www/itab/web/lib";
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";

use strict;
use DBI;
use Class::DBI::AbstractSearch;
use DateTime::Span;
use DateTime::Format::MySQL;

use Tab::General;
use Tab::ChapterJudge;
use Tab::Chapter;
use Tab::School;
use Tab::Region;
use Tab::Account;
use Tab::JudgeGroup;

print "Loading all judges\n";

foreach my $judge (Tab::ChapterJudge->retrieve_all) { 

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

print "Done. \n";


