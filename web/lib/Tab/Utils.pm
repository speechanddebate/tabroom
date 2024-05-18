package Tab::Utils;
use utf8;
use Text::Unidecode;


sub debuglog {
	my $string = unidecode(shift);
	return unless $string;
	Sys::Syslog::setlogsock('unix');
	Sys::Syslog::openlog ('tabroom','ndelay','local3');
	Sys::Syslog::syslog('debug',$string);
	Sys::Syslog::closelog;
	return;
}

sub log {
	my $string = unidecode(shift);
	return unless $string;
	Sys::Syslog::setlogsock('unix');
	Sys::Syslog::openlog ('tabroom','ndelay','local5');
	Sys::Syslog::syslog('info',$string);
	Sys::Syslog::closelog;
	return;
}

sub eventlog {
	my $string = shift;
	`$Tab::eventlog $string`;
}

# Function to sanitize LaTeX strings
sub texify {
	my $string = shift;
	return unless $string;

	$string =~ s/\\/\\\\/g;
	$string =~ s/#/\\#/g;
	$string =~ s/\&/\\\&/g;
	$string =~ s/%/\%/g;
	$string =~ s/\%/\\%/g;
	$string =~ s/\$/\\\$/g;
	$string =~ s/\£/\\pounds/g;
	$string =~ s/\€/\\euro/g;
	$string =~ s/\¥/\\yen/g;
	$string =~ s/{/\\{/g;
	$string =~ s/}/\\}/g;
	$string =~ s/_/\\_/g;
	$string =~ s/\^//g;
	$string =~ s/\%/\\\%/g;
	$string =~ s/<br>/ \\break /g;
	$string =~ s/<br \/>/ \\break /g;
	return $string;
}

sub school_year {

	my $now = DateTime->now;
    my $year = $now->year;
    $year-- if $now->month < 7;

	my $dt = DateTime->new({
		year   => $year,
		month  => 7,
		day    => 1,
		hour   => 0,
		minute => 0,
		second => 0 });

	return $dt;
}

sub phone {
	my $number = shift;
	$number =~ s/[\D_]//g;
	$number =~ s/^1//;

	if (length($number) == 10) {
		$number =~ s/^(\d{3})(\d{3})(\d{4})$/($1)\ $2-$3/;
	}
	return $number;
}

sub phoneme {
	#Real friggin brilliant, write the same subfunction 2x.
	Tab::phone(shift);
}

sub tzname {
	my $tz = shift;
	return unless $tz;
	my $tz_ob = DateTime::TimeZone->new( name => $tz );
	my $now = DateTime->now(time_zone => $tz);
	return $tz_ob->short_name_for_datetime( $now )
}

sub compress {
	my $text = shift;
	return unless $text;
	utf8::encode($text);
	$text = Compress::Zlib::compress($text, 8);
	$text = MIME::Base64::encode_base64($text);
	return $text;
}

sub decompress {
	my $text = shift;
	return Tab::Utils::uncompress($text)
}

sub uncompress {
	my $text = shift;
	return unless $text;
	$text = MIME::Base64::decode_base64($text);
	$text = Compress::Zlib::uncompress($text);
	return $text;
}

return 1;
