import React, { useEffect, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import styles from './header.module.css';
import { loadTournSearch, clearTournSearch } from '../redux/ducks/search';

const SearchBar = () => {

	const dispatch = useDispatch();
	const sector = useSelector((state) => state.sector);
	const tourn = useSelector((state) => state.tourn);
	const tournSearch = useSelector((state) => state.tournSearch);

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
		formState: { isValid },
		handleSubmit,
		setFocus,
	} = useForm({
		mode: 'all',
		defaultValues: {
			searchString: '',
		},
	});

	// Searching and hitting return
	const searchHandler = async (data) => {
		if (data.searchString.length > 2) {
			console.log('this is a string');
			dispatch(loadTournSearch(data.searchString, 'future'));
		}
	};

	// Dynamic search as people are typing
	const dynamicSearchHandler = async (data) => {
		if (data.searchString.length > 4) {
			dispatch(loadTournSearch(data.searchString, 'future'));
		}

		if (data.searchString.length < 1) {
			dispatch(clearTournSearch());
		}
	};

	// Various ways of clearing the search results and eliminating the window
	const escHandler = (e) => {
		if (e.key === 'Escape') {
			e.preventDefault();
			dispatch(clearTournSearch());
		}
	};

	const clearSearchResults = () => {
		dispatch(clearTournSearch());
	};

	const searchInput = useRef();

	// Control S to get to the search box

	useEffect(() => {
		const goToSearch = (e) => {
			if (e.ctrlKey && e.key === 's') {
				e.preventDefault();
				setFocus('searchString');
			}
		};

		document.addEventListener('keydown', goToSearch);

		return () => {
			document.removeEventListener('keydown', goToSearch);
		};
	}, [setFocus]);

	return (
		<span>
			<form
				onChange={handleSubmit(dynamicSearchHandler)}
				onSubmit={handleSubmit(searchHandler)}
			>
				<span id={styles.search} title={setupSearch.tag}>
					<input
						id             = {styles.searchtext}
						type           = "searchString"
						ref            = {searchInput}
						maxLength      = "128"
						placeholder    = {setupSearch.tag}
						className      = "notfirst"
						tabIndex       = "-1"
						autoComplete   = "off"
						autoCorrect    = "off"
						autoCapitalize = "off"
						spellCheck     = "false"
						onKeyDown      = {escHandler}
						{...register('searchString', { required: true })}
					/>
					<button
						type      = "submit"
						className = {styles.searchbutton}
						disabled  = {!isValid}
					>
						<img alt = "Search" src = "/images/search.png" />
					</button>
				</span>
			</form>

			{
				(tournSearch.exactMatches?.length > 0
					|| tournSearch.partialMatches?.length > 0
				) &&
				<div id={styles.searchOverlay} tabIndex="-1" onKeyDown={escHandler}>
					<div id={styles.searchResults}>
						<div
							id      = {styles.searchResultsHeader}
							onClick = {clearSearchResults}
						> Clear Search Results
							<span className="fa fa-sm fa-times" />
						</div>
						<SearchResults />
					</div>
				</div>
			}
		</span>
	);
};

const SearchResults = (results) => {
	const tournSearch = useSelector((state) => state.tournSearch);

	if (!tournSearch || !tournSearch.searchString?.length) {
		return;
	}

	if (!tournSearch.partialMatches.length && !tournSearch.exactMatches.length) {
		return (
			<div>
				<div id={styles.nada}>
					<p>
						No results found for {tournSearch.searchString}
					</p>
				</div>
			</div>
		);
	}

	return (
		<div>
			{
				tournSearch.exactMatches?.map((match) => (
					<SearchItem
						key   = {match.id}
						scope = "exact"
						term  = {tournSearch.searchString}
						tourn = {match}
					/>
				))
			}
			{
				tournSearch.partialMatches?.map((match) => (
					<SearchItem
						key   = {match.id}
						scope = "partial"
						term  = {tournSearch.searchString}
						tourn = {match}
					/>
				))
			}
		</div>
	);
};

const SearchItem = (props) => {

	const tournName = props.tourn.name;
	const start = new Date(props.tourn.start);
	const end = new Date(props.tourn.end);

	let dateString = '';
	const monthName = start.toLocaleString('default', { month: 'short' });

	// need to replace the below with a way to generate an actual string

	if (start.getDate() === end.getDate()) {
		dateString = `${monthName} ${start.getDate()}, ${start.getFullYear()}`;
	} else {
		dateString = `${monthName} ${start.getDate()}-${end.getDate()}, ${start.getFullYear()}`;
	}

	return (
		<div className={styles[props.scope]}>
			<a
				href={`${process.env.REACT_APP_LEGACY_URL}/index/tourn/index.mhtml?tourn_id=${props.tourn.id}`}
				className={styles.searchLink}
			>
				<div className={styles.tournName}>
					{ tournName }
				</div>
				<div className={styles.shorter}>
					<span className={`third ${styles.location}`}>
						{props.tourn.city}
						{props.tourn.state && props.tourn.city ? ', ' : '' }
						{props.tourn.state ? props.tourn.state : '' }
						{props.tourn.state !== 'US' && props.tourn.country ? props.tourn.country : '' }
					</span>
					<span className={`third ${styles.circuits}`}>
						{props.tourn.circuits}
					</span>
					<span className={`${styles.dates}`}>
						{ dateString }
					</span>
				</div>
			</a>
		</div>
	);
};

export default SearchBar;
