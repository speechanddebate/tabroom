package Tab::Calendar;
use base 'Class::DBI';

Tab::Calendar->connection("dbi:mysql:$Tab::calendar_dbname:$Tab::calendar_dbhost", $Tab::calendar_dbuser, $Tab::calendar_dbpass) || die $!;

Tab::Calendar->table('calendar');
Tab::Calendar->columns(Primary => qw/calendar_id/);
Tab::Calendar->columns(Essential => qw/start_date end_date title reg_start reg_end state country timezone status_code hidden location contact url tabroom_id source/);
Tab::Calendar->has_a(tabroom_id => "Tab::Tourn");

__PACKAGE__->_register_dates( qw/start_date end_date/);
__PACKAGE__->_register_datetimes( qw/reg_start reg_end/);

sub _register_datetimes {
 	my $class = shift;
 	$class = ref $class if ref $class;
 	foreach my $column (@_) {
		$class->has_a(
			$column => 'DateTime',
     		inflate => \&date_inflate,
     		deflate => \&date_deflate,
   		)
 	}
 	$class;
}

sub date_inflate {
   my $value = shift;
   my $dt = eval { return DateTime::Format::MySQL->parse_datetime($value); };
   $dt = DateTime->now unless $dt;

   $dt->set_time_zone('UTC');
   return $dt;
}

sub date_deflate {
    my $dt = shift;
	return unless $dt;

    $dt->set_time_zone('UTC');
    return DateTime::Format::MySQL->format_datetime($dt);
}

sub _register_dates {
 my $class = shift;
 $class = ref $class if ref $class;
 foreach my $column (@_) {
   $class->has_a(
     $column => 'DateTime',
     inflate => \&date_only_inflate,
     deflate => \&date_only_deflate,
   )
 }
 $class;
}

sub date_only_inflate {
   my $value = shift;
   my $dt = eval { return DateTime::Format::MySQL->parse_date($value); };
   $dt = DateTime->now unless $dt;
   $dt->set_time_zone('UTC');
   return $dt;
}

sub date_only_deflate {
    my $dt = shift;
   	$dt->set_time_zone('UTC');
    return DateTime::Format::MySQL->format_date($dt);
}

