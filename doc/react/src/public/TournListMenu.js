import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { loadTournList } from '../redux/ducks/tournList';
// import { useNavigate, useParams } from 'react-router';

const TournListMenu = () => {
	const dispatch = useDispatch();
	useEffect(() => {
		dispatch(loadTournList('TX'));
	}, [dispatch]);

	const handleChangeState = (e) => {
		dispatch(loadTournList(e.target.value));
	};

	return (
		<>
			<div>Hello World!</div>
			<select onChange={handleChangeState}>
				<option value="TX">Texas</option>
				<option value="IA">Iowa</option>
				<option value="CO">Colorado</option>
				<option value="UT">Utah</option>
			</select>
		</>
	);
};

export default TournListMenu;
