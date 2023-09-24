package Tab;
use utf8;
use Text::Unidecode;

# Path to your LaTeX binaries
$latex_path_prefix="/usr/bin";

# FQDN of your server.  Do NOT include http://
$hostname = "www.tabroom.com";

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
$toc_email = "tocbids\@tabroom.com";

# Upload size limit in bytes (after the multiply)
$upload_limit = 25 * 1000 * 1000;

#Email SMTP server.
$smtp_server ="localhost";
$admin_smtp_server = "localhost";

#Database name
$dbname = "tabroom";
#Database host.  "localhost" will use the mysql local socket, not the network
$dbhost = "host.docker.internal";
#Database username
$dbuser = "tabroom";
#Database password
$dbpass = "localdbpass";
#Token login random string
$string	= "localtokenstring"

#GeoIP data
$geoip = "/var/lib/geoip/GeoLite2-City.mmdb";
$geoisp = "/var/lib/geoip/GeoIP2-ISP.mmdb";

#Share API endpoint
$share_api_endpoint = "/v1/share";
$docshare_key = "";

#NSDA Jitsi Server key
$jitsi_key = "";
$jitsi_uri = "https://campus.speechanddebate.org";

# NSDA Store checkout details
$nsda_store_api = "https://www.speechanddebate.org/store/wp-json/cocart/v2/cart/add-item";
$nsda_store_redirect = "https://www.speechanddebate.org/store/cart?cocart-load-cart=";
$nsda_product_codes = {
    tabroom          => "96778",
    campus           => "98031",
    campus_observers => "98032"
};

# NSDA API Key
$nsda_api_user     = "10382636";
$nsda_api_key      = "";
$nsda_api_endpoint = "https://api.speechanddebate.org";
$nsda_api_version  = "/v2";

#Hacky API key
$hacky_api_key     = "";

#Component root
$file_root = '/www/tabroom/web/';
$data_dir = '/www/tabroom/web/mason/cache';

# S3 file storage
$s3_config = $file_root."lib/s3.config";
$s3_cmd    = "$latex_path_prefix/s3cmd --config $s3_config ";
$s3_bucket = "s3://tabroom-files";
$s3_base   = "https://s3.amazonaws.com/tabroom-files";
$s3_url    = $s3_base."/tourns";

# Discourse SSO
$discourse_secret = "";

#NAUDL Salesforce API integration
$naudl_username         = 'salesforce@tabroom.com';
$naudl_password         = '';
$naudl_token            = 'h3MOVRHXGcAlXmeweOuG3K0s';
$naudl_host             = 'https://cs45.salesforce.com';
$naudl_client_id        = '';
$naudl_client_secret    = '';

$naudl_tourn_endpoint   = '/services/apexrest/v.1/TournamentService';
$naudl_student_endpoint = '/services/apexrest/v.1/StudentServiceTabroom';
$naudl_sta_endpoint     = '/services/apexrest/v.1/STATabroomService';

######  You shouldn't have to change anything below this line  ##########

#Cookie domain
$cookie_domain = $hostname;

# Path to gzip for entryression
$bzip2 = "$latex_path_prefix/bzip2";

# Path to ghostscript for entryression
$gs_path = "$latex_path_prefix/gs";

#Path to latex for printing
$latex_path = "$latex_path_prefix/latex";
$pdflatex_path = "$latex_path_prefix/pdflatex";

#Path to latex2rtf for printing
$latex2rtf_path = "$latex_path_prefix/latex2rtf";

#Path to dvipdfm for printing
$dvipdfm_path = "$latex_path_prefix/dvipdfm -p letter";

#Mason root.  Keep this as a default unless you know what you're doing.
$mason_root = $file_root .'/mason';

#Perl module library.  Also keep as a default.
$perl_lib = $file_root.'/lib';

sub debuglog {
	my $string = unidecode(shift);
	Sys::Syslog::setlogsock('unix');
	Sys::Syslog::openlog ('tabroom','ndelay','local3');
	Sys::Syslog::syslog('debug',$string);
	Sys::Syslog::closelog;
	return;
}

sub log {
	my $string = unidecode(shift);
	Sys::Syslog::setlogsock('unix');
	Sys::Syslog::openlog ('tabroom','ndelay','local5');
	Sys::Syslog::syslog('info',$string);
	Sys::Syslog::closelog;
	return;
}

sub eventlog {
	my $string = shift;
	`$Tab::log $string`;
}

# Function to sanitize LaTeX strings

