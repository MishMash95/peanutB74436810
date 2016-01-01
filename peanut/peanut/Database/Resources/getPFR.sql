SELECT
CAST(
(
	SELECT COUNT(*) FROM history 
	INNER JOIN users ON(users.id = history.user_id)
	INNER JOIN possibleActions ON (possibleActions.id = history.action_id)
	INNER JOIN streets ON(streets.id = history.street_id)
	INNER JOIN positions ON(positions.id = history.position_id)
	WHERE (possibleActions.action_line LIKE '%R%')
	AND users.username = @username
	|ANDWHERE_POSITION|
	
) AS float)/ CAST(
(
	SELECT COUNT(*) FROM history 
	INNER JOIN users ON(users.id = history.user_id)
	INNER JOIN streets ON(streets.id = history.street_id)
	INNER JOIN positions ON(positions.id = history.position_id)
	WHERE users.username = @username
	|ANDWHERE_POSITION|
) AS float) * 100 AS PFR