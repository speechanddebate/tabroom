#!/usr/bin/perl

use DateTime;

my $dt = DateTime->now;
my $hour = $dt->hour;

my $db = "tabroom";
my $backup_dir = "/var/backups/sqlfiles";

my $sql_file = "$backup_dir/$db.sql";

`rm $backup_dir/*.bz2`;

print "BACKING UP $db to $sql_file....";
`/bin/mkdir -p $backup_dir`;
`/usr/bin/mysqldump $db > $sql_file`;
print "...done.\n";

print "\nCompressing.....";
`/bin/bzip2 $sql_file`;
print "...done.\n";

print "\nPermissions:....";
`/bin/chmod -R 660 $sql_file.bz2`;
print "...done. ";

print "\nCopying $sql_file.bz2 to S3:....";
`/usr/bin/s3cmd put $sql_file.bz2 s3://tabroom-db/hourly/$hour-$db.sql.bz2`;
`/usr/bin/s3cmd cp s3://tabroom-db/hourly/$hour-$db.sql.bz2 s3://tabroom-db/latest/$db.sql.bz2`;
print "...done. ";

print "\nBackup of $db completed on ".$dt->mdy('/')." at ".$dt->hms(':')." \n";

