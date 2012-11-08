package Tab::AccountConflict;
use base 'Tab::DBI';
Tab::AccountConflict->table('account_admin');
Tab::AccountConflict->columns(All => qw/id account conflict chapter added_by timestamp/);
Tab::AccountConflict->has_a(added_by => 'Tab::Account');
Tab::AccountConflict->has_a(account => 'Tab::Account');
Tab::AccountConflict->has_a(conflict => 'Tab::Account');
Tab::AccountConflict->has_a(chapter => 'Tab::Chapter');

