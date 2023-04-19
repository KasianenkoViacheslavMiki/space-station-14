using Content.Shared.UserRespawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserRespawn
{
    public sealed class UserRespawnSystem: SharedUserRespawnSystem
    {

        public override void Initialize()
        {
            base.Initialize();
        }
        public bool RespawnUser(string nickname)
        {
            var msg = new UserRespawnRequestEvent(nickname);
            RaiseNetworkEvent(msg);
            return true;
        }
        //float _timer = 0;

        //public Func<float>? OnTickTimer;
        //public Action? OnTimeGone;
        //public Action? OnTimeBegin;
        //public Predicate<string>? Respawn;

        //public void Initialize(float timer)
        //{
        //    _timer = timer;
        //}

        //private void TickTimer(float deltaSeconds)
        //{
        //    _timer -= deltaSeconds;
        //}
        //public bool RespawnUser(string username)
        //{
        //    Respawn?.Invoke(username);
        //    return true;
        //    //IoCManager.Resolve<IClientConsoleHost>().ExecuteCommand(
        //    //                $"respawn \"{username}\"");
        //}
    }
}
