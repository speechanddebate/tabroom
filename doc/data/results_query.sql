	select

		student.id, student.first, student.gender,
		chapter.name, chapter.zip, chapter.state, chapter.level,
		entry.id,
		tourn.id, tourn.name, tourn.start, tourn.state,
		event.name, event.abbr, event.type,
		round.name, round.label, round.type,
		panel.id, panel.room,
		ballot.id, ballot.side, ballot.bye, ballot.forfeit,
		judge.id, judge.first, chapter_judge.gender,
		winner.value,
		points.value,
		rank.value

	into outfile '/tmp/hymson-query.txt'

		from 
			(student, chapter, entry_student, entry,
			event, tourn, round, 
			panel, ballot, judge, chapter_judge)

			left join score winner 
				on winner.tag = "ballot" 
				and winner.ballot = ballot.id

			left join score points 
				on points.tag = "points" 
				and points.ballot = ballot.id
				and points.student = student.id

			left join score rank 
				on rank.tag = "rank" 
				and rank.ballot = ballot.id
				and rank.student = student.id

			where 

				student.id = entry_student.student
				and student.chapter = chapter.id
				and entry_student.entry = entry.id
				and entry.event = event.id
				and event.id = round.event
				and event.type in ("debate", "ld", "policy", "pf", "parli")
				and event.tourn = tourn.id
				and round.id = panel.round
				and panel.id = ballot.panel
				and entry.id = ballot.entry
				and judge.id = ballot.judge
				and judge.chapter_judge = chapter_judge.id
