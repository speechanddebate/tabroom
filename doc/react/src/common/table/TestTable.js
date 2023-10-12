import React from 'react';
import { useNavigate, useParams } from 'react-router';

import Table from './Table';

const TestTable = () => {
	const { name } = useParams();

	const navigate = useNavigate();

	const handleNavigate = () => {
		navigate('/route');
	};

	const data = [
		{
			firstName : 'tanner',
			lastName  : 'linsley',
			age       : 24,
			visits    : 100,
			status    : 'In Relationship',
			progress  : 50,
		},
		{
			firstName: 'tandy',
			lastName: 'miller',
			age: 40,
			visits: 40,
			status: 'Single',
			progress: 80,
		},
		{
			firstName: 'joe',
			lastName: 'dirte',
			age: 45,
			visits: 20,
			status: 'Complicated',
			progress: 10,
		},
	];

	const transforms = [
		{ accessor: 'firstName', header: 'First Name' },
		{ accessor: 'progress', hidden: true },
	];

	return (
		<div>
			<Table
				data={data}
				meta={{ transforms }}
			/>
			<button onClick={handleNavigate} type="button">Go to /route</button>
			<p>{name}</p>
		</div>
	);
};

export default TestTable;
