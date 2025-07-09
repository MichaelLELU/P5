using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OC_p5_Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class manytomanycarreparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reparations_Cars_CarId",
                table: "Reparations");

            migrationBuilder.DropIndex(
                name: "IX_Reparations_CarId",
                table: "Reparations");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Reparations");

            migrationBuilder.RenameColumn(
                name: "IdBrand",
                table: "Models",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "IdModel",
                table: "Finishings",
                newName: "ModelId");

            migrationBuilder.CreateTable(
                name: "CarReparation",
                columns: table => new
                {
                    CarsId = table.Column<int>(type: "int", nullable: false),
                    ReparationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarReparation", x => new { x.CarsId, x.ReparationsId });
                    table.ForeignKey(
                        name: "FK_CarReparation_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarReparation_Reparations_ReparationsId",
                        column: x => x.ReparationsId,
                        principalTable: "Reparations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Models_BrandId",
                table: "Models",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Finishings_ModelId",
                table: "Finishings",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarReparation_ReparationsId",
                table: "CarReparation",
                column: "ReparationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Finishings_Models_ModelId",
                table: "Finishings",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Brands_BrandId",
                table: "Models",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finishings_Models_ModelId",
                table: "Finishings");

            migrationBuilder.DropForeignKey(
                name: "FK_Models_Brands_BrandId",
                table: "Models");

            migrationBuilder.DropTable(
                name: "CarReparation");

            migrationBuilder.DropIndex(
                name: "IX_Models_BrandId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Finishings_ModelId",
                table: "Finishings");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Models",
                newName: "IdBrand");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Finishings",
                newName: "IdModel");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Reparations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reparations_CarId",
                table: "Reparations",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reparations_Cars_CarId",
                table: "Reparations",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
