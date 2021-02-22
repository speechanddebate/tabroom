
delete result.* from result
where not exists (
	select result_set.id
	from result_set
	where result_set.id = result.result_set
);

delete result_value.* from result_value
where not exists (
	select result.id
	from result
	where result.id = result_value.result
);

delete panel.* from panel
where not exists (
	select round.id
	from round
	where round.id = panel.round
);

delete ballot.* from ballot
where not exists (
	select panel.id
	from panel
	where panel.id = ballot.panel
);

delete score.* from score
where not exists (
	select ballot.id
	from ballot
	where ballot.id = score.ballot
);


create table result_key (
	id int auto_increment primary key,
	tag varchar(63),
	description varchar(255),
	no_sort bool,
	sort_desc bool,
	result_set int,
	timestamp timestamp
);

alter table result_value add result_key int not null default 0 after result;
alter table result_value add tiebreak_set int not null default 0 after result_key;

create index result_set on result_key(result_set);
create index result_key on result_value(result_key);

update score set timestamp=timestamp, tag="winloss" where tag="ballot";
update score set timestamp=timestamp, tag="point" where tag="points";
update score set timestamp=timestamp, tag="rank" where tag="ranks";
update score set timestamp=timestamp, tag="refute" where tag="rebuttal_points";
update score set timestamp=timestamp, tag="speech" where tag="congress_speech";
update score set timestamp=timestamp, tag="po" where tag="presiding_officer";

update score set timestamp=timestamp, content = null where tag="point";
update score set timestamp=timestamp, content = null where tag="rank";
update score set timestamp=timestamp, content = null where tag="winloss";

update entry_setting set timestamp=timestamp, tag="po" where tag="presiding_officer";

delete from tiebreak where name is null;
delete from tiebreak where name = "";
delete from score where tag like "subpoints_%";

alter table score modify tag varchar(15);
alter table result add place varchar(15) after rank;

set SESSION sql_mode = "ANSI";

update (result, result_value)
	set result.rank = CAST(result_value.value as INTEGER), result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.rank is null
		and result_value.tag = 'Place';

update (result, result_value)
	set result.place = result_value.value
		where result.id = result_value.result
		and result.place is null
		and result_value.tag = 'Place';

update (result, result_value)
	set result.rank = CAST(result_value.value as INTEGER), result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.rank is null
		and result_value.tag = 'Order';

update (result, result_value)
	set result.place = result_value.value, result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.place is null
		and result_value.tag = 'Order';

update (result, result_value)
	set result.rank = CAST(result_value.value as INTEGER), result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.rank is null
		and result_value.tag = 'Seed';

update (result, result_value)
	set result.place = result_value.value, result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.place is null
		and result_value.tag = 'Seed';

update (result, result_value)
	set result.rank = CAST(result_value.value as INTEGER), result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.rank is null
		and result_value.tag = 'Champion';

update (result, result_value)
	set result.place = result_value.value, result.timestamp=result.timestamp
		where result.id = result_value.result
		and result.place is null
		and result_value.tag = 'Champion';

set SESSION sql_mode = 'STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';

delete from result_value where result_value.tag = 'Champion';
delete from result_value where result_value.tag = 'Place';
delete from result_value where result_value.tag = 'Order';
delete from result_value where result_value.tag = 'Seed';

