	alter table sweep_set drop circuit;
	alter table sweep_set drop scope;
	alter table sweep_set add sweep_award int after tourn;

	alter table result_set add sweep_award int after sweep_set;
	alter table sweep_rule add truncate smallint after count_round;

	alter table sweep_event
		add event_type enum("all", "congress", "debate", "speech", "wsdc", "wudc")
		null
		after event;

	alter table sweep_event
		add event_level enum("all", "open", "jv", "novice", "champ", "es-open", "es-novice", "middle")
		null
		after event_type;

	alter table event
		add level enum("open", "jv", "novice", "champ", "es-open", "es-novice", "middle")
		not null default "open"
		after type;

	update event_setting set value="open" where value="Open";
	update event_setting set value="jv" where value="JV";
	update event_setting set value="novice" where value="Novice";
	update event_setting set value="es-novice" where value="spanish-novice";
	update event_setting set value="es-open" where value="spanish-varsity";
	update event_setting set value="es-open" where value="es-varsity";

	update event,event_setting
		set event.level = event_setting.value
		where event.id = event_setting.event
		and event_setting.tag = "level";

	update sweep_rule set count="prelim" where count="last_prelim";
	update sweep_rule set place="6" where tag like "rev_%";
	update sweep_rule set rev_min="1" where tag like "rev_%";
	update sweep_rule set rev_min="1" where tag = "rev_per_overall_place";

	update sweep_rule set tag="points_per_comp_rank" where tag = "points_per_composite_rank";
	update sweep_rule set tag="points_per_comp_rank_above" where tag = "points_per_composite_rank_above";
	update sweep_rule set tag="rev_per_comp_rank" where tag = "rev_composite";
	update sweep_rule set tag="rev_per_rank" where tag = "rev_per_rank_sansworst";

	update sweep_rule set count="prelim" where tag = "cume";
	update sweep_rule set count="prelim" where tag = "prelim_seed";
	update sweep_rule set count="all" where tag="place_above";
	update sweep_rule set count="all" where tag="place";
	update sweep_rule set count="all" where tag="rev_overall";
	update sweep_rule set count="all" where tag="rev_overall_base";
	update sweep_rule set count="all" where tag="rev_per_overall_place";

	update sweep_rule set tag="seed_above" where tag = "prelim_seed";
	update sweep_rule set tag="seed_above" where tag = "place_above";
	update sweep_rule set tag="seed" where tag ="place";
	update sweep_rule set tag="seed_above_percent" where tag="percentage";

	update sweep_rule set tag="rev_seed" where tag = "rev_overall";
	update sweep_rule set tag="rev_seed" where tag = "rev_overall_base";
	update sweep_rule set tag="rev_seed" where tag = "rev_per_overall_place";

	drop table if exists sweep_award;
	CREATE TABLE `sweep_award` (
		`id` int(11) NOT NULL AUTO_INCREMENT,
		`name` varchar(127) NOT NULL,
		`description` text,
		`target` enum("entry", "school", "individual"),
		`period` enum("annual", "cumulative"),
		`count` smallint not null default 0,
		`circuit` int(11) DEFAULT NULL,
		`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
		PRIMARY KEY (`id`)
	) ENGINE=InnoDB DEFAULT CHARSET=utf8;

