ALTER TABLE `permissions_users`
ADD COLUMN `cmd_dance` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_rave` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_roll` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_control` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_makesay` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_sitdown` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_lay` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_roomfreeze` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_redeempixel` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_redeemshell` enum ('0', '1') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_ranks`
ADD COLUMN `cmd_dance` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_rave` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_roll` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_control` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_makesay` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_sitdown` enum('0','1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_lay` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_roomfreeze` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_redeempixel` enum ('0', '1') NOT NULL DEFAULT '0',
ADD COLUMN `cmd_redeemshell` enum ('0', '1') NOT NULL DEFAULT '0';

INSERT INTO `texts` VALUES ('cmd_dance_desc', ':dance <username> - Make the selected user dance');
INSERT INTO `texts` VALUES ('cmd_rave_desc', ':rave - Make everyone dance');
INSERT INTO `texts` VALUES ('cmd_roll_desc', ':roll <username> <number> - Make a user roll the selected number');
INSERT INTO `texts` VALUES ('cmd_control_desc', ':control <username> - Control the selected user');
INSERT INTO `texts` VALUES ('cmd_makesay_desc', ':makesay <username> <message> - Make the selected user say a message');
INSERT INTO `texts` VALUES ('cmd_sitdown_desc', ':sitdown - Make everyone sitdown');
INSERT INTO `texts` VALUES ('cmd_dance_name', 'dance');
INSERT INTO `texts` VALUES ('cmd_rave_name', 'rave');
INSERT INTO `texts` VALUES ('cmd_roll_name', 'roll');
INSERT INTO `texts` VALUES ('cmd_control_name', 'control');
INSERT INTO `texts` VALUES ('cmd_makesay_name', 'makesay');
INSERT INTO `texts` VALUES ('cmd_sitdown_name', 'sitdown');
INSERT INTO `texts` VALUES ('cmd_lay_name', 'lay');
INSERT INTO `texts` VALUES ('cmd_roomfreeze_name', 'roomfreeze');
INSERT INTO `texts` VALUES ('cmd_redeempixel_name', 'redeempixel');
INSERT INTO `texts` VALUES ('cmd_redeemshell_name', 'redeemshell');