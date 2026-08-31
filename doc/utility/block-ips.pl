#!/usr/bin/perl

	open BANLIST, ">/etc/tabroom/IPBans.conf";

	use strict;
	open IPS, "< /var/log/apache2/php-blocks.log";

	my %scores;

	foreach my $line (<IPS>) {
		chomp $line;
		$scores{$line}++;
	}

	foreach my $ip (
		sort {
			$scores{$b} <=> $scores{$a}
		} keys %scores
	) {
		if ($scores{$ip} > 1000) {
			print BANLIST "Require not ip $ip\n";
		}
	}

	open ACCESS, "< /var/log/apache2/tabroom-public.log";
	my %hits;

	foreach my $line (<ACCESS>) {
		chomp $line;
		$hits{$line}++;
	}

	foreach my $ip (
		sort {
			$hits{$b} <=> $hits{$a}
		} keys %hits
	) {
		if ($hits{$ip} > 2000) {
			print BANLIST "Require not ip $ip\n";
		}
	}

	print "Fin\n";
