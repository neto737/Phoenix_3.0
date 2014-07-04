-- ----------------------------
-- Table structure for `group_requests`
-- ----------------------------
DROP TABLE IF EXISTS `group_requests`;
CREATE TABLE `group_requests` (
  `groupid` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  KEY `groupid` (`groupid`),
  KEY `userid` (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;