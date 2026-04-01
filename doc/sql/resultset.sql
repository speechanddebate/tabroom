
update result_set set tag='final', timestamp=timestamp where label="District Qualifiers";
update result_set set tag='final', timestamp=timestamp where label="Final Places";
update result_set set tag='qualifier', timestamp=timestamp where label="District Qualifiers";
update result_set set tag='speaker', timestamp=timestamp where label="Speaker Awards";

alter table result_set add nsda_category int after event;
alter table result_set drop tag;
alter table result_set add tag enum('final', 'bracket', 'seed', 'speaker', 'qualifier', 'sweep', 'toc', 'nsda', 'table', 'chamber', 'other') after id;
alter table result_set ADD CONSTRAINT fk_rs_nsda_category FOREIGN KEY (nsda_category) REFERENCES nsda_category(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
alter table result_set drop qualifier;

alter table result drop honor;
alter table result drop honor_site;
alter table result drop raw_scores;
alter table result drop details;
alter table result add cache text after panel;

delete r2.*
	from result r1, result r2
	where 1=1
	and r1.entry = r2.entry
	and r1.rank = r2.rank
	and r1.result_set = r2.result_set
	and r1.id != r2.id
	and NOT EXISTS (
		select rv.id
		from result_value rv
		where rv.result = r2.id
	);

delete from result
	where not exists (select result_set.id from result_set where result.result_set = result_set.id);

delete from result_value
	where not exists (select result.id from result where result.id = result_value.result);

delete from result_key
	where not exists (select result_value.id from result_value where result_key.id = result_value.result_key);

alter table result ADD CONSTRAINT fk_result_to_set FOREIGN KEY (result_set) REFERENCES result_set(id) ON UPDATE CASCADE ON DELETE CASCADE;
alter table result_value ADD CONSTRAINT fk_rv_to_result FOREIGN KEY (result) REFERENCES result(id) ON UPDATE CASCADE ON DELETE CASCADE;

update ballot set entry=NULL, timestamp=timestamp where entry=0;
delete from ballot where entry IS NOT NULL and not exists ( select entry.id from entry where ballot.entry = entry.id);

alter table ballot ADD CONSTRAINT fk_ballot_entry FOREIGN KEY (entry) REFERENCES entry (id) ON UPDATE CASCADE ON DELETE CASCADE;

optimize table ballot;
optimize table result;
optimize table result_set;
optimize table result_value;
optimize table result_key;
