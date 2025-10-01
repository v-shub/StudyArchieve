using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "academic_years",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year_label = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__academic__3213E83F1B6CA0E6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__authors__3213E83F90580668", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__3213E83F0B07FD31", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__subjects__3213E83F4FAD094E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tags__3213E83FCA339F7A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__task_typ__3213E83F1AADA2B1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83FB69E9D25", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_roles",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    condition_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    academic_year_id = table.Column<int>(type: "int", nullable: true),
                    type_id = table.Column<int>(type: "int", nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    user_added_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tasks__3213E83F35905B03", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tasks_AcademicYear",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tasks_Subject",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tasks_Type",
                        column: x => x.type_id,
                        principalTable: "task_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_tasks",
                        column: x => x.user_added_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "solutions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_id = table.Column<int>(type: "int", nullable: false),
                    solution_text = table.Column<string>(type: "text", nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    user_added_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__solution__3213E83F48212A39", x => x.id);
                    table.ForeignKey(
                        name: "FK_Solutions_Task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_solutions",
                        column: x => x.user_added_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "task_authors",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAuthors", x => new { x.task_id, x.author_id });
                    table.ForeignKey(
                        name: "FK_TaskAuthors_Author",
                        column: x => x.author_id,
                        principalTable: "authors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TaskAuthors_Task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_id = table.Column<int>(type: "int", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__task_fil__3213E83F73F69B58", x => x.id);
                    table.ForeignKey(
                        name: "FK_TaskFiles_Task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_tags",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTags", x => new { x.task_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_TaskTags_Tag",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TaskTags_Task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "solution_files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    solution_id = table.Column<int>(type: "int", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__solution__3213E83FA52D0F06", x => x.id);
                    table.ForeignKey(
                        name: "FK_SolutionFiles_Solution",
                        column: x => x.solution_id,
                        principalTable: "solutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__academic__588C243AB9AC4808",
                table: "academic_years",
                column: "year_label",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_solution_files_solution_id",
                table: "solution_files",
                column: "solution_id");

            migrationBuilder.CreateIndex(
                name: "IX_solutions_task_id",
                table: "solutions",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_solutions_user_added_id",
                table: "solutions",
                column: "user_added_id");

            migrationBuilder.CreateIndex(
                name: "UQ__subjects__72E12F1B80E3E474",
                table: "subjects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tags__72E12F1B5021F2F2",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_authors_author_id",
                table: "task_authors",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_files_task_id",
                table: "task_files",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_tags_tag_id",
                table: "task_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "UQ__task_typ__72E12F1B80BCA519",
                table: "task_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_academic_year_id",
                table: "tasks",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_subject_id",
                table: "tasks",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_type_id",
                table: "tasks",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_user_added_id",
                table: "tasks",
                column: "user_added_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "solution_files");

            migrationBuilder.DropTable(
                name: "task_authors");

            migrationBuilder.DropTable(
                name: "task_files");

            migrationBuilder.DropTable(
                name: "task_tags");

            migrationBuilder.DropTable(
                name: "solutions");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "academic_years");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "task_types");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
