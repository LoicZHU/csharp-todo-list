USE master;
GO
ALTER DATABASE TodoListDB
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE TodoListDB;
GO