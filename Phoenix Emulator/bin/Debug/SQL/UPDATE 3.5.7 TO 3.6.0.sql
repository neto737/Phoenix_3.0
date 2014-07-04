ALTER TABLE `rooms` ADD `wallthick` int(1) NOT NULL DEFAULT '0';
ALTER TABLE `rooms` ADD `floorthick` int(1) NOT NULL DEFAULT '0';


INSERT INTO `catalog_pages` VALUES ('179', '14', 'Turtle', '1', '126', '1', '1', '1', '0', '0', '3', 'pets', 'catalog_pet_headline1', '', '', 'Anything but slow, these guys are ready to ride the waves and swim laps at your beaches!', '', 'Name your Turtle:', '', '0');
INSERT INTO `catalog_pages` VALUES ('180', '14', 'Monkey', '1', '128', '1', '1', '1', '0', '0', '1', 'pets', 'catalog_pet_headline1', ' ', ' ', ' ', ' ', 'Give a name:Pick a color:', ' ', '0');


DROP TABLE IF EXISTS `achievements`;
CREATE TABLE `achievements` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `type` varchar(100) NOT NULL DEFAULT '',
  `levels` int(11) NOT NULL DEFAULT '1',
  `dynamic_badgelevel` enum('0','1') NOT NULL DEFAULT '1',
  `badge` varchar(100) NOT NULL,
  `pixels_base` int(5) NOT NULL DEFAULT '50',
  `score_base` int(5) NOT NULL DEFAULT '10',
  `pixels_multiplier` double NOT NULL DEFAULT '1.25',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=cp1251;

-- ----------------------------
-- Records of achievements
-- ----------------------------
INSERT INTO achievements VALUES ('1', 'identity', '1', '0', 'ACH_AvatarLooks1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('2', 'identity', '1', '0', 'ACH_EmailVerification1', '2500', '10', '0.5');
INSERT INTO achievements VALUES ('3', 'identity', '1', '0', 'ACH_Graduate1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('4', 'identity', '1', '0', 'ACH_HappyHour1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('5', 'identity', '1', '0', 'ACH_Motto1', '100', '10', '0.5');
INSERT INTO achievements VALUES ('6', 'identity', '1', '0', 'ACH_Student1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('7', 'identity', '1', '0', 'ACH_AvatarTags1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('8', 'social', '1', '0', 'ACH_RespectGiven1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('9', 'identity', '1', '0', 'ACH_Name1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('10', 'identity', '10', '1', 'ACH_RegistrationDuration', '50', '10', '0.5');
INSERT INTO achievements VALUES ('11', 'identity', '10', '1', 'ACH_Login', '50', '10', '0.5');
INSERT INTO achievements VALUES ('13', 'explore', '10', '1', 'ACH_RoomEntry', '5', '10', '0.5');
INSERT INTO achievements VALUES ('14', 'social', '10', '1', 'ACH_RespectEarned', '50', '10', '0.5');
INSERT INTO achievements VALUES ('15', 'social', '10', '1', 'ACH_MGM', '50', '10', '0.5');
INSERT INTO achievements VALUES ('16', 'identity', '10', '1', 'ACH_AllTimeHotelPresence', '50', '10', '0.5');
INSERT INTO achievements VALUES ('17', 'identity', '1', '0', 'ACH_TraderPass', '50', '10', '0.5');
INSERT INTO achievements VALUES ('18', 'identity', '1', '0', 'ACH_AIPerformanceVote', '50', '10', '0.5');
INSERT INTO achievements VALUES ('19', 'pets', '10', '1', 'ACH_PetFeeding', '50', '10', '0.5');
INSERT INTO achievements VALUES ('20', 'pets', '10', '1', 'ACH_PetLevelUp', '50', '10', '0.5');
INSERT INTO achievements VALUES ('21', 'pets', '10', '1', 'ACH_PetLover', '50', '10', '0.5');
INSERT INTO achievements VALUES ('22', 'pets', '10', '1', 'ACH_PetRespectGiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('23', 'pets', '10', '1', 'ACH_PetRespectReceiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('24', 'social', '10', '1', 'ACH_GiftGiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('25', 'social', '10', '1', 'ACH_GiftReceiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('26', 'identity', '5', '1', 'ACH_BasicClub', '50', '10', '0.5');
INSERT INTO achievements VALUES ('27', 'identity', '5', '1', 'ACH_VipClub', '50', '10', '0.5');
INSERT INTO achievements VALUES ('28', 'games', '20', '1', 'ACH_BattleBallTilesLocked', '50', '10', '0.5');
INSERT INTO achievements VALUES ('29', 'games', '20', '1', 'ACH_GamePlayerExperience', '50', '10', '0.5');
INSERT INTO achievements VALUES ('30', 'games', '20', '1', 'ACH_GameAuthorExperience', '50', '10', '0.5');
INSERT INTO achievements VALUES ('31', 'identity', '1', '0', 'Z64', '50', '10', '0.5');
INSERT INTO achievements VALUES ('32', 'identity', '1', '0', 'XXX', '0', '10', '0.5');
INSERT INTO achievements VALUES ('33', 'identity', '1', '0', 'RLX01', '200', '10', '0.5');
INSERT INTO achievements VALUES ('34', 'identity', '1', '0', 'AC5', '200', '10', '0.5');
INSERT INTO achievements VALUES ('35', 'identity', '1', '0', 'IT5', '200', '10', '0.5');
INSERT INTO achievements VALUES ('36', 'identity', '1', '0', 'ACH_Forum1', '50', '10', '0.5');
INSERT INTO achievements VALUES ('37', 'identity', '1', '0', 'RU3', '200', '10', '0.5');
INSERT INTO achievements VALUES ('38', 'identity', '1', '0', 'VA2', '200', '10', '0.5');
INSERT INTO achievements VALUES ('39', 'identity', '1', '0', 'EGG15', '200', '10', '0.5');