#!/usr/bin/perl

use JavaScript::Minifier qw(minify);

# Builds the javascript files together and then minifies them.

# order matters here, jquery comes first etc.

	my $source_dir = "sources";

	my @source_files = (
		'jquery.js',
		'jquery-passive.js',
		'jquery-ui.js',
		'jquery-migrate.js',
		'jquery.ui.touch-punch.min.js',
		'jquery.tablesorter.combined.js',
		'metadata.js',
		'bigtext.js',
		'select2.min.js',
		'widget-columnSelector.js',
		'widget-print.js',
		'widget-output.js',
		'uniform.min.js',
		'cycle.js',
		'alertify.js',
		'mousetrap.js',
		'timepicker.js',
		'stopwatch.js'
	);

	my $custom_file = "tabroom.js";
	my $output_file = "tabroom.v38.min.js";
	my $sources_file = "sources.js";

	# First, concatenate them together

	my $temp_file = $output_file.".tmp";

	`/bin/rm $sources_file`;

	foreach my $source_file (@source_files) {
		`/bin/cat $source_dir/$source_file >> $temp_file`;
		`/bin/cat $source_dir/$source_file >> $sources_file`;
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

	# Add to git if it doesn't exist already because you constantly forget that
	# and then break the world and then cannot take a shower in peace without
	# people calling your direct line and then wanting to set them all on fire.

	`/usr/bin/git add $output_file`;

	print "Minification complete.\n";


