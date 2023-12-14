CREATE TABLE [dbo].[ScreeningPrice] (
    [ScreeningPriceId]  INT              IDENTITY (1, 1) NOT NULL,
    [ScreeningPriceUid] UNIQUEIDENTIFIER NOT NULL,
    [Price]             INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([ScreeningPriceId] ASC),
    UNIQUE NONCLUSTERED ([Price] ASC),
    UNIQUE NONCLUSTERED ([ScreeningPriceUid] ASC)
);

