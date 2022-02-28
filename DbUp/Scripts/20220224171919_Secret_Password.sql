BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220224171919_Secret_Password')
BEGIN
    EXEC sp_rename N'[Secrets].[EncryptedPassword]', N'Password', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220224171919_Secret_Password')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220224171919_Secret_Password', N'6.0.2');
END;
GO

COMMIT;
GO

