import React from 'react';
import { useSelector } from 'react-redux';
import { useForm, useWatch } from 'react-hook-form';
import styles from './header.module.css';

const SearchBar = () => {

	const sector = useSelector((state) => state.sector);
	const tourn = useSelector((state) => state.tourn);

	const setupSearch = {};

	if (sector === 'tab') {
		setupSearch.api = `${process.env.REACT_APP_API_BASE}/tourn/${tourn}/register/search`;
		setupSearch.tag = 'Search Attendees';
	} else {
		setupSearch.api = `${process.env.REACT_APP_API_BASE}/public/search/all`;
		setupSearch.tag = 'Search Tournaments';
	}

	const {
		register,
		control,
		formState: { errors, isValid },
		handleSearch,
	} = useForm({
		mode: 'all',
		defaultValues: {
			name: '',
			setting: false,
		},
	});

	// Ask Hardy what the hell this is supposed to do again
	const watchedFields = useWatch({ control });
	useEffect(() => { }, [watchedFields]);

	const searchHandler = async (data) => {
		try {
			await fetch('http://local.tabroom.com:10010/v1/status', { method: 'GET', body: data });
		} catch (err) {
			console.log(err);
		}
	};

	return (
		<form onSubmit={handleSearch(searchHandler)}>
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
		</form>
	);
};

export default SearchBar;
