import React from 'react';
import PropTypes from 'prop-types';
import Header from './Header';
import styles from './Layout.module.css';

const Layout = ({ Main, Menu }) => {
	return (
		<div className={styles.App}>
			<div id={styles.container}>
				<Header />
				<div id={styles.wrapper}>
					<div id={styles.main}>
						<Main />
					</div>
					<div id={styles.menu}>
						<Menu />
					</div>
				</div>
			</div>
		</div>
	);
};

Layout.propTypes = {
	Main: PropTypes.func,
	Menu: PropTypes.func,
};

export default Layout;
