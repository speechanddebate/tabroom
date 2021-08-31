
	update permission set tag = "tabber" where tag  = "full_admin";

	update permission set tag = "checker" where tag = "entry_only";
	update permission set tag = "checker" where tag = "limited";
	update permission set tag = "checker" where tag = "category_tab";
	update permission set tag = "checker" where tag = "registration";
	update permission set tag = "checker" where tag = "tabbing";

	update permission set tag = "by_event" where tag  = "detailed";

	delete from event_setting where id in (select event_setting.id
	from event_setting
	where event_setting.tag = 'online_mode'
	and not exists (
		select es.id
		from event_setting es
		where es.tag = 'online'
		and es.event = event_setting.event
	));
