using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLegoProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cust_Recommendation",
                columns: table => new
                {
                    customer_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    first_name = table.Column<string>(type: "TEXT", nullable: true),
                    last_name = table.Column<string>(type: "TEXT", nullable: true),
                    Rec_1 = table.Column<string>(type: "TEXT", nullable: true),
                    Rec_2 = table.Column<string>(type: "TEXT", nullable: true),
                    Rec_3 = table.Column<string>(type: "TEXT", nullable: true),
                    Rec_4 = table.Column<string>(type: "TEXT", nullable: true),
                    Rec_5 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cust_Recommendation", x => x.customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customer_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    first_name = table.Column<string>(type: "TEXT", nullable: true),
                    last_name = table.Column<string>(type: "TEXT", nullable: true),
                    birth_date = table.Column<string>(type: "TEXT", nullable: true),
                    country_of_residence = table.Column<string>(type: "TEXT", nullable: true),
                    gender = table.Column<string>(type: "TEXT", nullable: true),
                    age = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "LineItem",
                columns: table => new
                {
                    transaction_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    product_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    qty = table.Column<int>(type: "INTEGER", nullable: true),
                    rating = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItem", x => new { x.transaction_ID, x.product_ID });
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    transaction_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    customer_ID = table.Column<int>(type: "INTEGER", nullable: true),
                    date = table.Column<string>(type: "TEXT", nullable: true),
                    day_of_week = table.Column<string>(type: "TEXT", nullable: true),
                    time = table.Column<int>(type: "INTEGER", nullable: true),
                    entry_mode = table.Column<string>(type: "TEXT", nullable: true),
                    amount = table.Column<double>(type: "REAL", nullable: true),
                    type_of_transaction = table.Column<string>(type: "TEXT", nullable: true),
                    country_of_transaction = table.Column<string>(type: "TEXT", nullable: true),
                    shipping_address = table.Column<string>(type: "TEXT", nullable: true),
                    bank = table.Column<string>(type: "TEXT", nullable: true),
                    type_of_card = table.Column<string>(type: "TEXT", nullable: true),
                    fraud = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.transaction_ID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    year = table.Column<int>(type: "INTEGER", nullable: true),
                    num_parts = table.Column<int>(type: "INTEGER", nullable: true),
                    price = table.Column<int>(type: "INTEGER", nullable: true),
                    img_link = table.Column<string>(type: "TEXT", nullable: true),
                    primary_color = table.Column<string>(type: "TEXT", nullable: true),
                    secondary_color = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    category = table.Column<string>(type: "TEXT", nullable: true),
                    subcategory = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.product_ID);
                });

            migrationBuilder.CreateTable(
                name: "Recommendation",
                columns: table => new
                {
                    product_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    pop_score = table.Column<int>(type: "INTEGER", nullable: true),
                    rec_1 = table.Column<string>(type: "TEXT", nullable: true),
                    rec_2 = table.Column<string>(type: "TEXT", nullable: true),
                    rec_3 = table.Column<string>(type: "TEXT", nullable: true),
                    rec_4 = table.Column<string>(type: "TEXT", nullable: true),
                    rec_5 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendation", x => x.product_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cust_Recommendation");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "LineItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Recommendation");
        }
    }
}
