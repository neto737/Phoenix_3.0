ALTER TABLE user_stats ADD AchievementScore int(7) NOT NULL DEFAULT '0';

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `achievements`
-- ----------------------------
DROP TABLE IF EXISTS `achievements`;
CREATE TABLE `achievements` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `levels` int(11) NOT NULL DEFAULT '1',
  `dynamic_badgelevel` enum('0','1') NOT NULL DEFAULT '1',
  `badge` varchar(100) NOT NULL,
  `pixels_base` int(5) NOT NULL DEFAULT '50',
  `score_base` int(5) NOT NULL DEFAULT '10',
  `pixels_multiplier` double NOT NULL DEFAULT '1.25',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=39 DEFAULT CHARSET=cp1251;

-- ----------------------------
-- Records of achievements
-- ----------------------------
INSERT INTO achievements VALUES ('1', '1', '1', 'ACH_AvatarLooks', '50', '10', '0.5');
INSERT INTO achievements VALUES ('2', '1', '1', 'ACH_EmailVerification', '2450', '10', '0.5');
INSERT INTO achievements VALUES ('3', '1', '1', 'ACH_Graduate', '50', '10', '0.5');
INSERT INTO achievements VALUES ('4', '1', '1', 'ACH_HappyHour', '50', '10', '0.5');
INSERT INTO achievements VALUES ('5', '1', '1', 'ACH_Motto', '100', '10', '0.5');
INSERT INTO achievements VALUES ('6', '1', '1', 'ACH_Student', '50', '10', '0.5');
INSERT INTO achievements VALUES ('7', '1', '1', 'ACH_AvatarTags', '50', '10', '0.5');
INSERT INTO achievements VALUES ('8', '1', '1', 'ACH_RespectGiven', '50', '10', '0.5');
INSERT INTO achievements VALUES ('9', '1', '1', 'ACH_Name', '50', '10', '0.5');
INSERT INTO achievements VALUES ('10', '10', '1', 'ACH_RegistrationDuration', '50', '10', '0.5');
INSERT INTO achievements VALUES ('11', '10', '1', 'ACH_Login', '50', '10', '0.5');
INSERT INTO achievements VALUES ('13', '10', '1', 'ACH_RoomEntry', '5', '10', '0.5');
INSERT INTO achievements VALUES ('14', '10', '1', 'ACH_RespectEarned', '50', '10', '0.5');
INSERT INTO achievements VALUES ('15', '10', '1', 'ACH_MGM', '50', '10', '0.5');
INSERT INTO achievements VALUES ('16', '10', '1', 'ACH_AllTimeHotelPresence', '50', '10', '0.5');
INSERT INTO achievements VALUES ('17', '10', '1', 'ACH_TraderPass', '50', '10', '0.5');
INSERT INTO achievements VALUES ('18', '10', '1', 'ACH_AIPerformanceVote', '50', '10', '0.5');
INSERT INTO achievements VALUES ('19', '10', '1', 'ACH_PetFeeding', '50', '10', '0.5');
INSERT INTO achievements VALUES ('20', '10', '1', 'ACH_PetLevelUp', '50', '10', '0.5');
INSERT INTO achievements VALUES ('21', '10', '1', 'ACH_PetLover', '50', '10', '0.5');
INSERT INTO achievements VALUES ('22', '10', '1', 'ACH_PetRespectGiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('23', '10', '1', 'ACH_PetRespectReceiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('24', '10', '1', 'ACH_GiftGiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('25', '10', '1', 'ACH_GiftReceiver', '50', '10', '0.5');
INSERT INTO achievements VALUES ('26', '5', '1', 'ACH_BasicClub', '50', '10', '0.5');
INSERT INTO achievements VALUES ('27', '5', '1', 'ACH_VipClub', '50', '10', '0.5');
INSERT INTO achievements VALUES ('28', '20', '1', 'ACH_BattleBallTilesLocked', '50', '10', '0.5');
INSERT INTO achievements VALUES ('29', '20', '1', 'ACH_GamePlayerExperience', '50', '10', '0.5');
INSERT INTO achievements VALUES ('30', '20', '1', 'ACH_GameAuthorExperience', '50', '10', '0.5');
INSERT INTO achievements VALUES ('31', '1', '0', 'Z64', '50', '10', '0.5');
INSERT INTO achievements VALUES ('32', '1', '0', 'XXX', '0', '10', '0.5');
INSERT INTO achievements VALUES ('33', '1', '0', 'RLX01', '200', '10', '0.5');
INSERT INTO achievements VALUES ('34', '1', '0', 'AC5', '200', '10', '0.5');
INSERT INTO achievements VALUES ('35', '1', '0', 'IT5', '200', '10', '0.5');
INSERT INTO achievements VALUES ('36', '10', '1', 'ACH_Forum', '50', '10', '0.5');
INSERT INTO achievements VALUES ('37', '1', '0', 'RU3', '200', '10', '0.5');
INSERT INTO achievements VALUES ('38', '1', '0', 'VA2', '200', '10', '0.5');
