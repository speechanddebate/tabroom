import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { loadProfile } from './redux/ducks/profile';

export const DisplayUser = () => {
	const profile = useSelector((state) => state.profile);
	const dispatch = useDispatch();

	useEffect(() => {
		const fetchData = async () => {
			dispatch(loadProfile());
		};
		fetchData();
	}, [dispatch]);

	return (
		<div id="userinfo">
			{
				profile.id ?
					<>
						<a
							tabIndex  = "-1"
							className = "fa fa-2x fa-sign-out"
							alt       = "Log Out of Tabroom"
							title     = "Log Out of Tabroom"
						/>

						<a
							tabIndex  = "-1"
							className = "fa fa-2x fa-user"
							alt       = "Tabroom Account Profile"
							title     = "Tabroom Account Profile"
						/>

						<a
							tabIndex  = "-1"
							className = "fa fa-2x fa-home borderright"
							alt       = "Tabroom Home Screen"
							title     = "Tabroom Home Screen"
						/>

						<a
							tabIndex   = "-1"
							className = "noborder padvert"
						>
							{profile.email}
						</a>
					</>
					:
					<>
						<a
							tabIndex   = "-1"
							className = "noborder padvert"
						>
							Login
						</a>
						<a
							tabIndex   = "-1"
							className = "noborder padvert"
						>
							Sign Up
						</a>
					</>
			}
		</div>
	);
};

export default DisplayUser;
