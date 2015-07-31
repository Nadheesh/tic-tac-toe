
1. Create a database called tic_tac_toe

	CREATE DATABASE tic_tac_toe;
	
	CREATE TABLE score (
		user_name varchar(32) NOT NULL,
  		won int(11) DEFAULT '0',
		lost int(11) DEFAULT '0',
		draw int(11) DEFAULT '0',
		PRIMARY KEY (user_name),
		UNIQUE KEY user_name (user_name)
	) ;
	
	INSERT INTO score VALUES ('abc',0,0,0),('jnj',0,0,0),('mithwick93',0,0,0),('waas',0,0,0),('Y2K',0,0,0),('ySenerath',0,0,0);

2. create a new mySQL user :
	CREATE USER 'admin'@'localhost' IDENTIFIED BY 'password';

3. Grant all permissions to 'admin' user
	GRANT ALL PRIVILEGES ON tic_tac_toe.score TO 'admin'@'localhost' WITH GRANT OPTION;