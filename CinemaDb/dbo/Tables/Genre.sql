CREATE TABLE [dbo].[Genre] (
    [GenreId]  INT              IDENTITY (1, 1) NOT NULL,
    [GenreUid] UNIQUEIDENTIFIER NOT NULL,
    [Name]     NVARCHAR (256)   NOT NULL,
    PRIMARY KEY CLUSTERED ([GenreId] ASC),
    UNIQUE NONCLUSTERED ([GenreUid] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

