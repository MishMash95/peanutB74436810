INSERT INTO history (action_id, hand_id, user_id, position_id, table_id, street_id, final_pot_size)
VALUES(
	(SELECT id FROM possibleActions WHERE action_line = @actionLine),
	@handId,
	(SELECT id FROM users WHERE username = @username),
	(SELECT id FROM positions WHERE positionName = @position),
	(SELECT id FROM tableNames WHERE name = @tableName),
	(SELECT id FROM streets WHERE name = @streetName),
	@finalPotSize
);