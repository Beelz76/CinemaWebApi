CREATE TABLE [dbo].[User] (
    [UserId]   INT              IDENTITY (1, 1) NOT NULL,
    [UserUid]  UNIQUEIDENTIFIER NOT NULL,
    [FullName] NVARCHAR (256)    NOT NULL,
    [Login]    NVARCHAR (256)    NOT NULL,
    [Password] NVARCHAR (256)    NOT NULL,
    [Email]    NVARCHAR (256)    NULL,
    [IsAdmin]  BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Login] ASC),
    UNIQUE NONCLUSTERED ([UserUid] ASC)
);

