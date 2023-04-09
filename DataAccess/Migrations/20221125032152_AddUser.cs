using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO users(UserName, Address, Contact) VALUES" +
                                                                                 $"('Freecode', 'spot.com', 'free@gmail.com')," +
                                                                                 $"('Freecode1', 'spot.com', 'free1@gmail.com')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
