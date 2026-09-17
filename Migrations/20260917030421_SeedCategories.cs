using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RepairSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairAttachments_RepairRequest_RepairRequestId",
                table: "RepairAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequest_AspNetUsers_RequesterId",
                table: "RepairRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequest_AspNetUsers_TechnicianId",
                table: "RepairRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequest_RepairCategories_CategoryId",
                table: "RepairRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairStatusHistories_RepairRequest_RepairRequestId",
                table: "RepairStatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairRequest",
                table: "RepairRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "RepairRequest",
                newName: "RepairRequests");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequest_TechnicianId",
                table: "RepairRequests",
                newName: "IX_RepairRequests_TechnicianId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequest_RequesterId",
                table: "RepairRequests",
                newName: "IX_RepairRequests_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequest_CategoryId",
                table: "RepairRequests",
                newName: "IX_RepairRequests_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairRequests",
                table: "RepairRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.InsertData(
                table: "RepairCategories",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "IT / อุปกรณ์คอมพิวเตอร์" },
                    { 2, true, "ไฟฟ้า" },
                    { 3, true, "ประปา" },
                    { 4, true, "ทั่วไป / อาคาร" },
                    { 5, true, "อื่นๆ" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairAttachments_RepairRequests_RepairRequestId",
                table: "RepairAttachments",
                column: "RepairRequestId",
                principalTable: "RepairRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_AspNetUsers_RequesterId",
                table: "RepairRequests",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_AspNetUsers_TechnicianId",
                table: "RepairRequests",
                column: "TechnicianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_RepairCategories_CategoryId",
                table: "RepairRequests",
                column: "CategoryId",
                principalTable: "RepairCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairStatusHistories_RepairRequests_RepairRequestId",
                table: "RepairStatusHistories",
                column: "RepairRequestId",
                principalTable: "RepairRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairAttachments_RepairRequests_RepairRequestId",
                table: "RepairAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_AspNetUsers_RequesterId",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_AspNetUsers_TechnicianId",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_RepairCategories_CategoryId",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairStatusHistories_RepairRequests_RepairRequestId",
                table: "RepairStatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairRequests",
                table: "RepairRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DeleteData(
                table: "RepairCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RepairCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RepairCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RepairCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RepairCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "RepairRequests",
                newName: "RepairRequest");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequests_TechnicianId",
                table: "RepairRequest",
                newName: "IX_RepairRequest_TechnicianId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequests_RequesterId",
                table: "RepairRequest",
                newName: "IX_RepairRequest_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairRequests_CategoryId",
                table: "RepairRequest",
                newName: "IX_RepairRequest_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairRequest",
                table: "RepairRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairAttachments_RepairRequest_RepairRequestId",
                table: "RepairAttachments",
                column: "RepairRequestId",
                principalTable: "RepairRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequest_AspNetUsers_RequesterId",
                table: "RepairRequest",
                column: "RequesterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequest_AspNetUsers_TechnicianId",
                table: "RepairRequest",
                column: "TechnicianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequest_RepairCategories_CategoryId",
                table: "RepairRequest",
                column: "CategoryId",
                principalTable: "RepairCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairStatusHistories_RepairRequest_RepairRequestId",
                table: "RepairStatusHistories",
                column: "RepairRequestId",
                principalTable: "RepairRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
