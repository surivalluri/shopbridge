ALTER TABLE dbo.Product
(
	[Id]			NVARCHAR(64)	NOT NULL PRIMARY KEY,
	[Name]			NVARCHAR(64)	NOT NULL,
	[Description]	NVARCHAR(256)		NULL,
	[Price]			DECIMAL			NOT NULL,
	[Product]		NVARCHAR(MAX)	NOT NULL,
	[RecordStatus]  TINYINT			NOT NULL
)

