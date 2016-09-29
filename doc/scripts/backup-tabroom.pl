#!/usr/bin/perl

use DateTime;

my @databases = "tabroom";

my $dt = DateTime->now;
my $date = $dt->ymd;
chomp $date;

print "Dumping databases to file:";

my $garbage;
my $backup_dir = "/var/backups/sqlfiles";

`rm $backup_dir/*.*`;

foreach my $db (@databases) { 

	chomp $db;
	my $sql_file = "$backup_dir/$db.sql";
	print "BACKING UP $db to $sql_file....";
	`/bin/rm $sql_file > /dev/null 2>&1`;
	`/bin/rm $sql_file.bz2 > /dev/null 2>&1`;

	`/bin/mkdir -p $backup_dir`;
	`/usr/bin/mysqldump $db > $sql_file`;
	print "...done.\n";

	print "\nCompressing.....";
	`/bin/bzip2 $sql_file`;
	print "...done.\n";

	print "\nPermissions:....";
	`/bin/chmod -R 660 $sql_file.bz2`;
	print "...done.\n";

	print "\nCopying to S3:....";
	`/usr/bin/s3cmd put $sql_file.bz2 s3://tabroom-db/$date/$db.sql.bz2`;
	`/usr/bin/s3cmd cp s3://tabroom-db/$date/$db.sql.bz2 s3://tabroom-db/latest/$db.sql.bz2`;
	print "...done.\n";

}

print "\nBackup of databases completed on ".$dt->mdy('/')." at ".$dt->hms(':')." \n";

# Delete the backup from 2 weeks ago, unless it's the one from the 1st day of the month.
my $delete_day = $dt->clone;
$delete_day->subtract(days => 14);

unless ($delete_day->day == 1) {
	print "Deleting the backup from two weeks ago:";
	my $delete_date = $delete_day->ymd;
	`/usr/bin/s3cmd del -r s3://tabroom-db/$delete_date`;
	print "..done\n";
}

