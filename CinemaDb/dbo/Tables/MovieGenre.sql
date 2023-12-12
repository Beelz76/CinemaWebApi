CREATE TABLE [dbo].[MovieGenre] (
    [MovieId] INT NOT NULL,
    [GenreId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC, [GenreId] ASC),
    FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([GenreId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movie] ([MovieId]) ON DELETE CASCADE ON UPDATE CASCADE
);

