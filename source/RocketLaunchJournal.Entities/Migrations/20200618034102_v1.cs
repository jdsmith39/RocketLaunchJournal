using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RocketLaunchJournal.Entities.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APILogs",
                columns: table => new
                {
                    APILogId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestContentBlock = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseContentBlock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetURL = table.Column<string>(maxLength: 200, nullable: false),
                    TransmissionDateTime = table.Column<DateTime>(nullable: false),
                    ResponseDateTime = table.Column<DateTime>(nullable: true),
                    IncomingRequest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APILogs", x => x.APILogId);
                });

            migrationBuilder.CreateTable(
                name: "DelayNotes",
                columns: table => new
                {
                    DelayNoteId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelayNotes", x => x.DelayNoteId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "LogTypes",
                columns: table => new
                {
                    LogTypeId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTypes", x => x.LogTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 200, nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 200, nullable: false),
                    LastName = table.Column<string>(maxLength: 200, nullable: false),
                    Email = table.Column<string>(maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 25, nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    IsLoginEnabled = table.Column<bool>(nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 500, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 200, nullable: true),
                    SecurityStamp = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LastSignInDateTime = table.Column<DateTimeOffset>(nullable: true),
                    LastPasswordChangeDateTime = table.Column<DateTimeOffset>(nullable: true),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: true),
                    AuthyUserId = table.Column<int>(nullable: true),
                    InactivatedById = table.Column<int>(nullable: true),
                    InactiveDateTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    RoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<byte>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 500, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.RoleClaimId);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rockets",
                columns: table => new
                {
                    RocketId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Weight = table.Column<decimal>(nullable: true),
                    Diameter = table.Column<decimal>(nullable: true),
                    Length = table.Column<decimal>(nullable: true),
                    ParachuteSize = table.Column<string>(maxLength: 200, nullable: true),
                    BlackPowderForApogee = table.Column<decimal>(nullable: true),
                    BlackPowderForMain = table.Column<decimal>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    InactivatedById = table.Column<int>(nullable: true),
                    InactiveDateTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rockets", x => x.RocketId);
                    table.ForeignKey(
                        name: "FK_Rockets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    LogId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    LogGroupKey = table.Column<Guid>(nullable: true),
                    LogTypeId = table.Column<byte>(nullable: false),
                    EventDescription = table.Column<string>(nullable: false),
                    EventDateTime = table.Column<DateTimeOffset>(nullable: false),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_SystemLogs_LogTypes_LogTypeId",
                        column: x => x.LogTypeId,
                        principalTable: "LogTypes",
                        principalColumn: "LogTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    UserClaimId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 500, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.UserClaimId);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 2000, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 2000, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 2000, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 100, nullable: true),
                    ExpiresOnDateTime = table.Column<DateTime>(nullable: false),
                    ImpersonationUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_Users_ImpersonationUserId",
                        column: x => x.ImpersonationUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 2000, nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Launches",
                columns: table => new
                {
                    LaunchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RocketId = table.Column<int>(nullable: false),
                    LaunchNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    Motors = table.Column<string>(maxLength: 200, nullable: false),
                    DelayNoteId = table.Column<short>(nullable: true),
                    Altitude = table.Column<decimal>(nullable: true),
                    TopSpeed = table.Column<decimal>(nullable: true),
                    BurnTime = table.Column<decimal>(nullable: true),
                    PeakAcceleration = table.Column<decimal>(nullable: true),
                    AverageAcceleration = table.Column<decimal>(nullable: true),
                    CoastToApogeeTime = table.Column<decimal>(nullable: true),
                    ApogeeToEjectionTime = table.Column<decimal>(nullable: true),
                    EjectionAltitude = table.Column<decimal>(nullable: true),
                    DescentSpeed = table.Column<decimal>(nullable: true),
                    Duration = table.Column<decimal>(nullable: true),
                    Note = table.Column<string>(maxLength: 2000, nullable: true),
                    InactivatedById = table.Column<int>(nullable: true),
                    InactiveDateTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Launches", x => x.LaunchId);
                    table.ForeignKey(
                        name: "FK_Launches_DelayNotes_DelayNoteId",
                        column: x => x.DelayNoteId,
                        principalTable: "DelayNotes",
                        principalColumn: "DelayNoteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Launches_Rockets_RocketId",
                        column: x => x.RocketId,
                        principalTable: "Rockets",
                        principalColumn: "RocketId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Launches_DelayNoteId",
                table: "Launches",
                column: "DelayNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Launches_RocketId",
                table: "Launches",
                column: "RocketId");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Rockets_UserId",
                table: "Rockets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_LogTypeId",
                table: "SystemLogs",
                column: "LogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_UserId",
                table: "SystemLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_ImpersonationUserId",
                table: "UserRefreshTokens",
                column: "ImpersonationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APILogs");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "Launches");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "DelayNotes");

            migrationBuilder.DropTable(
                name: "Rockets");

            migrationBuilder.DropTable(
                name: "LogTypes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
