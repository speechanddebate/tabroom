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
		my $text = Tab::Utils::compress($input);
		chomp $text;
		$self->content($text);
		$self->update();
	} else {
		return Tab::Utils::uncompress($self->content);
	}
}

