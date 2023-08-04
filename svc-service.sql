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

START TRANSACTION;

ALTER TABLE `Services` ADD `Name` varchar(32) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

CREATE INDEX `IX_Services_Name` ON `Services` (`Name`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230507120625_AddServiceName', '7.0.5');

COMMIT;

START TRANSACTION;

UPDATE `Services` SET `VersionPatch` = ''
WHERE `VersionPatch` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Services` MODIFY COLUMN `VersionPatch` varchar(32) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `Services` SET `VersionMinor` = ''
WHERE `VersionMinor` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Services` MODIFY COLUMN `VersionMinor` varchar(16) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `Services` SET `VersionMajor` = ''
WHERE `VersionMajor` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Services` MODIFY COLUMN `VersionMajor` varchar(16) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Services` MODIFY COLUMN `IdleRam` decimal(16,4) NOT NULL DEFAULT 0.0;

ALTER TABLE `Services` MODIFY COLUMN `IdleGpuMem` decimal(16,4) NOT NULL DEFAULT 0.0;

ALTER TABLE `Services` MODIFY COLUMN `IdleGpuCore` decimal(16,4) NOT NULL DEFAULT 0.0;

ALTER TABLE `Services` MODIFY COLUMN `IdleDisk` decimal(16,4) NOT NULL DEFAULT 0.0;

ALTER TABLE `Services` MODIFY COLUMN `IdleCpu` decimal(16,4) NOT NULL DEFAULT 0.0;

ALTER TABLE `Services` MODIFY COLUMN `DesiredCpu` decimal(65,30) NOT NULL;

ALTER TABLE `Services` ADD `HasIdleResource` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `Services` ADD `HasVersion` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230508042542_hasPropInsteadOfNullable', '7.0.5');

COMMIT;

START TRANSACTION;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230508045228_fixDesiredCpuPrecision', '7.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE `Services` MODIFY COLUMN `DesiredCpu` decimal(16,4) NOT NULL;

ALTER TABLE `Services` ADD `ImageUrl` varchar(256) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230515051450_addImageUrlforServiceEntity', '7.0.5');

COMMIT;

START TRANSACTION;

CREATE TABLE `Dependencies` (
    `CallerServiceId` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `CallerIdSuffix` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `CalleeServiceId` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `CalleeIdSuffix` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    `SerilizedData` varchar(4096) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Dependencies` PRIMARY KEY (`CallerServiceId`, `CallerIdSuffix`, `CalleeServiceId`, `CalleeIdSuffix`),
    CONSTRAINT `FK_Dependencies_Interfaces_CalleeServiceId_CalleeIdSuffix` FOREIGN KEY (`CalleeServiceId`, `CalleeIdSuffix`) REFERENCES `Interfaces` (`ServiceId`, `IdSuffix`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Interfaces_CallerServiceId_CallerIdSuffix` FOREIGN KEY (`CallerServiceId`, `CallerIdSuffix`) REFERENCES `Interfaces` (`ServiceId`, `IdSuffix`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Services_CalleeServiceId` FOREIGN KEY (`CalleeServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Dependencies_Services_CallerServiceId` FOREIGN KEY (`CallerServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Dependencies_CalleeServiceId_CalleeIdSuffix` ON `Dependencies` (`CalleeServiceId`, `CalleeIdSuffix`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230804063422_Dependency', '7.0.5');

COMMIT;

