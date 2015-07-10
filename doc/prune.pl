#!/usr/bin/perl

	use strict;

	my $counter;

	foreach my $subdir (`find . -type d`) { 

		chomp $subdir;

		next if substr($subdir, 0, 5) eq "./lib";
		next if substr($subdir, 0, 5) eq "./tmp";
		next if substr($subdir, 0, 5) eq "./api";
		next if substr($subdir, 0, 7) eq "./mason";

		foreach my $file (`/bin/ls $subdir`) { 

			chomp $file;
			next if $file eq "autohandler";
			next if $file eq "mason";
			next if $file eq "robots.txt";

			my $usage = (`/bin/grep -r $file /www/tabroom/web | grep -v svn | grep -v mason`);

			unless ($usage) { 
				print "File $subdir/$file is never called \n";
				$counter++;
				#system "svn rm /www/tabroom/web/$subdir/$file";
			}

		}
	}

	print "\n\nTOTAL: $counter\n";


