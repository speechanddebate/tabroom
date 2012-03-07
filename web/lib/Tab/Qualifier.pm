package Tab::RatingTierifier;
use base 'Tab::DBI';
Tab::RatingTierifier->table('qualifier');
Tab::RatingTierifier->columns(Primary => qw/id/);
Tab::RatingTierifier->columns(Essential => qw/entry name result tourn timestamp/);
Tab::RatingTierifier->has_a(tourn => 'Tab::Tourn');
Tab::RatingTierifier->has_a(entry => 'Tab::Entry');
