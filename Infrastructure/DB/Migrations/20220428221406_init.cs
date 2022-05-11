using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DB.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:db_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.CreateTable(
                name: "cultures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    code = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    name = table.Column<string>(type: "text", nullable: false, collation: "db_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cultures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "group_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false, collation: "db_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    key = table.Column<string>(type: "text", nullable: false, collation: "db_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    key = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    value = table.Column<string>(type: "text", nullable: false, collation: "db_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    email = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    master_password_hash = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    key = table.Column<string>(type: "text", nullable: false, collation: "db_collation")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    key = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    value = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    culture_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_resources_cultures_culture_id",
                        column: x => x.culture_id,
                        principalTable: "cultures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "group_invitations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    invited_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    invited_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invited_by_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_invitations", x => x.id);
                    table.ForeignKey(
                        name: "fk_group_invitations_users_invited_by_user_id",
                        column: x => x.invited_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_group_invitations_users_invited_user_id",
                        column: x => x.invited_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "group_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_group_users_group_roles_group_role_id",
                        column: x => x.group_role_id,
                        principalTable: "group_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_group_users_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_group_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    token = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    jwt_id = table.Column<string>(type: "text", nullable: false, collation: "db_collation"),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "secrets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false, collation: "db_collation"),
                    username = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, collation: "db_collation"),
                    password = table.Column<string>(type: "text", nullable: true, collation: "db_collation"),
                    description = table.Column<string>(type: "text", nullable: true, collation: "db_collation"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_secrets", x => x.id);
                    table.ForeignKey(
                        name: "fk_secrets_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_secrets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "cultures",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("044da860-268b-44df-b171-09e9238bcd48"), "et-EE", "Estonian (Estonia)" },
                    { new Guid("4cfb2a30-98da-48ea-b97f-6fe28ee64c91"), "en-GB", "English (United Kingdom)" }
                });

            migrationBuilder.InsertData(
                table: "group_roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0813fc0a-0719-4ea1-b99a-e46f50574e0b"), "Owner" },
                    { new Guid("7e8edd0e-be29-4fa7-aba8-3031423a4d7f"), "Writer" },
                    { new Guid("cfecfc02-da76-45eb-8eda-bde7bb03c738"), "Admin" },
                    { new Guid("e02a0e63-1474-4c68-b16f-5692c75bc347"), "Reader" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_cultures_code",
                table: "cultures",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_group_invitations_invited_by_user_id",
                table: "group_invitations",
                column: "invited_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_invitations_invited_user_id",
                table: "group_invitations",
                column: "invited_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_users_group_id",
                table: "group_users",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_users_group_role_id",
                table: "group_users",
                column: "group_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_users_user_id",
                table: "group_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_resources_culture_id",
                table: "resources",
                column: "culture_id");

            migrationBuilder.CreateIndex(
                name: "ix_resources_key_culture_id",
                table: "resources",
                columns: new[] { "key", "culture_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_secrets_group_id",
                table: "secrets",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_secrets_user_id",
                table: "secrets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_settings_key",
                table: "settings",
                column: "key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_invitations");

            migrationBuilder.DropTable(
                name: "group_users");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "secrets");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "group_roles");

            migrationBuilder.DropTable(
                name: "cultures");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
