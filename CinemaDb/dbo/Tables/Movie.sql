CREATE TABLE [dbo].[Movie] (
    [MovieId]     INT              IDENTITY (1, 1) NOT NULL,
    [MovieUid]    UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (256)    NOT NULL,
    [ReleaseYear] INT              NOT NULL,
    [Duration]    INT              NOT NULL,
    [Description] NVARCHAR (512)   NULL,
    [Image]       VARBINARY (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC),
    UNIQUE NONCLUSTERED ([MovieUid] ASC)
);

