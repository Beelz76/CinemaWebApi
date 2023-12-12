CREATE TABLE [dbo].[Country] (
    [CountryId]  INT              IDENTITY (1, 1) NOT NULL,
    [CountryUid] UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (256)    NOT NULL UNIQUE,
    PRIMARY KEY CLUSTERED ([CountryId] ASC),
    UNIQUE NONCLUSTERED ([CountryUid] ASC)
);

