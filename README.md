# SlayTheSpireIsTooEasy

A mod for a series of [Bilibili videos](https://space.bilibili.com/61979921/lists/8070807?type=season).

<div align="center">English | <a href="docs/README.zh-CN.md">简体中文</a></div>

Compatible with: Slay the Spire 2 stable release (non-beta).

## Introduction

This mod began as a set of whimsical ideas proposed by the video owner and other collaborators. I am only the programmer.

The rest of this document is organized as follows:

- [Installation](#installation) shows how to install the mod.
- [Cards](#cards) shows the extra cards provided by this mod.
- [User Config](#user-config) shows the available user config options and which features they control.
- [Collaborators](#collaborators) lists the other collaborators' social links. I am the only programmer for this mod.

## Installation

Download `SlayTheSpireIsTooEasy.zip` from the [release](https://github.com/kaatenn/SlayTheSpireIsTooEasy/releases/tag/v0.0.0).
After extracting it, put `ModConfig.dll` and `ModConfig.pck` into `mods/SlayTheSpireIsTooEasy/` under the game directory.

## Cards

### Genetic Snake Bite

- Cost: 2
- Type: Skill
- Rarity: Ancient
- Target: All enemies
- Keyword: Ethereal

Apply Poison to all enemies. Each time this card is played, permanently increase this card's Poison value for the current run.

Upgrading Genetic Snake Bite increases the amount of Poison gained after each play.

## User Config

The mod reads `config.ini` from the mod directory. If the file does not exist, the mod creates a default config file on startup.

The default config is:

```ini
[Gameplay]
MonsterIntangiblePerTurn=false
UpgradeToReplay=false
ForgeGiveMonsterStrength=false

[Cards]
GeneticSnakeBite=false
RegentStartingDeckHasSnakeBite=false
```

`[Gameplay]` controls gameplay changes:

- `MonsterIntangiblePerTurn`: Monsters gain Intangible each turn.
- `UpgradeToReplay`: Upgrade effects are changed to add replay stacks, and the upgrade limit is removed.
- `ForgeGiveMonsterStrength`: Forge effects give Strength to monsters. On the first forge, monsters are stunned and gain Intangible.

`[Cards]` controls new or modified cards:

- `GeneticSnakeBite`: Enables Genetic Snake Bite.
- `RegentStartingDeckHasSnakeBite`: Adds Genetic Snake Bite to the Regent's starting deck and removes Strike from the starting deck. This setting also requires `GeneticSnakeBite` to be enabled.

Config values support these boolean forms:

- Enabled: `true`, `1`, `yes`, `y`, `on`
- Disabled: `false`, `0`, `no`, `n`, `off`

INI files support blank lines and full-line comments that start with `;` or `#`. Unrecognized config values fall back to the mod defaults.

## Collaborators

- [cakeko](https://space.bilibili.com/61979921)
- [MuyeNaiNai](https://space.bilibili.com/269519870)
- Medicine_Doll
    - [bilibili](https://space.bilibili.com/14396838)
    - [twitter](https://x.com/Medicine_Doll51)
