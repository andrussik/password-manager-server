BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] DROP CONSTRAINT [FK_GroupUsers_GroupRoles_GroupRoleId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] DROP CONSTRAINT [FK_GroupUsers_Groups_GroupId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] DROP CONSTRAINT [FK_GroupUsers_Users_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_Users_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [Secrets] DROP CONSTRAINT [FK_Secrets_Groups_GroupId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [Secrets] DROP CONSTRAINT [FK_Secrets_Users_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    CREATE TABLE [GroupInvitations] (
        [Id] uniqueidentifier NOT NULL,
        [InvitedAt] datetime2 NOT NULL,
        [InvitedUserId] uniqueidentifier NOT NULL,
        [InvitedByUserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GroupInvitations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupInvitations_Users_InvitedByUserId] FOREIGN KEY ([InvitedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_GroupInvitations_Users_InvitedUserId] FOREIGN KEY ([InvitedUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    CREATE INDEX [IX_GroupInvitations_InvitedByUserId] ON [GroupInvitations] ([InvitedByUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    CREATE INDEX [IX_GroupInvitations_InvitedUserId] ON [GroupInvitations] ([InvitedUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] ADD CONSTRAINT [FK_GroupUsers_GroupRoles_GroupRoleId] FOREIGN KEY ([GroupRoleId]) REFERENCES [GroupRoles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] ADD CONSTRAINT [FK_GroupUsers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [GroupUsers] ADD CONSTRAINT [FK_GroupUsers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [RefreshTokens] ADD CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [Secrets] ADD CONSTRAINT [FK_Secrets_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    ALTER TABLE [Secrets] ADD CONSTRAINT [FK_Secrets_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301182508_add_GroupInvitation_table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220301182508_add_GroupInvitation_table', N'6.0.2');
END;
GO

COMMIT;
GO

