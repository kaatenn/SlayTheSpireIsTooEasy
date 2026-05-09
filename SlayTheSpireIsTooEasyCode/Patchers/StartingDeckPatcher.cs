using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Cards;
using System.Reflection;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;

[HarmonyPatch]
public static class StartingDeckPatcher
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        Type[] characterTypes =
        [
            typeof(Ironclad),
            typeof(Silent),
            typeof(Defect),
            typeof(Regent),
            typeof(Necrobinder),
            typeof(Deprived)
        ];

        return characterTypes
            .Select(type => type.GetProperty(nameof(CharacterModel.StartingDeck))?.GetMethod)
            .Where(method => method != null)
            .Cast<MethodBase>();
    }

    public static void Postfix(CharacterModel __instance, ref IEnumerable<CardModel> __result)
    {
        ConfigManager? config = ConfigManager.Instance;
        if (config == null || (!config.StartingDeckHasCustomCards && !config.RemoveAllStartDeck))
        {
            return;
        }

        List<CardModel> customCards = [];
        if (config.GeneticSnakeBite)
        {
            customCards.Add(ModelDb.Card<GeneticSnakeBite>());
        }

        if (config.Apotheoneurosis)
        {
            customCards.Add(ModelDb.Card<Apotheoneurosis>());
        }

        if (customCards.Count == 0)
        {
            return;
        }

        if (config.RemoveAllStartDeck)
        {
            __result = customCards;
            return;
        }

        if (!IsConfiguredCharacter(__instance, config.CharacterModifyStartingDeck))
        {
            return;
        }

        List<CardModel> startingDeck = __result.ToList();
        RemoveCardsForCustomCards(startingDeck, customCards.Count);
        __result = startingDeck.Concat(customCards);
    }

    private static bool IsConfiguredCharacter(CharacterModel character, string configuredCharacter)
    {
        return character.GetType().Name.Equals(configuredCharacter, StringComparison.OrdinalIgnoreCase);
    }

    private static void RemoveCardsForCustomCards(List<CardModel> startingDeck, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CardTag tag = i % 2 == 0 ? CardTag.Strike : CardTag.Defend;
            if (!TryRemoveFirstCardWithTag(startingDeck, tag) &&
                !TryRemoveFirstCardWithTag(startingDeck, tag == CardTag.Strike ? CardTag.Defend : CardTag.Strike))
            {
                return;
            }
        }
    }

    private static bool TryRemoveFirstCardWithTag(List<CardModel> cards, CardTag tag)
    {
        int index = cards.FindIndex(card => card.Tags.Contains(tag));
        if (index < 0)
        {
            return false;
        }

        cards.RemoveAt(index);
        return true;
    }
}