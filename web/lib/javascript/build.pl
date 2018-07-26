#!/usr/bin/perl

use JavaScript::Minifier qw(minify);

# Builds the javascript files together and then minifies them. 

# order matters here, jquery comes first etc. 

	my $source_dir = "sources";

	my @source_files = (
		'jquery.js',
		'jquery-ui.js',
		'metadata.js',
		'jquery-migrate.js',
		'jquery.ui.touch-punch.min.js',
		'chosen-selects.js',
		'jquery.tablesorter.combined.js',
		'widget-columnSelector.js',
		'widget-print.js',
		'widget-output.js',
		'uniform.min.js',
		'cycle.js',
		'alertify.js',
		'mousetrap.js',
		'timepicker.js',
		'stopwatch.js',
		'slick.js'
	);

	my $custom_file = "tabroom.js";
	my $output_file = "tabroom.min.js";

	# First, concatenate them together

	my $temp_file = $output_file.".tmp";

	foreach my $source_file (@source_files) { 
		`/bin/cat $source_dir/$source_file >> $temp_file`;
	}

	# Then, add in the custom stuff
	`/bin/cat $custom_file >> $temp_file`;

	# Then minify
	`/bin/mv $output_file $output_file.old`;

	open(INFILE, $temp_file) or die;

	open(OUTFILE, "> $output_file") or die;

  	minify(input => *INFILE, outfile => *OUTFILE);

  	close(INFILE);
  	close(OUTFILE);

	`/bin/rm $temp_file`;

	print "Minification complete.\n";


