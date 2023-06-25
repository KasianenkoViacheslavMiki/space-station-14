using Robust.Shared.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Sich.UserRespawn
{
    [RegisterComponent, NetworkedComponent, Access(typeof(SharedUserRespawnSystem))]
    public abstract partial class SharedUserRespawnComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        public bool CanRespawn
        {
            get => _canRespawn;
            set
            {
                if (_canRespawn == value) return;
                _canRespawn = value;
                Dirty();
            }
        }

        [DataField("canRespawn"), AutoNetworkedField]
        private bool _canRespawn;
    }
}
