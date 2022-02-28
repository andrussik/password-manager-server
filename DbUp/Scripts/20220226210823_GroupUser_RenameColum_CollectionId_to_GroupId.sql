BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId')
BEGIN
    ALTER TABLE [GroupUsers] DROP CONSTRAINT [FK_GroupUsers_Groups_CollectionId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId')
BEGIN
    EXEC sp_rename N'[GroupUsers].[CollectionId]', N'GroupId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId')
BEGIN
    EXEC sp_rename N'[GroupUsers].[IX_GroupUsers_CollectionId]', N'IX_GroupUsers_GroupId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId')
BEGIN
    ALTER TABLE [GroupUsers] ADD CONSTRAINT [FK_GroupUsers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220226210823_GroupUser_RenameColum_CollectionId_to_GroupId', N'6.0.2');
END;
GO

COMMIT;
GO

