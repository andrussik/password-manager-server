BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220224164610_Secret_EncryptedPassword')
BEGIN
    EXEC sp_rename N'[Secrets].[PasswordHash]', N'EncryptedPassword', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220224164610_Secret_EncryptedPassword')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220224164610_Secret_EncryptedPassword', N'6.0.2');
END;
GO

COMMIT;
GO

