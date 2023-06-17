using System.Diagnostics.CodeAnalysis;
using Content.Shared.Players.PlayTimeTracking;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Roles
{
    [ImplicitDataDefinitionForInheritors]
    public abstract class JobBlockAbstract { }

    [UsedImplicitly]
    public sealed class JobBlockForSpecie : JobBlockAbstract
    {
        [DataField("nameSpecie")] public string? NameSpecie;

        [DataField("inverted")] public bool Inverted;
    }

    public static class JobBlockForSpecies
    {
        public static bool TryRequirementMet(
            JobBlockAbstract jobBlockAbstract,
            string species,
            [NotNullWhen(false)] out string? reason)
        {

            if (!(jobBlockAbstract is JobBlockForSpecie jobBlockForSpecie))
            {
                reason = "";
                return false;
            }
            reason = null;

            if (jobBlockForSpecie == null)
            {
                reason = "";
                return false;
            }
            var race = jobBlockForSpecie.NameSpecie;

            if (!jobBlockForSpecie.Inverted)
            {
                if (race?.ToLower() != species.ToLower())
                    return true;
                string nameSpecies = Loc.GetString("species-name-" + species.ToLower().Replace("person", ""));
                reason = Loc.GetString("role-timer-race-ban", ("race", nameSpecies.ToLower()));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
