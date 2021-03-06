/* Create the table structures */
CREATE TABLE possibleActions (
	id INTEGER PRIMARY KEY, 
	action_line VARCHAR(50) UNIQUE
);
CREATE TABLE history (
	id INTEGER PRIMARY KEY, 
	action_id INTEGER REFERENCES playerActions(id),
	hand_id INTEGER,
	user_id INTEGER REFERENCES users(id),
	position_id INTEGER REFERENCES positions(id),
	table_id INTEGER REFERENCES tableNames(id),
	street_id INTEGER REFERENCES streets(id),
	pot_size INTEGER,			/* Size of the pot at the end of the betting round in pence */

	flg_has_position INTEGER,   /* If the player has position over all the other players in the hand */
	flg_open INTEGER,   /* If the player is the first to raise on the given street */
	flg_3bet INTEGER,
	flg_4bet INTEGER, 
	flg_limp INTEGER, /* If the player called the big blind */
	flg_cold_call INTEGER, /* If the player called an open raise */
	flg_squeeze INTEGER, /* A squeeze play is when a player raises pre-flop, gets at least one cold call, then a third player re-raises after this. The 3 bet is a squeeze play because of the cold caller(s) in the hand. */
	flg_aggressor INTEGER,  /* If the player was the last to raise on the proceeding street */
	flg_donk INTEGER,  /* Called a cbet on the proceeding street and then open bet on this street */
	flg_win INTEGER   /* If the player took down the pot on this street */
);
CREATE TABLE communityCards (
	id INTEGER PRIMARY KEY,
	hand_id INTEGER UNIQUE,
	flop_card1 INTEGER REFERENCES cards(id),
	flop_card2 INTEGER REFERENCES cards(id),
	flop_card3 INTEGER REFERENCES cards(id),
	turn_card INTEGER REFERENCES cards(id),
	river_card INTEGER REFERENCES cards(id)
);

/* 
	Raise 1,2,3,4,5 correspond to successive raises/reraises by the player (maximum of RRRRR)
	These are how much the player has raised by NOT how much it is raised to 
*/
CREATE TABLE raises (
	id INTEGER PRIMARY KEY,
	history_id INTEGER REFERENCES history(id),
	raise1 INTEGER,
	raise2 INTEGER,
	raise3 INTEGER,
	raise4 INTEGER,
	raise5 INTEGER
);
CREATE TABLE streets (
	id INTEGER PRIMARY KEY, 
	name VARCHAR(7)
);
CREATE TABLE userHands (
	id INTEGER PRIMARY KEY, 
	hand_id INTEGER UNIQUE, 
	hole_card1 INTEGER REFERENCES cards(id), 
	hole_card2 INTEGER REFERENCES cards(id)
);
CREATE TABLE tableNames (
	id INTEGER PRIMARY KEY, 
	name VARCHAR(100) UNIQUE
);
CREATE TABLE users (
	id INTEGER PRIMARY KEY, 
	username VARCHAR(100) UNIQUE
);
CREATE TABLE positions (
	id INTEGER PRIMARY KEY, 
	name VARCHAR(5) UNIQUE
);
CREATE TABLE cards(
	id INTEGER PRIMARY KEY,
	card VARCHAR(2)
);
CREATE TABLE hitOdds(
	id INTEGER PRIMARY KEY,
	outs INTEGER,
	onTurn REAL,
	onRiver REAL,
	onEither REAL
);

/* Populate the tables */
INSERT INTO tableNames (name) VALUES
("UNKNOWN");

INSERT INTO streets (name) VALUES
("preflop"),("flop"),("turn"),("river");

INSERT INTO cards (card) VALUES
("Ac"),("As"),("Ad"),("Ah"),("Kc"),("Ks"),("Kd"),("Kh"),("Qc"),("Qs"),("Qd"),("Qh"),("Jc"),("Js"),("Jd"),("Jh"),("Tc"),("Ts"),("Td"),("Th"),("9c"),("9s"),("9d"),("9h"),("8c"),("8s"),("8d"),("8h"),("7c"),("7s"),("7d"),("7h"),("6c"),("6s"),("6d"),("6h"),("5c"),("5s"),("5d"),("5h"),("4c"),("4s"),("4d"),("4h"),("3c"),("3s"),("3d"),("3h"),("2c"),("2s"),("2d"),("2h");

