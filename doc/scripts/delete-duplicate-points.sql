
		delete point

		from auto_instance tourn, AUTO_POINT_TRANSFER tourn_point, NEW_POINTS point,
			 auto_instance tourn2, AUTO_POINT_TRANSFER tourn_point2, NEW_POINTS point2

		where tourn.source = "TR"
		and tourn.instance_id = tourn_point.instance_id
		and tourn_point.point_id = point.point_id

		and tourn2.source = "TR"
		and tourn2.tourn_id = tourn.tourn_id
		and tourn2.instance_id > tourn.instance_id
		and tourn2.instance_id = tourn_point2.instance_id
		and tourn_point2.point_id = point2.point_id
		and tourn_point2.round = tourn_point.round

		and tourn_point2.alt_event_id = tourn_point.alt_event_id
		and tourn_point2.nfl_student_id = tourn_point.nfl_student_id


