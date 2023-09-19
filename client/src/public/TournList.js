import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router';
import moment from 'moment-timezone';

import JSONTable from '../common/table/JSONTable';

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

	const tournTableColumns = [
		{
			header   : 'Dates',
			style    : 'nowrap',
			sortKey  : 'week',
			accessor : (tourn) => {
				const start = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
				const end = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');
				return `${start.tz(tourn.tz).format('YYYY-MM-DD')}-${end.tz(tourn.tz).format('YYYY-MM-DD')}`;
			},
			cell    : (tourn) => {
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
		},
		{
			header : 'Tournament',
			key    : 'name',
			cell   : (tourn) => {

				let tournName = tourn.name;

				if (tourn.district) {

					tournName.replaceAll('District Tournament', ''); 

					tournName = `
						<span class='semibold half'>
								NSDA: ${ $tourns{$tourn_id}{'name'} %>
							</span>
							<span class='half'>
								<% $tourns{$tourn_id}{'weekend_name'} %>
							</span>

				}

				if (tourn.webname) {
					return `<a href='https://${tourn.webname}.tabroom.com'>${tourn.name}</a>`;
				}
				return `<a href='https://www.tabroom.com/index/tourn/index.mhtml?tourn_id=${tourn.id}'>${tourn.name}</a>`;
				}
				if (tourn.webname) {
					return `<a href='https://${tourn.webname}.tabroom.com'>${tourn.name}</a>`;
				}
				return `<a href='https://www.tabroom.com/index/tourn/index.mhtml?tourn_id=${tourn.id}'>${tourn.name}</a>`;
			},
		},
		{ key : 'city' },
		{ key : 'state' },
		{
			header   : 'Format',
			style    : 'nowrap centeralign',
			accessor : (tourn) => {
				return `${tourn.inp ? 'In Person ' : ''} ${tourn.online ? 'Online' : ''} ${tourn.hybrid ? 'Hybrid' : ''}`;
			},
			cell     : (tourn) => {
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
			accessor: (tourn) => {
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
			header : 'Judging Signups In',
			key    : 'signup',
		},
	];

	const tournTable = tourns.map( (tourn) => {

		const tournRow = {};

		['districts', 'nats', 'msnats', 'name'].forEach( (key) => {
			if (tourn[key]) {
				tournRow.style = 'semibold';
			}
		});

		const start = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
		const end = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');

		if (start.isValid() && end.isValid()) {
			if (start.tz(tourn.tz).date() === end.tz(tourn.tz).date()) {
				tournRow.dates = start.tz(tourn.tz).format('M/D');
			} else {
				tournRow.dates = `${start.tz(tourn.tz).format('M/D')} - ${end.tz(tourn.tz).format('M/D')}`;
			}
		} else {
			tournRow.dates = '';
		}

		if (tourn.webname) {
			tournRow.link = `https://${tourn.webname}.tabroom.com`;
		} else if (!tourn.link) {
			tournRow.link = `https://www.tabroom.com/index/tourn/index.mhtml?tourn_id=${tourn.id}`;
		}

		tournRow.tournament = tourn.name;
		tournRow.city = tourn.location;

		if (tourn.country && tourn.country !== 'US') {
			tournRow.state = tourn.country;
		} else {
			tournRow.state = tourn.state;
		}

		tournRow.type = {};
		['inp', 'hybrid', 'online'].forEach( (key) => {
			if (tourn[key] > 0) {
				tournRow.type[key] = true;
			}
		});

		const regStart = moment(tourn.start, 'YYYY-MM-DDTHH:mm:ssZ');
		const regEnd = moment(tourn.end, 'YYYY-MM-DDTHH:mm:ssZ');
		if (regStart.isValid() && regEnd.isValid()) {
			if (now < regStart) {
				tournRow.registration = `Opens on ${regStart.tz(tourn.tz).format('M/D hh:mm A z')}`;
			} else if (now < regEnd) {
				tournRow.registration = `Due by ${regEnd.tz(tourn.tz).format('M/D hh:mm A z')}`;
			} else {
				tournRow.registration = 'Closed';
			}
		} else {
			tournRow.registration = '';
		}

		tournRow.week = tourn.week;
		tournRow.signup = tourn.signup;

		return tournRow;
	});

	const transforms = [
		{ accessor : 'link'  , hidden : true , excludeCSV: true },
		{ accessor : 'style' , hidden : true , excludeCSV: true },
		{
			accessor : 'tournament',
			cell: (incoming) => {
				return <a href={`${incoming?.row?.original?.link}`}>{incoming.getValue()}</a>;
			},
		},
		{
			accessor : 'state',
			style    : 'centeralign',
		},
		{
			accessor : 'type',
			style    : 'centeralign',
			cell: (incoming) => {
				return (
					<>
						{
							incoming?.getValue().inp &&
							<span className='third fa fa-sm fa-user bluetext hover' title='Tournament has in-person events' />
						}
						{
							incoming?.getValue().online &&
							<span className='third fa fa-sm fa-laptop greentext hover' title='Tournament has online events' />
						}
						{
							incoming?.getValue().hybrid &&
							<span className='third fa fa-sm fa-handshake-o orangetext hover' title='Tournament has hybrid events' />
						}
					</>
				);
			},
		},
		{ accessor : 'week'  , hidden : true , excludeCSV: true },
		{
			accessor : 'dates',
			sortingFn: (a, b) => {
				if (a?.original?.week === b?.original?.week) { return 0; }
				if (a?.original?.week > b?.original?.week) { return 1; }
				if (a?.original?.week < b?.original?.week) { return -1; }
				return 0;
			},
		},
	];

	const customStyles = {
		table: 'tight',
	};

	return (
		<div>
			<JSONTable
				data    = {tournTable}
				options = {
					{
						transforms,
						customStyles,
						striped        : true,
						exportFileName : 'UpcomingTournament',
						title          : 'Upcoming Tournaments',
						getRowStyles   : (row) => (row?.original?.tournament === 'Nats' ? 'bold' : ''),
					}
				}
			/>
			<button onClick={handleNavigate} type="button">Go to /route</button>
			<p>{name}</p>
		</div>
	);
};

export default TournList;
