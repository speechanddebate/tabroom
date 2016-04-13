package Tab::GoogleCalendar;
use LWP::UserAgent;
use HTTP::Request::Common;
use JSON;
use JSON::WebToken;
use Data::Dumper;

# Authenticate with Google Calendar API
sub auth {
	my $self = shift;
	# Get an API access token from Google
	my $jwt = JSON::WebToken::encode_jwt(
		{
			iss   => $Tab::google_client_email,
			scope => 'https://www.googleapis.com/auth/calendar',
			aud   => 'https://accounts.google.com/o/oauth2/token',
			exp   => time() + 10,
			iat   => time(),
			prn   => $Tab::google_user_email
		},
		$Tab::google_client_key,
		'RS256',
		{ typ => 'JWT' }
	);
	my $ua = LWP::UserAgent->new;
	my $jwt_response = $ua->post(
		'https://accounts.google.com/o/oauth2/token',
		{
			grant_type => 'urn:ietf:params:oauth:grant-type:jwt-bearer',
			assertion => $jwt
		}
	);
	my $json_response = JSON->new->utf8->decode($jwt_response->decoded_content);
	return $json_response->{'access_token'};
}

# Create Google Calendar event (with Hangout automatically generated per Calendar settings)
sub createEvent {
	my $self = shift;
	my $args = shift;

	my $token = $self->auth();
	return undef unless $token;

	my $json = JSON->new;
	$json->allow_blessed();

	my $arguments = 
		{
			summary                 => $args->{'title'},
			description             => $args->{'description'},
			start                   => { dateTime => $args->{'start'} },
			end                     => { dateTime => $args->{'end'} },
			status                  => 'confirmed',
			anyoneCanAddSelf        => 'false',
			guestsCanInviteOthers   => 'false',
			guestsCanSeeOtherGuests => 'false',
			attendees               => $args->{'attendees'}
		};

	my $json_text = $json->utf8->encode($arguments);

	my $url = 'https://www.googleapis.com/calendar/v3/calendars/'.$Tab::google_calendar_id.'/events';
	my $req = HTTP::Request->new(POST => $url);
	$req->content_type('application/json');
	$req->content($json_text);
	$req->header(Authorization => 'Bearer '.$token);
	my $ua = LWP::UserAgent->new;
	my $response = $ua->request($req);
	$json_response = JSON->new->utf8->decode($response->decoded_content);

	return undef unless $json_response->{'id'};
	return {
		id => $json_response->{'id'},
		hangout_link => $json_response->{'hangoutLink'}
	};
}

# Delete a Google Calendar event
sub deleteEvent {
	my $self = shift;
	my $event_id = shift;

	return undef unless $event_id =~ /^[a-zA-Z0-9]+$/;

	my $token = $self->auth();
	return undef unless $token;

	my $json = JSON->new;
	$json->allow_blessed();
	my $url = 'https://www.googleapis.com/calendar/v3/calendars/'.$Tab::google_calendar_id.'/events/'.$event_id;
	my $req = HTTP::Request->new(DELETE => $url);
	$req->header(Authorization => 'Bearer '.$token);
	my $ua = LWP::UserAgent->new;
	my $response = $ua->request($req);

	return $response->is_success && !$response->content;
}

1;
