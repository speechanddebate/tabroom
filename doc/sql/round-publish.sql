
update round
	set published = 3
	where published = 0
	and exists (
		select rs.id from round_setting rs
		where rs.tag = 'publish_entry_list'
		and rs.round = round.id
	);

update round set published = 5 where published = 0 and exists (
	select rs.id from round_setting rs
	where rs.tag = 'publish_prelim_chamber'
	and rs.round = round.id
);

delete  from round_setting where tag = 'entry_list_no_byes';

