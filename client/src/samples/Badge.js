import React from 'react';
import { useSelector } from 'react-redux';

export const Badge = () => {
	const profile = useSelector((state) => state.profile);
	return (
		<h2>Hi, {profile.first}</h2>
	);
};

export default Badge;
