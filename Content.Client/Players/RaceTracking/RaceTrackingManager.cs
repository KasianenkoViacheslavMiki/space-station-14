using System.Diagnostics.CodeAnalysis;
using System.Text;
using Content.Client.Preferences;
using Content.Client.Preferences.UI;
using Content.Shared.CCVar;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Client;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Client.Players.RaceTracking
{
    public sealed class RaceTrackingManager
    {
        [Dependency] private readonly IBaseClient _client = default!;
        [Dependency] private readonly IClientNetManager _net = default!;
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypes = default!;
        [Dependency] private readonly IClientPreferencesManager _clientPreferences = default!;

        string? species = ""; 

        public void Initialize()
        {
            _client.RunLevelChanged += ClientOnRunLevelChanged;
        }

        private void ClientOnRunLevelChanged(object? sender, RunLevelChangedEventArgs e)
        {
            species = "";
        }

        public bool IsAllowed(JobPrototype job, [NotNullWhen(false)] out string? reason)
        {
            reason = null;

            var preferences = _clientPreferences.Preferences;
            if (preferences == null)
            {
                reason = "";
                return false;
            }
            species = ((HumanoidCharacterProfile) preferences.SelectedCharacter).Species;

            if (job.Requirements == null ||
                !_cfg.GetCVar(CCVars.GameRoleTimers))
                return true;

            var player = _playerManager.LocalPlayer?.Session;

            if (player == null) return true;

            var reasonBuilder = new StringBuilder();

            var first = true;
            foreach (var requirement in job.Requirements)
            {
                if (JobRequirements.TryRequirementMet(requirement, species, out reason, _prototypes))
                    continue;

                if (!first)
                    reasonBuilder.Append('\n');
                first = false;

                reasonBuilder.AppendLine(reason);
            }

            reason = reasonBuilder.Length == 0 ? null : reasonBuilder.ToString();
            return reason == null;
        }
    }
}
