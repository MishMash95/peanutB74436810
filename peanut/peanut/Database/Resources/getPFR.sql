SELECT
CAST(
(
	SELECT COUNT(*) FROM history_preflop 
	INNER JOIN users ON(users.id = history_preflop.user_id)
	INNER JOIN possibleActions ON (possibleActions.id = history_preflop.action_id)
	WHERE (possibleActions.action_line LIKE '%R%')
	AND users.username = @username
	|ANDWHERE_POSITION|
	
) AS float)/ CAST(
(
	SELECT COUNT(*) FROM history_preflop 
	INNER JOIN users ON(users.id = history_preflop.user_id)
	WHERE users.username = @username
	|ANDWHERE_POSITION|
) AS float) * 100 AS VPIP