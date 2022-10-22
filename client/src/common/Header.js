import React from 'react';
import styles from './header.module.css';
import DisplayUser from './DisplayUser';
import SearchBar from './SearchBar';
import NavMenu from './NavMenu';

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
			<span id={styles.rightstuff}>
				<span id={styles.profile}>
					<DisplayUser />
					<SearchBar />
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

				<NavMenu />
			</span>
		</div>
	);
};

export default Header;
