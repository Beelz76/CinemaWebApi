CREATE TABLE [dbo].[Hall] (
    [HallId]   INT              IDENTITY (1, 1) NOT NULL,
    [HallUid]  UNIQUEIDENTIFIER NOT NULL,
    [Name]     NVARCHAR (256)   NOT NULL,
    [Capacity] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([HallId] ASC),
    UNIQUE NONCLUSTERED ([HallUid] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

