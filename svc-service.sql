CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Services` (
    `Id` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Repo` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `VersionMajor` varchar(16) CHARACTER SET utf8mb4 NULL,
    `VersionMinor` varchar(16) CHARACTER SET utf8mb4 NULL,
    `VersionPatch` varchar(32) CHARACTER SET utf8mb4 NULL,
    `IdleCpu` decimal(16,4) NULL,
    `IdleRam` decimal(16,4) NULL,
    `IdleDisk` decimal(16,4) NULL,
    `IdleGpuCore` decimal(16,4) NULL,
    `IdleGpuMem` decimal(16,4) NULL,
    `DesiredCpu` decimal(16,4) NOT NULL,
    `DesiredRam` decimal(16,4) NOT NULL,
    `DesiredDisk` decimal(16,4) NOT NULL,
    `DesiredGpuCore` decimal(16,4) NOT NULL,
    `DesiredGpuMem` decimal(16,4) NOT NULL,
    `DesiredCapability` int NOT NULL,
    CONSTRAINT `PK_Services` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Interfaces` (
    `ServiceId` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `IdSuffix` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `Path` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
    `InputSize` int NOT NULL,
    `OutputSize` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Interfaces` PRIMARY KEY (`ServiceId`, `IdSuffix`),
    CONSTRAINT `FK_Interfaces_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230504032049_init', '7.0.5');

COMMIT;

