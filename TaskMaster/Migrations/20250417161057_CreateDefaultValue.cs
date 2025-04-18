using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMaster.Migrations
{
    /// <inheritdoc />
    public partial class CreateDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Etiquettes",
                table: "Tasks",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id_User", "Email", "Nom", "Password", "Prenom" },
                values: new object[] { 1, "admin@taskmaster.com", "Admin", "admin123", "Administrateur" });

            migrationBuilder.InsertData(
                table: "Projets",
                columns: new[] { "Id_Projet", "DateCreation", "Description", "Id_Createur", "Nom" },
                values: new object[] { 1, new DateTime(2025, 4, 17, 18, 10, 56, 500, DateTimeKind.Local).AddTicks(8261), "Projet par défaut pour les tâches non assignées", 1, "Projet par défaut" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id_Task", "Categorie", "DateCreation", "Description", "Echeance", "Etiquettes", "Id_Auteur", "Id_Projet", "Id_Realisateur", "Priorite", "Statut", "Titre" },
                values: new object[] { 1, "Travail", new DateTime(2025, 4, 17, 18, 10, 56, 507, DateTimeKind.Local).AddTicks(1719), "Ceci est une tâche exemple", null, null, 1, 1, 1, "Moyenne", "Afaire", "Tâche exemple" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id_Task",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projets",
                keyColumn: "Id_Projet",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id_User",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Etiquettes",
                keyValue: null,
                column: "Etiquettes",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Etiquettes",
                table: "Tasks",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
