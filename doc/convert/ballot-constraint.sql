ALTER TABLE ballot ADD CONSTRAINT ballot_ejp UNIQUE (entry, judge, panel);
ALTER TABLE ballot ADD CONSTRAINT ballot_sideorder UNIQUE (panel, judge, side, speakerorder);

