

	update sweep_rule set count="prelim" where tag="rev_per_prelim_rank";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_prelim_rank";
	update sweep_rule set count="prelim" where tag="rev_per_prelim_rank_sansworst";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_prelim_rank_sansworst";

	update sweep_rule set count="elim" where tag="rev_per_elim_rank";
	update sweep_rule set count="elim" where tag="rev_per_elim_place";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_elim_rank";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_elim_place";

	update sweep_rule set count="final" where tag="rev_per_final_rank";
	update sweep_rule set count="final" where tag="rev_per_final_place";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_final_rank";
	update sweep_rule set tag="rev_per_rank" where tag="rev_per_final_place";


	update sweep_rule set count="prelim" where tag="points_per_prelim_rank";
	update sweep_rule set tag="points_per_rank" where tag="points_per_prelim_rank";
	update sweep_rule set count="prelim" where tag="points_per_prelim_rank_sansworst";
	update sweep_rule set tag="points_per_rank" where tag="points_per_prelim_rank_sansworst";

	update sweep_rule set count="elim" where tag="points_per_elim_rank";
	update sweep_rule set count="elim" where tag="points_per_elim_place";
	update sweep_rule set tag="points_per_rank" where tag="points_per_elim_rank";
	update sweep_rule set tag="points_per_rank" where tag="points_per_elim_place";

	update sweep_rule set count="final" where tag="points_per_final_rank";
	update sweep_rule set count="final" where tag="points_per_final_place";
	update sweep_rule set tag="points_per_rank" where tag="points_per_final_rank";
	update sweep_rule set tag="points_per_rank" where tag="points_per_final_place";


	update sweep_rule set count="prelim" where tag="points_per_prelim";
	update sweep_rule set tag="points_per" where tag="points_per_prelim";

	update sweep_rule set count="elim" where tag="points_per_elim";
	update sweep_rule set tag="points_per" where tag="points_per_elim";

	update sweep_rule set count="final" where tag="points_per_final";
	update sweep_rule set tag="points_per" where tag="points_per_final";

