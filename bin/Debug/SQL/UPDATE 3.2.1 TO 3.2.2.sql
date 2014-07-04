ALTER TABLE `permissions_ranks` ADD `cmd_update_filter` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_update_filter` enum('1','0') NOT NULL DEFAULT '0';