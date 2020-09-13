package Tab::Score;
use base 'Tab::DBI';
Tab::Score->table('score');
Tab::Score->columns(Primary => qw/id/);
Tab::Score->columns(Essential => qw/ballot tag student value/);
Tab::Score->columns(Others => qw/content speech position topic timestamp/);
Tab::Score->columns(TEMP => qw/panelid entryid roundtype roundid studentid judgeid bye ballotid chair pullup/);

Tab::Score->has_a(ballot => 'Tab::Ballot');
Tab::Score->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub text { 

	my ($self, $input) = @_;

	if ($input) { 
		$self->content(compress($input));
		$self->update();
	} else { 
		return uncompress($self->content);
	}
}

sub compress { 
	my $text = shift;
	return unless $text;
	utf8::encode($text);
	$text = Compress::Zlib::compress($text, 8);
	$text = MIME::Base64::encode_base64($text);
	return $text;
}

sub uncompress { 
	my $text = shift;
	return unless $text;
	$text = MIME::Base64::decode_base64($text);
	$text = Compress::Zlib::uncompress($text);
	return $text;
}

