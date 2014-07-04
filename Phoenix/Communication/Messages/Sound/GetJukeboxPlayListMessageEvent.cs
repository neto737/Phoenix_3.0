using System;
using System.Linq;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.SoundMachine;
using Phoenix.HabboHotel.Rooms;

namespace Phoenix.Communication.Messages.Sound
{
    internal sealed class GetJukeboxPlayListMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                /*ServerMessage Message = new ServerMessage(334u);
                Message.AppendInt32(20);
                Message.AppendInt32(16);
                for (int i = 1; i <= 16; i++)
                {
                    Message.AppendInt32(i);
                    Message.AppendInt32(i);
                }
                Session.SendMessage(Message);*/

                Room currentRoom = Session.GetHabbo().CurrentRoom;
                RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
            }
        }
    }
}
