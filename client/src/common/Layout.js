import React from 'react';
import PropTypes from 'prop-types';

import styles from './Layout.module.css';
import DisplayUser from './DisplayUser';

const Layout = ({ children }) => {
	return (
		<div className={styles.App}>
			<div id="header">
				<span id="logo">
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

				<span id="profile">
					<DisplayUser />
					<span id="search" title="Search for tournaments">
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
						<button type="submit" className="search notfirst">
							<img alt="Search" src="/lib/images/search.png" />
						</button>
					</span>

					<span id="helpbutton" title="Tabroom Help">
						<a
							tabIndex  = "-1"
							href      = "http://docs.tabroom.com"
							target    = "_blank"
							rel       = "noopener noreferrer"
							className = "fa fa-question-circle"
						/>
					</span>
				</span>
				<div id="main">
					{children}
				</div>
			</div>
		</div>
	);
};

Layout.propTypes = {
	children: PropTypes.node,
};

export default Layout;
