import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useForm } from 'react-hook-form';
import styles from './header.module.css';
import { loadTournSearch } from '../redux/ducks/search';

const SearchBar = () => {

	const sector = useSelector((state) => state.sector);
	const tourn = useSelector((state) => state.tourn);
	const searchResults = useSelector((state) => state.searchResults);

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

	const dispatch = useDispatch();
	const searchHandler = async (data) => {
		dispatch(loadTournSearch(data.searchString));
	};

	return (
		<span>
			<form onSubmit={handleSubmit(searchHandler)}>
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

			<div id={styles.searchResults} className={searchResults ? '' : 'hidden'}>
				Oh shut it
			</div>
		</span>
	);
};

export default SearchBar;
