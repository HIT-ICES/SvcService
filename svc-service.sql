CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Services` (
    `Id` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Repo` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ImageUrl` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `HasVersion` tinyint(1) NOT NULL,
    `VersionMajor` varchar(16) CHARACTER SET utf8mb4 NOT NULL,
    `VersionMinor` varchar(16) CHARACTER SET utf8mb4 NOT NULL,
    `VersionPatch` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `HasIdleResource` tinyint(1) NOT NULL,
    `IdleCpu` decimal(16,4) NOT NULL,
    `IdleRam` decimal(16,4) NOT NULL,
    `IdleDisk` decimal(16,4) NOT NULL,
    `IdleGpuCore` decimal(16,4) NOT NULL,
    `IdleGpuMem` decimal(16,4) NOT NULL,
    `DesiredCpu` decimal(16,4) NOT NULL,
    `DesiredRam` decimal(16,4) NOT NULL,
    `DesiredDisk` decimal(16,4) NOT NULL,
    `DesiredGpuCore` decimal(16,4) NOT NULL,
    `DesiredGpuMem` decimal(16,4) NOT NULL,
    `DesiredCapability` int NOT NULL,
    CONSTRAINT `PK_Services` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Interfaces` (
    `ServiceId` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `IdSuffix` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Path` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `InputSize` decimal(16,4) NOT NULL,
    `OutputSize` decimal(16,4) NOT NULL,
    `Info` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `Method` varchar(16) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Interfaces` PRIMARY KEY (`ServiceId`, `IdSuffix`),
    CONSTRAINT `FK_Interfaces_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Dependencies` (
    `CallerServiceId` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `CallerIdSuffix` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `CalleeServiceId` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `CalleeIdSuffix` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `SerilizedData` varchar(4096) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Dependencies` PRIMARY KEY (`CallerServiceId`, `CallerIdSuffix`, `CalleeServiceId`, `CalleeIdSuffix`),
    CONSTRAINT `FK_Dependencies_Interfaces_CalleeServiceId_CalleeIdSuffix` FOREIGN KEY (`CalleeServiceId`, `CalleeIdSuffix`) REFERENCES `Interfaces` (`ServiceId`, `IdSuffix`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Interfaces_CallerServiceId_CallerIdSuffix` FOREIGN KEY (`CallerServiceId`, `CallerIdSuffix`) REFERENCES `Interfaces` (`ServiceId`, `IdSuffix`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Services_CalleeServiceId` FOREIGN KEY (`CalleeServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Services_CallerServiceId` FOREIGN KEY (`CallerServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Dependencies_CalleeServiceId_CalleeIdSuffix` ON `Dependencies` (`CalleeServiceId`, `CalleeIdSuffix`);

CREATE INDEX `IX_Services_Name` ON `Services` (`Name`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240619015853_Initial', '7.0.5');

COMMIT;

