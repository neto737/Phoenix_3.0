using System;
using System.Collections.Generic;
using Phoenix.Communication.Messages.Avatar;
using Phoenix.Communication.Messages.Wired;
using Phoenix.Communication.Messages.Sound;
using Phoenix.Communication.Messages.Register;
using Phoenix.Communication.Messages.Inventory.Badges;
using Phoenix.Communication.Messages.Recycler;
using Phoenix.Communication.Messages.Users;
using Phoenix.Communication.Messages.Inventory.Trading;
using Phoenix.Communication.Messages.Help;
using Phoenix.Communication.Messages.Rooms.Action;
using Phoenix.Communication.Messages.Rooms.Furniture;
using Phoenix.Communication.Messages.Rooms.Avatar;
using Phoenix.Communication.Messages.Rooms.Chat;
using Phoenix.Communication.Messages.Rooms.Engine;
using Phoenix.Communication.Messages.Rooms.Pets;
using Phoenix.Communication.Messages.Rooms.Session;
using Phoenix.Communication.Messages.Rooms.Settings;
using Phoenix.Communication.Messages.Navigator;
using Phoenix.Communication.Messages.Handshake;
using Phoenix.Communication.Messages.Messenger;
using Phoenix.Communication.Messages.Catalog;
using Phoenix.Communication.Messages.Marketplace;
using Phoenix.Communication.Messages.Inventory.AvatarFX;
using Phoenix.Communication.Messages.Inventory.Furni;
using Phoenix.Communication.Messages.Inventory.Purse;
using Phoenix.Communication.Messages.Inventory.Achievements;
using Phoenix.Communication.Messages.Quest;
using Phoenix.Communication.Messages.FriendStream;
namespace Phoenix.Communication
{
	internal sealed class MessageHandler
	{
        private Dictionary<uint, MessageEvent> Messages = new Dictionary<uint, MessageEvent>();

        public bool Get(uint Id, out MessageEvent ev)
        {
            if (this.Messages.ContainsKey(Id))
            {
                ev = this.Messages[Id];
                return true;
            }
            ev = null;
            return false;
        }

