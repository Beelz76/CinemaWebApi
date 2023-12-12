CREATE TABLE [dbo].[MovieCountry] (
    [MovieId]   INT NOT NULL,
    [CountryId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC, [CountryId] ASC),
    FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movie] ([MovieId]) ON DELETE CASCADE ON UPDATE CASCADE
);

