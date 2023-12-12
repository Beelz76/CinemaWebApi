CREATE TABLE [dbo].[MovieDirector] (
    [MovieId]    INT NOT NULL,
    [DirectorId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC, [DirectorId] ASC),
    FOREIGN KEY ([DirectorId]) REFERENCES [dbo].[Director] ([DirectorId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movie] ([MovieId]) ON DELETE CASCADE ON UPDATE CASCADE
);