		public void RegisterHandshake()
		{
			this.Messages.Add(512, new DisconnectMessageEvent());
			this.Messages.Add(2002, new GenerateSecretKeyMessageEvent());
			this.Messages.Add(1817, new GetSessionParametersMessageEvent());
			this.Messages.Add(7, new InfoRetrieveMessageEvent());
			this.Messages.Add(206, new InitCryptoMessageEvent());
			this.Messages.Add(196, new PongMessageEvent());
			this.Messages.Add(415, new SSOTicketMessageEvent());
			this.Messages.Add(756, new TryLoginMessageEvent());
			this.Messages.Add(813, new UniqueIDMessageEvent());
			this.Messages.Add(1170, new VersionCheckMessageEvent());
		}
		public void RegisterMessenger()
		{
			this.Messages.Add(37, new AcceptBuddyMessageEvent());
			this.Messages.Add(38, new DeclineBuddyMessageEvent());
			this.Messages.Add(262, new FollowFriendMessageEvent());
			this.Messages.Add(15, new FriendsListUpdateEvent());
			this.Messages.Add(233, new GetBuddyRequestsMessageEvent());
			this.Messages.Add(41, new HabboSearchMessageEvent());
			this.Messages.Add(12, new MessengerInitMessageEvent());
			this.Messages.Add(40, new RemoveBuddyMessageEvent());
			this.Messages.Add(39, new RequestBuddyMessageEvent());
			this.Messages.Add(33, new SendMsgMessageEvent());
			this.Messages.Add(34, new SendRoomInviteMessageEvent());
			this.Messages.Add(490, new FindNewFriendsMessageEvent());
		}
		public void RegisterNavigator()
		{
			this.Messages.Add(19, new AddFavouriteRoomMessageEvent());
			this.Messages.Add(347, new CancelEventMessageEvent());
			this.Messages.Add(345, new CanCreateEventMessageEvent());
			this.Messages.Add(387, new CanCreateRoomMessageEvent());
			this.Messages.Add(346, new CreateEventMessageEvent());
			this.Messages.Add(29, new CreateFlatMessageEvent());
			this.Messages.Add(20, new DeleteFavouriteRoomMessageEvent());
			this.Messages.Add(348, new EditEventMessageEvent());
			this.Messages.Add(385, new GetGuestRoomMessageEvent());
			this.Messages.Add(380, new GetOfficialRoomsMessageEvent());
			this.Messages.Add(382, new GetPopularRoomTagsMessageEvent());
			this.Messages.Add(388, new GetPublicSpaceCastLibsMessageEvent());
			this.Messages.Add(151, new GetUserFlatCatsMessageEvent());
			this.Messages.Add(439, new LatestEventsSearchMessageEvent());
			this.Messages.Add(435, new MyFavouriteRoomsSearchMessageEvent());
			this.Messages.Add(432, new MyFriendsRoomsSearchMessageEvent());
			this.Messages.Add(436, new MyRoomHistorySearchMessageEvent());
			this.Messages.Add(434, new MyRoomsSearchMessageEvent());
			this.Messages.Add(430, new PopularRoomsSearchMessageEvent());
			this.Messages.Add(261, new RateFlatMessageEvent());
			this.Messages.Add(433, new RoomsWhereMyFriendsAreSearchMessageEvent());
			this.Messages.Add(431, new RoomsWithHighestScoreSearchMessageEvent());
			this.Messages.Add(438, new RoomTagSearchMessageEvent());
			this.Messages.Add(437, new RoomTextSearchMessageEvent());
			this.Messages.Add(483, new ToggleStaffPickMessageEvent());
			this.Messages.Add(384, new UpdateNavigatorSettingsMessageEvent());
			this.Messages.Add(386, new UpdateRoomThumbnailMessageEvent());
		}
		public void RegisterRoomsAction()
		{
			this.Messages.Add(440, new CallGuideBotMessageEvent());
			this.Messages.Add(441, new KickBotMessageEvent());
			this.Messages.Add(96, new AssignRightsMessageEvent());
			this.Messages.Add(97, new RemoveRightsMessageEvent());
			this.Messages.Add(155, new RemoveAllRightsMessageEvent());
			this.Messages.Add(95, new KickUserMessageEvent());
			this.Messages.Add(320, new BanUserMessageEvent());
			this.Messages.Add(98, new LetUserInMessageEvent());
		}
		public void RegisterRoomsAvatar()
		{
			this.Messages.Add(94, new WaveMessageEvent());
			this.Messages.Add(93, new DanceMessageEvent());
			this.Messages.Add(79, new LookToMessageEvent());
			this.Messages.Add(471, new ChangeUserNameMessageEvent());
			this.Messages.Add(470, new ChangeUserNameMessageEvent());
		}
		public void RegisterRoomsChat()
		{
			this.Messages.Add(52, new ChatMessageEvent());
			this.Messages.Add(55, new ShoutMessageEvent());
			this.Messages.Add(56, new WhisperMessageEvent());
			this.Messages.Add(317, new StartTypingMessageEvent());
			this.Messages.Add(318, new CancelTypingMessageEvent());
		}
		public void RegisterRoomsEngine()
		{
			this.Messages.Add(480, new SetClothingChangeDataMessageEvent());
			this.Messages.Add(215, new GetFurnitureAliasesMessageEvent());
			this.Messages.Add(390, new GetRoomEntryDataMessageEvent());
			this.Messages.Add(67, new PickupObjectMessageEvent());
			this.Messages.Add(73, new MoveObjectMessageEvent());
			this.Messages.Add(91, new MoveWallItemMessageEvent());
			this.Messages.Add(90, new PlaceObjectMessageEvent());
			this.Messages.Add(83, new GetItemDataMessageEvent());
			this.Messages.Add(84, new SetItemDataMessageEvent());
			this.Messages.Add(85, new RemoveItemMessageEvent());
			this.Messages.Add(75, new MoveAvatarMessageEvent());
			this.Messages.Add(74, new SetObjectDataMessageEvent());
			this.Messages.Add(66, new ApplyRoomEffect());
			this.Messages.Add(182, new GetInterstitialMessageEvent());
		}
		public void RegisterRoomsFurniture()
		{
			this.Messages.Add(392, new UseFurnitureMessageEvent());
			this.Messages.Add(393, new UseFurnitureMessageEvent());
			this.Messages.Add(232, new UseFurnitureMessageEvent());
			this.Messages.Add(314, new UseFurnitureMessageEvent());
			this.Messages.Add(247, new UseFurnitureMessageEvent());
			this.Messages.Add(76, new UseFurnitureMessageEvent());
			this.Messages.Add(183, new CreditFurniRedeemMessageEvent());
			this.Messages.Add(78, new PresentOpenMessageEvent());
			this.Messages.Add(77, new DiceOffMessageEvent());
			this.Messages.Add(341, new RoomDimmerGetPresetsMessageEvent());
			this.Messages.Add(342, new RoomDimmerSavePresetMessageEvent());
			this.Messages.Add(343, new RoomDimmerChangeStateMessageEvent());
			this.Messages.Add(3254, new PlacePostItMessageEvent());
		}
		public void RegisterRoomsPets()
		{
			this.Messages.Add(3001, new GetPetInfoMessageEvent());
			this.Messages.Add(3002, new PlacePetMessageEvent());
			this.Messages.Add(3003, new RemovePetFromFlatMessageEvent());
			this.Messages.Add(3004, new GetPetCommandsMessageEvent());
			this.Messages.Add(3005, new RespectPetMessageEvent());
		}
		public void RegisterRoomsSession()
		{
			this.Messages.Add(53, new QuitMessageEvent());
			this.Messages.Add(391, new OpenFlatConnectionMessageEvent());
			this.Messages.Add(2, new OpenConnectionMessageEvent());
			this.Messages.Add(59, new GoToFlatMessageEvent());
		}
		public void RegisterRoomsSettings()
		{
			this.Messages.Add(400, new GetRoomSettingsMessageEvent());
			this.Messages.Add(401, new SaveRoomSettingsMessageEvent());
			this.Messages.Add(23, new DeleteRoomMessageEvent());
		}
		public void RegisterCatalog()
		{
			this.Messages.Add(101, new GetCatalogIndexEvent());
			this.Messages.Add(102, new GetCatalogPageEvent());
			this.Messages.Add(3031, new GetClubOffersMessageEvent());
			this.Messages.Add(473, new GetGiftWrappingConfigurationEvent());
			this.Messages.Add(3038, new GetHabboBasicMembershipExtendOfferEvent());
			this.Messages.Add(3035, new GetHabboClubExtendOfferMessageEvent());
			this.Messages.Add(3030, new GetIsOfferGiftableEvent());
			this.Messages.Add(3007, new GetSellablePetBreedsEvent());
			this.Messages.Add(3034, new MarkCatalogNewAdditionsPageOpenedEvent());
			this.Messages.Add(3037, new PurchaseBasicMembershipExtensionEvent());
			this.Messages.Add(472, new PurchaseFromCatalogAsGiftEvent());
			this.Messages.Add(100, new PurchaseFromCatalogEvent());
			this.Messages.Add(3036, new PurchaseVipMembershipExtensionEvent());
			this.Messages.Add(129, new RedeemVoucherMessageEvent());
			this.Messages.Add(475, new SelectClubGiftEvent());
		}
		public void RegisterMarketplace()
		{
			this.Messages.Add(3013, new BuyMarketplaceTokensMessageEvent());
			this.Messages.Add(3014, new BuyOfferMessageEvent());
			this.Messages.Add(3015, new CancelOfferMessageEvent());
			this.Messages.Add(3012, new GetMarketplaceCanMakeOfferEvent());
			this.Messages.Add(3020, new GetMarketplaceItemStatsEvent());
			this.Messages.Add(3018, new GetOffersMessageEvent());
			this.Messages.Add(3019, new GetOwnOffersMessageEvent());
			this.Messages.Add(3010, new MakeOfferMessageEvent());
			this.Messages.Add(3016, new RedeemOfferCreditsMessageEvent());
			this.Messages.Add(3011, new GetMarketplaceConfigurationMessageEvent());
		}
		public void RegisterRecycler()
		{
			this.Messages.Add(412, new GetRecyclerPrizesMessageEvent());
			this.Messages.Add(413, new GetRecyclerStatusMessageEvent());
			this.Messages.Add(414, new RecycleItemsMessageEvent());
		}
		public void RegisterQuest()
		{
			this.Messages.Add(3102, new AcceptQuestMessageEvent());
			this.Messages.Add(3101, new GetQuestsMessageEvent());
			this.Messages.Add(3107, new OpenQuestTrackerMessageEvent());
			this.Messages.Add(3106, new RejectQuestMessageEvent());
		}
		public void RegisterInventoryPurse()
		{
			this.Messages.Add(8, new GetCreditsInfoEvent());
		}
		public void RegisterInventoryFurni()
		{
			this.Messages.Add(3000, new GetPetInventoryEvent());
			this.Messages.Add(404, new RequestFurniInventoryEvent());
		}
		public void RegisterInventoryBadges()
		{
			this.Messages.Add(3032, new GetBadgePointLimitsEvent());
			this.Messages.Add(157, new GetBadgesEvent());
			this.Messages.Add(158, new SetActivatedBadgesEvent());
		}
		public void RegisterInventoryTrading()
		{
			this.Messages.Add(68, new UnacceptTradingEvent());
			this.Messages.Add(69, new AcceptTradingEvent());
			this.Messages.Add(71, new OpenTradingEvent());
			this.Messages.Add(72, new AddItemToTradeEvent());
			this.Messages.Add(402, new ConfirmAcceptTradingEvent());
			this.Messages.Add(403, new ConfirmDeclineTradingEvent());
			this.Messages.Add(70, new ConfirmDeclineTradingEvent());
			this.Messages.Add(405, new RemoveItemFromTradeEvent());
		}
		public void RegisterInventoryAvatarFX()
		{
			this.Messages.Add(372, new AvatarEffectSelectedEvent());
			this.Messages.Add(373, new AvatarEffectActivatedEvent());
		}
		public void RegisterInventoryAchievements()
		{
			this.Messages.Add(370, new GetAchievementsEvent());
		}
		public void RegisterAvatar()
		{
			this.Messages.Add(484, new ChangeMottoMessageEvent());
			this.Messages.Add(375, new GetWardrobeMessageEvent());
			this.Messages.Add(376, new SaveWardrobeOutfitMessageEvent());
		}
		public void RegisterRegister()
		{
			this.Messages.Add(44, new UpdateFigureDataMessageEvent());
		}
		public void RegisterUsers()
		{
			this.Messages.Add(26, new ScrGetUserInfoMessageEvent());
			this.Messages.Add(42, new ApproveNameMessageEvent());
			this.Messages.Add(263, new GetUserTagsMessageEvent());
			this.Messages.Add(159, new GetSelectedBadgesMessageEvent());
			this.Messages.Add(230, new GetHabboGroupBadgesMessageEvent());
			this.Messages.Add(231, new GetHabboGroupDetailsMessageEvent());
			this.Messages.Add(3260, new LoadUserGroupsEvent());
			this.Messages.Add(319, new IgnoreUserMessageEvent());
			this.Messages.Add(322, new UnignoreUserMessageEvent());
			this.Messages.Add(371, new RespectUserMessageEvent());
			this.Messages.Add(3257, new JoinGuildEvent());
			this.Messages.Add(3258, new GetGuildFavorite());
			this.Messages.Add(3259, new RemoveGuildFavorite());
		}
		public void RegisterHelp()
		{
			this.Messages.Add(453, new CallForHelpMessageEvent());
			this.Messages.Add(452, new CloseIssuesMessageEvent());
			this.Messages.Add(238, new DeletePendingCallsForHelpMessageEvent());
			this.Messages.Add(457, new GetCfhChatlogMessageEvent());
			this.Messages.Add(416, new GetClientFaqsMessageEvent());
			this.Messages.Add(417, new GetFaqCategoriesMessageEvent());
			this.Messages.Add(420, new GetFaqCategoryMessageEvent());
			this.Messages.Add(418, new GetFaqTextMessageEvent());
			this.Messages.Add(459, new GetModeratorRoomInfoMessageEvent());
			this.Messages.Add(454, new GetModeratorUserInfoMessageEvent());
			this.Messages.Add(456, new GetRoomChatlogMessageEvent());
			this.Messages.Add(458, new GetRoomVisitsMessageEvent());
			this.Messages.Add(455, new GetUserChatlogMessageEvent());
			this.Messages.Add(461, new ModAlertMessageEvent());
			this.Messages.Add(464, new ModBanMessageEvent());
			this.Messages.Add(460, new ModerateRoomMessageEvent());
			this.Messages.Add(200, new ModeratorActionMessageEvent());
			this.Messages.Add(463, new ModKickMessageEvent());
			this.Messages.Add(462, new ModMessageMessageEvent());
			this.Messages.Add(450, new PickIssuesMessageEvent());
			this.Messages.Add(451, new ReleaseIssuesMessageEvent());
			this.Messages.Add(419, new SearchFaqsMessageEvent());
		}
		public void RegisterSound()
		{
			this.Messages.Add(228, new GetSoundSettingsEvent());
			this.Messages.Add(229, new SetSoundSettingsEvent());
			this.Messages.Add(245, new GetSoundMachinePlayListMessageEvent());
			this.Messages.Add(221, new GetSongInfoMessageEvent());
			this.Messages.Add(249, new GetNowPlayingMessageEvent());
			this.Messages.Add(258, new GetJukeboxPlayListMessageEvent());
			this.Messages.Add(259, new GetUserSongDisksMessageEvent());
            this.Messages.Add(255, new AddNewJukeboxCD()); //UPDATED! --> Jukebox fix --> Don't work this SHIIIIIIIIIT!
            this.Messages.Add(256, new RemoveCDToJukebox()); //UPDATED! --> Jukebox fix --> Don't work this SHIIIIIIIIIT!
		}
		public void RegisterWired()
		{
			this.Messages.Add(3050, new UpdateTriggerMessageEvent());
			this.Messages.Add(3051, new UpdateActionMessageEvent());
			this.Messages.Add(3052, new UpdateConditionMessageEvent());
		}
        public void RegisterFriendStream()
        {
            this.Messages.Add(500, new GetEventStreamComposer()); //NEW --> Friend Stream fix
            this.Messages.Add(501, new SetEventStreamingAllowedEvent()); //NEW --> Friend Stream fix
            this.Messages.Add(502, new EventStreamingLikeButton()); //NEW --> Friend Stream fix
        }
		public void UnregisterPackets()
		{
			this.Messages.Clear();
		}
	}
}
