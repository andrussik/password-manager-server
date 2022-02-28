IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE TABLE [GroupRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_GroupRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE TABLE [Groups] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Key] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Groups] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [MasterPasswordHash] nvarchar(max) NOT NULL,
        [Key] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE TABLE [GroupUsers] (
        [Id] uniqueidentifier NOT NULL,
        [CollectionId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [GroupRoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GroupUsers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GroupUsers_GroupRoles_GroupRoleId] FOREIGN KEY ([GroupRoleId]) REFERENCES [GroupRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_GroupUsers_Groups_CollectionId] FOREIGN KEY ([CollectionId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_GroupUsers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE TABLE [Secrets] (
        [Id] uniqueidentifier NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [UserId] uniqueidentifier NULL,
        [GroupId] uniqueidentifier NULL,
        CONSTRAINT [PK_Secrets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Secrets_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]),
        CONSTRAINT [FK_Secrets_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[GroupRoles]'))
        SET IDENTITY_INSERT [GroupRoles] ON;
    EXEC(N'INSERT INTO [GroupRoles] ([Id], [Name])
    VALUES (''0813fc0a-0719-4ea1-b99a-e46f50574e0b'', N''Owner''),
    (''7e8edd0e-be29-4fa7-aba8-3031423a4d7f'', N''Writer''),
    (''cfecfc02-da76-45eb-8eda-bde7bb03c738'', N''Admin''),
    (''e02a0e63-1474-4c68-b16f-5692c75bc347'', N''Reader'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[GroupRoles]'))
        SET IDENTITY_INSERT [GroupRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE INDEX [IX_GroupUsers_CollectionId] ON [GroupUsers] ([CollectionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE INDEX [IX_GroupUsers_GroupRoleId] ON [GroupUsers] ([GroupRoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE INDEX [IX_GroupUsers_UserId] ON [GroupUsers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE INDEX [IX_Secrets_GroupId] ON [Secrets] ([GroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    CREATE INDEX [IX_Secrets_UserId] ON [Secrets] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220220182802_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220220182802_init', N'6.0.2');
END;
GO

COMMIT;
GO

