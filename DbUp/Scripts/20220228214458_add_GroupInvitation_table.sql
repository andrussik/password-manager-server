BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220228214458_add_GroupInvitation_table')
BEGIN
    CREATE TABLE [GroupInvitations] (
        [Id] uniqueidentifier NOT NULL,
        [InvitedAt] datetime2 NOT NULL,
        [InvitedUserId] uniqueidentifier NOT NULL,
        [InvitedByUserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GroupInvitations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupInvitations_Users_InvitedByUserId] FOREIGN KEY ([InvitedByUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_GroupInvitations_Users_InvitedUserId] FOREIGN KEY ([InvitedUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220228214458_add_GroupInvitation_table')
BEGIN
    CREATE INDEX [IX_GroupInvitations_InvitedByUserId] ON [GroupInvitations] ([InvitedByUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220228214458_add_GroupInvitation_table')
BEGIN
    CREATE INDEX [IX_GroupInvitations_InvitedUserId] ON [GroupInvitations] ([InvitedUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220228214458_add_GroupInvitation_table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220228214458_add_GroupInvitation_table', N'6.0.2');
END;
GO

COMMIT;
GO