INSERT INTO possibleActions (action_line) VALUES
("-"), /* No action eg folds around to BB preflop (walk) */ 
("F"),("X"),("R"),("C"),
("FF"),("FX"),("FC"),("FR"),("XF"),("XX"),("XC"),("XR"),("CF"),("CX"),("CC"),("CR"),("RF"),("RX"),("RC"),("RR"),
("FFF"),("FFX"),("FFC"),("FFR"),("FXF"),("FXX"),("FXC"),("FXR"),("FCF"),("FCX"),("FCC"),("FCR"),("FRF"),("FRX"),("FRC"),("FRR"),("XFF"),("XFX"),("XFC"),("XFR"),("XXF"),("XXX"),("XXC"),("XXR"),("XCF"),("XCX"),("XCC"),("XCR"),("XRF"),("XRX"),("XRC"),("XRR"),("CFF"),("CFX"),("CFC"),("CFR"),("CXF"),("CXX"),("CXC"),("CXR"),("CCF"),("CCX"),("CCC"),("CCR"),("CRF"),("CRX"),("CRC"),("CRR"),("RFF"),("RFX"),("RFC"),("RFR"),("RXF"),("RXX"),("RXC"),("RXR"),("RCF"),("RCX"),("RCC"),("RCR"),("RRF"),("RRX"),("RRC"),("RRR"),
("FFFF"),("FFFX"),("FFFC"),("FFFR"),("FFXF"),("FFXX"),("FFXC"),("FFXR"),("FFCF"),("FFCX"),("FFCC"),("FFCR"),("FFRF"),("FFRX"),("FFRC"),("FFRR"),("FXFF"),("FXFX"),("FXFC"),("FXFR"),("FXXF"),("FXXX"),("FXXC"),("FXXR"),("FXCF"),("FXCX"),("FXCC"),("FXCR"),("FXRF"),("FXRX"),("FXRC"),("FXRR"),("FCFF"),("FCFX"),("FCFC"),("FCFR"),("FCXF"),("FCXX"),("FCXC"),("FCXR"),("FCCF"),("FCCX"),("FCCC"),("FCCR"),("FCRF"),("FCRX"),("FCRC"),("FCRR"),("FRFF"),("FRFX"),("FRFC"),("FRFR"),("FRXF"),("FRXX"),("FRXC"),("FRXR"),("FRCF"),("FRCX"),("FRCC"),("FRCR"),("FRRF"),("FRRX"),("FRRC"),("FRRR"),("XFFF"),("XFFX"),("XFFC"),("XFFR"),("XFXF"),("XFXX"),("XFXC"),("XFXR"),("XFCF"),("XFCX"),("XFCC"),("XFCR"),("XFRF"),("XFRX"),("XFRC"),("XFRR"),("XXFF"),("XXFX"),("XXFC"),("XXFR"),("XXXF"),("XXXX"),("XXXC"),("XXXR"),("XXCF"),("XXCX"),("XXCC"),("XXCR"),("XXRF"),("XXRX"),("XXRC"),("XXRR"),("XCFF"),("XCFX"),("XCFC"),("XCFR"),("XCXF"),("XCXX"),("XCXC"),("XCXR"),("XCCF"),("XCCX"),("XCCC"),("XCCR"),("XCRF"),("XCRX"),("XCRC"),("XCRR"),("XRFF"),("XRFX"),("XRFC"),("XRFR"),("XRXF"),("XRXX"),("XRXC"),("XRXR"),("XRCF"),("XRCX"),("XRCC"),("XRCR"),("XRRF"),("XRRX"),("XRRC"),("XRRR"),("CFFF"),("CFFX"),("CFFC"),("CFFR"),("CFXF"),("CFXX"),("CFXC"),("CFXR"),("CFCF"),("CFCX"),("CFCC"),("CFCR"),("CFRF"),("CFRX"),("CFRC"),("CFRR"),("CXFF"),("CXFX"),("CXFC"),("CXFR"),("CXXF"),("CXXX"),("CXXC"),("CXXR"),("CXCF"),("CXCX"),("CXCC"),("CXCR"),("CXRF"),("CXRX"),("CXRC"),("CXRR"),("CCFF"),("CCFX"),("CCFC"),("CCFR"),("CCXF"),("CCXX"),("CCXC"),("CCXR"),("CCCF"),("CCCX"),("CCCC"),("CCCR"),("CCRF"),("CCRX"),("CCRC"),("CCRR"),("CRFF"),("CRFX"),("CRFC"),("CRFR"),("CRXF"),("CRXX"),("CRXC"),("CRXR"),("CRCF"),("CRCX"),("CRCC"),("CRCR"),("CRRF"),("CRRX"),("CRRC"),("CRRR"),("RFFF"),("RFFX"),("RFFC"),("RFFR"),("RFXF"),("RFXX"),("RFXC"),("RFXR"),("RFCF"),("RFCX"),("RFCC"),("RFCR"),("RFRF"),("RFRX"),("RFRC"),("RFRR"),("RXFF"),("RXFX"),("RXFC"),("RXFR"),("RXXF"),("RXXX"),("RXXC"),("RXXR"),("RXCF"),("RXCX"),("RXCC"),("RXCR"),("RXRF"),("RXRX"),("RXRC"),("RXRR"),("RCFF"),("RCFX"),("RCFC"),("RCFR"),("RCXF"),("RCXX"),("RCXC"),("RCXR"),("RCCF"),("RCCX"),("RCCC"),("RCCR"),("RCRF"),("RCRX"),("RCRC"),("RCRR"),("RRFF"),("RRFX"),("RRFC"),("RRFR"),("RRXF"),("RRXX"),("RRXC"),("RRXR"),("RRCF"),("RRCX"),("RRCC"),("RRCR"),("RRRF"),("RRRX"),("RRRC"),("RRRR"),
("FFFFF"),("FFFFX"),("FFFFC"),("FFFFR"),("FFFXF"),("FFFXX"),("FFFXC"),("FFFXR"),("FFFCF"),("FFFCX"),("FFFCC"),("FFFCR"),("FFFRF"),("FFFRX"),("FFFRC"),("FFFRR"),("FFXFF"),("FFXFX"),("FFXFC"),("FFXFR"),("FFXXF"),("FFXXX"),("FFXXC"),("FFXXR"),("FFXCF"),("FFXCX"),("FFXCC"),("FFXCR"),("FFXRF"),("FFXRX"),("FFXRC"),("FFXRR"),("FFCFF"),("FFCFX"),("FFCFC"),("FFCFR"),("FFCXF"),("FFCXX"),("FFCXC"),("FFCXR"),("FFCCF"),("FFCCX"),("FFCCC"),("FFCCR"),("FFCRF"),("FFCRX"),("FFCRC"),("FFCRR"),("FFRFF"),("FFRFX"),("FFRFC"),("FFRFR"),("FFRXF"),("FFRXX"),("FFRXC"),("FFRXR"),("FFRCF"),("FFRCX"),("FFRCC"),("FFRCR"),("FFRRF"),("FFRRX"),("FFRRC"),("FFRRR"),("FXFFF"),("FXFFX"),("FXFFC"),("FXFFR"),("FXFXF"),("FXFXX"),("FXFXC"),("FXFXR"),("FXFCF"),("FXFCX"),("FXFCC"),("FXFCR"),("FXFRF"),("FXFRX"),("FXFRC"),("FXFRR"),("FXXFF"),("FXXFX"),("FXXFC"),("FXXFR"),("FXXXF"),("FXXXX"),("FXXXC"),("FXXXR"),("FXXCF"),("FXXCX"),("FXXCC"),("FXXCR"),("FXXRF"),("FXXRX"),("FXXRC"),("FXXRR"),("FXCFF"),("FXCFX"),("FXCFC"),("FXCFR"),("FXCXF"),("FXCXX"),("FXCXC"),("FXCXR"),("FXCCF"),("FXCCX"),("FXCCC"),("FXCCR"),("FXCRF"),("FXCRX"),("FXCRC"),("FXCRR"),("FXRFF"),("FXRFX"),("FXRFC"),("FXRFR"),("FXRXF"),("FXRXX"),("FXRXC"),("FXRXR"),("FXRCF"),("FXRCX"),("FXRCC"),("FXRCR"),("FXRRF"),("FXRRX"),("FXRRC"),("FXRRR"),("FCFFF"),("FCFFX"),("FCFFC"),("FCFFR"),("FCFXF"),("FCFXX"),("FCFXC"),("FCFXR"),("FCFCF"),("FCFCX"),("FCFCC"),("FCFCR"),("FCFRF"),("FCFRX"),("FCFRC"),("FCFRR"),("FCXFF"),("FCXFX"),("FCXFC"),("FCXFR"),("FCXXF"),("FCXXX"),("FCXXC"),("FCXXR"),("FCXCF"),("FCXCX"),("FCXCC"),("FCXCR"),("FCXRF"),("FCXRX"),("FCXRC"),("FCXRR"),("FCCFF"),("FCCFX"),("FCCFC"),("FCCFR"),("FCCXF"),("FCCXX"),("FCCXC"),("FCCXR"),("FCCCF"),("FCCCX"),("FCCCC"),("FCCCR"),("FCCRF"),("FCCRX"),("FCCRC"),("FCCRR"),("FCRFF"),("FCRFX"),("FCRFC"),("FCRFR"),("FCRXF"),("FCRXX"),("FCRXC"),("FCRXR"),("FCRCF"),("FCRCX"),("FCRCC"),("FCRCR"),("FCRRF"),("FCRRX"),("FCRRC"),("FCRRR"),("FRFFF"),("FRFFX"),("FRFFC"),("FRFFR"),("FRFXF"),("FRFXX"),("FRFXC"),("FRFXR"),("FRFCF"),("FRFCX"),("FRFCC"),("FRFCR"),("FRFRF"),("FRFRX"),("FRFRC"),("FRFRR"),("FRXFF"),("FRXFX"),("FRXFC"),("FRXFR"),("FRXXF"),("FRXXX"),("FRXXC"),("FRXXR"),("FRXCF"),("FRXCX"),("FRXCC"),("FRXCR"),("FRXRF"),("FRXRX"),("FRXRC"),("FRXRR"),("FRCFF"),("FRCFX"),("FRCFC"),("FRCFR"),("FRCXF"),("FRCXX"),("FRCXC"),("FRCXR"),("FRCCF"),("FRCCX"),("FRCCC"),("FRCCR"),("FRCRF"),("FRCRX"),("FRCRC"),("FRCRR"),("FRRFF"),("FRRFX"),("FRRFC"),("FRRFR"),("FRRXF"),("FRRXX"),("FRRXC"),("FRRXR"),("FRRCF"),("FRRCX"),("FRRCC"),("FRRCR"),("FRRRF"),("FRRRX"),("FRRRC"),("FRRRR"),("XFFFF"),("XFFFX"),("XFFFC"),("XFFFR"),("XFFXF"),("XFFXX"),("XFFXC"),("XFFXR"),("XFFCF"),("XFFCX"),("XFFCC"),("XFFCR"),("XFFRF"),("XFFRX"),("XFFRC"),("XFFRR"),("XFXFF"),("XFXFX"),("XFXFC"),("XFXFR"),("XFXXF"),("XFXXX"),("XFXXC"),("XFXXR"),("XFXCF"),("XFXCX"),("XFXCC"),("XFXCR"),("XFXRF"),("XFXRX"),("XFXRC"),("XFXRR"),("XFCFF"),("XFCFX"),("XFCFC"),("XFCFR"),("XFCXF"),("XFCXX"),("XFCXC"),("XFCXR"),("XFCCF"),("XFCCX"),("XFCCC"),("XFCCR"),("XFCRF"),("XFCRX"),("XFCRC"),("XFCRR"),("XFRFF"),("XFRFX"),("XFRFC"),("XFRFR"),("XFRXF"),("XFRXX"),("XFRXC"),("XFRXR"),("XFRCF"),("XFRCX"),("XFRCC"),("XFRCR"),("XFRRF"),("XFRRX"),("XFRRC"),("XFRRR"),("XXFFF"),("XXFFX"),("XXFFC"),("XXFFR"),("XXFXF"),("XXFXX"),("XXFXC"),("XXFXR"),("XXFCF"),("XXFCX"),("XXFCC"),("XXFCR"),("XXFRF"),("XXFRX"),("XXFRC"),("XXFRR"),("XXXFF"),("XXXFX"),("XXXFC"),("XXXFR"),("XXXXF"),("XXXXX"),("XXXXC"),("XXXXR"),("XXXCF"),("XXXCX"),("XXXCC"),("XXXCR"),("XXXRF"),("XXXRX"),("XXXRC"),("XXXRR"),("XXCFF"),("XXCFX"),("XXCFC"),("XXCFR"),("XXCXF"),("XXCXX"),("XXCXC"),("XXCXR"),("XXCCF"),("XXCCX"),("XXCCC"),("XXCCR"),("XXCRF"),("XXCRX"),("XXCRC"),("XXCRR"),("XXRFF"),("XXRFX"),("XXRFC"),("XXRFR"),("XXRXF"),("XXRXX"),("XXRXC"),("XXRXR"),("XXRCF"),("XXRCX"),("XXRCC"),("XXRCR"),("XXRRF"),("XXRRX"),("XXRRC"),("XXRRR"),("XCFFF"),("XCFFX"),("XCFFC"),("XCFFR"),("XCFXF"),("XCFXX"),("XCFXC"),("XCFXR"),("XCFCF"),("XCFCX"),("XCFCC"),("XCFCR"),("XCFRF"),("XCFRX"),("XCFRC"),("XCFRR"),("XCXFF"),("XCXFX"),("XCXFC"),("XCXFR"),("XCXXF"),("XCXXX"),("XCXXC"),("XCXXR"),("XCXCF"),("XCXCX"),("XCXCC"),("XCXCR"),("XCXRF"),("XCXRX"),("XCXRC"),("XCXRR"),("XCCFF"),("XCCFX"),("XCCFC"),("XCCFR"),("XCCXF"),("XCCXX"),("XCCXC"),("XCCXR"),("XCCCF"),("XCCCX"),("XCCCC"),("XCCCR"),("XCCRF"),("XCCRX"),("XCCRC"),("XCCRR"),("XCRFF"),("XCRFX"),("XCRFC"),("XCRFR"),("XCRXF"),("XCRXX"),("XCRXC"),("XCRXR"),("XCRCF"),("XCRCX"),("XCRCC"),("XCRCR"),("XCRRF"),("XCRRX"),("XCRRC"),("XCRRR"),("XRFFF"),("XRFFX"),("XRFFC"),("XRFFR"),("XRFXF"),("XRFXX"),("XRFXC"),("XRFXR"),("XRFCF"),("XRFCX"),("XRFCC"),("XRFCR"),("XRFRF"),("XRFRX"),("XRFRC"),("XRFRR"),("XRXFF"),("XRXFX"),("XRXFC"),("XRXFR"),("XRXXF"),("XRXXX"),("XRXXC"),("XRXXR"),("XRXCF"),("XRXCX"),("XRXCC"),("XRXCR"),("XRXRF"),("XRXRX"),("XRXRC"),("XRXRR"),("XRCFF"),("XRCFX"),("XRCFC"),("XRCFR"),("XRCXF"),("XRCXX"),("XRCXC"),("XRCXR"),("XRCCF"),("XRCCX"),("XRCCC"),("XRCCR"),("XRCRF"),("XRCRX"),("XRCRC"),("XRCRR"),("XRRFF"),("XRRFX"),("XRRFC"),("XRRFR"),("XRRXF"),("XRRXX"),("XRRXC"),("XRRXR"),("XRRCF"),("XRRCX"),("XRRCC"),("XRRCR"),("XRRRF"),("XRRRX"),("XRRRC"),("XRRRR"),("CFFFF"),("CFFFX"),("CFFFC"),("CFFFR"),("CFFXF"),("CFFXX"),("CFFXC"),("CFFXR"),("CFFCF"),("CFFCX"),("CFFCC"),("CFFCR"),("CFFRF"),("CFFRX"),("CFFRC"),("CFFRR"),("CFXFF"),("CFXFX"),("CFXFC"),("CFXFR"),("CFXXF"),("CFXXX"),("CFXXC"),("CFXXR"),("CFXCF"),("CFXCX"),("CFXCC"),("CFXCR"),("CFXRF"),("CFXRX"),("CFXRC"),("CFXRR"),("CFCFF"),("CFCFX"),("CFCFC"),("CFCFR"),("CFCXF"),("CFCXX"),("CFCXC"),("CFCXR"),("CFCCF"),("CFCCX"),("CFCCC"),("CFCCR"),("CFCRF"),("CFCRX"),("CFCRC"),("CFCRR"),("CFRFF"),("CFRFX"),("CFRFC"),("CFRFR"),("CFRXF"),("CFRXX"),("CFRXC"),("CFRXR"),("CFRCF"),("CFRCX"),("CFRCC"),("CFRCR"),("CFRRF"),("CFRRX"),("CFRRC"),("CFRRR"),("CXFFF"),("CXFFX"),("CXFFC"),("CXFFR"),("CXFXF"),("CXFXX"),("CXFXC"),("CXFXR"),("CXFCF"),("CXFCX"),("CXFCC"),("CXFCR"),("CXFRF"),("CXFRX"),("CXFRC"),("CXFRR"),("CXXFF"),("CXXFX"),("CXXFC"),("CXXFR"),("CXXXF"),("CXXXX"),("CXXXC"),("CXXXR"),("CXXCF"),("CXXCX"),("CXXCC"),("CXXCR"),("CXXRF"),("CXXRX"),("CXXRC"),("CXXRR"),("CXCFF"),("CXCFX"),("CXCFC"),("CXCFR"),("CXCXF"),("CXCXX"),("CXCXC"),("CXCXR"),("CXCCF"),("CXCCX"),("CXCCC"),("CXCCR"),("CXCRF"),("CXCRX"),("CXCRC"),("CXCRR"),("CXRFF"),("CXRFX"),("CXRFC"),("CXRFR"),("CXRXF"),("CXRXX"),("CXRXC"),("CXRXR"),("CXRCF"),("CXRCX"),("CXRCC"),("CXRCR"),("CXRRF"),("CXRRX"),("CXRRC"),("CXRRR"),("CCFFF"),("CCFFX"),("CCFFC"),("CCFFR"),("CCFXF"),("CCFXX"),("CCFXC"),("CCFXR"),("CCFCF"),("CCFCX"),("CCFCC"),("CCFCR"),("CCFRF"),("CCFRX"),("CCFRC"),("CCFRR"),("CCXFF"),("CCXFX"),("CCXFC"),("CCXFR"),("CCXXF"),("CCXXX"),("CCXXC"),("CCXXR"),("CCXCF"),("CCXCX"),("CCXCC"),("CCXCR"),("CCXRF"),("CCXRX"),("CCXRC"),("CCXRR"),("CCCFF"),("CCCFX"),("CCCFC"),("CCCFR"),("CCCXF"),("CCCXX"),("CCCXC"),("CCCXR"),("CCCCF"),("CCCCX"),("CCCCC"),("CCCCR"),("CCCRF"),("CCCRX"),("CCCRC"),("CCCRR"),("CCRFF"),("CCRFX"),("CCRFC"),("CCRFR"),("CCRXF"),("CCRXX"),("CCRXC"),("CCRXR"),("CCRCF"),("CCRCX"),("CCRCC"),("CCRCR"),("CCRRF"),("CCRRX"),("CCRRC"),("CCRRR"),("CRFFF"),("CRFFX"),("CRFFC"),("CRFFR"),("CRFXF"),("CRFXX"),("CRFXC"),("CRFXR"),("CRFCF"),("CRFCX"),("CRFCC"),("CRFCR"),("CRFRF"),("CRFRX"),("CRFRC"),("CRFRR"),("CRXFF"),("CRXFX"),("CRXFC"),("CRXFR"),("CRXXF"),("CRXXX"),("CRXXC"),("CRXXR"),("CRXCF"),("CRXCX"),("CRXCC"),("CRXCR"),("CRXRF"),("CRXRX"),("CRXRC"),("CRXRR"),("CRCFF"),("CRCFX"),("CRCFC"),("CRCFR"),("CRCXF"),("CRCXX"),("CRCXC"),("CRCXR"),("CRCCF"),("CRCCX"),("CRCCC"),("CRCCR"),("CRCRF"),("CRCRX"),("CRCRC"),("CRCRR"),("CRRFF"),("CRRFX"),("CRRFC"),("CRRFR"),("CRRXF"),("CRRXX"),("CRRXC"),("CRRXR"),("CRRCF"),("CRRCX"),("CRRCC"),("CRRCR"),("CRRRF"),("CRRRX"),("CRRRC"),("CRRRR"),("RFFFF"),("RFFFX"),("RFFFC"),("RFFFR"),("RFFXF"),("RFFXX"),("RFFXC"),("RFFXR"),("RFFCF"),("RFFCX"),("RFFCC"),("RFFCR"),("RFFRF"),("RFFRX"),("RFFRC"),("RFFRR"),("RFXFF"),("RFXFX"),("RFXFC"),("RFXFR"),("RFXXF"),("RFXXX"),("RFXXC"),("RFXXR"),("RFXCF"),("RFXCX"),("RFXCC"),("RFXCR"),("RFXRF"),("RFXRX"),("RFXRC"),("RFXRR"),("RFCFF"),("RFCFX"),("RFCFC"),("RFCFR"),("RFCXF"),("RFCXX"),("RFCXC"),("RFCXR"),("RFCCF"),("RFCCX"),("RFCCC"),("RFCCR"),("RFCRF"),("RFCRX"),("RFCRC"),("RFCRR"),("RFRFF"),("RFRFX"),("RFRFC"),("RFRFR"),("RFRXF"),("RFRXX"),("RFRXC"),("RFRXR"),("RFRCF"),("RFRCX"),("RFRCC"),("RFRCR"),("RFRRF"),("RFRRX"),("RFRRC"),("RFRRR"),("RXFFF"),("RXFFX"),("RXFFC"),("RXFFR"),("RXFXF"),("RXFXX"),("RXFXC"),("RXFXR"),("RXFCF"),("RXFCX"),("RXFCC"),("RXFCR"),("RXFRF"),("RXFRX"),("RXFRC"),("RXFRR"),("RXXFF"),("RXXFX"),("RXXFC"),("RXXFR"),("RXXXF"),("RXXXX"),("RXXXC"),("RXXXR"),("RXXCF"),("RXXCX"),("RXXCC"),("RXXCR"),("RXXRF"),("RXXRX"),("RXXRC"),("RXXRR"),("RXCFF"),("RXCFX"),("RXCFC"),("RXCFR"),("RXCXF"),("RXCXX"),("RXCXC"),("RXCXR"),("RXCCF"),("RXCCX"),("RXCCC"),("RXCCR"),("RXCRF"),("RXCRX"),("RXCRC"),("RXCRR"),("RXRFF"),("RXRFX"),("RXRFC"),("RXRFR"),("RXRXF"),("RXRXX"),("RXRXC"),("RXRXR"),("RXRCF"),("RXRCX"),("RXRCC"),("RXRCR"),("RXRRF"),("RXRRX"),("RXRRC"),("RXRRR"),("RCFFF"),("RCFFX"),("RCFFC"),("RCFFR"),("RCFXF"),("RCFXX"),("RCFXC"),("RCFXR"),("RCFCF"),("RCFCX"),("RCFCC"),("RCFCR"),("RCFRF"),("RCFRX"),("RCFRC"),("RCFRR"),("RCXFF"),("RCXFX"),("RCXFC"),("RCXFR"),("RCXXF"),("RCXXX"),("RCXXC"),("RCXXR"),("RCXCF"),("RCXCX"),("RCXCC"),("RCXCR"),("RCXRF"),("RCXRX"),("RCXRC"),("RCXRR"),("RCCFF"),("RCCFX"),("RCCFC"),("RCCFR"),("RCCXF"),("RCCXX"),("RCCXC"),("RCCXR"),("RCCCF"),("RCCCX"),("RCCCC"),("RCCCR"),("RCCRF"),("RCCRX"),("RCCRC"),("RCCRR"),("RCRFF"),("RCRFX"),("RCRFC"),("RCRFR"),("RCRXF"),("RCRXX"),("RCRXC"),("RCRXR"),("RCRCF"),("RCRCX"),("RCRCC"),("RCRCR"),("RCRRF"),("RCRRX"),("RCRRC"),("RCRRR"),("RRFFF"),("RRFFX"),("RRFFC"),("RRFFR"),("RRFXF"),("RRFXX"),("RRFXC"),("RRFXR"),("RRFCF"),("RRFCX"),("RRFCC"),("RRFCR"),("RRFRF"),("RRFRX"),("RRFRC"),("RRFRR"),("RRXFF"),("RRXFX"),("RRXFC"),("RRXFR"),("RRXXF"),("RRXXX"),("RRXXC"),("RRXXR"),("RRXCF"),("RRXCX"),("RRXCC"),("RRXCR"),("RRXRF"),("RRXRX"),("RRXRC"),("RRXRR"),("RRCFF"),("RRCFX"),("RRCFC"),("RRCFR"),("RRCXF"),("RRCXX"),("RRCXC"),("RRCXR"),("RRCCF"),("RRCCX"),("RRCCC"),("RRCCR"),("RRCRF"),("RRCRX"),("RRCRC"),("RRCRR"),("RRRFF"),("RRRFX"),("RRRFC"),("RRRFR"),("RRRXF"),("RRRXX"),("RRRXC"),("RRRXR"),("RRRCF"),("RRRCX"),("RRRCC"),("RRRCR"),("RRRRF"),("RRRRX"),("RRRRC"),("RRRRR");

INSERT INTO positions (name) VALUES
("BTN"),("SB"),("BB"),("UTG"),("MP"),("CO");

INSERT INTO hitOdds (outs, onTurn, onRiver, onEither) VALUES
(1, 2.13, 2.17, 4.26),
(2, 4.26, 4.35, 8.42),
(3, 6.38, 6.52, 12.49),
(4, 8.51, 8.67, 16.47),
(5, 10.64, 10.87, 20.35),
(6, 12.77, 13.04, 24.14),
(7, 14.89, 15.22, 27.84),
(8, 17.02, 17.39, 31.45),
(9, 19.15, 19.57, 34.97),
(10, 21.28, 21.72, 38.39),
(11, 23.40, 23.91, 41.72),
(12, 25.53, 26.09, 44.96),
(13, 27.66, 28.26, 48.10),
(14, 29.79, 30.43, 51.16),
(15, 31.91, 32.61, 54.12),
(16, 34.04, 34.78, 56.98),
(17, 36.17, 36.96, 59.76);