sub texify {

	my $string = shift;
	return unless $string;

	$string =~ s/\\/\\\\/g;
	$string =~ s/#/\\#/g;
	$string =~ s/\&/\\\&/g;
	$string =~ s/\$/\\\$/g;
	$string =~ s/\£/\\pounds/g;
	$string =~ s/\€/\\euro/g;
	$string =~ s/\¥/\\yen/g;
	$string =~ s/{/\\{/g;
	$string =~ s/}/\\}/g;
	$string =~ s/_/\\_/g;
	$string =~ s/\^//g;
	$string =~ s/<br>/\\break/g;

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

sub phoneme {
	my $phone = shift;
	if (length($phone) == 10) {
		my ($area, $exch, $number) = unpack "A3A3A4", $phone;
		return "($area) $exch-$number";
	} else  {
		return $phone;
	}
}

sub niceshortdt {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day." ".$dt->hour_12.":".$dt->strftime('%M').substr($dt->strftime('%p'),0,1);
	return $string;
}

sub shortdate {
    my $dt = shift;
    return unless $dt;
    my $string = $dt->month."/".$dt->day."/".substr($dt->year,2,2);
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

sub shortertime {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->hour.":".$dt->strftime('%M');
	return $string;
}

sub nicedate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->day_abbr.", ".$dt->month_name." ".$dt->day.", ".$dt->year;
	return $string;
}

sub nicefulldate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month_name." ".$dt->day.", ".$dt->year;
	return $string;
}

sub nicefulldayte {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month_name." ".$dt->day.", ".$dt->year;
	return $string;
}

sub eurodate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->day." ".$dt->month_abbr." ".$dt->year;
	return $string;
}

sub niceshortdate {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->month."/".$dt->day;
	return $string;
}

sub niceshortdayte {
	my $dt = shift;
	return unless $dt;
	my $string = $dt->day_abbr." ".$dt->month."/".$dt->day;
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
	my ($date, $time, $tz) = @_;
	my $month;
	my $day;
	my $year;
	my $hour = "00";
	my $minute = "00";
	my $ampm = "AM";

	$tz = 'UTC' unless $tz;

	if ($date) {
		($month,$day,$year) = split('/', $date);
	}

	if ($time) {

		my $rest;

		($hour,$rest) = split(':', $time);

		$minute = $rest;
		$ampm = $rest;

		$minute =~ s/\D//g;
		$ampm =~ s/\d//g;
		$ampm =~ s/\s//g;

		$minute = "00" unless $minute;
		$ampm = uc($ampm);
		$ampm = "PM" if $ampm eq "P";
		$ampm = "AM" unless $ampm eq "PM";

		$hour = '00' if $hour == 12;
		$hour = $hour +12 if $ampm eq "PM";

	}

	my $dt;

		$dt = DateTime->new(
			year      => $year,
			month     => $month,
			day       => $day,
			hour      => $hour,
			minute    => $minute,
			time_zone => $tz
		);

	return $dt;

}

sub xmldtme {

	my ($datetime, $tz) = @_;

	my ($date, $time, $ampm) = split (/\ /, $datetime);
	my ($month,$day,$year) = split('/', $date);

	my $rest;
	my $hour;
	my $minute;

	$tz = "UTC" unless $tz;

	($hour, $minute) = split(':', $time);

	if ($hour eq "12") {
		if (substr(uc($ampm), 0, 1) eq "P") {
			$hour = "12";
		} else {
			$hour = "00";
		}
	}

	my $dt = DateTime->new(
		year      => $year,
		month     => $month,
		day       => $day,
		hour      => $hour,
		minute    => $minute,
		time_zone => $tz
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
			year   => $year,
			month  => $month,
			day    => $day,
			hour   => $hour,
			minute => $minute
		);

	};

	return $dt;

}

sub tzname {
	my $tz = shift;
	my $tz_ob = DateTime::TimeZone->new( name => $tz );
	my $now = DateTime->now(time_zone => $tz);
	return $tz_ob->short_name_for_datetime( $now )
}

sub phone {
	my $number = shift;
	$number =~ s/^(\d{3})(\d{3})(\d{4})$/($1)\ $2-$3/;
	return $number;
}


