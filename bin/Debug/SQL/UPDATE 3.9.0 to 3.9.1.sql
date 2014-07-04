INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_emptyitems_success', 'Inventory items cleared!');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_emptypets_success', 'Inventory pets cleared!');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_emptypets_name', 'emptypets');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('cmd_emptypets_desc', ':emptypets - Removes all pets from your inventory');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('marketplace_error_expired', 'Sorry, this offer has expired.');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('marketplace_error_credits', 'Sorry, you don\'t have enough credits!');
INSERT INTO `texts` (`identifier`, `display_text`) VALUES ('wired_error_permissions', 'Saving item failed, you do not have permission to play with this item');


ALTER TABLE `furniture`
MODIFY COLUMN `interaction_type`  enum('default','gate','postit','roomeffect','dimmer','trophy','bed','scoreboard','vendingmachine','alert','onewaygate','loveshuffler','habbowheel','dice','bottle','teleport','rentals','pet','roller','water','ball','bb_red_gate','bb_green_gate','bb_yellow_gate','bb_puck','bb_blue_gate','bb_patch','bb_teleport','blue_score','green_score','red_score','yellow_score','fbgate','tagpole','counter','red_goal','blue_goal','yellow_goal','green_goal','wired','wf_trg_onsay','wf_act_saymsg','wf_trg_enterroom','wf_act_moveuser','wf_act_togglefurni','wf_trg_furnistate','wf_trg_onfurni','pressure_pad','wf_trg_offfurni','wf_trg_gameend','wf_trg_gamestart','wf_trg_timer','wf_act_givepoints','wf_trg_attime','wf_trg_atscore','wf_act_moverotate','rollerskate','stickiepole','wf_xtra_random','wf_cnd_trggrer_on_frn','wf_cnd_furnis_hv_avtrs','wf_act_matchfurni','wf_cnd_has_furni_on','puzzlebox','switch','wf_act_give_phx','wf_cnd_phx') NOT NULL DEFAULT 'default';

ALTER TABLE `permissions_ranks`
ADD COLUMN `wired_give_sql`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_badge`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_effect`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_award`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_send`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_credits`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_pixels`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_points`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_rank`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_roomusers`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasachievement`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasbadge`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasvip`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhaseffect`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userrank`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_usercredits`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userpixels`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userpoints`  enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_users`
ADD COLUMN `wired_give_sql`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_badge`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_effect`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_award`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_send`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_credits`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_pixels`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_points`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_give_rank`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_roomusers`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasachievement`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasbadge`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhasvip`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userhaseffect`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userrank`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_usercredits`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userpixels`  enum('1','0') NOT NULL DEFAULT '0',
ADD COLUMN `wired_cnd_userpoints`  enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `server_settings`
ADD COLUMN `unload_crashedrooms` enum('0','1') NOT NULL DEFAULT '1';