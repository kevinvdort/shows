USE [RTL]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tv_maze_show_index] (
	[id]				int IDENTITY(1,1)	NOT NULL,
	[timestamp]			datetime			NOT NULL,
	[duration]			bigint				NOT NULL,
	[in_progress]		bit					NOT NULL,
	[records_created]	int					NOT NULL,
	[records_updated]	int					NOT NULL,
	CONSTRAINT PK_TV_MAZE_SHOW_INDEX PRIMARY KEY NONCLUSTERED ([id])
) ON [PRIMARY] 
GO

CREATE TABLE [dbo].[tv_maze_show](
	[id]			int IDENTITY(1,1)	NOT NULL,
	[tv_maze_id]		int					NOT NULL UNIQUE,
	[name]			nvarchar(255)		NOT NULL,
	CONSTRAINT PK_TV_MAZE_SHOW PRIMARY KEY NONCLUSTERED ([id])
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tv_maze_person](
	[id]		int IDENTITY(1,1)	NOT NULL,
	[tv_maze_id]	int					NOT NULL UNIQUE ,
	[name]		nvarchar(255)		NOT NULL,
	[birthday]	datetime			NULL,
	CONSTRAINT PK_TV_MAZE_PERSON PRIMARY KEY ([id], [tv_maze_id])
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tv_maze_show_cast](
	[id]				int IDENTITY(1,1)	NOT NULL,
	[tv_maze_show_id]	int					NOT NULL,
	[tv_maze_person_id]	int					NOT NULL,
	CONSTRAINT PK_TV_MAZE_SHOW_CAST					PRIMARY KEY NONCLUSTERED ([id]),
	CONSTRAINT FK_TV_MAZE_SHOW_CAST_TVMAZE_PEOPLE	FOREIGN KEY ([tv_maze_person_id])	REFERENCES [dbo].[tv_maze_person] ([tv_maze_id]),
	CONSTRAINT FK_TV_MAZE_SHOW_CAST_TVMAZE_SHOW		FOREIGN KEY ([tv_maze_show_id])		REFERENCES [dbo].[tv_maze_show] ([tv_maze_id])
) ON [PRIMARY]
GO