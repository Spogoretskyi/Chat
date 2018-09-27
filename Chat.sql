 USE master

 IF DB_ID ('Chat') IS NOT NULL
 DROP DATABASE Chat;
 CREATE DATABASE Chat

GO

USE Chat

GO

IF OBJECT_ID ('Users', 'U') IS NOT NULL
DROP TABLE Users;
CREATE TABLE Users
(
USER_id int IDENTITY(1,1) PRIMARY KEY, 
Name nvarchar(20), 
Phone nvarchar(12),
Email nvarchar(256)
)

INSERT INTO Catalog(USER_id, Name, Phone, Email)
VALUES
(1, 'NotExists', '000000000', 'notexists@chat.net'),

IF OBJECT_ID ('Account', 'U') IS NOT NULL
DROP TABLE Account;
CREATE TABLE Account
(
Account_Id INT IDENTITY(1,1) PRIMARY KEY,
User_Id INT FOREIGN KEY REFERENCES Users(User_Id),
Password nvarchar(80),
registration int
)

IF OBJECT_ID ('Log_messaging', 'U') IS NOT NULL
DROP TABLE Log_messaging;
CREATE TABLE Log_messaging
(
Message_Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
Sender INT FOREIGN KEY REFERENCES Users(User_Id),
message nvarchar(MAX),
file_name nvarchar(256),
reciever nvarchar(30),
Time datetime
)

IF OBJECT_ID ('Log_Connect', 'U') IS NOT NULL
DROP TABLE Log_Connect;
CREATE TABLE Log_Connect
(
Log_c_Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
User_id INT FOREIGN KEY REFERENCES Users(User_Id),
Action INT,
Time datetime
)

IF OBJECT_ID ('Log_Authentication', 'U') IS NOT NULL
DROP TABLE Log_Authentication;
CREATE TABLE Log_Authentication
(
Log_a_Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
User_id INT FOREIGN KEY REFERENCES Users(User_Id),
Action INT,
Time datetime
)

IF OBJECT_ID ('Catalog', 'U') IS NOT NULL
DROP TABLE Catalog;
CREATE TABLE Catalog
(
Action_Id INT,
Action NVARCHAR(256) 
)

INSERT INTO Catalog(Action_Id, Action)
VALUES
(1, 'connect'),
(2, 'disconnect'),
(3, 'new user has registered'),
(4, 'code verification'),
(5, 'send email'),
(6, 'code verified'),
(7, 'non-existent user'),
(8, 'user logged already'),
(9, 'incorrect password');