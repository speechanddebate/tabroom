import React from 'react';
import styles from './App.module.css';
import DisplayUser from './DisplayUser';
import TestTable from './TestTable';

const App = () => {
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
				<TestTable />
			</div>
		</div>
	);
};

export default App;
