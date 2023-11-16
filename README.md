

-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

	Copyright (C) 2004-2023 National Forensics League,
	d.b.a. National Speech and Debate Association
		6600 Westown Parkway Suite 270
		West Des Moines, IA 50266
		info@speechanddebate.org

	The Tabroom system began as two projects; the CAT (Computer Assisted Tab)
	by Jon Bruschke, professor at Cal State Fullerton, and Tabroom.com by Chris
	Palmer, a then part-time high school coach in Massachusetts.

	Many new debate tabulation features, including the integrated debate
	tabulation system, were supported by a grant from the Open Society
	Foundations. It contains source code and other contributions by Chris
	Palmer, Jon Bruschke, Aaron Hardy, Michael Stroud, Gary Larson, Sam Rouse,
	Peter Dong, and Cal Ellowitz.

	It is now an ongoing supported project of the National Speech and Debate
	Association, https://www.speechanddebate.org.

	You can redistribute it and/or modify it under either the terms of the RPL
	1.5, available at https://opensource.org/licenses/RPL-1.5 or in the COPYING
	file at the root of this project

-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

## Where did everything else go!?

	As part of an ongoing effort to port Tabroom code from the ancient
	Perl/Mason-1 stack it's merrily chugged along on for years, I've split the
	repositories into its constituent parts.  This repo remains the home of the
	legacy Perl code.

### TABROOM MASON DOCKER

	Tabroom production is being containerized to run on our new cloud services
	in a more responsive and cost-effective way.  As part of the process,
	standard Docker container definitions have been created because recent
	updates to Perl libraries broke some features with particular versions.

	Trust me, I was as surprised as you that someone's out there applying
	updates to mod_perl2 and JSON.pm, but here we are.

	The current staging & production architecture of Tabroom creates a docker
	image solely with apache, mod_perl, mason,  and other ancillary libraries
	to run the code, while the codebase itself lives on the host machine with
	configuration hooked in via mounts in docker-compose.yml.  It requires an
	active /www/tabroom repo of the code on the local machine and
	configurations to live in /etc/tabroom and /etc/apache2.  The docker image
	therefore only contains dependencies and should not change often. To
	prevent github from rebuilding the docker image every time I check in a
	change to the tabroom source code, the docker definitions are now in their
	own repo at

	https://github.com/speechanddebate/mason-docker

### INDEXCARDS API

	The NodeJS Express API that serves as the data backend for the new client
	(and a growing number of legacy tabroom frontend functions) has been dubbed
	Indexcards. That code now is stored in a separate repo at

	https://github.com/speechanddebate/indexcards

### SCHEMATS FRONTEND

	The frontend client of the rewritten code is not yet anywhere in
	production, but work is proceeding on it after deciding at long last my
	brain simply does not like to speak React that much and switching to
	SvelteJS. The frontend naturally is now dubbed "schemats" and its code
	lives in

	https://github.com/speechanddebate/schemats


