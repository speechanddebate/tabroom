import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router';
import moment from 'moment-timezone';

import Table from '../common/table/Table';

const TournList = () => {

	const { name } = useParams();
	const navigate = useNavigate();

	const handleNavigate = () => {
		navigate('/route');
	};

	const [tourns, setTourns] = useState([]);

	useEffect(() => {
		const fetchList = async () => {
			const response = await fetch(`${process.env.REACT_APP_API_BASE}/invite/upcoming`, { method: 'GET' });
			setTourns(await response.json());
		};
		fetchList();
	}, []);

	const tableColumns = [
		{
			header   : 'Dates',
			style    : 'nowrap',
			sortKey  : 'week',
			accessor : (row) => {
				const tourn = row?.original;
				if (!tourn) {
					return '';
				}
				const start = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
				const end = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');
				return `${start.tz(tourn.tz).format('YYYY-MM-DD')}-${end.tz(tourn.tz).format('YYYY-MM-DD')}`;
			},
			cell    : (row) => {
				const tourn = row?.original;
				if (!tourn) {
					return '';
				}
				const start = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
				const end = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');

				if (start.isValid() && end.isValid()) {
					if (start.tz(tourn.tz).date() === end.tz(tourn.tz).date()) {
						return start.tz(tourn.tz).format('M/D');
					}
					return `${start.tz(tourn.tz).format('M/D')} - ${end.tz(tourn.tz).format('M/D')}`;
				}
				return '';
			},
			sortingFn: (a, b) => {
				if (a?.original?.week === b?.original?.week) { return 0; }
				if (a?.original?.week > b?.original?.week) { return 1; }
				if (a?.original?.week < b?.original?.week) { return -1; }
				return 0;
			},
		},
		{
			header   : 'Tournament',
			accessor : 'name',
			cell     : (row) => {
				const tourn = row?.original;
				if (!tourn) {
					return '';
				}
				let tournName = tourn.name;
				if (tourn.district) {
					tournName.replaceAll('District Tournament', '');
					tournName = `
						<span class='semibold half'>NSDA: ${tournName}</span>
						<span class='half'>${tourn.weekendName}</span>
					`;
				}
				if (tourn.webname) {
					return `<a href='https://${tourn.webname}.tabroom.com'>${tourn.name}</a>`;
				}
				return `<a href='https://www.tabroom.com/index/tourn/index.mhtml?tourn_id=${tourn.id}'>${tourn.name}</a>`;
			},
		},
		{ accessor : 'city' },
		{ accessor : 'state' },
		{
			header   : 'Format',
			style    : 'nowrap centeralign',
			accessor : (row) => {
				return `${row?.original?.inp ? 'In Person ' : ''} 
					${row?.original?.online ? 'Online' : ''}
					${row?.original?.hybrid ? 'Hybrid' : ''}`;
			},
			cell     : (row) => {
				const tourn = row?.original;
				if (!tourn) {
					return '';
				}
				return (
					<>
						{
							tourn.inp &&
							<span className='third fa fa-sm fa-user bluetext hover' title='Tournament has in-person events' />
						}
						{
							tourn.online &&
							<span className='third fa fa-sm fa-laptop greentext hover' title='Tournament has online events' />
						}
						{
							tourn.hybrid &&
							<span className='third fa fa-sm fa-handshake-o orangetext hover' title='Tournament has hybrid events' />
						}
					</>
				);
			},
		},
		{
			header: 'Registration',
			accessor: (row) => {
				const tourn = row?.original;
				if (!tourn) {
					return '';
				}
				const regStart = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
				const regEnd = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');
				const now = moment();
				if (regStart.isValid() && regEnd.isValid()) {
					if (now < regStart) {
						tourn.registration = `Opens on ${regStart.tz(tourn.tz).format('M/D hh:mm A z')}`;
					} else if (now < regEnd) {
						tourn.registration = `Due by ${regEnd.tz(tourn.tz).format('M/D hh:mm A z')}`;
					} else {
						tourn.registration = 'Closed';
					}
				} else {
					tourn.registration = '';
				}
			},
		},
		{
			header   : 'Judging Signups In',
			accessor : 'signup',
		},
	];

	const customStyles = {
		table: 'tight',
	};

	return (
		<div>
			<Table
				data    = {tourns}
				options = {
					{
						tableRows : tourns,
						tableColumns,
						customStyles,
						striped        : true,
						exportFileName : 'UpcomingTournament',
						title          : 'Upcoming Tournaments',
						getRowStyles   : (row) => (row?.original?.tournament === 'Nats' ? 'semibold' : ''),
					}
				}
			/>
			<button onClick={handleNavigate} type="button">Go to /route</button>
			<p>{name}</p>
		</div>
	);
};

export default TournList;
