ALTER TABLE `server_settings` ADD `enable_externalchatlinks` enum('disabled','blacklist','whitelist') NOT NULL DEFAULT 'disabled';

DROP TABLE IF EXISTS `linkfilter`;
CREATE TABLE `linkfilter` (
  `externalsite` varchar(80) NOT NULL,
  PRIMARY KEY (`externalsite`),
  KEY `site` (`externalsite`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;