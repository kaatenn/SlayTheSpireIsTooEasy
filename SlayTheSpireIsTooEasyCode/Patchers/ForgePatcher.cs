using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterForge))]
public static class HookAfterForgePatch
{
    private sealed class ForgeCombatState
    {
        public bool HasTriggeredFirstForge;
    }

    private static readonly ConditionalWeakTable<CombatState, ForgeCombatState> States = new();

    public static async Task Postfix(
        Task __result,
        CombatState combatState,
        decimal amount,
        Player forger,
        AbstractModel? source)
    {
        await __result;

        ForgeCombatState state = States.GetOrCreateValue(combatState);
        bool isFirstForge = !state.HasTriggeredFirstForge;
        if (isFirstForge)
        {
            state.HasTriggeredFirstForge = true;
        }

        CardModel? sourceCard = source as CardModel;

        foreach (Creature enemy in combatState.Enemies)
        {
            if (enemy.IsDead)
            {
                continue;
            }

            await PowerCmd.Apply<StrengthPower>(
                enemy,
                amount,
                forger.Creature,
                sourceCard,
                true
            );

            if (!isFirstForge)
            {
                continue;
            }

            await CreatureCmd.Stun(enemy);

            await PowerCmd.Apply<IntangiblePower>(
                enemy,
                1M,
                forger.Creature,
                sourceCard,
                true
            );
        }
    }
}
