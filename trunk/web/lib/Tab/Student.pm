package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/first last chapter novice grad_year retired gender/);
Tab::Student->columns(Other => qw/timestamp phonetic/);
Tab::Student->columns(TEMP => qw/num_entries total_sweeps entrycode chname/);
Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_many(entrys => 'Tab::Entry', 'student');
Tab::Student->has_many(parts => 'Tab::Entry', 'partner');
Tab::Student->has_many(results => 'Tab::StudentResult', 'student');

Tab::Student->set_sql(active_by_chapter => " 
				select distinct student.* from student
				where chapter = ? 
				and grad_year >= ? 
				and retired != 1");

Tab::Student->set_sql(team_members => "
			select distinct student.*
			from team_member,student
			where team_member.entry= ? 
			and team_member.student = student.id ");

Tab::Student->set_sql(by_tourn => "
			select distinct student.* from student,entry,school
			where (student.id = entry.student or student.id = entry.partner)
			and entry.school = school.id
			and school.tourn = ? ");

Tab::Student->set_sql(by_school => " 
					select distinct me.* from student me, entry 
					where (me.id = entry.student or me.id = entry.partner)
					and entry.school = ?
					and entry.dropped != 1");
	
Tab::Student->set_sql(team_members_by_school => "
			select student.* from student,entry,team_member
			where entry.school = ? 
			and entry.dropped != 1
			and team_member.entry = entry.id
			and team_member.student = student.id ");

sub teams {
    my ($self,$tourn) = @_;
    return Tab::Entry->search_team_members($self->id, $tourn->id);
}

sub entries { 
    my ($self,$tourn) = @_;
	my @entries; 
	push (@entries, $self->teams($tourn));
	push (@entries, $self->entrys(tourn => $tourn->id));
	push (@entries, $self->parts(tourn => $tourn->id));
	return @entries;
}

sub print_events {
	my ($self,$tourn)  = @_;
	my $string;
	foreach my $entry ($self->entries($tourn)) { 
		$string .= ", " if $string;
		$string .= $entry->event->abbr;
	}
	return $string;
}
		

sub housing { 
	my ($self, $tourn, $day) = @_;
	my @housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id, night => $day->ymd ) if $day;
	@housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id ) unless $day;
	return shift @housings if $day;
	return @housings;
}

Tab::Student->set_sql(sweeps_points => qq{
	select student.id,student.first,student.last,
		SUM(entry.sweeps_points) as total_sweeps,COUNT(entry.id) as num_entries
    from entry,student
    where (entry.student = student.id or entry.partner = student.id)
    and entry.tourn = ?
	and entry.dropped != 1
    GROUP BY student.id
    ORDER BY student.last });

Tab::Student->set_sql(event_students => "
            select student.*,entry.code as entrycode,chapter.name as chname
            from student,entry,chapter
            where student.id = entry.student
			and student.chapter = chapter.id
            and entry.event = ?");

Tab::Student->set_sql(event_partners => "
            select student.*,entry.code as entrycode,chapter.name as chname
            from student,entry,chapter
            where student.id = entry.partner
			and student.chapter = chapter.id
            and entry.event = ?");

Tab::Student->set_sql(housed_by_school => "
			select distinct student.* from student,entry,housing
			where student.id = entry.student 
			and entry.school = ? 
			and housing.tourn = entry.tourn
			and housing.student = student.id ");

Tab::Student->set_sql(housed_partners_by_school => "
			select distinct student.* from student,entry,housing
			where student.id = entry.partner 
			and entry.school = ? 
			and housing.tourn = entry.tourn
			and housing.student = student.id ");

