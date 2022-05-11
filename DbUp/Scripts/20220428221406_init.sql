CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE COLLATION db_collation (LC_COLLATE = 'en-u-ks-primary',
        LC_CTYPE = 'en-u-ks-primary',
        PROVIDER = icu,
        DETERMINISTIC = False
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE cultures (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        code text COLLATE db_collation NOT NULL,
        name text COLLATE db_collation NOT NULL,
        CONSTRAINT pk_cultures PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE group_roles (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name text COLLATE db_collation NOT NULL,
        CONSTRAINT pk_group_roles PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE groups (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name text COLLATE db_collation NOT NULL,
        key text COLLATE db_collation NOT NULL,
        CONSTRAINT pk_groups PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE settings (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        key text COLLATE db_collation NOT NULL,
        value text COLLATE db_collation NOT NULL,
        CONSTRAINT pk_settings PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE users (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name text COLLATE db_collation NOT NULL,
        email text COLLATE db_collation NOT NULL,
        master_password_hash text COLLATE db_collation NOT NULL,
        key text COLLATE db_collation NOT NULL,
        CONSTRAINT pk_users PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE resources (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        key text COLLATE db_collation NOT NULL,
        value text COLLATE db_collation NOT NULL,
        culture_id uuid NOT NULL,
        CONSTRAINT pk_resources PRIMARY KEY (id),
        CONSTRAINT fk_resources_cultures_culture_id FOREIGN KEY (culture_id) REFERENCES cultures (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE group_invitations (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        invited_at timestamp with time zone NOT NULL,
        invited_user_id uuid NOT NULL,
        invited_by_user_id uuid NOT NULL,
        CONSTRAINT pk_group_invitations PRIMARY KEY (id),
        CONSTRAINT fk_group_invitations_users_invited_by_user_id FOREIGN KEY (invited_by_user_id) REFERENCES users (id) ON DELETE RESTRICT,
        CONSTRAINT fk_group_invitations_users_invited_user_id FOREIGN KEY (invited_user_id) REFERENCES users (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE group_users (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        group_id uuid NOT NULL,
        user_id uuid NOT NULL,
        group_role_id uuid NOT NULL,
        CONSTRAINT pk_group_users PRIMARY KEY (id),
        CONSTRAINT fk_group_users_group_roles_group_role_id FOREIGN KEY (group_role_id) REFERENCES group_roles (id) ON DELETE RESTRICT,
        CONSTRAINT fk_group_users_groups_group_id FOREIGN KEY (group_id) REFERENCES groups (id) ON DELETE RESTRICT,
        CONSTRAINT fk_group_users_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE refresh_tokens (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        token text COLLATE db_collation NOT NULL,
        jwt_id text COLLATE db_collation NOT NULL,
        is_used boolean NOT NULL,
        is_revoked boolean NOT NULL,
        added_at timestamp with time zone NOT NULL,
        expires_at timestamp with time zone NOT NULL,
        user_id uuid NOT NULL,
        CONSTRAINT pk_refresh_tokens PRIMARY KEY (id),
        CONSTRAINT fk_refresh_tokens_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE TABLE secrets (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        name character varying(1024) COLLATE db_collation NOT NULL,
        username character varying(1024) COLLATE db_collation NULL,
        password text COLLATE db_collation NULL,
        description text COLLATE db_collation NULL,
        user_id uuid NULL,
        group_id uuid NULL,
        CONSTRAINT pk_secrets PRIMARY KEY (id),
        CONSTRAINT fk_secrets_groups_group_id FOREIGN KEY (group_id) REFERENCES groups (id) ON DELETE RESTRICT,
        CONSTRAINT fk_secrets_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    INSERT INTO cultures (id, code, name)
    VALUES ('044da860-268b-44df-b171-09e9238bcd48', 'et-EE', 'Estonian (Estonia)');
    INSERT INTO cultures (id, code, name)
    VALUES ('4cfb2a30-98da-48ea-b97f-6fe28ee64c91', 'en-GB', 'English (United Kingdom)');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    INSERT INTO group_roles (id, name)
    VALUES ('0813fc0a-0719-4ea1-b99a-e46f50574e0b', 'Owner');
    INSERT INTO group_roles (id, name)
    VALUES ('7e8edd0e-be29-4fa7-aba8-3031423a4d7f', 'Writer');
    INSERT INTO group_roles (id, name)
    VALUES ('cfecfc02-da76-45eb-8eda-bde7bb03c738', 'Admin');
    INSERT INTO group_roles (id, name)
    VALUES ('e02a0e63-1474-4c68-b16f-5692c75bc347', 'Reader');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE UNIQUE INDEX ix_cultures_code ON cultures (code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_group_invitations_invited_by_user_id ON group_invitations (invited_by_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_group_invitations_invited_user_id ON group_invitations (invited_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_group_users_group_id ON group_users (group_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_group_users_group_role_id ON group_users (group_role_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_group_users_user_id ON group_users (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE UNIQUE INDEX ix_refresh_tokens_token ON refresh_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_refresh_tokens_user_id ON refresh_tokens (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_resources_culture_id ON resources (culture_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE UNIQUE INDEX ix_resources_key_culture_id ON resources (key, culture_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_secrets_group_id ON secrets (group_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE INDEX ix_secrets_user_id ON secrets (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    CREATE UNIQUE INDEX ix_settings_key ON settings (key);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20220428221406_init') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20220428221406_init', '6.0.4');
    END IF;
END $EF$;
COMMIT;

