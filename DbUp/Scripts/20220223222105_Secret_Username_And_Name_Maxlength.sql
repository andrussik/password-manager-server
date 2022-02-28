BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223222105_Secret_Username_And_Name_Maxlength')
BEGIN
    EXEC sp_rename N'[Secrets].[UserName]', N'Username', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223222105_Secret_Username_And_Name_Maxlength')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Secrets]') AND [c].[name] = N'Username');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Secrets] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Secrets] ALTER COLUMN [Username] nvarchar(1024) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223222105_Secret_Username_And_Name_Maxlength')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Secrets]') AND [c].[name] = N'Name');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Secrets] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Secrets] ALTER COLUMN [Name] nvarchar(1024) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223222105_Secret_Username_And_Name_Maxlength')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220223222105_Secret_Username_And_Name_Maxlength', N'6.0.2');
END;
GO

COMMIT;
GO

