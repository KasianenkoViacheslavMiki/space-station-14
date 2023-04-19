using Robust.Shared.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.UserInterface.Respawn
{
    [NetworkedComponent]
    [AutoGenerateComponentState]
    public abstract partial class SharedUserRespawnComponent : Component
    {
        //[DataField("timeOfDeath"), AutoNetworkedField]
        //private DateTime _timeOfDeath = DateTime.Now;

        //[ViewVariables(VVAccess.ReadWrite)]
        //public DateTime TimeOfDeath
        //{
        //    get
        //    {
        //        return _timeOfDeath;
        //    }
        //    set
        //    {
        //        _timeOfDeath = value;
        //    }
        //}

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
