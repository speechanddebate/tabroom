#!/bin/sh

perl -pi -e 's/\$method\) / \$method->\$tourn->setting\(\"/g' *.m*
perl -pi -e 's/ == 1/\"\) == 1/g' *.m*
perl -pi -e 's/ eq 1/\"\) eq 1/g' *.m*
perl -pi -e 's/group->/group->setting\(\"/g' *.m*
perl -pi -e 's/\(\$/\", \$/g' *.m*
