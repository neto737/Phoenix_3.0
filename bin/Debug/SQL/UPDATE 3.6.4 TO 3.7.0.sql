ALTER TABLE `server_settings` ADD `enable_cmdlogs` enum('0','1') NOT NULL DEFAULT '1';
ALTER TABLE `server_settings` ADD `allow_friendfurnidrops` enum('0','1') NOT NULL DEFAULT '1';

ALTER TABLE `permissions_ranks` ADD `cmd_sa` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `receive_sa` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_ipban` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `floodtime` int(3) NOT NULL DEFAULT '30';
ALTER TABLE `permissions_ranks` ADD `cmd_spull` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_disconnect` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_update_achievements` enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users` ADD `cmd_sa` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `receive_sa` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_ipban` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_spull` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_disconnect` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_update_achievements` enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_vip` ADD `cmdFollow` enum('1','0') NOT NULL DEFAULT '1';

DROP TABLE IF EXISTS `cmdlogs`;
CREATE TABLE `cmdlogs` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` int(10) NOT NULL,
  `user_name` varchar(100) NOT NULL,
  `command` varchar(50) NOT NULL,
  `extra_data` text NOT NULL,
  `timestamp` int(120) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `texts`;
CREATE TABLE `texts` (
  `identifier` varchar(50) NOT NULL,
  `display_text` text NOT NULL,
  PRIMARY KEY (`identifier`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of texts
-- ----------------------------
INSERT INTO `texts` VALUES ('emu_cleandb', 'Cleaning up database..');
INSERT INTO `texts` VALUES ('emu_connectdb', 'Connecting to database..');
INSERT INTO `texts` VALUES ('emu_loadroles', 'Loading Roles..');
INSERT INTO `texts` VALUES ('emu_loadsettings', 'Loading Settings..');
INSERT INTO `texts` VALUES ('emu_loadtexts', 'Loading Texts..');

ALTER TABLE `catalog_pages` ADD `page_link_description` TEXT NOT NULL;
ALTER TABLE `catalog_pages` ADD `page_link_pagename` TEXT NOT NULL;

ALTER TABLE `catalog_items`
ADD COLUMN `vip`  enum('0','1','2') NOT NULL DEFAULT '0';

ALTER TABLE `furniture`
MODIFY COLUMN `interaction_type`  enum('default','gate','postit','roomeffect','dimmer','trophy','bed','scoreboard','vendingmachine','alert','onewaygate','loveshuffler','habbowheel','dice','bottle','teleport','rentals','pet','roller','water','ball','bb_red_gate','bb_green_gate','bb_yellow_gate','bb_puck','bb_blue_gate','bb_patch','bb_teleport','blue_score','green_score','red_score','yellow_score','fbgate','tagpole','counter','red_goal','blue_goal','yellow_goal','green_goal','wired','wf_trg_onsay','wf_act_saymsg','wf_trg_enterroom','wf_act_moveuser','wf_act_togglefurni','wf_trg_furnistate','wf_trg_onfurni','pressure_pad','wf_trg_offfurni','wf_trg_gameend','wf_trg_gamestart','wf_trg_timer','wf_act_givepoints','wf_trg_attime','wf_trg_atscore','wf_act_moverotate','rollerskate','stickiepole','wf_xtra_random','wf_cnd_trggrer_on_frn','wf_cnd_furnis_hv_avtrs','wf_act_matchfurni','wf_cnd_has_furni_on','puzzlebox','switch') DEFAULT 'default';