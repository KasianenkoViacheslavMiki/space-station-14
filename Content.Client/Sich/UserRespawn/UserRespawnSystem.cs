using Content.Shared.Sich.UserRespawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.Sich.UserRespawn
{
    public sealed class UserRespawnSystem : SharedUserRespawnSystem
    {
        private TimeSpan _respawn_time;

        public TimeSpan Respawn_time { get => _respawn_time; set => _respawn_time = value; }

        public event Action<UserRespawnTimeResponseEvent>? UserTimeOfDeathResponse;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeNetworkEvent<UserRespawnTimeResponseEvent>(OnUserRespawnTimeResponse);
        }
        public bool OnUserRespawnRequest()
        {
            var msg = new UserRespawnRequestEvent();
            RaiseNetworkEvent(msg);
            return true;
        }
        private void OnUserRespawnTimeResponse(UserRespawnTimeResponseEvent msg)
        {
            if (msg._respawn_time == TimeSpan.Zero)
            {
                return;
            }

            this.Respawn_time = msg._respawn_time;
            UserTimeOfDeathResponse?.Invoke(msg);
        }
        public void OnUserRespawnTimeRequest()
        {
            var msg = new UserRespawnTimeRequestEvent();
            RaiseNetworkEvent(msg);
        }
    }
}
