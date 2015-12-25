INSERT INTO history (action_id, hand_id, user_id, position_id, table_id, street_id, pot_size, flg_has_position, flg_opp, flg_3bet, flg_4bet, flg_agg, flg_win)
VALUES(
	(SELECT id FROM possibleActions WHERE action_line = @actionLine),
	@handId,
	(SELECT id FROM users WHERE username = @username),
	(SELECT id FROM positions WHERE positionName = @position),
	(SELECT id FROM tableNames WHERE name = @tableName),
	(SELECT id FROM streets WHERE name = @streetName),
	@potSize,
	@flg_has_position,
	@flg_opp,
	@flg_3bet,
	@flg_4bet,
	@flg_agg,
	@flg_win
);