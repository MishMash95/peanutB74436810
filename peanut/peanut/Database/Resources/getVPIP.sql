SELECT COUNT(*)/(
	SELECT COUNT(*) FROM history_preflop WHERE
	user_id = (SELECT id FROM users WHERE username = @username) 
	|ANDWHERE_POSITION|
)*100 AS VPIP FROM history_preflop WHERE 
user_id = (SELECT id FROM users WHERE username = @username) AND 
action_id = (SELECT id FROM playerActions WHERE (action_line LIKE '%C%' OR action_line LIKE '%R%'))
|ANDWHERE_POSITION|