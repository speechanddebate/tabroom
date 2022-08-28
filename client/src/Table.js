/* eslint-disable no-nested-ternary */
import React, { useMemo } from 'react';
import PropTypes from 'prop-types';

import {
	useTable,
	useResizeColumns,
	useFlexLayout,
	useBlockLayout,
	useFilters,
	usePagination,
	useSortBy,
	useExpanded,
} from '@tanstack/react-table';

// import Pagination from './features/Pagination.js';
// import DefaultColumnFilter from './features/DefaultColumnFilter.js';
// import CSVDownload from './features/CSVDownload.js';
// import PDFDownload from './features/PDFDownload.js';

import './Table.css';

const getStyles = props => [
	props,
	{
		style: {},
	},
];

const headerGroupStyles = props => [
	props,
	{
		style: {
			flexDirection: 'column',
		},
	},
];

const cellProps = (props, { cell }) => getStyles(props, cell.column.align);

const renderRows = (rows, prepareRow, SubComponent) => rows.map(row => {
	prepareRow(row);
	const rowProps = row.getRowProps();
	return (
		<React.Fragment key={rowProps.key}>
			<div {...rowProps} styleName="tr">
				{
					row.cells.map(cell => (
						<div {...cell.getCellProps(cellProps)} styleName="td">
							{cell.render('Cell')}
						</div>
					))
				}
			</div>
			{
				row.isExpanded && (
					<div styleName="tr">
						<SubComponent row={row} rowProps={rowProps} />
					</div>
				)
			}
		</React.Fragment>
	);
});

/**
 * Table
 */
const Table = ({
	columns,
	data,
	pagination = false,
	// csv = false,
	// exportFileName = 'export',
	// generatePDF = false,
	loading = false,
	noDataText = 'No data found!',
	defaultFilterMethod = 'text',
	layout = 'block',
	defaultPageSize,
	SubComponent,
	Footer,
}) => {
	const defaultColumn = useMemo(
		() => {
			const defaultColumnOptions = {
				minWidth: 30,
				filter: defaultFilterMethod,
			};
			return defaultColumnOptions;
		},
		[defaultFilterMethod]
	);

	const {
		getTableProps,
		getTableBodyProps,
		headerGroups,
		rows,
		prepareRow,
		// pageOptions,
		page,
		// state: { pageIndex, pageSize },
		// gotoPage,
		// previousPage,
		// nextPage,
		// setPageSize,
		// canPreviousPage,
		// canNextPage,
	} = useTable(
		{
			columns,
			data,
			defaultColumn,
			initialState: {
				pageSize: defaultPageSize || 10,
				hiddenColumns: columns
					.filter(col => col.show === false)
					.map(column => column.accessor || column.id),
			},
		},
		useResizeColumns,
		(layout === 'flex' ? useFlexLayout : useBlockLayout),
		useFilters,
		useSortBy,
		useExpanded,
		usePagination,
	);

	// const canPaginate = data.length > 0 && !loading && pagination;
	const hasFooter = Footer || false;

	return headerGroups.length > 0 ? (
		<div data-testid="reportTable">
			<div {...getTableProps()} styleName="table">
				<div styleName="download-options-row" data-testid="download-options-row" {...getTableBodyProps()}>
					{
						// csv &&
						// <CSVDownload
						// 	columns={headerGroups[0].headers}
						// 	exportFileName={exportFileName}
						// />
					}
					{/* {generatePDF && <PDFDownload columns={headerGroups[0].headers} generatePDF={generatePDF} style={csv ? { marginLeft: '5px' } : {}} />} */}
					{
						// emmaExport &&
						// <EmmaExport columns={headerGroups[0].headers} emailField={emailField} />
					}
				</div>
				{
					headerGroups.map(headerGroup => (
						<div {...headerGroup.getHeaderGroupProps(headerGroupStyles)}>
							<div styleName="tr tr-hg header-row no-select">
								{
									headerGroup.headers.map(column => (
										<div {...column.getHeaderProps()} styleName="th">
											<div {...column.getSortByToggleProps()} data-testid="sortDiv" styleName="col-header">
												{column.render('Header')}
												{
													column.isSorted
														? column.isSortedDesc
															? <span styleName="sorted-desc" data-testid="sortedDesc" />
															: <span styleName="sorted-asc" data-testid="sortedAsc" />
														: null
												}
											</div>
											{column.canResize && <div {...column.getResizerProps()} styleName="resizer" data-testid="resizer" />}
										</div>
									))
								}
							</div>
							{
								<div styleName="tr tr-hg no-select">
									{
										headerGroup.headers.map(column => (
											<div styleName="th" {...column.getHeaderProps()}>
												{/* {
													column.canFilter
														? column.Filter
															? column.render('Filter')
															: <DefaultColumnFilter column={column} />
														:
														<DefaultColumnFilter
															column={column}
															disabled
														/>
												} */}
											</div>
										))
									}
								</div>
							}
						</div>
					))
				}
				<div {...getTableBodyProps()} styleName="tbody">
					{
						loading
							? <p styleName="loading">Loading...</p>
							: data.length > 0
								? pagination
									? renderRows(page, prepareRow, SubComponent)
									: renderRows(rows, prepareRow, SubComponent)
								: <p styleName="no-data">{noDataText}</p>
					}
					{
						// canPaginate &&
						// <Pagination
						// 	pageOptions={pageOptions}
						// 	pageIndex={pageIndex}
						// 	pageSize={pageSize}
						// 	gotoPage={gotoPage}
						// 	previousPage={previousPage}
						// 	nextPage={nextPage}
						// 	setPageSize={setPageSize}
						// 	canPreviousPage={canPreviousPage}
						// 	canNextPage={canNextPage}
						// 	maxDisplay={data.length}
						// />
					}
					{
						hasFooter &&
						<Footer />
					}
				</div>
			</div>
		</div>
	) : <p> Table has no visible columns </p>;
};

Table.propTypes = {
	columns: PropTypes.array.isRequired,
	data: PropTypes.array.isRequired,
	pagination: PropTypes.bool,
	csv: PropTypes.bool,
	exportFileName: PropTypes.string,
	generatePDF: PropTypes.oneOfType([
		PropTypes.bool,
		PropTypes.func,
	]),
	emmaExport: PropTypes.bool,
	emailField: PropTypes.string,
	loading: PropTypes.bool,
	noDataText: PropTypes.oneOfType([
		PropTypes.string,
		PropTypes.element,
	]),
	defaultFilterMethod: PropTypes.oneOfType([
		PropTypes.string,
		PropTypes.func,
	]),
	layout: PropTypes.string,
	defaultPageSize: PropTypes.number,
	SubComponent: PropTypes.func,
	Footer: PropTypes.element,
};

export default Table;
