import React from 'react';
import styles from './header.module.css';

export const PublicMenu = () => {

	return (
		<span id={styles.navmenu}>
			<ul id={styles.nav}>
				<li className={styles.top}>
					<a className={styles.top_link} href="/">
						<span className={styles.down}>
							Home
						</span>
					</a>
				</li>

				<li className={styles.top}>
					<a className={styles.top_link} href={`${process.env.REACT_APP_LEGACY_URL}/index/circuits.mhtml`}>
						<span className={styles.down}>
							Circuits
						</span>
					</a>
				</li>

				<li className={styles.top}>
					<a className={styles.top_link} href={`${process.env.REACT_APP_LEGACY_URL}/index/results`}>
						<span className={styles.down}>
							Results
						</span>
					</a>
				</li>

				<li className={styles.top}>
					<a className={styles.top_link} href={`${process.env.REACT_APP_LEGACY_URL}/index/paradigm.mhtml`}>
						<span className={styles.down}>
							Paradigms
						</span>
					</a>
				</li>

				<li className={styles.top}>
					<a className={styles.top_link} href={`${process.env.REACT_APP_LEGACY_URL}/index/help.mhtml`}>
						<span className={styles.down}>
							Help
						</span>
					</a>
				</li>

				<li className={styles.top}>
					<a className={styles.top_link} href={`${process.env.REACT_APP_LEGACY_URL}/index/about.mhtml`}>
						<span className={styles.down}>
							About
						</span>
					</a>
				</li>
			</ul>
		</span>
	);
};

export default PublicMenu;
