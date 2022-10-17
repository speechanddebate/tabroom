import React from 'react';
import PropTypes from 'prop-types';

import styles from './Layout.module.css';
import Header from './Header';

const Layout = ({ children }) => {
	return (
		<div className={styles.App}>
			<div id={styles.container}>
				<Header />
				<div id={styles.main}>
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
