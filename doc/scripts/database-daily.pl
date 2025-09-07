#!/usr/bin/perl

	use strict;

	use MIME::Lite;
	use DateTime;
	use HTML::FromText;

	my $test_mode;# = "foo"; # Enable this for testing

	if ($test_mode) {
		print "Running the test mode:\n";
	} else {
		print "Dumping databases to file:\n";
	}

	my $database_dir = "/backups/database-dailies";
	my $db_host      = "localhost";
	my $s3_bucket    = "s3://nsda-backups";
	my $s3_archive   = "s3://nsda-archives";

	# Databases that should be backed up if none are supplied.
	my @databases;

	if (@ARGV) {
		@databases = @ARGV;
	} elsif ($test_mode) {

	} else {
		@databases = (
			"tabroom",
		);
	}

	my $now  = DateTime->now(time_zone => "America/Chicago");
	my $monthdir = $now->year."-".$now->strftime('%m');
	my $day  = $now->strftime('%d');
	my $day_abbr  = $now->day_abbr;

	my $date = $now->ymd;
	chomp $date;

	my $db_dest = $database_dir."/".$date;

	foreach my $db (@databases) {

		chomp $db;
		my $sql_file = $db_dest."/$db.sql";

		print "BACKING UP $db to $sql_file....";

		`rm -f $sql_file.bz2 /dev/null 2>&1`;
		`mkdir -p $db_dest`;
		`/usr/bin/mysqldump -h $db_host --quick --single-transaction --routines --triggers $db > $sql_file`;

		print "...done.\n";
	}

	foreach my $db (@databases) {

		chomp $db;

		my $sql_file = $db_dest."/$db.sql";

		print "\nCompressing $db.....";
		`/usr/bin/pbzip2 $sql_file`;
		print "...done.\n";

		`rm -f $sql_file /dev/null 2>&1`;

		print "\nPermissions:....";
		`/bin/chmod -R 660 $sql_file.bz2`;
		print "...done.\n";
	}

	my $mail_results;
	my $failures;

	foreach my $db (@databases) {

		chomp $db;
		my $sql_file = $db_dest."/$db.sql";

		print "\nUploading $db to S3:....";
		`/usr/local/bin/aws s3 cp $sql_file.bz2 $s3_bucket/$monthdir/$day/$db.sql.bz2`;
		print "...done.\n";

		my $verify = `/usr/local/bin/aws s3 ls $s3_bucket/$monthdir/$day/$db.sql.bz2`;
		print "Verification Output: $verify.\n";

		print "\nCopying to latest:...";
		`/usr/local/bin/aws s3 cp $s3_bucket/$monthdir/$day/$db.sql.bz2 $s3_bucket/00-latest/$db.sql.bz2`;
		print "...done.\n";

		my $ls_result = `/usr/local/bin/aws s3 ls $s3_bucket/$monthdir/$day/$db.sql.bz2`;
		$ls_result =~ s/^\s+//;
		$ls_result =~ s/\s+$//;
		$ls_result =~ s/\s+/ /g;

		my (
			$filedate, $filetime, $filesize, $filename
		) = split(/\ /, $ls_result);

		if ($filesize > 0) {
			$mail_results .= "Database $db\nBackup OK\n";
			$mail_results .= "Size: $filesize\n";
			$mail_results .= "Directory: $s3_bucket/$filedate\n";
			$mail_results .= "Time: $filetime\n\n\n\n";
		} else {
			$failures .= "BACKUP OF DB $db APPARENTLY FAILED?!\n";
			$failures .= "Size $filesize\nFull Result: $ls_result\n\n";
		}

		if ($day == 1 || $day eq '01') {
			print "Copying AWS S3 backup to first of month archive in archives/$monthdir";
			$mail_results .= "Copying AWS S3 backup to first of month archive in archives/$monthdir: ";
			my $answer = `/usr/local/bin/aws s3 cp $s3_bucket/$monthdir/$day/$db.sql.bz2 $s3_archive/$monthdir/$db.sql.bz2`;
			$mail_results .= $answer;
			print $answer."\n";
		}
	}

	$mail_results .= "\nBackup of databases completed on ".$now->mdy('/')." at ".$now->hms(':')." \n";
	my $subject = "Database Backups success: $day_abbr";
	$subject = "DATABASE BACKUP FAILURE: $day_abbr" if $failures;

	my $body;

	$body .= $failures."\n\n\n\n" if $failures;
	$body .= "Successful backups:\n\n";
	$body .= $mail_results."\n";

	my $html = text2html(
		$body,
		lines     => 1,
		metachars => 0
	);

	if ($test_mode) {

		print "REPORT email would say:\n";
		print $body;
		print "\n";
		print $html;
		print "\n";
		print "\n";

	} else {

		system "/usr/bin/find $database_dir -mtime +14 -type f -exec rm -rf {} +";

		# creating new "base"-object for an email
		my $msg = MIME::Lite->new(
		    From     => "NSDA Backup System <backups\@speechanddebate.org>",
		    To       => 'palmer@speechanddebate.org',
		    Subject  => $subject,
		    Type     => "text/html",
		    Data     => $html."\n\n"
		);

		$msg->send();
	}

