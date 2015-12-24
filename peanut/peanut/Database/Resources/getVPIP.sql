SELECT COUNT(*)/(
	SELECT COUNT(*) FROM history_preflop WHERE
	user_id = (SELECT id FROM users WHERE username = @username) 
	|WHERE_ALLHANDS|
)*100 FROM history_preflop WHERE 
user_id = (SELECT id FROM users WHERE username = @username) AND 
action_id = (SELECT id FROM playerActions WHERE action_line LIKE '%C%')
|WHERE_VPIPHANDS|;