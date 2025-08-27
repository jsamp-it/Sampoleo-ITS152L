CREATE TABLE [dbo].[Posts] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [Title] NVARCHAR(100) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Posts_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

