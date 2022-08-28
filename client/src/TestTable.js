import React from 'react';

import {
	createColumnHelper,
} from '@tanstack/react-table';

import Table from './Table';

const TestTable = () => {
	const data = [
		{
			firstName: 'tanner',
			lastName: 'linsley',
			age: 24,
			visits: 100,
			status: 'In Relationship',
			progress: 50,
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

	const columnHelper = createColumnHelper();

	const columns = [
		columnHelper.accessor('firstName', {
			cell: info => info.getValue(),
			footer: info => info.column.id,
		}),
		columnHelper.accessor(row => row.lastName, {
			id: 'lastName',
			cell: info => <i>{info.getValue()}</i>,
			header: () => <span>Last Name</span>,
			footer: info => info.column.id,
		}),
		columnHelper.accessor('age', {
			header: () => 'Age',
			cell: info => info.renderValue(),
			footer: info => info.column.id,
		}),
		columnHelper.accessor('visits', {
			header: () => <span>Visits</span>,
			footer: info => info.column.id,
		}),
		columnHelper.accessor('status', {
			header: 'Status',
			footer: info => info.column.id,
		}),
		columnHelper.accessor('progress', {
			header: 'Profile Progress',
			footer: info => info.column.id,
		}),
	];

	return (
		<div>
			<Table
				data={data}
				columns={columns}
			/>
		</div>
	);
};

export default TestTable;
