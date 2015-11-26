Hangout Timer
=============

A timer extension for Google Hangouts.

### About

This is a project sponsord by the [University of Southern California](http://www.usc.edu). It is an extension for Google Hangouts. The extension is a timer for conducting online debates. There is a user (judge) that controls the timer, sets the time and starts and stops it, and all other  participants (users in Google Hangouts) are then able to view the timer.

* [Google Application](https://console.developers.google.com/project/caiosba-timer?authuser=1)

### Usage

Once permissioned, just create a hangout and on the left sidebar choose "add application", click on tab "developer" and then click on 
"Hangout Timer". Other people can join the hangout and they don't need to add the extension or be a developer on the project.

The extension can be run with two differnt roles:
* [Administrator](https://plus.google.com/hangouts/_/?gid=813898675135&gd=admin:true)
* [Participant](https://plus.google.com/hangouts/_/?gid=813898675135)

You can provide data to the extension by adding a `gd` parameter with the following format: `gd=key1:value1;key2:value2` and so
they will be available under a variable called `params`, which is a hash.


### Credits

The Jibe (http://thejibe.com/)
