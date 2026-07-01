-- Sample Employees table for Exercise 30
CREATE DATABASE IF NOT EXISTS TestDB;
USE TestDB;

IF OBJECT_ID('dbo.Employees', 'U') IS NULL
CREATE TABLE dbo.Employees (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(100) NOT NULL,
  Age INT NOT NULL
);

INSERT INTO dbo.Employees (Name, Age) VALUES ('Alice', 28);
INSERT INTO dbo.Employees (Name, Age) VALUES ('Bob', 35);
