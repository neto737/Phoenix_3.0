/* Project Phoenix version 3.11.0 
 * Last update: Coded enable command and set only for VIP users.
 */
 
ALTER TABLE `permissions_ranks` DROP COLUMN `cmd_enable`;
ALTER TABLE `permissions_vip` ADD COLUMN `cmdEnable` enum('1','0') NOT NULL DEFAULT '1';