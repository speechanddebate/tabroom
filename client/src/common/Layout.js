import React from 'react';
import PropTypes from 'prop-types';

import styles from './Layout.module.css';
import Header from './Header';

const Layout = ({ children }) => {
	return (
		<div className={styles.App}>
			<Header />
			<div id="main">
				{children}
			</div>
		</div>
	);
};

Layout.propTypes = {
	children: PropTypes.node,
};

export default Layout;
