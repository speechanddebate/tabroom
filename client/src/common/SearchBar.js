import React from 'react';
import { useSelector } from 'react-redux';

import styles from './header.module.css';

const SearchBar = () => {

	const sector = useSelector((state) => state.sector);
	const tourn = useSelector((state) => state.tourn);
	const setupSearch = {};

	if (sector === 'tab') {
		setupSearch.api = `/tourn/${tourn}/search`;
		setupSearch.tag = 'Search Attendees';
	} else {
		setupSearch.api = `/public/search`;
		setupSearch.tag = 'Search Tournaments';
	}

	// Presumably here I can dispatch some task to send the search term to an
	// API and then display a results listing.  Gotta build said API first.

	// Which isn't such a bad next step...

	return (
		<span id={styles.search} title={setupSearch.tag}>
			<input
				id             = {styles.searchtext}
				type           = "text"
				maxLength      = "128"
				name           = "search"
				placeholder    = {setupSearch.tag}
				className      = "notfirst"
				tabIndex       = "-1"
				autoComplete   = "off"
				autoCorrect    = "off"
				autoCapitalize = "off"
				spellCheck     = "false"
			/>
			<button type="submit" className={styles.searchbutton}>
				<img alt="Search" src="/images/search.png" />
			</button>
		</span>
	);
};

export default SearchBar;
