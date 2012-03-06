package Tab::Chapter;
use base 'Tab::DBI';
Tab::Chapter->table('chapter');
Tab::Chapter->columns(Primary => qw/id/);
Tab::Chapter->columns(Essential => qw/name state/);
Tab::Chapter->columns(Others => qw/timestamp/);
Tab::Chapter->columns(TEMP => qw/count/);
Tab::Chapter->has_many(students => 'Tab::Student', 'chapter');
Tab::Chapter->has_many(ubers => 'Tab::Uber', 'chapter');
Tab::Chapter->has_many(credits => 'Tab::Coach', 'chapter');
Tab::Chapter->has_many(schools => 'Tab::School', 'chapter');
Tab::Chapter->has_many(tourns => 'Tab::School', 'chapter');
Tab::Chapter->has_many(league_joins => 'Tab::ChapterLeague', 'chapter');


Tab::Chapter->set_sql(by_tourn => "select distinct me.* from chapter me, school
										where me.id = school.chapter
										and school.tourn = ?");

Tab::Chapter->set_sql(leagues => "select distinct me.*
						from chapter me,chapter_league 
						where me.id = chapter_league.chapter 
						and chapter_league.league = ?");

Tab::Chapter->set_sql(league_and_membership => "select distinct chapter.id 
						from chapter,chapter_league 
						where chapter.id = chapter_league.chapter 
						and chapter_league.league = ? 
						and chapter_league.full_member = ?");

Tab::Chapter->set_sql(league_and_name => "select distinct chapter.id 
						from chapter,chapter_league 
						where chapter.id = chapter_league.chapter 
						and chapter_league.league = ?
						and chapter.name like ? ");

Tab::Chapter->set_sql(  name => "select distinct chapter.id 
						from chapter 
						where chapter.name like ? ");

Tab::Chapter->set_sql(league_and_code => "select distinct chapter.id
                        from chapter,chapter_league
                        where chapter.id = chapter_league.chapter
                        and chapter_league.league = ?
                        and chapter_league.code like ? ");

Tab::Chapter->set_sql(by_admin => "
						select distinct chapter.*
						from chapter,chapter_admin
						where chapter.id = chapter_admin.chapter 
						and chapter_admin.account = ?");

Tab::Chapter->set_sql(by_no_schools => "select chapter.id
        					from chapter left join school
		        				on chapter.id = school.chapter
				        		where school.chapter is null");

Tab::Chapter->set_sql(by_no_students => "select chapter.id
        					from chapter left join student
		        				on chapter.id = student.chapter
				        		where student.chapter is null");

Tab::Chapter->set_sql(school_count =>"select chapter.id,count(school.id) as count 
											from chapter,school
        									where chapter.id = school.chapter
		        							group by chapter.id
				        					order by count");

Tab::Chapter->set_sql(student_count =>"select chapter.id,count(student.id) as count 
											from chapter,student
        									where chapter.id = student.chapter
		        							group by chapter.id
				        					order by count");

sub leagues {
    my $self = shift;
    return sort {$a->name cmp $b->name } Tab::League->search_chapters($self->id);
}

sub dues {
    my ($self, $league) = @_;
    return Tab::Dues->search( chapter => $self->id, league => $league->id );
}

sub paid_dues {

    my ($self, $league) = @_;

	my $school_year = &Tab::school_year;

    my @dues = Tab::Dues->search_where( 
				chapter => $self->id, 
				league => $league->id,
				paid_on => {">=", $school_year}
				);

	my $total = 0;

	foreach (@dues) { $total += $_->amount; }

	return $total; 
}

sub coaches {
    my $self = shift;
    return Tab::Account->search_coaches($self->id);
}

sub full_member {
    my ($self, $league) = @_;
	my @membership = Tab::ChapterLeague->search( 
			chapter => $self->id, league => $league->id );
    return $membership[0]->full_member if @membership;
    return;
}

sub code {
    my ($self, $league) = @_;
    my @membership = Tab::ChapterLeague->search( 
			chapter => $self->id, 
			league => $league->id );
    return $membership[0]->code if @membership;
    return;
}

sub region {
    my ($self, $league) = @_;
    my @membership = Tab::ChapterLeague->search( 
			chapter => $self->id, league => $league->id );
    return $membership[0]->region if @membership;
    return;
}

sub league_membership {
    my ($self, $league) = @_;
    my @membership = Tab::ChapterLeague->search( 
			chapter => $self->id, league => $league->id );
    return $membership[0] if @membership;
    return;
}

sub school { 
	my ($self, $tourn) = @_;
	my @school = Tab::School->search( 
			chapter => $self->id, tourn => $tourn->id);
	return $school[0] if @school;
	return;
}

sub open_tourns {
	my $self = shift;
	return Tab::Tourn->search_open_by_chapter($self->id);
}

sub entered_tourns {
	my $self = shift;
	return Tab::Tourn->search_entered_by_chapter($self->id);
}

sub results_tourns {
	my $self = shift;
	return Tab::Tourn->search_results_by_chapter($self->id);
}

sub nongrads {

    my $self = shift;

    my $now = DateTime->now;

    my $minimum_grad_year = $now->year;

    unless ($now->month < 7) {
        $minimum_grad_year++;
    }

    return Tab::Student->search_active_by_chapter($self->id, $minimum_grad_year);

}

