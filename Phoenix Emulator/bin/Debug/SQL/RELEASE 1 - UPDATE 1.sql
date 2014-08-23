INSERT INTO `texts` VALUES ('cmd_sit_name', 'sit');
INSERT INTO `texts` VALUES ('cmd_sit_desc', ':sit - Sit on the floor');
INSERT INTO `texts` VALUES ('cmd_giveitem_name', 'giveitem');
INSERT INTO `texts` VALUES ('cmd_giveitem_desc', ':giveitem <username> - Give the item to another user');
INSERT INTO `texts` VALUES ('cmd_dismount_name', 'dismount');
INSERT INTO `texts` VALUES ('cmd_dismount_desc', ':dismount - Lets get off the horse');
INSERT INTO `texts` VALUES ('cmd_faceless_name', 'faceless');
INSERT INTO `texts` VALUES ('cmd_faceless_desc', ':faceless - Use this command to go Faceless');

ALTER TABLE `permissions_vip` ADD COLUMN `cmdFaceless` enum('1','0') NOT NULL DEFAULT '1';