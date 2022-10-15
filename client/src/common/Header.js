import React from 'react';

import styles from './Header.module.css';
import DisplayUser from './DisplayUser';

const Header = () => {
	return (
		<div id={styles.header}>
			<span id={styles.logo}>
				<a
					tabIndex = "-1"
					href     = "/"
				>
					<img
						src = "/images/tabroom-logo.png"
						alt = "Tabroom.com by the National Speech and Debate Association"
					/>
				</a>
			</span>

			<span id={styles.profile}>
				<DisplayUser />
				<span id={styles.search} title="Search for tournaments">
					<input
						id             = "searchtext"
						type           = "text"
						maxLength      = "128"
						name           = "search"
						caller         = "home"
						placeholder    = "SEARCH TOURNAMENTS"
						className      = "notfirst"
						tabIndex       = "-1"
						autoComplete   = "off"
						autoCorrect    = "off"
						autoCapitalize = "off"
						spellCheck     = "false"
					/>
					<button type="submit" className={styles.searchbox}>
						<img alt="Search" src="/images/search.png" />
					</button>
				</span>

				<span id={styles.helpbutton} title="Tabroom Help">
					<a
						tabIndex  = "-1"
						href      = "http://docs.tabroom.com"
						target    = "_blank"
						rel       = "noopener noreferrer"
						className = "fa fa-question-circle"
					/>
				</span>
			</span>
		</div>
	);
};

export default Header;
