using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Server.Ghost.Components;
using Content.Shared.Ghost;
using Content.Shared.UserRespawn;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.UserRespawn
{
    public sealed class UserRespawnSystem:  SharedUserRespawnSystem
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeNetworkEvent<UserRespawnRequestEvent>(OnUserRespawnRequest);
            SubscribeNetworkEvent<UserRespawnTimeRequestEvent>(OnUserTimeOfDeathRequest);

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

        private void OnUserTimeOfDeathRequest(UserRespawnTimeRequestEvent msg, EntitySessionEventArgs args)
        {
            if (false)
            {
                Logger.Warning($"User {args.SenderSession.Name} sent an invalid {nameof(UserRespawnRequestEvent)}");
                return;
            }

            //var ticker = EntitySystem.Get<GameTicker>();
            //EntityManager.TryGetComponent(args.SenderSession.AttachedEntity, out ActorComponent? actor);

            //if (actor == null)
            //{
            //    return;
            //}
            EntityUid? entityUid = args.SenderSession.AttachedEntity;
            if (entityUid == null)
            {
                return;
            }
            var ghostComponent = Comp<GhostComponent>((EntityUid)entityUid);
            if (ghostComponent.TimeOfDeath == null)
            {
                return;
            }
            TimeSpan timeOfDeath = ghostComponent.TimeOfDeath;
            TimeSpan timerForRespawn = TimeSpan.FromSeconds(60);
            TimeSpan curTime = _gameTiming.CurTime;
            var respawn_time = (timeOfDeath + timerForRespawn) - curTime;
            var response = new UserRespawnTimeResponseEvent(respawn_time);
            RaiseNetworkEvent(response, args.SenderSession.ConnectedClient);
        }
    }
}
