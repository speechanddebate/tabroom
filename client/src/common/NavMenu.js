import React from 'react';
import { useSelector } from 'react-redux';

import styles from './header.module.css';

export const NavMenu = () => {

	const sector = useSelector((state) => state.sector);

	if (sector === 'tab') {

		// Tournament Administrator Menu

	} else if (sector === 'user') {

		// User Registration Menu

	} else {

		return (
			<span id={styles.navmenu}>
				<ul id={styles.nav}>
					<li class={styles.top}>
						<a class={styles.top_link} href="/">
							<span class={styles.down}>
								Home
							</span>
						</a>
					</li>

					<li class={styles.top}>
						<a class={styles.top_link} href="/index/circuits.mhtml">
							<span class={styles.down}>
								Circuits
							</span>
						</a>
					</li>

					<li class={styles.top}>
						<a class={styles.top_link} href="/index/results/">
							<span class={styles.down}>
								Results
							</span>
						</a>
					</li>

					<li class={styles.top}>
						<a class={styles.top_link} href="/index/paradigm.mhtml">
							<span class={styles.down}>
								Paradigms
							</span>
						</a>
					</li>

					<li class={styles.top}>
						<a class={styles.top_link} href="/index/help.mhtml">
							<span class={styles.down}>
								Help
							</span>
						</a>
					</li>

					<li class={styles.top}>
						<a class={styles.top_link} href="/index/about.mhtml">
							<span class={styles.down}>
								About
							</span>
						</a>
					</li>
				</ul>
			</span>
		);
	}
};

export default NavMenu;
