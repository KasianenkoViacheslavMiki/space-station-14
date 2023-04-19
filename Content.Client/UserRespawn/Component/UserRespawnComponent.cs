using Content.Shared.UserInterface.Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserRespawn.Component
{
    [RegisterComponent]
    [ComponentReference(typeof(SharedUserRespawnComponent))]
    public sealed class UserRespawnComponent : SharedUserRespawnComponent
    {

    }
}
