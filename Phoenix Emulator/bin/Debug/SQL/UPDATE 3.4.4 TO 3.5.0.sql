ALTER TABLE furniture CHANGE COLUMN interaction_type interaction_type ENUM ('default','gate','postit','roomeffect','dimmer','trophy','bed','scoreboard','vendingmachine','alert','onewaygate','loveshuffler','habbowheel','dice','bottle','teleport','rentals','pet','roller','water','ball','bb_red_gate','bb_green_gate','bb_yellow_gate','bb_puck','bb_blue_gate','bb_patch','bb_teleport','blue_score','green_score','red_score','yellow_score','fbgate','tagpole','counter','red_goal','blue_goal','yellow_goal','green_goal','wired','wf_trg_onsay','wf_act_saymsg','wf_trg_enterroom','wf_act_moveuser','wf_act_togglefurni','wf_trg_furnistate','wf_trg_onfurni','pressure_pad','wf_trg_offfurni','wf_trg_gameend','wf_trg_gamestart','wf_trg_timer','wf_act_givepoints','wf_trg_attime','wf_trg_atscore','wf_act_moverotate','rollerskate', 'stickiepole') DEFAULT 'default';

INSERT INTO `catalog_items` VALUES ('6003', '173', '20603', 'a0 pet12', '20', '0', '0', '1');
INSERT INTO `furniture` VALUES ('20603', 'Dragon', 'a0 pet12', 's', '1', '1', '1', '1', '0', '0', '10117', '0', '0', '0', '0', '0', 'pet', '0', '0', '0');
INSERT INTO `catalog_pages` (`id`, `parent_id`, `caption`, `page_layout`, `page_headline`, `page_text1`, `page_text_details`) VALUES ('173', '14', 'Dragon', 'pets', 'catalog_pet_headline1', 'Rawr', 'Give a name:Pick a color:Pick a race:');

