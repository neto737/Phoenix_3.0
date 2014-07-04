DROP TABLE IF EXISTS `fuserights`;
DROP TABLE IF EXISTS `fuserights_subs`;

-- ----------------------------
-- Table structure for `permissions_ranks`
-- ----------------------------
DROP TABLE IF EXISTS `permissions_ranks`;
CREATE TABLE `permissions_ranks` (
  `rank` int(1) unsigned NOT NULL,
  `cmd_update_permissions` enum('1','0') NOT NULL,
  `cmd_update_settings` enum('1','0') NOT NULL,
  `cmd_update_bots` enum('1','0') NOT NULL,
  `cmd_update_catalogue` enum('1','0') NOT NULL,
  `cmd_update_navigator` enum('1','0') NOT NULL,
  `cmd_update_items` enum('1','0') NOT NULL,
  `cmd_award` enum('1','0') NOT NULL,
  `cmd_coords` enum('1','0') NOT NULL,
  `cmd_override` enum('1','0') NOT NULL,
  `cmd_coins` enum('1','0') NOT NULL,
  `cmd_pixels` enum('1','0') NOT NULL,
  `cmd_ha` enum('1','0') NOT NULL,
  `cmd_hal` enum('1','0') NOT NULL,
  `cmd_freeze` enum('1','0') NOT NULL,
  `cmd_enable` enum('1','0') NOT NULL,
  `cmd_roommute` enum('1','0') NOT NULL,
  `cmd_setspeed` enum('1','0') NOT NULL,
  `cmd_masscredits` enum('1','0') NOT NULL,
  `cmd_globalcredits` enum('1','0') NOT NULL,
  `cmd_roombadge` enum('1','0') NOT NULL,
  `cmd_massbadge` enum('1','0') NOT NULL,
  `cmd_userinfo` enum('1','0') NOT NULL,
  `cmd_shutdown` enum('1','0') NOT NULL,
  `cmd_givebadge` enum('1','0') NOT NULL,
  `cmd_invisible` enum('1','0') NOT NULL,
  `cmd_ban` enum('1','0') NOT NULL,
  `cmd_superban` enum('1','0') NOT NULL,
  `cmd_roomkick` enum('1','0') NOT NULL,
  `cmd_roomalert` enum('1','0') NOT NULL,
  `cmd_mute` enum('1','0') NOT NULL,
  `cmd_unmute` enum('1','0') NOT NULL,
  `cmd_alert` enum('1','0') NOT NULL,
  `cmd_motd` enum('1','0') NOT NULL,
  `cmd_kick` enum('1','0') NOT NULL,
  `acc_anyroomrights` enum('1','0') NOT NULL,
  `acc_anyroomowner` enum('1','0') NOT NULL,
  `acc_supporttool` enum('1','0') NOT NULL,
  `acc_chatlogs` enum('1','0') NOT NULL,
  `acc_enter_fullrooms` enum('1','0') NOT NULL,
  `acc_enter_anyroom` enum('1','0') NOT NULL,
  `acc_restrictedrooms` enum('1','0') NOT NULL,
  `acc_unkickable` enum('1','0') NOT NULL,
  `acc_unbannable` enum('1','0') NOT NULL,
  PRIMARY KEY (`rank`),
  UNIQUE KEY `rank` (`rank`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of permissions_ranks
-- ----------------------------
INSERT INTO permissions_ranks VALUES ('1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO permissions_ranks VALUES ('2', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO permissions_ranks VALUES ('3', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO permissions_ranks VALUES ('4', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO permissions_ranks VALUES ('5', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '1', '0', '1', '1', '0', '1', '1', '1', '1', '1', '1', '0');
INSERT INTO permissions_ranks VALUES ('6', '1', '0', '0', '0', '0', '0', '1', '1', '1', '1', '0', '1', '1', '0', '1', '1', '0', '1', '1', '1', '1', '0', '0', '1', '0', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1');
INSERT INTO permissions_ranks VALUES ('7', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1');

-- ----------------------------
-- Table structure for `permissions_users`
-- ----------------------------
DROP TABLE IF EXISTS `permissions_users`;
CREATE TABLE `permissions_users` (
  `userid` int(11) unsigned NOT NULL,
  `cmd_update_permissions` enum('1','0') NOT NULL,
  `cmd_update_settings` enum('1','0') NOT NULL,
  `cmd_update_bots` enum('1','0') NOT NULL,
  `cmd_update_catalogue` enum('1','0') NOT NULL,
  `cmd_update_navigator` enum('1','0') NOT NULL,
  `cmd_update_items` enum('1','0') NOT NULL,
  `cmd_award` enum('1','0') NOT NULL,
  `cmd_coords` enum('1','0') NOT NULL,
  `cmd_override` enum('1','0') NOT NULL,
  `cmd_coins` enum('1','0') NOT NULL,
  `cmd_pixels` enum('1','0') NOT NULL,
  `cmd_ha` enum('1','0') NOT NULL,
  `cmd_hal` enum('1','0') NOT NULL,
  `cmd_freeze` enum('1','0') NOT NULL,
  `cmd_enable` enum('1','0') NOT NULL,
  `cmd_roommute` enum('1','0') NOT NULL,
  `cmd_setspeed` enum('1','0') NOT NULL,
  `cmd_masscredits` enum('1','0') NOT NULL,
  `cmd_globalcredits` enum('1','0') NOT NULL,
  `cmd_roombadge` enum('1','0') NOT NULL,
  `cmd_massbadge` enum('1','0') NOT NULL,
  `cmd_userinfo` enum('1','0') NOT NULL,
  `cmd_shutdown` enum('1','0') NOT NULL,
  `cmd_givebadge` enum('1','0') NOT NULL,
  `cmd_invisible` enum('1','0') NOT NULL,
  `cmd_ban` enum('1','0') NOT NULL,
  `cmd_superban` enum('1','0') NOT NULL,
  `cmd_roomkick` enum('1','0') NOT NULL,
  `cmd_roomalert` enum('1','0') NOT NULL,
  `cmd_mute` enum('1','0') NOT NULL,
  `cmd_unmute` enum('1','0') NOT NULL,
  `cmd_alert` enum('1','0') NOT NULL,
  `cmd_motd` enum('1','0') NOT NULL,
  `cmd_kick` enum('1','0') NOT NULL,
  `acc_anyroomrights` enum('1','0') NOT NULL,
  `acc_anyroomowner` enum('1','0') NOT NULL,
  `acc_supporttool` enum('1','0') NOT NULL,
  `acc_chatlogs` enum('1','0') NOT NULL,
  `acc_enter_fullrooms` enum('1','0') NOT NULL,
  `acc_enter_anyroom` enum('1','0') NOT NULL,
  `acc_restrictedrooms` enum('1','0') NOT NULL,
  `acc_unkickable` enum('1','0') NOT NULL,
  `acc_unbannable` enum('1','0') NOT NULL,
  PRIMARY KEY (`userid`),
  UNIQUE KEY `userid` (`userid`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of permissions_users
-- ----------------------------
