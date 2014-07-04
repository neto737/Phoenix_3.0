ALTER TABLE `server_settings` ADD `vipclothesforhcusers` enum('1','0') NOT NULL DEFAULT '1';

-- ----------------------------
-- Table structure for `wordfilter`
-- ----------------------------
DROP TABLE IF EXISTS `wordfilter`;
CREATE TABLE `wordfilter` (
  `word` varchar(100) NOT NULL,
  `replacement` varchar(100) NOT NULL,
  `strict` enum('1','0') NOT NULL,
  PRIMARY KEY (`word`),
  UNIQUE KEY `word` (`word`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of wordfilter
-- ----------------------------