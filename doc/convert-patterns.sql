
alter table category add pattern int after tourn;

alter table pattern modify exclude text; 

update pattern set max=1 where type=1;

update pattern set type=3 where type=1;

