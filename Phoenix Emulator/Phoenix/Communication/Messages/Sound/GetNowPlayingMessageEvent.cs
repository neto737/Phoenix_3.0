using System;
using Phoenix.HabboHotel.GameClients;
using Phoenix.Messages;
using Phoenix.HabboHotel.Rooms;
using System.Threading.Tasks;
namespace Phoenix.Communication.Messages.Sound
{
    internal sealed class GetNowPlayingMessageEvent : MessageEvent
    {
        public void parse(GameClient Session, ClientMessage Event)
        {
            if (Session != null)
            {
                Room @class = PhoenixEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null)
                {
                    Room currentRoom = Session.GetHabbo().CurrentRoom;
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (currentRoom.GotMusicController())
                    {
                        currentRoom.GetRoomMusicController().OnNewUserEnter(class2);
                    }
                }
            }
        }

        public void Action(Task Task, GameClient Session, uint RoomID)
        {
            /*ServerMessage Message = new ServerMessage(327u);

            int SongID = new Random().Next(1, 16);

            Message.AppendInt32(SongID);
            Message.AppendInt32(SongID - 1);
            Message.AppendInt32(SongID);
            Message.AppendInt32(0);

            if (Session.GetHabbo().Class14_0 != null)
            {
                Message.AppendInt32(Session.GetHabbo().Class14_0.int_13);
            }
            else
            {
                Message.AppendInt32(0);
            }

            Session.SendMessage(Message);

            Task.Wait(SongManager.GetSong(SongID).Length);

            if (Session.GetConnection() != null)
            {
                if (Session.GetHabbo().Class14_0 != null)
                {
                    if (Session.GetHabbo().CurrentRoomId == RoomID)
                    {
                        Task T = null;
                        T = new Task(delegate() { Action(T, Session, Session.GetHabbo().CurrentRoomId); });
                        T.Start();
                    }
                    else
                    {
                        Task.Wait();
                        Task.Dispose();
                    }
                }
                else
                {
                    Task.Wait();
                    Task.Dispose();
                }
            }
            else
            {
                Task.Wait();
                Task.Dispose();
            }
        }*/

            /*public void Handle(GameClient Session, ClientMessage Event)
            {
                ServerMessage Message = new ServerMessage(327u);
                int RoomSongID;

                Message.AppendInt32(3);
                Message.AppendInt32(6);
                Message.AppendInt32(3);
                Message.AppendInt32(0);

                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                {
                    RoomSongID = @class.ReadInt32("SELECT JukeboxSongID FROM rooms WHERE id = '" + Session.GetHabbo().Class14_0.Id + "'");
                }

                if (RoomSongID > 0)
                {
                    Message.AppendInt32(RoomSongID);
                    Message.AppendInt32(RoomSongID - 1);
                    Message.AppendInt32(RoomSongID);
                    Message.AppendInt32(0);
                }
                else
                {
                    Message.AppendInt32(-1);
                    Message.AppendInt32(-1);
                    Message.AppendInt32(-1);
                    Message.AppendInt32(-1);
                }

                if (Session.GetHabbo().Class14_0 != null)
                {
                    Message.AppendInt32(Session.GetHabbo().Class14_0.int_13);
                }
                else
                {
                    Message.AppendInt32(0);
                }
                Session.SendMessage(Message);
            }*/
        }
    }
}
