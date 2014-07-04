/*
Navicat MySQL Data Transfer

Source Server         : Local
Source Server Version : 50141
Source Host           : localhost:3306
Source Database       : phoenix3

Target Server Type    : MYSQL
Target Server Version : 50141
File Encoding         : 65001

Date: 2011-04-11 14:31:27
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `ranks`
-- ----------------------------
DROP TABLE IF EXISTS `ranks`;
CREATE TABLE `ranks` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `badgeid` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of ranks
-- ----------------------------
INSERT INTO ranks VALUES ('1', 'User', null);
INSERT INTO ranks VALUES ('2', 'VIP', 'VIP');
INSERT INTO ranks VALUES ('3', 'Silver Hobba', 'NWB');
INSERT INTO ranks VALUES ('4', 'Gold Hobba', 'HBA');
INSERT INTO ranks VALUES ('5', 'Super Hobba', 'HBA');
INSERT INTO ranks VALUES ('6', 'Moderator', 'ADM');
INSERT INTO ranks VALUES ('7', 'Administrator', 'ADM');
