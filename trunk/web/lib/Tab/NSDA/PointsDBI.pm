package Tab::NSDA::PointsDBI;
use base 'Class::DBI';
use Class::DBI::AbstractSearch;

Tab::NSDA::PointsDBI->connection("dbi:mysql:$Tab::points_dbname:$Tab::points_dbhost", $Tab::points_dbuser, $Tab::points_dbpass) || die $!;;



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

