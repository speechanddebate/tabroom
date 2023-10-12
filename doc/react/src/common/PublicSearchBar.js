import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import styles from './header.module.css';
import { loadTournSearch, clearTournSearch } from '../redux/ducks/search';

const SearchBar = () => {

	const dispatch = useDispatch();
	const tournSearch = useSelector((state) => state.tournSearch);

	const setupSearch = {
		api : `${process.env.REACT_APP_API_BASE}/public/search/future`,
		tag : 'Search Tournaments',
	};

	const {
		register,
		formState: { isValid },
		handleSubmit,
		setFocus,
		setValue,
	} = useForm({
		mode: 'all',
		defaultValues: {
			searchString: '',
		},
	});

	// Searching and hitting return
	const searchHandler = async (data) => {
		if (data.searchString.length > 2) {
			dispatch(loadTournSearch(data.searchString, 'future'));
		}
	};

	// Dynamic search as people are typing, if they type more than 4 characters anyway.
	const dynamicSearchHandler = async (data) => {
		if (data.searchString.length > 4) {
			dispatch(loadTournSearch(data.searchString, 'future'));
		}
	};

	// Various ways of clearing the search results and eliminating the window
	const escHandler = (e) => {
		if (e.key === 'Escape') {
			e.preventDefault();
			dispatch(clearTournSearch());

			// For the record, it took me a positively stupid amount of time to
			// figure out how to do this operation.
			setValue('searchString', '');
		}

		// Apparently the onChange handler with dynamicSearch does not fire if
		// the value is blank so handle it here.
		if (
			(e.key === 'Delete' || e.key === 'Backspace')
			&& e.target?.value?.length === 1
		) {
			clearSearchResults();
		}
	};

	const clearSearchResults = () => {
		dispatch(clearTournSearch());
	};

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

			{
				( tournSearch.searchString
				&& (tournSearch.searchString === 'I feel so dumb'
					|| tournSearch.searchString === 'i feel so dumb'
				)
				&& (
					tournSearch.partialMatches?.length < 1
					&& tournSearch.exactMatches?.length < 1
				)) &&
					<div>
						<div id={styles.nada}>
							<p>
								Honey, trust us, we at Tabroom.com know that feeling REALLY WELL
							</p>
						</div>
					</div>
			}

			{
				( tournSearch.searchString
					&& tournSearch.searchString !== 'I feel so dumb'
					&& (
						tournSearch.partialMatches?.length < 1
						&& tournSearch.exactMatches?.length < 1
					)
				) &&
					<div>
						<div id={styles.nada}>
							<p>
									No search results found for <span className='semibold'>{tournSearch.searchString}</span>`
							</p>
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
