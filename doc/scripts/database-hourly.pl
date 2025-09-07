#!/usr/bin/perl

	use DateTime;
	use MIME::Lite;
	use DateTime;
	use HTML::FromText;

	my $test_mode; # = "foo"; Enable this for testing

	if ($test_mode) {
		print "Running the test mode:\n";
	} else {
		print "Dumping databases to file:\n";
	}

	my $database_dir = "/backups/database-hourlies";
	my $db_host      = "localhost";
	my $s3_bucket    = "s3://nsda-backups";

	# Databases that should be backed up if none are supplied.
	my @databases;

	if (@ARGV) {
		@databases = @ARGV;
	} elsif ($test_mode) {

	} else {
		@databases = (
			"tabroom"
		);
	}

	my $now  = DateTime->now(time_zone => "America/Chicago");
	my $week = $now->year."-".$now->strftime('%m')."-".$now->week_of_month;
	my $day  = $now->strftime('%d');
	my $hour = $now->strftime('%H');
	my $then = $now->clone();
	$then->subtract(hours => 6);
	my $then_hour = $then->strftime('%H');

	my $date = $now->ymd;
	chomp $date;

	my $db_dest = "$database_dir/$hour";

	my $failures;

	foreach my $db (@databases) {

		chomp $db;
		my $sql_file = $db_dest."/$db.sql";

		print "BACKING UP $db to $sql_file....";

		`rm -f $sql_file.bz2 /dev/null 2>&1`;
		`mkdir -p $db_dest`;
		`/usr/bin/mysqldump -h $db_host --quick --single-transaction --routines --triggers $db > $sql_file`;

		my $size = `/usr/bin/ls -lash $sql_file`;

		print "\n";
		print "Uncompressed file $sql_file ls output $size";
		print "\n";

		print "...done.\n";

		print "\nCompressing $db.....";
		`/usr/bin/pbzip2 $sql_file`;
		print "...done.\n";

		$size = `/usr/bin/ls -lash $sql_file.bz2`;

		print "\n";
		print "Uncompressed file size of $sql_file.bz2 is $size";
		print "\n";

		`rm -f $sql_file /dev/null 2>&1`;

		print "\nPermissions:....";
		`/bin/chmod -R 660 $sql_file.bz2`;
		print "...done.\n";

		print "\nCopying to AWS latest:...";
		my $cli_output = `/usr/local/bin/aws s3 cp $sql_file.bz2 $s3_bucket/00-latest/$db.sql.bz2`;
		print $cli_output."\n";
		print "...done.\n";

		my $ls_result = `/usr/local/bin/aws s3 ls $s3_bucket/00-latest/$db.sql.bz2`;
		$ls_result =~ s/^\s+//;
		$ls_result =~ s/\s+$//;
		$ls_result =~ s/\s+/ /g;

		my (
			$filedate, $filetime, $filesize, $filename
		) = split(/\ /, $ls_result);

		unless ($filesize > 0) {
			$failures .= "BACKUP OF DB $db APPARENTLY FAILED?!\n";
			$failures .= "CLI output $cli_output\n";
			$failures .= "Size $filesize\nFull Result: $ls_result\n\n";
		}
	}

	# Prune old database dumps periodically
	`/bin/rm $database_dir/$then_hour/*`;

	if ($failures) {

		my $subject = "Hourly backup ran";
		my $mail_results .= "\nThe Tabroom hourly backup allegdly ran just now, but ran into a problem\n";
		$mail_results .= $failures;

		my $html = text2html(
			$mail_results,
			lines     => 1,
			metachars => 0
		);

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

	print "Done\n";

