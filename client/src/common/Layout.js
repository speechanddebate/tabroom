import React from 'react';
import PropTypes from 'prop-types';
import Header from './Header';
import styles from './Layout.module.css';

const Layout = ({ main, menu }) => {
	return (
		<div className={styles.App}>
			<div id={styles.container}>
				<Header />
				<div id={styles.main}>
					{main}
				</div>
				<div id={styles.menu}>
					{menu}
				</div>
			</div>
		</div>
	);
};

Layout.propTypes = {
	main: PropTypes.node,
	menu: PropTypes.node,
};

export default Layout;
