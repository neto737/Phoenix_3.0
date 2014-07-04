using Phoenix.HabboHotel.GameClients;
using Phoenix.HabboHotel.Items;
using Phoenix.HabboHotel.Items.Interactors;
using Phoenix.HabboHotel.SoundMachine;
using Phoenix.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.HabboHotel.Items.Interactors
{
    class InteractorJukebox : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            RoomMusicController roomMusicController = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(Item);
            roomMusicController.Stop();
            Session.GetHabbo().CurrentRoom.LoadMusic();
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
            RoomMusicController roomMusicController = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.Stop();
            roomMusicController.UnLinkRoomOutputItem();
            Item.UpdateState(true, true);
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            RoomMusicController roomMusicController = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(Item);

            if ((UserHasRights && (Session != null)) && (Item != null))
            {
                if (roomMusicController.IsPlaying)
                {
                    roomMusicController.Stop();
                }
                else
                {
                    roomMusicController.Start(Request);
                }
            }
        }
    }
}
