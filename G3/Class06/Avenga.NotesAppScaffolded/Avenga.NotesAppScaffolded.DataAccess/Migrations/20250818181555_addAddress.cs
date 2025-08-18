using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avenga.NotesAppScaffolded.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name:"Address",
                table:"Users",
                type: "nvarchar(100)",
                nullable:true
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");
        }
    }
}
