using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo_list.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table =>
					new
					{
						CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.CategoryId);
				}
			);

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table =>
					new
					{
						RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Name = table.Column<int>(type: "int", maxLength: 100, nullable: false),
						Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
						NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
						ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.RoleId);
				}
			);

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table =>
					new
					{
						UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
						Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
						CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
						RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserId);
					table.ForeignKey(
						name: "FK_Users_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "RoleId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "Avatars",
				columns: table =>
					new
					{
						AvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
						link = table.Column<string>(type: "nvarchar(max)", nullable: false),
						CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Avatars", x => x.AvatarId);
					table.ForeignKey(
						name: "FK_Avatars_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "Todos",
				columns: table =>
					new
					{
						TodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
						Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
						Status = table.Column<int>(type: "int", nullable: false),
						CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
						UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Todos", x => x.TodoId);
					table.ForeignKey(
						name: "FK_Todos_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateTable(
				name: "Statistics",
				columns: table =>
					new
					{
						StatisticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						TodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						CategoryTimesUsed = table.Column<int>(type: "int", nullable: false)
					},
				constraints: table =>
				{
					table.PrimaryKey("PK_Statistics", x => x.StatisticId);
					table.ForeignKey(
						name: "FK_Statistics_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "CategoryId",
						onDelete: ReferentialAction.Cascade
					);
					table.ForeignKey(
						name: "FK_Statistics_Todos_TodoId",
						column: x => x.TodoId,
						principalTable: "Todos",
						principalColumn: "TodoId",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateIndex(name: "IX_Avatars_UserId", table: "Avatars", column: "UserId");

			migrationBuilder.CreateIndex(name: "IX_Statistics_CategoryId", table: "Statistics", column: "CategoryId");

			migrationBuilder.CreateIndex(name: "IX_Statistics_TodoId", table: "Statistics", column: "TodoId");

			migrationBuilder.CreateIndex(name: "IX_Todos_UserId", table: "Todos", column: "UserId");

			migrationBuilder.CreateIndex(name: "IX_Users_RoleId", table: "Users", column: "RoleId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "Avatars");

			migrationBuilder.DropTable(name: "Statistics");

			migrationBuilder.DropTable(name: "Categories");

			migrationBuilder.DropTable(name: "Todos");

			migrationBuilder.DropTable(name: "Users");

			migrationBuilder.DropTable(name: "Roles");
		}
	}
}
