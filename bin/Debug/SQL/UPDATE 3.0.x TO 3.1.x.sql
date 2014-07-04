-- --------------------------------------------------------------------------------------------------------------------------
-- This script will merge your user_items & room_items tables into a new items table, MAKE A BACKUP BEFORE YOU RUN THIS!!
-- --------------------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS `items_copy`;
CREATE TABLE `items_copy` (
  `id` int(10) unsigned NOT NULL,
  `user_id` int(10) NOT NULL DEFAULT '0',
  `room_id` int(10) unsigned NOT NULL DEFAULT '0',
  `base_item` int(10) unsigned NOT NULL,
  `extra_data` text NOT NULL,
  `x` int(11) NOT NULL DEFAULT '0',
  `y` int(11) NOT NULL DEFAULT '0',
  `z` double NOT NULL DEFAULT '0',
  `rot` int(11) NOT NULL DEFAULT '0',
  `wall_pos` varchar(100) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `id` int(10) unsigned NOT NULL,
  `user_id` int(10) NOT NULL DEFAULT '0',
  `room_id` int(10) unsigned NOT NULL DEFAULT '0',
  `base_item` int(10) unsigned NOT NULL,
  `extra_data` text NOT NULL,
  `x` int(11) NOT NULL DEFAULT '0',
  `y` int(11) NOT NULL DEFAULT '0',
  `z` double NOT NULL DEFAULT '0',
  `rot` int(11) NOT NULL DEFAULT '0',
  `wall_pos` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`) USING BTREE,
  KEY `userid` (`user_id`) USING BTREE,
  KEY `roomid` (`room_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

INSERT INTO `items_copy` (id, user_id, base_item, extra_data) SELECT DISTINCT * FROM `user_items`;
INSERT INTO `items_copy` (id, room_id, base_item, extra_data, x, y, z, rot, wall_pos) SELECT DISTINCT * FROM `room_items`;

INSERT INTO `items` SELECT * FROM `items_copy` GROUP BY id;

DROP TABLE IF EXISTS `items_copy`;
DROP TABLE IF EXISTS `user_items`;
DROP TABLE IF EXISTS `room_items`;

-- --------------------------------------------------------------------------------------------------------------------------
-- Done with the main crap, updating the server_settings table & catalog_marketplace_offers tables :)
-- --------------------------------------------------------------------------------------------------------------------------

ALTER TABLE server_settings
 ADD (	`enable_chatlogs` enum('0','1') NOT NULL DEFAULT '1',
	`enable_roomlogs` enum('0','1') NOT NULL DEFAULT '1'
	);

ALTER TABLE catalog_marketplace_offers
 ADD `furni_id` int(10) unsigned NOT NULL;