sub short_name  {

	my ($name, $limit) = @_;

	return unless $name;

	chomp $name;
    $name =~ s/\s+$//;  #trailing spaces

	#screw these people.
	$name = "Thomas Jefferson" if lc($name) eq "thomas jefferson high school of science and technology";
	$name = "Thomas Jefferson" if lc($name) eq "thomas jefferson high school of science & technology";
	$name = "Bronx Science" if lc($name) eq "the bronx high school of science";
	$name = "Whitney Young" if lc($name) eq "whitney m. young magnet high school";
	$name = "Lane Tech" if lc($name) eq "lane tech college prep h.s.";
	$name = "NewSkool" if $name eq "New School";
	$name = "NewSkool" if $name eq "The New School";
	$name = "BCAC" if $name eq "BC Academy";

	$name =~ s/\.$//g;
    $name =~ s/\s+$//;  #trailing spaces

	$name =~ s/College Prep H.S.//g;
	$name =~ s/College Prep HS//g;
	$name =~ s/College Prep High School//g;
	$name =~ s/College\ Prep$/CP/g;

	$name = "BC" if $name eq "Boston College";
	$name = "BU" if $name eq "Boston University";

	$name =~ s/Debate Association$//g;
	$name =~ s/Debate Panel$//g;
	$name =~ s/Debate Society$//g;
	$name =~ s/Debating Society$//g;
	$name = "NYU" if $name eq "New York University";
	$name =~ s/of Math and Science$//g;
	$name =~ s/ Academy$//g;
	$name =~ s/Regional\ High\ School$//g;
	$name =~ s/High\ School$//g;
	$name =~ s/ Colleges$//g;
	$name =~ s/ School$//g;
	$name =~ s/ school$//g;
	$name =~ s/ Schools$//g;
	$name =~ s/ schools$//g;
	$name =~ s/ High$//g;


    $name =~ s/\s+$//;  #trailing spaces
	$name =~ s/\.$//g;

	$name =~ s/ H.S$//g;
	$name =~ s/ HS$//g;
	$name =~ s/ M.S$//g;
	$name =~ s/ MS$//g;
	$name =~ s/ \(MS\)$//g;
	$name =~ s/ JH$//g;
	$name =~ s/ Jr$//g;
	$name =~ s/ JR$//g;
	$name =~ s/ Middle$//g;
	$name =~ s/ \(Middle\)$//g;
	$name =~ s/ Elementary$//g;
	$name =~ s/ \(Elementary\)$//g;
	$name =~ s/ Intermediate$//g;
	$name =~ s/ Community$//g;
	$name =~ s/ \(Intermediate\)$//g;
	$name =~ s/ Junior$//g;
	$name =~ s/ Middle School of the Arts$/ Arts/g;
	$name =~ s/ School of the Arts$/ Arts/g;
	$name =~ s/ \(Middle\)$//g;
	$name =~ s/Regional$//g;
	$name =~ s/ Academy$//g;
	$name =~ s/ School$//g;
	$name =~ s/ school$//g;
	$name =~ s/ Schools$//g;
	$name =~ s/ schools$//g;
	$name =~ s/ Sr$//g;

	$name =~ s/ Preparatory$/ Prep/g;


	#Why do people do this of all things?
	$name =~ s/ Speech and Debate Club$//g;
	$name =~ s/ Club$//g;
	$name =~ s/ Forensics$//g;
	$name =~ s/ Speech$//g;
	$name =~ s/ Debate$//g;
	$name =~ s/ \&$//g;
	$name =~ s/ and$//g;
	$name =~ s/ \+$//g;
	$name =~ s/ Speech$//g;
	$name =~ s/ Debate$//g;
	$name =~ s/ Parliamentary$//g;

	$name =~ s/Public\ Charter//g;
	$name =~ s/ Charter\ Public//g;
	$name =~ s/University\ of//g;
	$name =~ s/California State University,/CSU/g;
	$name =~ s/California State University/CSU/g;
	$name =~ s/California,/UC/g;
	$name =~ s/ University$//g;
	$name =~ s/ College$//g;
	$name =~ s/State\ University,/State\ /g;
	$name =~ s/^The //g;
	$name =~ s/^Saint\ /St\ /g;
	$name =~ s/^St\.\ /St\ /g;
	$name =~ s/H\.\ S\./HS/g;
	$name =~ s/High\ School/HS/g;
	$name = "Boston College" if $name eq "BC";
	$name = "Boston Uni" if $name eq "BU";
	$name = "College Prep" if $name eq "CP";  #Sometimes it's the whole school name.  Oops.
	$name =~ s/ CP //g;
	$name =~ s/ CP$//g;
	$name =~ s/NewSkool/New School/g;
	$name =~ s/BCAC/BC Academy/g;

    $name =~ s/^\s+//;  #leading spaces
    $name =~ s/\s+$//;  #trailing spaces

    if ($limit) {
        return substr($name,0,$limit);
    }

    return $name;

}


