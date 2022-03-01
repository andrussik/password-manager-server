BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301195729_Secrets_Add_Description_Column')
BEGIN
    ALTER TABLE [Secrets] ADD [Description] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220301195729_Secrets_Add_Description_Column')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220301195729_Secrets_Add_Description_Column', N'6.0.2');
END;
GO

COMMIT;
GO

