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

	tourns.forEach( (tourn)  => {

		const now = moment();

		if (now < moment(tourn.reg_start)) {
			tourn.registration = `Opens on ${moment(tourn.reg_start).tz(tourn.tz).format('M/D hh:mm A z')}`;
		} else if (now < moment(tourn.reg_end)) {
			tourn.registration = `Due by ${moment(tourn.reg_end).tz(tourn.tz).format('M/D hh:mm A z')}`;
		} else {
			tourn.registration = 'Closed';
		}

		if (moment(tourn.start).tz(tourn.tz).date() === moment(tourn.end).tz(tourn.tz).date()) {
			tourn.dates = moment(tourn.start).tz(tourn.tz).format('M/D');
		} else {
			tourn.dates = `${moment(tourn.start).tz(tourn.tz).format('M/D')} - ${moment(tourn.end).tz(tourn.tz).format('M/D')}`;
		}

		if (tourn.webname) {
			tourn.link = `https://${tourn.webname}.tabroom.com`;
		} else if (!tourn.link) {
			tourn.link = `https://www.tabroom.com/index/tourn/index.mhtml?tourn_id=${tourn.id}`;
		}

		['districts', 'nats', 'msnats'].forEach( (key) => {
			if (tourn[key]) {
				tourn.style = 'semibold';
				delete tourn[key];
			}
		});

		if (tourn.country && tourn.country !== 'US') {
			tourn.state = tourn.country;
		}

		tourn.type = '';
		['inp', 'hybrid', 'online'].forEach( (key) => {
			if (tourn[key] > 0) {
				tourn.type += `${key.substring(0, 3)} `;
			}
		});

		tourn.signups = tourn.signup;
	});

	const transforms = [
		{ accessor : 'name'    , header : 'Tournament' }   ,
		{ accessor : 'signups' , header : 'Judge Signups' },
		{ accessor : 'state'   , header : 'S/C' }          ,
		{ accessor : 'link'        , hidden : true , excludeCSV: true },
		{ accessor : 'signup'      , hidden : true } ,
		{ accessor : 'tz'          , hidden : true } ,
		{ accessor : 'id'          , hidden : true } ,
		{ accessor : 'hidden'      , hidden : true, excludeCSV: true } ,
		{ accessor : 'webname'     , hidden : true } ,
		{ accessor : 'reg_end'     , hidden : true } ,
		{ accessor : 'msnats'      , hidden : true } ,
		{ accessor : 'closed'      , hidden : true } ,
		{ accessor : 'nats'        , hidden : true } ,
		{ accessor : 'week'        , hidden : true } ,
		{ accessor : 'year'        , hidden : true } ,
		{ accessor : 'start'       , hidden : true } ,
		{ accessor : 'end'         , hidden : true } ,
		{ accessor : 'schoolcount' , hidden : true } ,
		{ accessor : 'inp'         , hidden : true } ,
		{ accessor : 'hybrid'      , hidden : true } ,
		{ accessor : 'online'      , hidden : true } ,
		{ accessor : 'style'       , hidden : true } ,
		{ accessor : 'country'     , hidden : true } ,
		{ accessor : 'reg_start'   , hidden : true } ,
		{
			accessor : 'tournament',
			cell: (row) => {
				return <a href='row.original.link'>{row.getValue()}</a>;
			},
		},
		{
			accessor : 'dates',
			sortingFn: (rowA, rowB) => {
				if (rowA.original.week === rowB.original.week) { return 0; }
				if (rowA.original.week > rowB.original.week) { return 1; }
				if (rowA.original.week < rowB.original.week) { return -1; }
				return 0;
			},
		},
	];

	const customStyles = {
		table: 'tight',
	};

	return (
		<div>
			<h1>Upcoming Tournaments</h1>
			<JSONTable
				data={tourns}
				options={{ transforms, customStyles, striped: true, exportFileName: 'UpcomingTournament'  }}
			/>
			<button onClick={handleNavigate} type="button">Go to /route</button>
			<p>{name}</p>
		</div>
	);
};

export default TournList;
