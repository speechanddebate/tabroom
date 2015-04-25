#!/usr/bin/perl

	use strict;

	foreach my $file (`/bin/ls /www/tabroom/web/funclib`) { 

		chomp $file;

		my $usage = (`/bin/grep -r $file /www/tabroom/web | grep -v svn | grep -v mason`);

		unless ($usage) { 
			print "File $file is never called \n";
			system "svn rm /www/tabroom/web/funclib/$file";
		}

	}


