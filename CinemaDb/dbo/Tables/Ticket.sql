CREATE TABLE [dbo].[Ticket] (
    [TicketId]    INT              IDENTITY (1, 1) NOT NULL,
    [TicketUid]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]      INT              NOT NULL,
    [ScreeningId] INT              NOT NULL,
    [SeatId]      INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([TicketId] ASC),
    FOREIGN KEY ([ScreeningId]) REFERENCES [dbo].[Screening] ([ScreeningId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([SeatId]) REFERENCES [dbo].[Seat] ([SeatId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE ON UPDATE CASCADE
);

