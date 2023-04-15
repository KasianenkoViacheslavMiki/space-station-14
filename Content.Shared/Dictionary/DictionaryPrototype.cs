using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Dictionary
{
    [Prototype("dictionary")]
    public sealed class DictionaryPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; } = default!;

        [DataField("value")] public IReadOnlyList<UkrainianWord> Values { get; } = new List<UkrainianWord>();
    }

    [ImplicitDataDefinitionForInheritors]
    public abstract class RowDictionaary { }

    [UsedImplicitly]
    public sealed class UkrainianWord : RowDictionaary
    {
        /// <summary>

        /// </summary>
        [DataField("abbreviationvalues")] public string? AbbreviationValues;
        /// <summary>

        /// </summary>
        [DataField("fullvalues")] public string? FullValues;

    }
}
