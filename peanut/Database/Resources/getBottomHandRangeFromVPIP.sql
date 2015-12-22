SELECT strength FROM preFlopHandStrength
ORDER by strength DESC
LIMIT 1 OFFSET (@vpip / 100) * (SELECT COUNT() FROM preFlopHandStrength);