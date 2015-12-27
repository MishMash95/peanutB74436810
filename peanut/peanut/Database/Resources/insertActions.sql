INSERT INTO history (action_id, hand_id, user_id, position_id, table_id, street_id, pot_size, 
					flg_has_position, flg_open, flg_3bet, flg_4bet, flg_limp, flg_cold_call, flg_squeeze, flg_aggressor, flg_donk, flg_win)
VALUES(
	(SELECT id FROM possibleActions WHERE action_line = @actionLine),
	@handId,
	(SELECT id FROM users WHERE username = @username),
	(SELECT id FROM positions WHERE name = @position),
	(SELECT id FROM tableNames WHERE name = @tableName),
	(SELECT id FROM streets WHERE name = @streetName),
	@potSize,
	@flg_has_position,
	@flg_open,
	@flg_3bet,
	@flg_4bet,
	@flg_limp,
	@flg_cold_call,
	@flg_squeeze,
	@flg_aggressor,
	@flg_donk,
	@flg_win
);