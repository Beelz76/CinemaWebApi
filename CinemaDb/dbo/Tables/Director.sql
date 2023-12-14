CREATE TABLE [dbo].[Director] (
    [DirectorId]  INT              IDENTITY (1, 1) NOT NULL,
    [DirectorUid] UNIQUEIDENTIFIER NOT NULL,
    [FullName]    NVARCHAR (256)   NOT NULL,
    PRIMARY KEY CLUSTERED ([DirectorId] ASC),
    UNIQUE NONCLUSTERED ([DirectorUid] ASC)
);

