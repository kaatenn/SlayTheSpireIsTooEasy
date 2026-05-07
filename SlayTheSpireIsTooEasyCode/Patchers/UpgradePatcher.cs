using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;

[HarmonyPatch(typeof(CardModel), "get_MaxUpgradeLevel")]
public static class CardModelMaxUpgradeLevelPatch
{
    public static void Postfix(ref int __result)
    {
        __result = 999;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.UpgradeInternal))]
public static class CardModelUpgradeInternalPatch
{
    private static readonly MethodInfo OnUpgradeMethod =
        AccessTools.Method(typeof(CardModel), "OnUpgrade");

    private static readonly MethodInfo AddReplayMethod =
        AccessTools.Method(typeof(CardModelUpgradeInternalPatch), nameof(AddReplayInstead));

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.Calls(OnUpgradeMethod))
            {
                yield return new CodeInstruction(OpCodes.Call, AddReplayMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    private static void AddReplayInstead(CardModel card)
    {
        card.BaseReplayCount += 1;
    }
}
