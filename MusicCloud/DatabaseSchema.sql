IF DB_ID('MusicCloud') IS NOT NULL BEGIN
	DROP DATABASE [MusicCloud]
END

CREATE DATABASE [MusicCloud] ON
PRIMARY (
	NAME = [MusicCloud],
	FILENAME = 'M:\Databases\MusicCloud\MusicCloud.mdf'
), 
FILEGROUP [MusicCloud_fs] CONTAINS FILESTREAM (
	NAME = [MusicCloud_fs],
	FILENAME = 'M:\Databases\MusicCloud\MusicCloud_fs')
LOG ON (
	NAME = [MusicCloud_log],
	FILENAME = 'M:\Databases\MusicCloud\MusicCloud.ldf')
GO

USE [MusicCloud]
GO

CREATE TABLE [dbo].[Sound] (
	[Id] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL UNIQUE,
	[Title] NVARCHAR(100) NOT NULL,
	[Slug] NVARCHAR(100) NOT NULL UNIQUE,
	[Audio] VARBINARY(MAX) FILESTREAM NULL
)
GO