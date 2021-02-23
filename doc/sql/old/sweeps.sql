
	alter table result add panel int after round;
	alter table result_set add tag enum('entry', 'student', 'chapter') not null default "entry";
	alter table result_set add sweep_set int after tourn;

	alter table sweep_set add circuit int after tourn;
	alter table sweep_set add scope text after circuit;

