import React from 'react';
import PropTypes from 'prop-types';

import styles from './CSVDownload.module.css';

// This is wrapped in `` (with a new line added) to add a new line after headers are done
export const sanitizeHeaders = headers => `${headers
	.map(h => h.replace('#', 'Number'))
	.map(value => `"${value}"`)
	.join(',')}
`;

export const sanitizeRows = (rows, rowKeys) => rows.map(r => {
	return rowKeys.map(key => {
		return typeof r[key] === 'string' ? r[key].replace('#', '') : r[key];
	})
		// Adding "" around each csv cell value
		.map(value => `"${value}"`)
		// Replace any null value with ""
		.map(value => value.replace('"null"', '""'));
})
	.map(e => e.join(','))
	.join('\n');

export const filterExcludedColumns = columns => columns.filter(c => !c.excludeCSV).map(c => {
	if (typeof c.header !== 'string') { return c.id || ''; }
	return c.header;
});

export const filterExcludedColumnsIds = columns => columns
	.filter(c => !c.excludeCSV)
	.map(c => {
		return c.id;
	});

export const csvEncode = (headers, rows) => `data:text/csv;charset=utf-8,${encodeURIComponent(headers)}${encodeURIComponent(rows)}`;

export const customGenerateCSV = (columns, rows) => {
	const columnIds = filterExcludedColumnsIds(columns);

	const headers = filterExcludedColumns(columns);
	const headersCustom = sanitizeHeaders(headers);

	const allRowValues = rows.map(r => r.original);
	const rowsAsArrayCustom = sanitizeRows(allRowValues, columnIds);

	return csvEncode(headersCustom, rowsAsArrayCustom);
};

const CSVDownload = ({ columns, rows, exportFileName = 'default' }) => {
	const downloadCSV = (event, fileName) => {
		event.preventDefault();
		const csvLink = document.createElement('a');
		csvLink.href = customGenerateCSV(columns, rows);
		csvLink.download = `${fileName}.csv`;
		csvLink.click();
	};

	return <button type='button' onClick={(event) => downloadCSV(event, exportFileName)} className={`fa fa-file-excel-o ${styles.download}`} />;
};

CSVDownload.propTypes = {
	columns: PropTypes.array,
	rows: PropTypes.array,
	exportFileName: PropTypes.string,
};

export default CSVDownload;
