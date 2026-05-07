using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Cards;

[Pool(typeof(SilentCardPool))]
public class GeneticSnakeBite() : SlayTheSpireIsTooEasyCard(
    2,
    CardType.Skill,
    CardRarity.Ancient,
    TargetType.AllEnemies,
    autoAdd: ConfigManager.Instance?.GeneticSnakeBite == true)
{
    private const string IncreaseKey = "Increase";

    private int _currentPoison = 1;
    private int _increasedPoison;

    [SavedProperty]
    public int CurrentPoison
    {
        get => _currentPoison;
        set
        {
            AssertMutable();
            _currentPoison = value;
            DynamicVars.Poison.BaseValue = _currentPoison;
        }
    }

    [SavedProperty]
    public int IncreasedPoison
    {
        get => _increasedPoison;
        set
        {
            AssertMutable();
            _increasedPoison = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PoisonPower>(CurrentPoison),
        new IntVar(IncreaseKey, 1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<PoisonPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Ethereal
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<Creature> enemies = Owner.Creature.CombatState!.Enemies.Where(enemy => !enemy.IsDead).ToList();
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        foreach (Creature enemy in enemies)
        {
            VfxCmd.PlayOnCreatureCenter(enemy, "vfx/vfx_bite");
        }

        await PowerCmd.Apply<PoisonPower>(enemies, DynamicVars.Poison.BaseValue, Owner.Creature, this, true);

        int poisonIncrease = DynamicVars[IncreaseKey].IntValue;
        BuffFromPlay(poisonIncrease);

        if (DeckVersion is GeneticSnakeBite deckVersion)
        {
            deckVersion.BuffFromPlay(poisonIncrease);
        }
    }

    protected override void OnUpgrade() => DynamicVars[IncreaseKey].UpgradeValueBy(1M);

    protected override void AfterDowngraded() => UpdatePoison();

    private void BuffFromPlay(int extraPoison)
    {
        IncreasedPoison += extraPoison;
        UpdatePoison();
    }

    private void UpdatePoison() => CurrentPoison = 1 + IncreasedPoison;
}
