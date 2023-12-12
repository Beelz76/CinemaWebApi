CREATE TABLE [dbo].[Screening] (
    [ScreeningId]      INT              IDENTITY (1, 1) NOT NULL,
    [ScreeningUid]     UNIQUEIDENTIFIER NOT NULL,
    [MovieId]          INT              NOT NULL,
    [HallId]           INT              NOT NULL,
    [ScreeningStart]   DATETIME         NOT NULL,
    [ScreeningEnd]     DATETIME         NOT NULL,
    [ScreeningPriceId] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([ScreeningId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [dbo].[Hall] ([HallId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movie] ([MovieId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ScreeningPriceId]) REFERENCES [dbo].[ScreeningPrice] ([ScreeningPriceId]) ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([ScreeningUid] ASC)
);

