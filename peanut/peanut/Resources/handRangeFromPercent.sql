SELECT strength FROM preFlopHandStrength
ORDER by strength DESC
LIMIT(1, (@vpip / 100) * (SELECT COUNT() FROM preFlopHandStrength))