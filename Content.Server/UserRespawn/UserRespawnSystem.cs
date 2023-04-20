using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Server.Ghost.Components;
using Content.Shared.Ghost;
using Content.Shared.UserRespawn;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
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
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        TimeSpan _timerForRespawn = TimeSpan.FromSeconds(120);

        public override void Initialize()
        {
            base.Initialize();
            SubscribeNetworkEvent<UserRespawnRequestEvent>(OnUserRespawnRequest);
            SubscribeNetworkEvent<UserRespawnTimeRequestEvent>(OnUserTimeOfDeathRequest);

            _prototypeManager.TryIndex<TimeRespawnPrototype>("timerespawntime", out TimeRespawnPrototype? timeRespawn);

            if (timeRespawn != null && timeRespawn.TimeToRespawn != null)
            {
                _timerForRespawn = TimeSpan.FromSeconds((double) timeRespawn.TimeToRespawn);
            }
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
            TimeSpan curTime = _gameTiming.CurTime;
            var respawn_time = (timeOfDeath + _timerForRespawn) - curTime;
            var response = new UserRespawnTimeResponseEvent(respawn_time);
            RaiseNetworkEvent(response, args.SenderSession.ConnectedClient);
        }
    }
}
