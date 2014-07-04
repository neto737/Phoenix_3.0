DELETE FROM texts WHERE identifier = 'pet_breeds_bunny';
UPDATE `texts` SET `identifier`='pet_breeds_0' WHERE (`identifier`='pet_breeds_dogs') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_1' WHERE (`identifier`='pet_breeds_cats') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_2' WHERE (`identifier`='pet_breeds_crocs') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_3' WHERE (`identifier`='pet_breeds_terriers') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_4' WHERE (`identifier`='pet_breeds_bears') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_5' WHERE (`identifier`='pet_breeds_pigs') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_6' WHERE (`identifier`='pet_breeds_lions') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_7' WHERE (`identifier`='pet_breeds_rhinos') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_8' WHERE (`identifier`='pet_breeds_spiders') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_9' WHERE (`identifier`='pet_breeds_turtles') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_10' WHERE (`identifier`='pet_breeds_chics') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_11' WHERE (`identifier`='pet_breeds_frogs') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_12' WHERE (`identifier`='pet_breeds_dragons') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_13' WHERE (`identifier`='pet_breeds_horses') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_14' WHERE (`identifier`='pet_breeds_monkies') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_16' WHERE (`identifier`='pet_breeds_monster') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_18' WHERE (`identifier`='pet_breeds_evil_bunny') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_19' WHERE (`identifier`='pet_breeds_depressed_bunny') LIMIT 1;
UPDATE `texts` SET `identifier`='pet_breeds_20' WHERE (`identifier`='pet_breeds_love_bunny') LIMIT 1;

INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_15', 'ISCHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_17', 'IQDHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_21', 'IQEHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_22', 'IREHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_23', 'ISEHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_24', 'IPFHIH');
INSERT INTO texts(identifier, display_text) VALUES('pet_breeds_25', 'IQFHIH');

DROP TABLE IF EXISTS `group_memberships`;
CREATE TABLE `group_memberships` (
  `groupid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  KEY `groupid` (`groupid`),
  KEY `userid` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `permissions_ranks` ADD `cmd_points` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_points` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_teleport` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_teleport` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_masspoints` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_masspoints` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_globalpoints` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_globalpoints` enum('1','0') NOT NULL DEFAULT '0';