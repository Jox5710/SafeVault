-- init_db.sql
-- Create Users table for SafeVault (LocalDB)
CREATE TABLE IF NOT EXISTS Users (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Username NVARCHAR(100) NOT NULL UNIQUE,
  PasswordHash NVARCHAR(200) NOT NULL,
  Role NVARCHAR(20) NOT NULL
);
