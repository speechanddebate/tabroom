import React, { useMemo } from 'react';
import moment from 'moment-timezone';

import Table from './Table';

const JSONTable = ({ data = [], options = {} }) => {

	// Construct default columns from the row key names
	let columns = useMemo(() => {
		return Object.keys(data[0] ?? [])?.map((k) => {
			if (typeof data[1][k] === 'number') {
				return {
					accessor : k,
					header   : k.length > 1 ? k[0].toUpperCase() + k.slice(1) : k,
					style    : `numeric`,
				};
			}
			if (typeof data[1][k] === 'string' && moment(data[1][k]).isValid()) {
				return {
					accessor : k,
					header   : k.length > 1 ? k[0].toUpperCase() + k.slice(1) : k,
					cell     : info => moment(info.getValue()).format('l'),
				};
			}
			return {
				accessor: k,
				header: k.length > 1 ? k[0].toUpperCase() + k.slice(1) : k,
			};
		}) || [];
	}, [data]);

	// Allow completely overriding a column definition
	columns = columns?.map(c => {
		const mc = options?.columns?.find(f => f.accessor === c.accessor);
		return mc || c;
	});

	// Apply column transformations
	columns = columns?.map(c => {
		let transformed = null;
		options?.transforms?.forEach(tc => {
			if (c.accessor === tc.accessor) {
				transformed = { ...c, ...tc };
			}
		});
		return transformed ?? c;
	});

	return (
		<Table data={data} columns={columns ?? []} options={{ csv: true, ...options }} />
	);
};

export default JSONTable;
