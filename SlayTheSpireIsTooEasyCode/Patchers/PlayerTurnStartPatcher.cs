using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode.Patchers;

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterSideTurnStart))]
public static class HookAfterSideTurnStartPatch
{
    public static async Task Postfix(Task __result, CombatState combatState, CombatSide side)
    {
        await __result;

        if (side != CombatSide.Player)
        {
            return;
        }

        foreach (Creature enemy in combatState.Enemies)
        {
            if (enemy.IsDead)
            {
                continue;
            }

            await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), enemy, 1M, enemy, null);
        }
    }
}