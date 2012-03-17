#!/opt/local/bin/perl

use lib "/www/itab/web/lib";
use lib "/opt/local/lib/perl5";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/";
use lib "/opt/local/lib/perl5/vendor_perl/5.12.3/darwin-multi-2level";

use strict;
use DBI;
use Class::DBI::AbstractSearch;
use DateTime::Span;
use DateTime::Format::MySQL;

use Tab::General;
use Tab::Account;
use Tab::Region;
use Tab::RegionAdmin;

use Tab::SchoolFine;
use Tab::TournFee;

print "Loading all fines\n";

my @fines = Tab::SchoolFine->retrieve_all;

foreach my $fine (@fines) { 

    next unless $fine->tourn;

    Tab::TournFee->create({
        tourn => $fine->tourn->id,
        start => $fine->start,
        reason => $fine->reason,
        end => $fine->end,
        amount => $fine->amount,
    });
 
}

