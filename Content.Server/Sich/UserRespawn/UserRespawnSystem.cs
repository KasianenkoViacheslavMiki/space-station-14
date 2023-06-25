using Content.Server.GameTicking;
using Content.Server.Ghost.Components;
using Content.Shared.Sich.UserRespawn;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;


namespace Content.Server.Sich.UserRespawn
{
    public sealed class UserRespawnSystem : SharedUserRespawnSystem
    {
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

            TimeSpan respawn_time =  _timerForRespawn - ghostComponent.TimeOfDeath;
            RaiseNetworkEvent(new UserRespawnTimeResponseEvent(respawn_time), args.SenderSession.ConnectedClient);
        }
    }
}
