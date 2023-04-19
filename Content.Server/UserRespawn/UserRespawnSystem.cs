using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Shared.Ghost;
using Content.Shared.UserRespawn;
using Robust.Server.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.UserRespawn
{
    public sealed class UserRespawnSystem: SharedUserRespawnSystem
    {
        [Dependency] private readonly GhostSystem _ghosts = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeNetworkEvent<UserRespawnRequestEvent>(OnUserRespawnRequest);

        }

        private void OnUserRespawnRequest(UserRespawnRequestEvent msg, EntitySessionEventArgs args)
        {
            if (false)
            {
                Logger.Warning($"User {args.SenderSession.Name} sent an invalid {nameof(UserRespawnRequestEvent)}");
                return;
            }
            var ticker = EntitySystem.Get<GameTicker>();
            EntityManager.TryGetComponent(args.SenderSession.AttachedEntity, out ActorComponent? actor);
            if (actor == null)
            {
                return;
            }
            ticker.Respawn(actor.PlayerSession);
        }
    }
}
