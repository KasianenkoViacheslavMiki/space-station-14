using Content.Server.GameTicking;
using Content.Server.Ghost.Components;
using Content.Shared.Sich.UserRespawn;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Sich.UserRespawn
{
    public sealed class UserRespawnSystem : SharedUserRespawnSystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        [Dependency] private readonly IGameTiming _gameTiming = default!;
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
            var ticker = EntityManager.EntitySysManager.GetEntitySystem<GameTicker>();

            EntityManager.TryGetComponent(args.SenderSession.AttachedEntity, out ActorComponent? actor);
            if (actor == null)
            {
                return;
            }

            ticker.Respawn(actor.PlayerSession);
        }

        private void OnUserTimeOfDeathRequest(UserRespawnTimeRequestEvent msg, EntitySessionEventArgs args)
        {
            if (args.SenderSession.AttachedEntity == null)
            {
                return;
            }

            EntityUid entityUid = (EntityUid) args.SenderSession.AttachedEntity;

            var ghostComponent = Comp<GhostComponent>(entityUid);

            TimeSpan curTime = _gameTiming.CurTime;
            TimeSpan respawn_time = (ghostComponent.TimeOfDeath + _timerForRespawn) - curTime;
            RaiseNetworkEvent(new UserRespawnTimeResponseEvent(respawn_time), args.SenderSession.ConnectedClient);
        }
    }
}
