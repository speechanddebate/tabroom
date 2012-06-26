package Tab::ChapterCircuit;
use base 'Tab::DBI';
Tab::ChapterCircuit->table('chapter_circuit');
Tab::ChapterCircuit->columns(Primary => qw/id/);
Tab::ChapterCircuit->columns(Essential => qw/circuit chapter code membership full_member active paid region/);
Tab::ChapterCircuit->has_a(circuit => "Tab::Circuit");
Tab::ChapterCircuit->has_a(chapter => "Tab::Chapter");
Tab::ChapterCircuit->has_a(region => "Tab::Region");
Tab::ChapterCircuit->has_a(membership => "Tab::CircuitMembership");
