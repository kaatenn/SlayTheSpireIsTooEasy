using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Modding;
using SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;
using System.Reflection;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "SlayTheSpireIsTooEasy"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        ConfigManager config = new();

        Harmony harmony = new(ModId);

        if (config.UpgradeToReplay)
        {
            harmony.CreateClassProcessor(typeof(CardModelMaxUpgradeLevelPatch)).Patch();
            harmony.CreateClassProcessor(typeof(CardModelUpgradeInternalPatch)).Patch();
        }

        if (config.MonsterIntangiblePerTurn)
        {
            harmony.CreateClassProcessor(typeof(HookAfterSideTurnStartPatch)).Patch();
        }

        if (config.ForgeGiveMonsterStrength)
        {
            harmony.CreateClassProcessor(typeof(HookAfterForgePatch)).Patch();
        }

        if (config.GeneticSnakeBite)
        {
            RemoveIncompatibleBaseLibPatches(harmony);
        }

        if (config.StartingDeckHasCustomCards || (config.RemoveAllStartDeck &&
                                                  (config.GeneticSnakeBite || config.Apotheoneurosis)))
        {
            harmony.CreateClassProcessor(typeof(StartingDeckPatcher)).Patch();
        }
    }

    private static void RemoveIncompatibleBaseLibPatches(Harmony harmony)
    {
        MethodInfo? baseLibPatch = AccessTools.Method(
            "BaseLib.Patches.Fixes.CardCmdAutoPlayAnyPlayerPatch:RandomAnyPlayer");
        MethodInfo? autoPlay = AccessTools.Method(typeof(CardCmd), nameof(CardCmd.AutoPlay));

        if (baseLibPatch == null || autoPlay == null)
        {
            Logger.Warn("Could not locate BaseLib CardCmd.AutoPlay compatibility patch for removal.");
            return;
        }

        harmony.Unpatch(autoPlay, baseLibPatch);
        Logger.Info("Removed incompatible BaseLib CardCmd.AutoPlay RandomAnyPlayer patch.");
    }
}
