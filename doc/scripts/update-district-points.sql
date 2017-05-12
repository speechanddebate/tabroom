

	update
		AUTO_POINT_TRANSFER tourn_point, NEW_POINTS point, auto_instance instance,
			tabroom.tourn tourn, tabroom.tourn_setting tourn_setting

		set point.event_cat_id = 3

		where point.point_id = tourn_point.point_id 		
		and tourn_point.instance_id = instance.instance_id 		
		and instance.source="TR" 		
		and instance.tourn_id= tourn.id
		and tourn.id = tourn_setting.tourn
		and tourn_setting.tag = 'nsda_district';

	update  		
		AUTO_POINT_TRANSFER tourn_point, NEW_POINTS point, auto_instance instance,
			tabroom.tourn tourn, tabroom.tourn_setting tourn_setting

		set tourn_point.event_cat_id = 3
		where point.point_id = tourn_point.point_id 		
		and tourn_point.instance_id = instance.instance_id 		
		and instance.source="TR" 		
		and instance.tourn_id= tourn.id
		and tourn.id = tourn_setting.tourn
		and tourn_setting.tag = 'nsda_district';
