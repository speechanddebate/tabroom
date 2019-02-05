ALTER TABLE ballot ADD CONSTRAINT ballot_sides UNIQUE (entry, judge, panel);
ALTER TABLE ballot ADD CONSTRAINT ballot_ejp UNIQUE (side, judge, speakerorder, panel);

