using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.Prototypes;

namespace Content.Server.Sich.UserRespawn
{
    [Prototype("timerespawn")]
    public sealed class TimeRespawnPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; } = default!;

        [ViewVariables]
        [DataField("timetorespawn")]
        public double? TimeToRespawn;
    }
}
