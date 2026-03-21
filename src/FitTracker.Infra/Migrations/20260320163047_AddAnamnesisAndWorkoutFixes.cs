using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddAnamnesisAndWorkoutFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Workouts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "WorkoutExecutions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "WorkoutExecutions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AnamnesisFormId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Exercises",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<int>(
                name: "RestPeriod",
                table: "Exercises",
                type: "integer",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.CreateTable(
                name: "AnamnesisForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    SchemaJson = table.Column<string>(type: "text", nullable: false),
                    PersonalId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamnesisForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutExecutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeightUsed = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RepsDone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseExecutions_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExerciseExecutions_WorkoutExecutions_WorkoutExecutionId",
                        column: x => x.WorkoutExecutionId,
                        principalTable: "WorkoutExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnamnesisResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnamnesisFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponsesJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamnesisResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamnesisResponses_AnamnesisForms_AnamnesisFormId",
                        column: x => x.AnamnesisFormId,
                        principalTable: "AnamnesisForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_StudentId",
                table: "Workouts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AnamnesisResponses_AnamnesisFormId",
                table: "AnamnesisResponses",
                column: "AnamnesisFormId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecutions_ExerciseId",
                table: "ExerciseExecutions",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExecutions_WorkoutExecutionId",
                table: "ExerciseExecutions",
                column: "WorkoutExecutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_StudentId",
                table: "Workouts",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_StudentId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "AnamnesisResponses");

            migrationBuilder.DropTable(
                name: "ExerciseExecutions");

            migrationBuilder.DropTable(
                name: "AnamnesisForms");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_StudentId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "WorkoutExecutions");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "WorkoutExecutions");

            migrationBuilder.DropColumn(
                name: "AnamnesisFormId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RestPeriod",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Exercises",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);
        }
    }
}
