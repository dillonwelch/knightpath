CREATE SCHEMA dbo;

CREATE TABLE knightpathazdb.dbo.Paths (
	PathId int IDENTITY(1,1) NOT NULL,
	TrackingId uniqueidentifier NOT NULL,
	ShortestPath nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NumberOfMoves int NOT NULL,
	SourcePosition nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	TargetPosition nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Paths PRIMARY KEY (PathId)
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Paths_TrackingId ON dbo.Paths (  TrackingId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;