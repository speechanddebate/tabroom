
	update sweep_rule set tag = "ballot_win" where tag = "debate_win";
	update sweep_rule set tag = "ballot_loss" where tag = "debate_loss";
	update sweep_rule set tag = "round_win" where tag = "debate_round_win";
	update sweep_rule set tag = "round_loss" where tag = "debate_round_loss";
	update sweep_rule set tag = "round_bye" where tag = "debate_round_bye";

