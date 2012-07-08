package Tab;

# Path to your LaTeX binaries
my $latex_path_prefix="/usr/bin";
#my $latex_path_prefix="/opt/local/bin";

# FQDN of your server.  Do NOT include http:// 
my $hostname = "itab.tabroom.com";

# Debugging status.  Set to a non-zero number to increase logging levels
my $debug;

#URL Prefix of the server
$url_prefix = "https://$hostname" if $hostname eq "www.tabroom.com";
$url_prefix = "http://$hostname" unless $url_prefix;
$url_domain = "tabroom.com";

#Static cookie-free URL 
$static_url = "http://static.tabroom.com" if $hostname eq "www.tabroom.com";
$static_url = $url_prefix unless $static_url;

# Administrator's email address
$admin_email = "help\@tabroom.com";

#Email SMTP server.  
$smtp_server ="localhost";

#Database name
$dbname = "itab";

#Database host.  "localhost" will use the mysql local socket, not the network
$dbhost = "localhost";

#Database username
$dbuser = "itab";

#Database password
$dbpass = "ifoo1uKahs7aecu%See2xa0ieX";

#Entryonent root
$file_root = '/www/itab/web/';

######  You shouldn't have to change anything below this line  ##########

#Cookie domain
$cookie_domain = $hostname;

# Path to gzip for entryression
$bzip2 = "$latex_path_prefix/bzip2";

# Path to ghostscript for entryression
$gs_path = "$latex_path_prefix/gs";

#Path to latex for printing
$latex_path = "$latex_path_prefix/latex";

#Path to latex2rtf for printing
$latex2rtf_path = "$latex_path_prefix/latex2rtf";

#Path to dvipdfm for printing
$dvipdfm_path = "$latex_path_prefix/dvipdfm -p letter";

#Mason root.  Keep this as a default unless you know what you're doing.
$mason_root = $file_root .'/mason';

#Perl module library.  Also keep as a default.  
$perl_lib = $file_root.'/lib';

#String
$string	= "3EZjdkNB9k92a4qG4Q61";

sub debuglog {
	my $string = shift;
	Sys::Syslog::setlogsock('unix'); 
	Sys::Syslog::openlog ('tabroom','ndelay','local3');
	Sys::Syslog::syslog('debug',$string); 
	Sys::Syslog::closelog;
	return;
}

sub log {
	my $string = shift;
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

# Function to santizie LaTeX strings

sub texify { 

	my $string = shift; 
	return unless $string;

	$string =~ s/\\/\\\\/g;
	$string =~ s/#/\\#/g;
	$string =~ s/\&/\\\&/g;
	$string =~ s/\$/\\\$/g;
	$string =~ s/{/\\{/g;
	$string =~ s/}/\\}/g;
	$string =~ s/_/\\_/g;
	$string =~ s/<br>/\\break/g;

	return $string;

}

sub school_year {

	my $now = DateTime->now;
    my $year = $now->year;
    $year-- if $now->month < 7;

	my $dt = DateTime->new({ 
		year => $year,
		month => 7,
		day => 1,
		hour => 0,
		minute => 0,
		second => 0 });
	
	return $dt;

}

sub niceshortdt {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day." ".$dt->hour_12.":".$dt->strftime('%M').substr($dt->strftime('%p'),0,1);
	return $string;
}

sub niceshortdayt {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->day_abbr." ".$dt->hour_12.":".$dt->strftime('%M')." ".$dt->strftime('%p');
	return $string;
}


sub nicedt {

	my $dt = shift;
	return unless $dt;
	my $string = $dt->day_abbr.", ".$dt->month_abbr." ".$dt->day.", ".$dt->year." at ".$dt->hour_12.":".$dt->strftime('%M')." ".$dt->strftime('%p');
	return $string;

}

sub nicetime {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->hour_12.":".$dt->strftime('%M')." ".$dt->strftime('%p');
	return $string;
}

sub shorttime {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->hour_12.":".$dt->strftime('%M')." ".substr($dt->strftime('%p'),0,1);
	return $string;
}

sub nicedate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->day_abbr.", ".$dt->month_name." ".$dt->day.", ".$dt->year;
	return $string;
}

sub niceshortdate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day;
	return $string;
}

sub pickerdate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day."/".$dt->year;
	return $string;
}

sub pickertime { 
	my $dt = shift;
	return unless $dt;
	my $string = $dt->hour_12.":".$dt->strftime('%M');
	$string .= " PM" if $dt->strftime('%p') eq "PM";
	$string .= " AM" if $dt->strftime('%p') eq "AM";
	return $string;
}

sub xmldt { 
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day."/".$dt->year." ".$dt->hour.":".$dt->strftime('%M')." ".$dt->strftime('%p');
	return $string;
}

sub csvdt { 
	my $dt = shift;
	return unless $dt;
	my $string = $dt->year."-".$dt->month."-".$dt->day." ".$dt->hour.":".$dt->strftime('%M');
	return $string;
}

sub dtme { 
	my ($date, $time) = @_;
	my $month;
	my $day;
	my $year;
	my $hour = "00";
	my $minute = "00";
	my $ampm = "AM";

	if ($date) { 
		($month,$day,$year) = split('/', $date);
	}

	if ($time) { 

		my $rest;

		($hour,$rest) = split(':', $time);
		($minute,$ampm) = split(/\ /, $rest);

		if ($hour == 12) { $hour = '00'; }
		if ($ampm eq "PM" || $ampm eq "pm" || $ampm eq "P" || $ampm eq "p") { $hour = $hour +12; }

	}

	my $dt;

		$dt = DateTime->new(
			year => $year,
			month => $month,
			day => $day,
			hour => $hour, 
			minute => $minute
		);

	return $dt;

}

sub dateme { 

	my $date = shift;
	return unless $date;
	my $month;
	my $day;
	my $year;
	my $hour = "00";
	my $minute = "00";
	my $ampm = "a";

	if ($date) { 
		($month,$day,$year) = split('/', $date);
	}

	my $dt;

	eval{

		$dt = DateTime->new(
			year => $year,
			month => $month,
			day => $day,
			hour => $hour, 
			minute => $minute
		);

	};

	return $dt;

}


