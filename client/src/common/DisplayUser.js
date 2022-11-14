import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { loadProfile } from '../redux/ducks/profile';

import styles from './header.module.css';

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
		<div id={styles.userinfo}>
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
							className = {styles.email}
						>
							{profile.email}
						</a>
					</>
					:
					<>
						<a
							tabIndex   = "-1"
							className = {styles.login}
						>
							Login
						</a>
						<a
							tabIndex   = "-1"
							className = {styles.signup}
						>
							Sign Up
						</a>
					</>
			}
		</div>
	);
};

export default DisplayUser;
