BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220221003526_add_refresh_tokens_table')
BEGIN
    CREATE TABLE [RefreshTokens] (
        [Id] uniqueidentifier NOT NULL,
        [Token] nvarchar(max) NOT NULL,
        [JwtId] nvarchar(max) NOT NULL,
        [IsUsed] bit NOT NULL,
        [IsRevoked] bit NOT NULL,
        [AddedAt] datetime2 NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220221003526_add_refresh_tokens_table')
BEGIN
    CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220221003526_add_refresh_tokens_table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220221003526_add_refresh_tokens_table', N'6.0.2');
END;
GO

COMMIT;
GO

