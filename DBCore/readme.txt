-- Setting up mysql database --
-- 1. Create a database called tic_tac_toe

	DROP DATABASE IF EXISTS tic_tac_toe;
	CREATE DATABASE tic_tac_toe;
	USE tic_tac_toe;

-- 2. Create table score
	
	CREATE TABLE score (
		user_name varchar(32) NOT NULL UNIQUE,
  		won int(11) DEFAULT '0',
		lost int(11) DEFAULT '0',
		draw int(11) DEFAULT '0',
		PRIMARY KEY (user_name)
	) ;
	
-- 3. create a new mySQL user :
	CREATE USER 'admin'@'localhost' IDENTIFIED BY 'password';

-- 4. Grant all permissions to 'admin' user
	GRANT ALL PRIVILEGES ON tic_tac_toe.score TO 'admin'@'localhost' WITH GRANT OPTION;