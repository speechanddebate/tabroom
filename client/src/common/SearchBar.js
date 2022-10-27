import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import styles from './header.module.css';
import { loadTournSearch } from '../redux/ducks/search';

const SearchBar = () => {

	const dispatch = useDispatch();
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
		formState: { errors, isValid },
		handleSubmit,
	} = useForm({
		mode: 'all',
		defaultValues: {
			searchString: '',
		},
	});

	const searchHandler = async (data) => {
		if (data.searchString.length > 2) {
			dispatch(loadTournSearch(data.searchString, 'future'));
		}
	};

	const dynamicSearchHandler = async (data) => {
		if (data.searchString.length > 4) {
			dispatch(loadTournSearch(data.searchString, 'future'));
		}
		if (!data.searchString.length) {
			// tell the state to wipe the results altogether here somehow.
			// also trigger this with a button click
		}
	};

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
						name           = "search"
						placeholder    = {setupSearch.tag}
						className      = "notfirst"
						tabIndex       = "-1"
						autoComplete   = "off"
						autoCorrect    = "off"
						autoCapitalize = "off"
						spellCheck     = "false"
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

			<div id={styles.searchError} className={errors.name ? '' : 'hidden'}>
				{ errors.name?.type === 'required' && <p>Please input text to search for.</p>}
			</div>

			<SearchResults />
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
			<div id={styles.searchResults} className={tournSearch ? '' : 'hidden'}>
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
		</div>
	);
};

const SearchItem = (props) => {

	const startDateString = 'Nov 14-15 2022';
	const tournName = props.tourn.name;

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
						{ startDateString }
					</span>
				</div>
			</a>
		</div>
	);
};

export default SearchBar;
