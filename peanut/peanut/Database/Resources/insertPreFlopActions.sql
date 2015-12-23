INSERT INTO history_preflop (action_id, hand_id, user_id, position_id, table_id)
VALUES(
	(SELECT id FROM playerActions WHERE action_line = @actionLine),
	@handId,
	@userId,
	(SELECT id FROM positions WHERE positionName = @position),
	(SELECT id FROM tableNames WHERE name = @tableName)
);