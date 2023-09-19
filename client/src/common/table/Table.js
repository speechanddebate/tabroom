import React, { useState, useReducer, useMemo } from 'react';
import {
	flexRender,
	getCoreRowModel,
	getSortedRowModel,
	useReactTable,
	createColumnHelper,
} from '@tanstack/react-table';

import CSVDownload from './CSVDownload';

import styles from './Table.module.css';

const Table = ({ data = [], columns = [], options = {} }) => {
	const rerender = useReducer(() => ({}), {})[1];
	const columnHelper = createColumnHelper();
	const [sorting, setSorting] = useState([]);

	const tableColumns = useMemo(() => {
		return columns?.map(c => {
			return columnHelper.accessor(c.accessor, {
				id           : c.id || c.accessor || c.header,
				cell         : info => info.getValue(),
				footer       : info => info.column.id,
				enableHiding : true,
				...c,
			});
		});
	}, [columns, columnHelper]);

	const columnVisibility = {};
	tableColumns.forEach(c => {
		columnVisibility[c.accessor] = !c.hidden;
	});

	const table = useReactTable({
		data,
		columns: tableColumns,
		enableHiding: true,
		state: {
			sorting,
			columnVisibility,
		},
		initialState: {
			columnVisibility,
		},
		onSortingChange: setSorting,
		getCoreRowModel: getCoreRowModel(),
		getSortedRowModel: getSortedRowModel(),
		options: {
			getRowStyles: (row, tableobj) => {
				const index = tableobj?.getSortedRowModel()?.flatRows.findIndex(fr => fr.id === row.id);
				return (options.striped && index % 2 === 0 ? styles.striped : '');
			},
		},
	});

	Object.keys(options.customStyles || {}).forEach( (key) => {
		styles[key] = styles[options.customStyles[key]];
	});

	return (
		<div className={styles.container}>
			<div className={styles.tableHeaderRow}>
				<span className={styles.headerTitle}>
					<h1 className={styles.header}>
						{options.title}
					</h1>
				</span>
				<span className={styles.headerButtons}>
					{
						options.csv &&
						<CSVDownload
							rows           = {table.getRowModel().rows}
							columns        = {tableColumns}
							exportFileName = {options.exportFileName}
						/>
					}
				</span>
			</div>
			<table className={styles.table}>
				<thead className={styles.thead}>
					{
						table.getHeaderGroups().map(headerGroup => (
							<tr key={headerGroup.id} className={styles.headerRow}>
								{
									headerGroup.headers.map(header => (
										<th key={header.id} className={styles.th}>
											{
												header.isPlaceholder
													? null
													: (
														<div
															{...{
																className: header.column.getCanSort()
																	? styles['cursor-pointer']
																	: '',
																onClick: header.column.getToggleSortingHandler(),
															}}
														>
															<span className={styles.thText}>
																{
																	flexRender(
																		header.column.columnDef.header,
																		header.getContext()
																	)
																}
															</span>
															<span className={styles.thArrows}>
																{
																	{
																		asc: ' 🔼',
																		desc: ' 🔽',
																	}[header.column.getIsSorted()] ?? ' ⬍'
																}
															</span>
														</div>
													)
											}
										</th>
									))
								}
							</tr>
						))
					}
				</thead>
				<tbody className={styles.tbody}>
					{
						table.getRowModel().rows.map(row => (
							<tr key={row.id} className={`${table.options.options.getRowStyles(row, table)} ${options.getRowStyles(row, table)} ${styles.tr}`}>
								{
									row.getVisibleCells().map(cell => (
										<td key={cell.id} className={`${cell.column.columnDef?.style} ${styles.td}`}>
											{ flexRender(cell.column.columnDef.cell, cell.getContext()) }
										</td>
									))
								}
							</tr>
						))
					}
				</tbody>
				<tfoot className={styles.tfoot}>
					{
						table.getFooterGroups().map(footerGroup => (
							<tr key={footerGroup.id} className={styles.footerRow}>
								{
									footerGroup.footers?.map(footer => (
										<th key={footer.id} className={styles.th}>
											{
												footer.isPlaceholder
													? null
													: flexRender(
														footer.column.columnDef.footer,
														footer.getContext()
													)
											}
										</th>
									))
								}
							</tr>
						))
					}
				</tfoot>
			</table>

			<div className="rightalign padtop">
				<button type="button" onClick={() => rerender()} className="bluebutton fa fa-lg fa-refresh" />
			</div>
		</div>
	);
};

export default Table;
