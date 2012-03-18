package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/first last chapter novice grad_year retired gender/);
Tab::Student->columns(Other => qw/timestamp phonetic/);
Tab::Student->columns(TEMP => qw/num_entries total_sweeps compcode chname/);
Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_many(comps => 'Tab::Comp', 'student');
Tab::Student->has_many(parts => 'Tab::Comp', 'partner');
Tab::Student->has_many(results => 'Tab::StudentResult', 'student');

Tab::Student->set_sql(active_by_chapter => " 
				select distinct student.* from student
				where chapter = ? 
				and grad_year >= ? 
				and retired != 1");

Tab::Student->set_sql(team_members => "
			select distinct student.*
			from team_member,student
			where team_member.comp= ? 
			and team_member.student = student.id ");

Tab::Student->set_sql(by_tournament => "
			select distinct student.* from student,comp,school
			where (student.id = comp.student or student.id = comp.partner)
			and comp.school = school.id
			and school.tournament = ? ");

Tab::Student->set_sql(by_school => " 
					select distinct me.* from student me, comp 
					where (me.id = comp.student or me.id = comp.partner)
					and comp.school = ?
					and comp.dropped != 1");
	
Tab::Student->set_sql(team_members_by_school => "
			select student.* from student,comp,team_member
			where comp.school = ? 
			and comp.dropped != 1
			and team_member.comp = comp.id
			and team_member.student = student.id ");

sub teams {
    my ($self,$tourn) = @_;
    return Tab::Comp->search_team_members($self->id, $tourn->id);
}

sub entries { 
    my ($self,$tourn) = @_;
	my @entries; 
	push (@entries, $self->teams($tourn));
	push (@entries, $self->comps(tournament => $tourn->id));
	push (@entries, $self->parts(tournament => $tourn->id));
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
	my @housings = Tab::Housing->search( student => $self->id, tournament => $tourn->id, night => $day->ymd ) if $day;
	@housings = Tab::Housing->search( student => $self->id, tournament => $tourn->id ) unless $day;
	return shift @housings if $day;
	return @housings;
}

Tab::Student->set_sql(sweeps_points => qq{
	select student.id,student.first,student.last,
		SUM(comp.sweeps_points) as total_sweeps,COUNT(comp.id) as num_entries
    from comp,student
    where (comp.student = student.id or comp.partner = student.id)
    and comp.tournament = ?
	and comp.dropped != 1
    GROUP BY student.id
    ORDER BY student.last });

Tab::Student->set_sql(event_students => "
            select student.*,comp.code as compcode,chapter.name as chname
            from student,comp,chapter
            where student.id = comp.student
			and student.chapter = chapter.id
            and comp.event = ?");

Tab::Student->set_sql(event_partners => "
            select student.*,comp.code as compcode,chapter.name as chname
            from student,comp,chapter
            where student.id = comp.partner
			and student.chapter = chapter.id
            and comp.event = ?");

Tab::Student->set_sql(housed_by_school => "
			select distinct student.* from student,comp,housing
			where student.id = comp.student 
			and comp.school = ? 
			and housing.tournament = comp.tournament
			and housing.student = student.id ");

Tab::Student->set_sql(housed_partners_by_school => "
			select distinct student.* from student,comp,housing
			where student.id = comp.partner 
			and comp.school = ? 
			and housing.tournament = comp.tournament
			and housing.student = student.id ");


