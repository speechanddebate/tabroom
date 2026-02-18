
alter table event add nsda_category int after rating_subset;

alter table event add foreign key(nsda_category) references nsda_category(id) on delete set null on update cascade;

update event, event_setting es
	set event.nsda_category = es.value
	where event.id = es.event
	and es.tag = 'nsda_event_category';

alter table sweep_event RENAME COLUMN nsda_event_category TO nsda_category;
alter table sweep_event add foreign key(nsda_category) references nsda_category(id) on delete set null on update cascade;


