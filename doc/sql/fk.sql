
delete from panel where not exists (select round.id from round where round.panel = panel.id);
alter table panel ADD CONSTRAINT fk_panel_round FOREIGN KEY (round) REFERENCES round(id) ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE rating ADD CONSTRAINT rating UNIQUE (entry, judge, side, rating_subset);

delete from shift where not exists ( select category.id from category where category.id = shift.category );
alter table shift ADD CONSTRAINT fk_shift_category FOREIGN KEY (category) REFERENCES category(id) ON UPDATE CASCADE ON DELETE CASCADE;

delete from pattern where not exists ( select tourn.id from tourn where tourn.id = pattern.tourn );
alter table pattern ADD CONSTRAINT fk_pattern_tourn FOREIGN KEY (tourn) REFERENCES tourn(id) ON UPDATE CASCADE ON DELETE CASCADE;

