ALTER TABLE `permissions_ranks` ADD `cmd_masspixels` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_ranks` ADD `cmd_globalpixels` enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users` ADD `cmd_masspixels` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_globalpixels` enum('1','0') NOT NULL DEFAULT '0';

DROP TABLE IF EXISTS `permissions_vip`;
CREATE TABLE `permissions_vip` (
  `cmdPush` enum('0','1') NOT NULL DEFAULT '1',
  `cmdPull` enum('0','1') NOT NULL DEFAULT '1',
  `cmdFlagme` enum('0','1') NOT NULL DEFAULT '1',
  `cmdMimic` enum('0','1') NOT NULL DEFAULT '1',
  `cmdMoonwalk` enum('0','1') NOT NULL DEFAULT '1'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

INSERT INTO permissions_vip VALUES ('1', '1', '1', '1', '1');