package Tab::Method;
use base 'Tab::DBI';
Tab::Method->table('method');
Tab::Method->columns(Primary => qw/id/);
Tab::Method->columns(Essential => qw/is_standard timestamp name noshows_never_break double_entry judge_event_twice league/);
Tab::Method->columns(Others => qw/
			add_fine
			allow_judge_own
			allow_neutral_judges
			allow_school_panels
			ask_qualifying_tournament
			ask_two_quals
			audit_method
			bid_min_cume
			bid_min_number
			bid_min_round
			bid_min_round_type
			bid_percent
			bid_round_to_rank
			concession_name
			default_chamber_size
			default_panel_size
			default_qualification
			disable_region_strikes
			double_max
			drop_best_elim
			drop_best_final
			drop_best_rank
			drop_fine
			drop_worst_elim
			drop_worst_final
			drop_worst_rank
			elim_method
			elim_method_basis
			first_school_code
			hide_codes
			hide_debate_codes
			honorable_mentions
			housing
			housing_message
			incremental_school_codes
			judge_cells
			judge_quality_system
			judge_sheet_notice
			master_printouts
			max_chamber_size
			max_panel_size
			mfl_flex_finals
			mfl_time_violation
			min_chamber_size
			min_panel_size
			must_pay_dues
			no_back_to_back
			noshow_judge_fine
			noshow_judge_fine_elim
			novices
			num_judges
			per_student_fee
			points_per_elim
			points_per_entry
			points_per_finalist
			publish_paradigms
			publish_schools
			rating_covers
			rating_system
			require_adult_contact
			schemat_display
			schemat_school_code
			sweep_class_based
			sweep_count_elims
			sweep_count_finals
			sweep_count_prelims
			sweep_event_total
			sweep_final_rank
			sweep_method
			sweep_only_place_final
			sweep_per_event
			sweep_rank_in_elims
			sweep_wildcards
			track_first_year
			track_novice_judges
			track_reg_changes
			truncate_ranks_to
			truncate_to_smallest
			/);

Tab::Method->has_a(default_qualification => 'Tab::Qual');
Tab::Method->has_many(tournaments => 'Tab::Tournament', 'method');
Tab::Method->has_many(tiebreaks => 'Tab::Tiebreak', 'method');

return 1;
