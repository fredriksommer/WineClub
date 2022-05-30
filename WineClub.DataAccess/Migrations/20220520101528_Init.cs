using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WineClub.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grapes",
                columns: table => new
                {
                    GrapeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrapeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grapes", x => x.GrapeId);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "WineEvents",
                columns: table => new
                {
                    WineEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxPersons = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineEvents", x => x.WineEventId);
                });

            migrationBuilder.CreateTable(
                name: "Wineries",
                columns: table => new
                {
                    WineryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wineries", x => x.WineryId);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedByUserId = table.Column<int>(type: "int", nullable: true),
                    DateOfRating = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_RatedByUserId",
                        column: x => x.RatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWineEvent",
                columns: table => new
                {
                    AttendeesUserId = table.Column<int>(type: "int", nullable: false),
                    WineEventsWineEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWineEvent", x => new { x.AttendeesUserId, x.WineEventsWineEventId });
                    table.ForeignKey(
                        name: "FK_UserWineEvent_Users_AttendeesUserId",
                        column: x => x.AttendeesUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWineEvent_WineEvents_WineEventsWineEventId",
                        column: x => x.WineEventsWineEventId,
                        principalTable: "WineEvents",
                        principalColumn: "WineEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    WineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WineryId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AlcoholContent = table.Column<double>(type: "float", nullable: false),
                    WineType = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AddedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.WineId);
                    table.ForeignKey(
                        name: "FK_Wines_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wines_Wineries_WineryId",
                        column: x => x.WineryId,
                        principalTable: "Wineries",
                        principalColumn: "WineryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrapeWine",
                columns: table => new
                {
                    GrapesGrapeId = table.Column<int>(type: "int", nullable: false),
                    WinesWineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrapeWine", x => new { x.GrapesGrapeId, x.WinesWineId });
                    table.ForeignKey(
                        name: "FK_GrapeWine_Grapes_GrapesGrapeId",
                        column: x => x.GrapesGrapeId,
                        principalTable: "Grapes",
                        principalColumn: "GrapeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrapeWine_Wines_WinesWineId",
                        column: x => x.WinesWineId,
                        principalTable: "Wines",
                        principalColumn: "WineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingWine",
                columns: table => new
                {
                    RatingsRatingId = table.Column<int>(type: "int", nullable: false),
                    WinesWineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingWine", x => new { x.RatingsRatingId, x.WinesWineId });
                    table.ForeignKey(
                        name: "FK_RatingWine_Ratings_RatingsRatingId",
                        column: x => x.RatingsRatingId,
                        principalTable: "Ratings",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingWine_Wines_WinesWineId",
                        column: x => x.WinesWineId,
                        principalTable: "Wines",
                        principalColumn: "WineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionWine",
                columns: table => new
                {
                    RegionsRegionId = table.Column<int>(type: "int", nullable: false),
                    WinesWineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionWine", x => new { x.RegionsRegionId, x.WinesWineId });
                    table.ForeignKey(
                        name: "FK_RegionWine_Regions_RegionsRegionId",
                        column: x => x.RegionsRegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionWine_Wines_WinesWineId",
                        column: x => x.WinesWineId,
                        principalTable: "Wines",
                        principalColumn: "WineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WineWineEvent",
                columns: table => new
                {
                    EventsWineEventId = table.Column<int>(type: "int", nullable: false),
                    WinesWineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineWineEvent", x => new { x.EventsWineEventId, x.WinesWineId });
                    table.ForeignKey(
                        name: "FK_WineWineEvent_WineEvents_EventsWineEventId",
                        column: x => x.EventsWineEventId,
                        principalTable: "WineEvents",
                        principalColumn: "WineEventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineWineEvent_Wines_WinesWineId",
                        column: x => x.WinesWineId,
                        principalTable: "Wines",
                        principalColumn: "WineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrapeWine_WinesWineId",
                table: "GrapeWine",
                column: "WinesWineId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatedByUserId",
                table: "Ratings",
                column: "RatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingWine_WinesWineId",
                table: "RatingWine",
                column: "WinesWineId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionWine_WinesWineId",
                table: "RegionWine",
                column: "WinesWineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWineEvent_WineEventsWineEventId",
                table: "UserWineEvent",
                column: "WineEventsWineEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_AddedByUserId",
                table: "Wines",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_WineryId",
                table: "Wines",
                column: "WineryId");

            migrationBuilder.CreateIndex(
                name: "IX_WineWineEvent_WinesWineId",
                table: "WineWineEvent",
                column: "WinesWineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrapeWine");

            migrationBuilder.DropTable(
                name: "RatingWine");

            migrationBuilder.DropTable(
                name: "RegionWine");

            migrationBuilder.DropTable(
                name: "UserWineEvent");

            migrationBuilder.DropTable(
                name: "WineWineEvent");

            migrationBuilder.DropTable(
                name: "Grapes");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "WineEvents");

            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wineries");
        }
    }
}
