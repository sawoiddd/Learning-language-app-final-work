using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningLanguageApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DictionaryID",
                table: "WORDS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DICTIONARIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SourceLanguage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DICTIONARIES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DICTIONARIES_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WORDS_DictionaryID",
                table: "WORDS",
                column: "DictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_DICTIONARIES_UserId",
                table: "DICTIONARIES",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WORDS_DICTIONARIES_DictionaryID",
                table: "WORDS",
                column: "DictionaryID",
                principalTable: "DICTIONARIES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WORDS_DICTIONARIES_DictionaryID",
                table: "WORDS");

            migrationBuilder.DropTable(
                name: "DICTIONARIES");

            migrationBuilder.DropIndex(
                name: "IX_WORDS_DictionaryID",
                table: "WORDS");

            migrationBuilder.DropColumn(
                name: "DictionaryID",
                table: "WORDS");
        }
    }
}
