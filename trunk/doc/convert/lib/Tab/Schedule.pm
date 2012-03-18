
package Tab::Schedule;
use base 'Tab::DBI';
Tab::Schedule->table('schedule');
Tab::Schedule->columns(Primary => qw/id/);
Tab::Schedule->columns(Essential => qw/name timestamp location start end/);
Tab::Schedule->columns(Others => qw/file description league/);
__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);

