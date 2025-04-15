#!/usr/bin/perl

	use DBI;
	use strict;

	my $db_name = 'tabroom';
	my $db_host = 'tabroom-db.in.speechanddebate.org';
	my $db_user = 'ansiblebot';
	my $db_pass = 'readOnlyPassword';

	my $dbh = DBI->connect(
	"DBI:mysql:database=$db_name;host=$db_host",
		$db_user,
		$db_pass,
		{mysql_enable_utf8 => 1}
	) || die $!;

	my $sth = $dbh->prepare('
		select server.id, server.created_at, server.hostname, server.status
		from server
		where status = "deploying"
		order by created_at
	');
	$sth->execute();

	my $results = $sth->fetchall_hashref('id');

	my $status_sth = $dbh->prepare('update server set status = ? where id = ?');

	foreach my $id (keys %{$results}) {
		my $row = $results->{$id};
		print "Machine ".$row->{hostname}." created ".$row->{created_at}." has status ".$row->{status}."\n";
		system "logger -t tabweb.info Machine ".$row->{hostname}." created ".$row->{created_at}." has status ".$row->{status};

		if ($row->{status} eq "deploying") {

			$status_sth->execute("installing", $id);

			print "Deploying images to ".$row->{hostname}."\n";
			system "logger -t tabweb.info Deploying images to ".$row->{hostname};
			my $command = 'cd /opt/deathstar; /home/ansible/.local/bin/ansible-playbook ansible/tabroom.yml --limit='.$row->{hostname}.' ';
			my $other = '/home/ansible/.local/bin/ansible-playbook ansible/tabroom.yml --limit='.$row->{hostname}.' ';
			my $output = `$command`;

			print "Deploy of ".$row->{hostname}." finished with output: \n";
			print "$output \n";

			system "logger -t tabweb.info Deploy of ".$row->{hostname}." finished";
			$status_sth->execute("running", $id);
		}
	}


