CREATE TYPE [dbo].[tbl_Product] AS TABLE
(
	[Id]			NVARCHAR(64)	NOT NULL,
	[Name]			NVARCHAR(64)	NOT NULL,
	[Description]	NVARCHAR(256)	NULL,
	[Price]			DECIMAL			NOT NULL
)

