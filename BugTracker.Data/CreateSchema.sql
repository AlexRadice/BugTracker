if not exists (select * from sys.databases where name = 'BugTracker')
CREATE DATABASE BugTracker
GO

USE BugTracker
GO

if not exists (select * from sys.tables where name = 'Users')
CREATE TABLE dbo.Users
(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(50) NOT NULL
)
GO

if not exists (select * from sys.tables where name = 'Bugs')
CREATE TABLE dbo.Bugs
(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Title nvarchar(100) NOT NULL,
	Description nvarchar(4000) NULL,
	DateOpened datetime NOT NULL,
	DateClosed datetime NULL,
	AssignedToUser int NULL
)
GO

if not exists (select * from sys.foreign_keys where name = 'FK_Bugs_Users_Id')
ALTER TABLE dbo.Bugs ADD CONSTRAINT FK_Bugs_Users_Id FOREIGN KEY (AssignedToUser) REFERENCES dbo.Users (Id)
GO