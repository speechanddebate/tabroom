package Tab::ResultFile;
use base 'Tab::DBI';
Tab::ResultFile->table('resultfile');
Tab::ResultFile->columns(Primary => qw/id/);
Tab::ResultFile->columns(Essential => qw/name filename tournament timestamp/);
Tab::ResultFile->has_a(tournament => 'Tab::Tournament');
