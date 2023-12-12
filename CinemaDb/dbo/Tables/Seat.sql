CREATE TABLE [dbo].[Seat] (
    [SeatId]  INT              IDENTITY (1, 1) NOT NULL,
    [SeatUid] UNIQUEIDENTIFIER NOT NULL,
    [HallId]  INT              NOT NULL,
    [Row]     INT              NOT NULL,
    [Number]  INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([SeatId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [dbo].[Hall] ([HallId]) ON DELETE CASCADE ON UPDATE CASCADE
);

