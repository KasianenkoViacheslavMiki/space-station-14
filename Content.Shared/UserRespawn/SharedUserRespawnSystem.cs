using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Shared.DragDrop;
using Content.Shared.Emoting;
using Content.Shared.Ghost;
using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.UserInterface.Respawn;
using Robust.Shared.Serialization;

namespace Content.Shared.UserRespawn
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

        public void SetTimeOfDeath(SharedUserRespawnComponent component, DateTime value)
        {
            //component.TimeOfDeath = value;
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
        /// A server to client response for a <see cref="UserTimeOfDeathRequestEvent"/>.
        /// 
        /// </summary>
        [Serializable, NetSerializable]
        public sealed class UserTimeOfDeathResponseEvent : EntityEventArgs
        {
            public UserTimeOfDeathResponseEvent()
            {
            }

            /// <summary>
            /// A list of warp points.
            /// </summary>
        }

        /// <summary>
        ///  A client to server request for 
        /// </summary>
        [Serializable, NetSerializable]
        public sealed class UserTimeOfDeathRequestEvent : EntityEventArgs
        {

            public UserTimeOfDeathRequestEvent()
            {
            }
        }
    }
}
