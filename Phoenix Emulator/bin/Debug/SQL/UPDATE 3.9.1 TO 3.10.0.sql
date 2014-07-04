ALTER TABLE `server_settings`
ADD COLUMN `ShowUsersAndRoomsInAbout`  enum('0','1') NOT NULL DEFAULT '1',
ADD COLUMN `idlesleep`  int(6) NOT NULL DEFAULT 300,
ADD COLUMN `idlekick`  int(6) NOT NULL DEFAULT 1200,
ADD COLUMN `ip_lastforbans`  enum('0','1') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users`
ADD COLUMN `wired_give_handitem`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_respect`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_dance`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_usergroups`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_userinfo_viewip`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_wearing`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_carrying`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_alert`  enum('0','1') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_ranks`
ADD COLUMN `wired_give_handitem`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_respect`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_dance`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_usergroups`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_userinfo_viewip`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_wearing`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_carrying`  enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_alert`  enum('0','1') NOT NULL DEFAULT '0';

ALTER TABLE `navigator_publics`
CHANGE COLUMN `category_id` `category`  enum('0','1') NOT NULL DEFAULT '0';

ALTER TABLE `navigator_flatcats`
ADD COLUMN `cantrade`  enum('0','1') NOT NULL DEFAULT '1';

ALTER TABLE `furniture`
ADD COLUMN `effectM`  int(3) NOT NULL DEFAULT 0,
ADD COLUMN `effectF`  int(3) NOT NULL DEFAULT 0;

INSERT INTO `texts` VALUES ('pet_breeds_21', 'IQEHIH');
INSERT INTO `texts` VALUES ('pet_breeds_22', 'IREHIH');
INSERT INTO `texts` VALUES ('pet_breeds_23', 'ISEHIH');
INSERT INTO `texts` VALUES ('pet_breeds_24', 'IPFHIH');
INSERT INTO `texts` VALUES ('pet_breeds_25', 'IQFHIH');
INSERT INTO `texts` VALUES ('pet_breeds_26', 'IRFHIH');
INSERT INTO `texts` VALUES ('pet_breeds_27', 'ISFHIH');
INSERT INTO `texts` VALUES ('pet_breeds_28', 'IPGHIH');
INSERT INTO `texts` VALUES ('pet_breeds_29', 'IQGHIH');
INSERT INTO `texts` VALUES ('pet_breeds_30', 'IRGHIH');

INSERT INTO `texts` VALUES ('pet_cmd_sleep', 'sleep');
INSERT INTO `texts` VALUES ('pet_cmd_free', 'free');
INSERT INTO `texts` VALUES ('pet_cmd_sit', 'sit');
INSERT INTO `texts` VALUES ('pet_cmd_lay', 'lay');
INSERT INTO `texts` VALUES ('pet_cmd_stay', 'stay');
INSERT INTO `texts` VALUES ('pet_cmd_here', 'here');
INSERT INTO `texts` VALUES ('pet_cmd_dead', 'dead');
INSERT INTO `texts` VALUES ('pet_cmd_beg', 'beg');
INSERT INTO `texts` VALUES ('pet_cmd_jump', 'jump');
INSERT INTO `texts` VALUES ('pet_cmd_stfu', 'shutup');
INSERT INTO `texts` VALUES ('pet_cmd_talk', 'talk');

INSERT INTO `texts` VALUES ('trade_error_roomdisabled', 'Trading has been disabled in this room, sorry!');