ALTER TABLE `user_stats` ADD `quest_id` int(10) unsigned NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `quest_progress` int(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `lev_builder` int(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `lev_social` int(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `lev_identity` int(10) NOT NULL DEFAULT '0';
ALTER TABLE `user_stats` ADD `lev_explore` int(10) NOT NULL DEFAULT '0';

CREATE TABLE IF NOT EXISTS `quests` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `type` varchar(100) NOT NULL,
  `action` varchar(100) NOT NULL,
  `needofcount` int(10) NOT NULL DEFAULT '1',
  `enabled` enum('0','1') NOT NULL DEFAULT '1',
  `level_num` int(10) NOT NULL DEFAULT '1',
  `pixel_reward` int(10) NOT NULL DEFAULT '50',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=25 ;


INSERT INTO `quests` (`id`, `type`, `action`, `needofcount`, `enabled`, `level_num`, `pixel_reward`) VALUES
(1, 'room_builder', 'MOVE_ITEM', 3, '1', 1, 50),
(2, 'identity', 'CHANGE_FIGURE', 1, '1', 1, 50),
(3, 'social', 'CHAT_WITH_SOMEONE', 1, '1', 6, 50),
(4, 'social', 'REQUEST_FRIEND', 1, '1', 2, 50),
(5, 'social', 'GIVE_RESPECT', 1, '1', 3, 50),
(7, 'room_builder', 'ROTATE_ITEM', 3, '1', 2, 50),
(6, 'social', 'DANCE', 1, '1', 5, 50),
(8, 'social', 'WAVE', 1, '1', 4, 50),
(9, 'room_builder', 'STACKITEM', 3, '1', 3, 50),
(10, 'room_builder', 'PICKUPITEM', 1, '1', 4, 50),
(11, 'room_builder', 'PLACEWALLPAPER', 1, '1', 7, 50),
(12, 'room_builder', 'SWITCHSTATE', 3, '1', 6, 50),
(13, 'room_builder', 'PLACEFLOOR', 1, '1', 8, 50),
(14, 'room_builder', 'PLACEITEM', 1, '1', 5, 50),
(15, 'social', 'ENTEROTHERSROOM', 1, '1', 1, 50),
(16, 'identity', 'WEARBADGE', 1, '1', 3, 50),
(17, 'identity', 'CHANGEMOTTO', 1, '1', 2, 50),
(18, 'explore', 'FINDLIFEGUARDTOWER', 1, '1', 1, 50),
(19, 'explore', 'SWIM', 1, '1', 2, 50),
(20, 'explore', 'FINDSURFBOARD', 1, '1', 3, 50),
(21, 'explore', 'FINDBEETLE', 1, '1', 4, 50),
(22, 'explore', 'FINDNEONFLOOR', 1, '1', 5, 50),
(23, 'explore', 'FINDDISCOBALL', 1, '1', 6, 50),
(24, 'explore', 'FINDJUKEBOX', 1, '1', 7, 50);

CREATE TABLE IF NOT EXISTS `user_quests` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(10) unsigned NOT NULL,
  `quest_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=0;

DELETE FROM `catalog_items` WHERE (`id`>2 AND `id`<70);

INSERT INTO `catalog_items` (`id`, `page_id`, `catalog_name`, `cost_credits`, `cost_pixels`, `cost_snow`, `amount`, `item_ids`) VALUES
(3001, 15, 'wallpaper_single_101', 3, 0, 0, 1, 19896),
(3002, 15, 'wallpaper_single_102', 3, 0, 0, 1, 19896),
(3003, 15, 'wallpaper_single_103', 3, 0, 0, 1, 19896),
(3004, 15, 'wallpaper_single_104', 3, 0, 0, 1, 19896),
(3005, 15, 'wallpaper_single_105', 3, 0, 0, 1, 19896),
(3006, 15, 'wallpaper_single_106', 3, 0, 0, 1, 19896),
(3007, 15, 'wallpaper_single_107', 3, 0, 0, 1, 19896),
(3008, 15, 'wallpaper_single_108', 3, 0, 0, 1, 19896),
(3009, 15, 'wallpaper_single_109', 3, 0, 0, 1, 19896),
(3010, 15, 'wallpaper_single_110', 3, 0, 0, 1, 19896),
(3011, 15, 'wallpaper_single_111', 3, 0, 0, 1, 19896),
(3012, 15, 'wallpaper_single_112', 3, 0, 0, 1, 19896),
(3013, 15, 'wallpaper_single_113', 3, 0, 0, 1, 19896),
(3014, 15, 'wallpaper_single_114', 3, 0, 0, 1, 19896),
(3015, 15, 'wallpaper_single_115', 3, 0, 0, 1, 19896),
(3016, 15, 'wallpaper_single_201', 3, 0, 0, 1, 19896),
(3017, 15, 'wallpaper_single_202', 3, 0, 0, 1, 19896),
(3018, 15, 'wallpaper_single_203', 3, 0, 0, 1, 19896),
(3019, 15, 'wallpaper_single_204', 3, 0, 0, 1, 19896),
(3020, 15, 'wallpaper_single_204', 3, 0, 0, 1, 19896),
(3021, 15, 'wallpaper_single_205', 3, 0, 0, 1, 19896),
(3022, 15, 'wallpaper_single_206', 3, 0, 0, 1, 19896),
(3023, 15, 'wallpaper_single_207', 3, 0, 0, 1, 19896),
(3024, 15, 'wallpaper_single_208', 3, 0, 0, 1, 19896),
(3025, 15, 'wallpaper_single_209', 3, 0, 0, 1, 19896),
(3026, 15, 'wallpaper_single_210', 3, 0, 0, 1, 19896),
(3027, 15, 'wallpaper_single_211', 3, 0, 0, 1, 19896),
(3028, 15, 'wallpaper_single_212', 3, 0, 0, 1, 19896),
(3029, 15, 'wallpaper_single_213', 3, 0, 0, 1, 19896),
(3030, 15, 'wallpaper_single_214', 3, 0, 0, 1, 19896),
(3031, 15, 'wallpaper_single_215', 3, 0, 0, 1, 19896),
(3032, 15, 'wallpaper_single_216', 3, 0, 0, 1, 19896),
(3033, 15, 'wallpaper_single_217', 3, 0, 0, 1, 19896),
(3034, 15, 'wallpaper_single_218', 3, 0, 0, 1, 19896),
(3035, 15, 'wallpaper_single_301', 3, 0, 0, 1, 19896),
(3036, 15, 'wallpaper_single_302', 3, 0, 0, 1, 19896),
(3037, 15, 'wallpaper_single_303', 3, 0, 0, 1, 19896),
(3038, 15, 'wallpaper_single_304', 3, 0, 0, 1, 19896),
(3039, 15, 'wallpaper_single_305', 3, 0, 0, 1, 19896),
(3040, 15, 'wallpaper_single_306', 3, 0, 0, 1, 19896),
(3041, 15, 'wallpaper_single_307', 3, 0, 0, 1, 19896),
(3042, 15, 'wallpaper_single_401', 3, 0, 0, 1, 19896),
(3043, 15, 'wallpaper_single_402', 3, 0, 0, 1, 19896),
(3044, 15, 'wallpaper_single_403', 3, 0, 0, 1, 19896),
(3045, 15, 'wallpaper_single_404', 3, 0, 0, 1, 19896),
(3046, 15, 'wallpaper_single_405', 3, 0, 0, 1, 19896),
(3047, 15, 'wallpaper_single_406', 3, 0, 0, 1, 19896),
(3048, 15, 'wallpaper_single_407', 3, 0, 0, 1, 19896),
(3049, 15, 'wallpaper_single_408', 3, 0, 0, 1, 19896),
(3050, 15, 'wallpaper_single_501', 3, 0, 0, 1, 19896),
(3051, 15, 'wallpaper_single_502', 3, 0, 0, 1, 19896),
(3052, 15, 'wallpaper_single_503', 3, 0, 0, 1, 19896),
(3053, 15, 'wallpaper_single_504', 3, 0, 0, 1, 19896),
(3054, 15, 'wallpaper_single_505', 3, 0, 0, 1, 19896),
(3055, 15, 'wallpaper_single_506', 3, 0, 0, 1, 19896),
(3056, 15, 'wallpaper_single_507', 3, 0, 0, 1, 19896),
(3057, 15, 'wallpaper_single_508', 3, 0, 0, 1, 19896),
(3058, 15, 'wallpaper_single_601', 3, 0, 0, 1, 19896),
(3059, 15, 'wallpaper_single_602', 3, 0, 0, 1, 19896),
(3060, 15, 'wallpaper_single_603', 3, 0, 0, 1, 19896),
(3061, 15, 'wallpaper_single_604', 3, 0, 0, 1, 19896),
(3062, 15, 'wallpaper_single_605', 3, 0, 0, 1, 19896),
(3063, 15, 'wallpaper_single_606', 3, 0, 0, 1, 19896),
(3064, 15, 'wallpaper_single_607', 3, 0, 0, 1, 19896),
(3065, 15, 'wallpaper_single_608', 3, 0, 0, 1, 19896),
(3066, 15, 'wallpaper_single_609', 3, 0, 0, 1, 19896),
(3067, 15, 'wallpaper_single_610', 3, 0, 0, 1, 19896),
(3068, 15, 'wallpaper_single_701', 3, 0, 0, 1, 19896),
(3069, 15, 'wallpaper_single_702', 3, 0, 0, 1, 19896),
(3070, 15, 'wallpaper_single_703', 3, 0, 0, 1, 19896),
(3071, 15, 'wallpaper_single_704', 3, 0, 0, 1, 19896),
(3072, 15, 'wallpaper_single_705', 3, 0, 0, 1, 19896),
(3073, 15, 'wallpaper_single_706', 3, 0, 0, 1, 19896),
(3074, 15, 'wallpaper_single_707', 3, 0, 0, 1, 19896),
(3075, 15, 'wallpaper_single_708', 3, 0, 0, 1, 19896),
(3076, 15, 'wallpaper_single_709', 3, 0, 0, 1, 19896),
(3077, 15, 'wallpaper_single_710', 3, 0, 0, 1, 19896),
(3078, 15, 'wallpaper_single_801', 3, 0, 0, 1, 19896),
(3079, 15, 'wallpaper_single_802', 3, 0, 0, 1, 19896),
(3080, 15, 'wallpaper_single_803', 3, 0, 0, 1, 19896),
(3081, 15, 'wallpaper_single_804', 3, 0, 0, 1, 19896),
(3082, 15, 'wallpaper_single_805', 3, 0, 0, 1, 19896),
(3083, 15, 'wallpaper_single_806', 3, 0, 0, 1, 19896),
(3084, 15, 'wallpaper_single_807', 3, 0, 0, 1, 19896),
(3085, 15, 'wallpaper_single_808', 3, 0, 0, 1, 19896),
(3086, 15, 'wallpaper_single_809', 3, 0, 0, 1, 19896),
(3087, 15, 'wallpaper_single_810', 3, 0, 0, 1, 19896),
(3088, 15, 'wallpaper_single_901', 3, 0, 0, 1, 19896),
(3089, 15, 'wallpaper_single_902', 3, 0, 0, 1, 19896),
(3090, 15, 'wallpaper_single_903', 3, 0, 0, 1, 19896),
(3091, 15, 'wallpaper_single_904', 3, 0, 0, 1, 19896),
(3092, 15, 'wallpaper_single_905', 3, 0, 0, 1, 19896),
(3093, 15, 'wallpaper_single_906', 3, 0, 0, 1, 19896),
(3094, 15, 'wallpaper_single_907', 3, 0, 0, 1, 19896),
(3095, 15, 'wallpaper_single_908', 3, 0, 0, 1, 19896),
(3096, 15, 'wallpaper_single_1001', 3, 0, 0, 1, 19896),
(3097, 15, 'wallpaper_single_1002', 3, 0, 0, 1, 19896),
(3098, 15, 'wallpaper_single_1003', 3, 0, 0, 1, 19896),
(3099, 15, 'wallpaper_single_1004', 3, 0, 0, 1, 19896),
(3100, 15, 'wallpaper_single_1005', 3, 0, 0, 1, 19896),
(3101, 15, 'wallpaper_single_1006', 3, 0, 0, 1, 19896),
(3102, 15, 'wallpaper_single_1007', 3, 0, 0, 1, 19896),
(3103, 15, 'wallpaper_single_1101', 3, 0, 0, 1, 19896),
(3104, 15, 'wallpaper_single_1201', 3, 0, 0, 1, 19896),
(3105, 15, 'wallpaper_single_1301', 3, 0, 0, 1, 19896),
(3106, 15, 'wallpaper_single_1401', 3, 0, 0, 1, 19896),
(3107, 15, 'wallpaper_single_1501', 3, 0, 0, 1, 19896),
(3108, 15, 'wallpaper_single_1601', 3, 0, 0, 1, 19896),
(3109, 15, 'wallpaper_single_1701', 3, 0, 0, 1, 19896),
(3110, 15, 'wallpaper_single_1801', 3, 0, 0, 1, 19896),
(3111, 15, 'wallpaper_single_1901', 3, 0, 0, 1, 19896),
(3112, 15, 'wallpaper_single_1902', 3, 0, 0, 1, 19896),
(3113, 15, 'wallpaper_single_2001', 3, 0, 0, 1, 19896),
(3114, 15, 'wallpaper_single_2002', 3, 0, 0, 1, 19896),
(3115, 15, 'wallpaper_single_2003', 3, 0, 0, 1, 19896),
(3116, 15, 'wallpaper_single_2101', 4, 0, 0, 1, 19896),
(3117, 15, 'wallpaper_single_2102', 4, 0, 0, 1, 19896),
(3118, 15, 'wallpaper_single_2103', 4, 0, 0, 1, 19896),
(3119, 15, 'wallpaper_single_2201', 4, 0, 0, 1, 19896),
(3120, 15, 'wallpaper_single_2202', 4, 0, 0, 1, 19896),
(3121, 15, 'wallpaper_single_2203', 4, 0, 0, 1, 19896),
(3122, 15, 'wallpaper_single_2204', 4, 0, 0, 1, 19896),
(3123, 15, 'wallpaper_single_2205', 4, 0, 0, 1, 19896),
(3124, 15, 'wallpaper_single_2206', 4, 0, 0, 1, 19896),
(3125, 15, 'wallpaper_single_2207', 4, 0, 0, 1, 19896),
(3126, 15, 'wallpaper_single_2301', 4, 0, 0, 1, 19896),
(3127, 15, 'wallpaper_single_2302', 4, 0, 0, 1, 19896),
(3128, 15, 'wallpaper_single_2303', 4, 0, 0, 1, 19896),
(3129, 15, 'wallpaper_single_2304', 4, 0, 0, 1, 19896),
(3130, 15, 'wallpaper_single_2401', 4, 0, 0, 1, 19896),
(3131, 15, 'wallpaper_single_2402', 4, 0, 0, 1, 19896),
(3132, 15, 'wallpaper_single_2403', 4, 0, 0, 1, 19896),
(3133, 15, 'wallpaper_single_2501', 4, 0, 0, 1, 19896),
(3134, 15, 'wallpaper_single_2502', 4, 0, 0, 1, 19896),
(3135, 15, 'wallpaper_single_2503', 4, 0, 0, 1, 19896),
(3136, 15, 'wallpaper_single_2504', 4, 0, 0, 1, 19896),
(3137, 15, 'wallpaper_single_2601', 4, 0, 0, 1, 19896),
(3138, 15, 'wallpaper_single_2602', 4, 0, 0, 1, 19896),
(3139, 15, 'wallpaper_single_2603', 4, 0, 0, 1, 19896),
(3140, 15, 'wallpaper_single_2604', 4, 0, 0, 1, 19896),
(3141, 15, 'wallpaper_single_2701', 4, 0, 0, 1, 19896),
(3142, 15, 'wallpaper_single_2702', 4, 0, 0, 1, 19896),
(3143, 15, 'wallpaper_single_2703', 4, 0, 0, 1, 19896),
(3144, 15, 'wallpaper_single_2704', 4, 0, 0, 1, 19896),
(3145, 15, 'wallpaper_single_2801', 4, 0, 0, 1, 19896),
(3146, 15, 'wallpaper_single_2802', 4, 0, 0, 1, 19896),
(3147, 15, 'wallpaper_single_2803', 4, 0, 0, 1, 19896),
(3148, 15, 'wallpaper_single_2804', 4, 0, 0, 1, 19896),
(3149, 15, 'wallpaper_single_2901', 4, 0, 0, 1, 19896),
(3150, 15, 'wallpaper_single_2902', 4, 0, 0, 1, 19896),
(3151, 15, 'wallpaper_single_2903', 4, 0, 0, 1, 19896),
(3152, 15, 'wallpaper_single_2904', 4, 0, 0, 1, 19896),
(3153, 15, 'wallpaper_single_3001', 3, 0, 0, 1, 19896),
(3154, 15, 'wallpaper_single_3002', 3, 0, 0, 1, 19896),
(3155, 15, 'wallpaper_single_3003', 3, 0, 0, 1, 19896),
(3156, 15, 'wallpaper_single_3004', 3, 0, 0, 1, 19896),
(3157, 15, 'wallpaper_single_3101', 3, 0, 0, 1, 19896),
(3158, 15, 'wallpaper_single_3102', 3, 0, 0, 1, 19896),
(3159, 15, 'wallpaper_single_3103', 3, 0, 0, 1, 19896),
(3160, 15, 'wallpaper_single_3104', 3, 0, 0, 1, 19896),
(3161, 15, 'wallpaper_single_3105', 3, 0, 0, 1, 19896),
(3162, 15, 'wallpaper_single_3106', 3, 0, 0, 1, 19896),
(3163, 15, 'floor_single_101', 2, 0, 0, 1, 19894),
(3164, 15, 'floor_single_102', 2, 0, 0, 1, 19894),
(3165, 15, 'floor_single_102', 2, 0, 0, 1, 19894),
(3166, 15, 'floor_single_104', 2, 0, 0, 1, 19894),
(3167, 15, 'floor_single_105', 2, 0, 0, 1, 19894),
(3168, 15, 'floor_single_106', 2, 0, 0, 1, 19894),
(3169, 15, 'floor_single_107', 0, 100, 0, 1, 19894),
(3170, 15, 'floor_single_108', 2, 0, 0, 1, 19894),
(3171, 15, 'floor_single_109', 2, 0, 0, 1, 19894),
(3172, 15, 'floor_single_110', 0, 100, 0, 1, 19894),
(3173, 15, 'floor_single_111', 2, 0, 0, 1, 19894),
(3174, 15, 'floor_single_201', 2, 0, 0, 1, 19894),
(3175, 15, 'floor_single_202', 2, 0, 0, 1, 19894),
(3176, 15, 'floor_single_202', 2, 0, 0, 1, 19894),
(3177, 15, 'floor_single_204', 2, 0, 0, 1, 19894),
(3178, 15, 'floor_single_205', 2, 0, 0, 1, 19894),
(3179, 15, 'floor_single_206', 2, 0, 0, 1, 19894),
(3180, 15, 'floor_single_207', 2, 0, 0, 1, 19894),
(3181, 15, 'floor_single_208', 2, 0, 0, 1, 19894),
(3182, 15, 'floor_single_209', 2, 0, 0, 1, 19894),
(3183, 15, 'floor_single_210', 2, 0, 0, 1, 19894),
(3184, 15, 'floor_single_211', 2, 0, 0, 1, 19894),
(3185, 15, 'floor_single_201', 2, 0, 0, 1, 19894),
(3186, 15, 'floor_single_202', 2, 0, 0, 1, 19894),
(3187, 15, 'floor_single_202', 2, 0, 0, 1, 19894),
(3188, 15, 'floor_single_204', 2, 0, 0, 1, 19894),
(3189, 15, 'floor_single_205', 2, 0, 0, 1, 19894),
(3190, 15, 'floor_single_206', 2, 0, 0, 1, 19894),
(3191, 15, 'floor_single_207', 2, 0, 0, 1, 19894),
(3192, 15, 'floor_single_401', 2, 0, 0, 1, 19894),
(3193, 15, 'floor_single_402', 2, 0, 0, 1, 19894),
(3194, 15, 'floor_single_402', 2, 0, 0, 1, 19894),
(3195, 15, 'floor_single_404', 2, 0, 0, 1, 19894),
(3196, 15, 'floor_single_405', 2, 0, 0, 1, 19894),
(3197, 15, 'floor_single_406', 2, 0, 0, 1, 19894),
(3198, 15, 'floor_single_407', 2, 0, 0, 1, 19894),
(3199, 15, 'floor_single_408', 2, 0, 0, 1, 19894),
(3200, 15, 'floor_single_409', 2, 0, 0, 1, 19894),
(3201, 15, 'floor_single_410', 2, 0, 0, 1, 19894),
(3202, 15, 'floor_single_501', 2, 0, 0, 1, 19894),
(3203, 15, 'floor_single_502', 2, 0, 0, 1, 19894),
(3204, 15, 'floor_single_502', 2, 0, 0, 1, 19894),
(3205, 15, 'floor_single_504', 2, 0, 0, 1, 19894),
(3206, 15, 'floor_single_505', 2, 0, 0, 1, 19894),
(3207, 15, 'floor_single_506', 2, 0, 0, 1, 19894),
(3208, 15, 'floor_single_507', 2, 0, 0, 1, 19894),
(3209, 15, 'floor_single_601', 2, 0, 0, 1, 19894),
(3210, 15, 'floor_single_602', 2, 0, 0, 1, 19894),
(3211, 15, 'floor_single_602', 2, 0, 0, 1, 19894),
(3212, 15, 'floor_single_604', 2, 0, 0, 1, 19894),
(3213, 15, 'floor_single_605', 2, 0, 0, 1, 19894),
(3214, 15, 'floor_single_606', 2, 0, 0, 1, 19894),
(3215, 15, 'floor_single_607', 2, 0, 0, 1, 19894),
(3216, 15, 'floor_single_608', 2, 0, 0, 1, 19894),
(3217, 15, 'floor_single_609', 2, 0, 0, 1, 19894),
(3218, 15, 'floor_single_610', 2, 0, 0, 1, 19894),
(3219, 15, 'landscape_single_1.1', 3, 0, 0, 1, 19926),
(3220, 15, 'landscape_single_2.1', 3, 0, 0, 1, 19926),
(3221, 15, 'landscape_single_3.1', 3, 0, 0, 1, 19926),
(3222, 15, 'landscape_single_4.1', 3, 0, 0, 1, 19926),
(3223, 15, 'landscape_single_5.1', 3, 0, 0, 1, 19926),
(3224, 15, 'landscape_single_6.1', 3, 0, 0, 1, 19926),
(3225, 15, 'landscape_single_7.1', 3, 0, 0, 1, 19926),
(3226, 15, 'landscape_single_1.2', 3, 0, 0, 1, 19926),
(3227, 15, 'landscape_single_1.3', 3, 0, 0, 1, 19926),
(3228, 15, 'landscape_single_2.3', 3, 0, 0, 1, 19926),
(3229, 15, 'landscape_single_3.3', 3, 0, 0, 1, 19926),
(3230, 15, 'landscape_single_4.3', 3, 0, 0, 1, 19926),
(3231, 15, 'landscape_single_5.3', 3, 0, 0, 1, 19926),
(3232, 15, 'landscape_single_6.3', 3, 0, 0, 1, 19926),
(3233, 15, 'landscape_single_7.3', 3, 0, 0, 1, 19926),
(3234, 15, 'landscape_single_1.4', 3, 0, 0, 1, 19926),
(3235, 15, 'landscape_single_2.4', 3, 0, 0, 1, 19926),
(3236, 15, 'landscape_single_3.4', 3, 0, 0, 1, 19926),
(3237, 15, 'landscape_single_4.4', 3, 0, 0, 1, 19926),
(3238, 15, 'landscape_single_5.4', 3, 0, 0, 1, 19926),
(3239, 15, 'landscape_single_6.4', 3, 0, 0, 1, 19926),
(3240, 15, 'landscape_single_7.4', 3, 0, 0, 1, 19926),
(3241, 15, 'landscape_single_1.5', 3, 0, 0, 1, 19926),
(3242, 15, 'landscape_single_2.5', 3, 0, 0, 1, 19926),
(3243, 15, 'landscape_single_3.5', 3, 0, 0, 1, 19926),
(3244, 15, 'landscape_single_4.5', 3, 0, 0, 1, 19926),
(3245, 15, 'landscape_single_5.5', 3, 0, 0, 1, 19926),
(3246, 15, 'landscape_single_6.5', 3, 0, 0, 1, 19926),
(3247, 15, 'landscape_single_7.5', 3, 0, 0, 1, 19926),
(3248, 15, 'landscape_single_1.6', 3, 0, 0, 1, 19926),
(3249, 15, 'landscape_single_2.6', 3, 0, 0, 1, 19926),
(3250, 15, 'landscape_single_3.6', 3, 0, 0, 1, 19926),
(3251, 15, 'landscape_single_4.6', 3, 0, 0, 1, 19926),
(3252, 15, 'landscape_single_5.6', 3, 0, 0, 1, 19926),
(3253, 15, 'landscape_single_6.6', 3, 0, 0, 1, 19926),
(3254, 15, 'landscape_single_7.6', 3, 0, 0, 1, 19926),
(3255, 15, 'landscape_single_1.7', 3, 0, 0, 1, 19926),
(3256, 15, 'landscape_single_2.7', 3, 0, 0, 1, 19926),
(3257, 15, 'landscape_single_3.7', 3, 0, 0, 1, 19926),
(3258, 15, 'landscape_single_4.7', 3, 0, 0, 1, 19926),
(3259, 15, 'landscape_single_5.7', 3, 0, 0, 1, 19926),
(3260, 15, 'landscape_single_6.7', 3, 0, 0, 1, 19926),
(3261, 15, 'landscape_single_7.7', 3, 0, 0, 1, 19926),
(3262, 15, 'landscape_single_1.8', 3, 0, 0, 1, 19926),
(3263, 15, 'landscape_single_1.9', 3, 0, 0, 1, 19926),
(3264, 15, 'landscape_single_1.10', 3, 0, 0, 1, 19926),
(3265, 15, 'landscape_single_1.11', 3, 0, 0, 1, 19926),
(3266, 15, 'landscape_single_7.12', 3, 0, 0, 1, 19926);