using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Cards;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Apotheoneurosis() : SlayTheSpireIsTooEasyCard(2,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self,
    autoAdd: ConfigManager.Instance?.Apotheoneurosis == true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(4)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Innate
    ];

    public override int MaxUpgradeLevel => 999;

    public override bool HasBuiltInOverlay => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int count = DynamicVars.Cards.IntValue;

        CardModel[] pool = ModelDb.AllCards.ToArray();
        MegaCrit.Sts2.Core.Random.Rng rng = RunState?.Rng.CombatCardSelection
                                            ?? MegaCrit.Sts2.Core.Random.Rng.Chaotic;
        count = Math.Min(count, pool.Length);

        for (int i = 0; i < count; i++)
        {
            int index = rng.NextInt(i, pool.Length);
            (pool[i], pool[index]) = (pool[index], pool[i]);

            CardModel card = CardScope!.CreateCard(pool[i], Owner);
            await CardCmd.AutoPlay(choiceContext, card, play.Target);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(CurrentUpgradeLevel);
    }
}
