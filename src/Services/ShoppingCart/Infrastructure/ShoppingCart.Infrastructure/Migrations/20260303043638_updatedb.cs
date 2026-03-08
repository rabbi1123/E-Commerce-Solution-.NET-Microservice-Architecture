using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "CartHeaders");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "CartHeaders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CartHeaders");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetails");

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "CartHeaders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
