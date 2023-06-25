using Content.Shared.Emoting;
using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Robust.Shared.Serialization;

namespace Content.Shared.Sich.UserRespawn
{
    public abstract class SharedUserRespawnSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<SharedUserRespawnComponent, UseAttemptEvent>(OnAttempt);
            SubscribeLocalEvent<SharedUserRespawnComponent, InteractionAttemptEvent>(OnAttempt);
            SubscribeLocalEvent<SharedUserRespawnComponent, EmoteAttemptEvent>(OnAttempt);
            SubscribeLocalEvent<SharedUserRespawnComponent, DropAttemptEvent>(OnAttempt);
            SubscribeLocalEvent<SharedUserRespawnComponent, PickupAttemptEvent>(OnAttempt);
        }
        public override void Update(float frameTime)
        {
            base.Update(frameTime);
        }
        private void OnAttempt(EntityUid uid, SharedUserRespawnComponent component, CancellableEntityEventArgs args)
        {
            if (!component.CanRespawn)
                args.Cancel();
        }
    }
    /// <summary>
    /// A server to client response for a <see cref="UserRespawnRequestEvent"/>.
    /// 
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class UserRespawnResponseEvent : EntityEventArgs
    {
        /// <summary>
        /// UserName Player which should be respawn
        /// </summary>

        public UserRespawnResponseEvent(string? userName)
        {
        }
    }

    /// <summary>
    ///  A client to server request for 
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class UserRespawnRequestEvent : EntityEventArgs
    {
        public UserRespawnRequestEvent()
        {
        }
    }

    /// <summary>
    /// A server to client response for a <see cref="UserRespawnTimeRequestEvent"/>.
    /// 
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class UserRespawnTimeResponseEvent : EntityEventArgs
    {
        /// <summary>
        /// Time of death player
        /// </summary>
        public TimeSpan _respawn_time;

        public UserRespawnTimeResponseEvent(TimeSpan respawn_time)
        {
            _respawn_time = respawn_time;
        }
    }

    /// <summary>
    ///  A client to server request for 
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class UserRespawnTimeRequestEvent : EntityEventArgs
    {
        public UserRespawnTimeRequestEvent()
        {

        }
    }
}
