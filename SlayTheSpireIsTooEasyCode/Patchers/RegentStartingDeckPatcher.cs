using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Cards;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;

[HarmonyPatch(typeof(Regent), nameof(Regent.StartingDeck), MethodType.Getter)]
public static class RegentStartingDeckPatcher
{
    public static void Postfix(ref IEnumerable<CardModel> __result)
    {
        if (ConfigManager.Instance?.GeneticSnakeBite != true)
        {
            return;
        }

        __result = __result
            .Where(card => card is not StrikeRegent)
            .Concat([ModelDb.Card<GeneticSnakeBite>()]);
    }
}
