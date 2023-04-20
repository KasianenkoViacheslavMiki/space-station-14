using Content.Shared.UserRespawn;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserRespawn
{
    public sealed class UserRespawnSystem: SharedUserRespawnSystem
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IConfigurationManager _cfg = IoCManager.Resolve<IConfigurationManager>();

        float _timer = 0;
        private TimeSpan _respawn_time;
        bool startRespawnTimer = false;

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
            startRespawnTimer = true;
            UserTimeOfDeathResponse?.Invoke(msg);
        }
        public void OnUserRespawnTimeRequest()
        {
            var msg = new UserRespawnTimeRequestEvent();
            RaiseNetworkEvent(msg);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);
            //if (startRespawnTimer)
            //{
            //    var old = _respawn_time;
            //    _respawn_time -= TimeSpan.FromMilliseconds(frameTime * 100);
            //    var difference = _gameTicker.StartTime - _gameTiming.CurTime;
            //    var seconds = difference.TotalSeconds;
            //    if (seconds < 0)
            //    {
            //        text = Loc.GetString(seconds < -5 ? "lobby-state-right-now-question" : "lobby-state-right-now-confirmation");
            //    }
            //    else
            //    {
            //        text = $"{difference.Minutes}:{difference.Seconds:D2}";
            //    }
            //    if (_respawn_time.TotalSeconds < 1)
            //    {

            //    }
            //}
        }
    }
}
