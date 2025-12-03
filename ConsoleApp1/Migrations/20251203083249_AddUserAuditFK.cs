using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuditFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Users_CreatedBy",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Users_DeletedBy",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Users_LastModifiedBy",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_CreatedBy",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_DeletedBy",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_LastModifiedBy",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_CreatedBy",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_DeletedBy",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_LastModifiedBy",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_CreatedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_DeletedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_LastModifiedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Users_CreatedBy",
                table: "Publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Users_DeletedBy",
                table: "Publishers");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Users_LastModifiedBy",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_CreatedBy",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_DeletedBy",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_LastModifiedBy",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DeletedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_LastModifiedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_CreatedBy",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_DeletedBy",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_LastModifiedBy",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Books_CreatedBy",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_DeletedBy",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_LastModifiedBy",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_CreatedBy",
                table: "BookCopies");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_DeletedBy",
                table: "BookCopies");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_LastModifiedBy",
                table: "BookCopies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Publishers_CreatedBy",
                table: "Publishers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_DeletedBy",
                table: "Publishers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_LastModifiedBy",
                table: "Publishers",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DeletedBy",
                table: "Categories",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LastModifiedBy",
                table: "Categories",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_CreatedBy",
                table: "Borrows",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_DeletedBy",
                table: "Borrows",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_LastModifiedBy",
                table: "Borrows",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CreatedBy",
                table: "Books",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Books_DeletedBy",
                table: "Books",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LastModifiedBy",
                table: "Books",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_CreatedBy",
                table: "BookCopies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_DeletedBy",
                table: "BookCopies",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_LastModifiedBy",
                table: "BookCopies",
                column: "LastModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Users_CreatedBy",
                table: "BookCopies",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Users_DeletedBy",
                table: "BookCopies",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Users_LastModifiedBy",
                table: "BookCopies",
                column: "LastModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_CreatedBy",
                table: "Books",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_DeletedBy",
                table: "Books",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_LastModifiedBy",
                table: "Books",
                column: "LastModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_CreatedBy",
                table: "Borrows",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_DeletedBy",
                table: "Borrows",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_LastModifiedBy",
                table: "Borrows",
                column: "LastModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_CreatedBy",
                table: "Categories",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_DeletedBy",
                table: "Categories",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_LastModifiedBy",
                table: "Categories",
                column: "LastModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Users_CreatedBy",
                table: "Publishers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Users_DeletedBy",
                table: "Publishers",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Users_LastModifiedBy",
                table: "Publishers",
                column: "LastModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
