ALTER TABLE server_settings ADD MaxRoomsPerUser int(4) NOT NULL DEFAULT '50';

ALTER TABLE `permissions_ranks` ADD `cmd_removebadge` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_removebadge` enum('1','0') NOT NULL DEFAULT '0';

ALTER TABLE `permissions_ranks` ADD `cmd_summon` enum('1','0') NOT NULL DEFAULT '0';
ALTER TABLE `permissions_users` ADD `cmd_summon` enum('1','0') NOT NULL DEFAULT '0';

INSERT INTO `catalog_pages` (`id`, `parent_id`, `caption`, `icon_image`, `order_num`, `page_layout`, `page_headline`, `page_text1`, `page_text_details`) VALUES ('169', '14', 'Spider', '95', '0', 'pets', 'catalog_pet_headline1', 'One of the most feared creatures in nature, and perhaps the most misunderstood. The majority of Spiders are predators with sharp fangs that inject venom into their prey- but dont worry, these spiders wont bite you! Maybe...', 'Name your pet:');
INSERT INTO `catalog_items` (`id`, `page_id`, `item_ids`, `catalog_name`, `cost_credits`, `cost_pixels`, `amount`) VALUES ('6000', '169', '20600', 'a0 pet8', '20', '0', '1');
INSERT INTO `furniture` (`id`, `public_name`, `item_name`, `sprite_id`, `allow_recycle`, `allow_trade`, `allow_marketplace_sell`, `allow_gift`, `allow_inventory_stack`, `interaction_type`, `interaction_modes_count`) VALUES ('20600', 'Spiders', 'a0 pet8', '3817', '0', '0', '0', '0', '0', 'pet', '0');

INSERT INTO `catalog_pages` (`id`, `parent_id`, `caption`, `icon_image`, `order_num`, `page_layout`, `page_headline`, `page_text1`, `page_text_details`) VALUES ('170', '14', 'Frog', '97', '0', 'pets', 'catalog_pet_headline1', 'The Frog. Cute, green and slimy! Frogs come in a variety of weird colours and can be found all over the world. Frogs are great jumpers, and make great pets, but are harder to hold onto than a supermodel in a tornado', 'Name your frog:');
INSERT INTO `catalog_items` (`id`, `page_id`, `item_ids`, `catalog_name`, `cost_credits`, `amount`) VALUES ('6001', '170', '20601', 'a0 pet11', '20', '1');
INSERT INTO `furniture` (`id`, `public_name`, `item_name`, `sprite_id`, `allow_recycle`, `allow_trade`, `allow_marketplace_sell`, `allow_gift`, `allow_inventory_stack`, `interaction_type`, `interaction_modes_count`) VALUES ('20601', 'Frogs', 'a0 pet11', '1532', '0', '0', '0', '0', '0', 'pet', '0');

INSERT INTO `catalog_pages` (`id`, `parent_id`, `caption`, `icon_image`, `order_num`, `page_layout`, `page_headline`, `page_text1`, `page_text_details`) VALUES ('171', '14', 'Chick', '107', '0', 'pets', 'catalog_pet_headline1', 'Habboon is full of chicks (the pet kind!) Adopt your new born chicklet now and start training it to perform the Chicken Dance for your friends... it will be a show to remember!', 'Name your chick:');
INSERT INTO `catalog_items` (`id`, `page_id`, `item_ids`, `catalog_name`, `cost_credits`, `amount`) VALUES ('6002', '171', '20602', 'a0 pet10', '20', '1');
INSERT INTO `furniture` (`id`, `public_name`, `item_name`, `sprite_id`, `allow_recycle`, `allow_trade`, `allow_marketplace_sell`, `allow_gift`, `allow_inventory_stack`, `interaction_type`, `interaction_modes_count`) VALUES ('20602', 'Chicks', 'a0 pet10', '1532', '0', '0', '0', '0', '0', 'pet', '0');