INSERT INTO `texts` VALUES ('pet_breeds_monster', 'IPDHIH');
INSERT INTO `texts` VALUES ('pet_breeds_evil_bunny', 'IRDHIH');
INSERT INTO `texts` VALUES ('pet_breeds_depressed_bunny', 'ISDHIH');
INSERT INTO `texts` VALUES ('pet_breeds_love_bunny', 'IPEHIH');

ALTER TABLE `server_settings` ADD COLUMN `enable_cmd_redeemcredits`  enum('0','1') NOT NULL DEFAULT '1';