insert into circuit (name, abbr, active, country, webname) values ('National Debate Coaches Association', 'ndca', 1, 'US', 'https://www.debatecoaches.org');
update result_set set tag = NULL where tag IN ('toc', 'ndca', 'ada', 'ceda', 'ndt');
alter table result_set modify tag enum('final','bracket','seed','speaker','qualifier','sweep','cume','nsda','table','chamber','scores','circuit', 'other');

update result_set set tag='final' where id = 69771;
update result_set set tag='bracket' where label="Bracket";
update result_set set tag='cume' where label="Cume Sheet";
update result_set set tag='cume' where label like "Cume Sheet%";
update result_set set tag='seed' where label="Prelim Seeds";
update result_set set tag='sweep', entity='school' where label like "%(Schools)";
update result_set set tag='sweep', entity='entry' where label like "%(Entry)";
update result_set set tag='sweep', entity='student' where label like "%(Individual)";
update result_set set tag='chamber', entity='entry' where label like "%Chamber%";
update result_set set tag='bracket', entity='entry' where label like "%Bracket%";
update result_set set tag='seed', entity='entry' where label like "Prelim Seeds%";
update result_set set tag='final', entity='entry' where label like "Final Places%";
update result_set set tag='final', label="Final Places", entity='entry' where label = "Final Placements";
update result_set set tag='speaker', entity='student' where label like "%Speaker%";
update result_set set tag='table', entity='entry' where label = "Prelims Table";
update result_set set tag='scores', entity='entry' where label like "Results up to round%";
update result_set set tag='scores', entity='entry' where label like "All Rounds";
update result_set set tag='qualifier', entity='entry' where label like "District Qualifiers%";
update result_set set tag='qualifier', entity='entry' where label like "%Qualification%";
update result_set set circuit=198 where label like "TFA%";
update result_set set circuit=228 where label like "TOC%";

update result_set set tag='qualifier', entity='entry' where label like "NDCA%";
update result_set set circuit = 274 where label like "NDCA%";

update result_set set tag='qualifier', entity='entry' where label like "TOC%";
update result_set set tag='circuit', entity='entry' where label like "ADA%";
update result_set set circuit=103 where label like "ADA%";
update result_set set circuit=103 where label like "ada%";
update result_set set tag='circuit', entity='entry' where label like "ada_pts%";
update result_set set tag='circuit', entity='entry' where label like "CEDA%";
update result_set set tag='circuit', entity='entry' where label like "ceda_pts%";

update result_set set circuit=43 where label like "CEDA%";
update result_set set circuit=43 where label like "ceda%";

update result_set set circuit=43 where label like "NDT%";
update result_set set circuit=43 where label like "ndt%";

update result_set set tag='circuit', entity='entry' where label like "ndt_honors";
update result_set set tag='circuit', entity='entry' where label like "ndt_pts";
update result_set set tag='circuit', entity='entry' where label like "WUDC Points";


update result_set set entity="entry" where tag IN ('final', 'bracket', 'seed', 'toc', 'nsda', 'ndca', 'table', 'chamber', 'qualifier');
update result_set set entity="student" where tag IN ('speaker');
update result_set set entity="school" where tag IN ('school');
