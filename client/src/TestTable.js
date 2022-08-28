import React from 'react';

import Table from './Table';

const TestTable = () => {
	const columns = [
		{
			id: 'Column1',
			Header: 'Column1',
			accessor: (row) => {
				return row.first;
			},
			Cell: (row) => {
				return <span>{row.value}</span>;
			},
		},
		{
			Header: 'Column2',
			accessor: 'last',
		},
	];

	const data = [
		{ first: 'Aaron', last: 'Hardy' },
		{ first: 'Chris', last: 'Palmer' },
	];

	return (
		<div>
			<Table
				data={data}
				columns={columns}
				noDataText="No Data Found!"
				sortable
				resizable
				filterable
			/>
		</div>
	);
};

export default TestTable;
