BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223205904_Secret_Add_Name_and_UserName_Columns')
BEGIN
    ALTER TABLE [Secrets] ADD [Name] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223205904_Secret_Add_Name_and_UserName_Columns')
BEGIN
    ALTER TABLE [Secrets] ADD [UserName] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220223205904_Secret_Add_Name_and_UserName_Columns')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220223205904_Secret_Add_Name_and_UserName_Columns', N'6.0.2');
END;
GO

COMMIT;
GO

