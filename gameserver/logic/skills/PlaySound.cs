#region

using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.player;
using System.Linq;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class PlaySound : Behavior
    {
        private readonly int soundId;

        public PlaySound(
            int soundId = 0
            )
        {
            this.soundId = soundId;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            foreach (var i in host.GetNearestEntities(25, null).OfType<Player>())
            {
                i.Client.SendMessage(new PLAYSOUND
                {
                    OwnerId = host.Id,
                    SoundId = soundId
                });
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}