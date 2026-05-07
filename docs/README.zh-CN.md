# SlayTheSpireIsTooEasy

这是一个用于 [Bilibili 视频系列](https://space.bilibili.com/61979921/lists/8070807?type=season)的模组。

<div align="center"><a href="../README.md">English</a> | 简体中文</div>

兼容版本：Slay the Spire 2 稳定版（非 beta 版）。

## 简介

这个模组起源于视频作者和其他合作者提出的一些突发奇想。我只是一位普通的程序。

本文档其余部分按以下结构组织：

- [安装](#安装)：说明如何安装模组。
- [卡牌](#卡牌)：说明本模组提供的额外卡牌。
- [用户配置](#用户配置)：说明用户可用的配置项，以及这些配置项对应的功能。
- [合作者](#合作者)：列出其他合作者的社交链接。本模组只有我一名程序员，因此无法提供 collaborator 的 GitHub 信息。

## 安装

从 [release](https://github.com/kaatenn/SlayTheSpireIsTooEasy/releases/tag/v0.0.0) 中下载模组压缩包 `SlayTheSpireIsTooEasy.zip`。
解压后将 `ModConfig.dll` 和 `ModConfig.pck` 放入游戏目录 `mods/SlayTheSpireIsTooEasy/` 即可。

## 卡牌

## 用户配置

模组会从模组目录读取 `config.ini`。如果该文件不存在，模组会在启动时自动创建一份默认配置。

默认配置如下：

```ini
[Gameplay]
MonsterIntangiblePerTurn=false
UpgradeToReplay=false
ForgeGiveMonsterStrength=false

[Cards]
GeneticSnakeBite=false
RegentStartingDeckHasSnakeBite=false
```

`[Gameplay]` 用于控制游戏机制调整：

- `MonsterIntangiblePerTurn`：怪物每回合获得无实体。
- `UpgradeToReplay`：升级相关效果改为增加重放层数，并且解除升级上限。
- `ForgeGiveMonsterStrength`：铸造相关效果改为给予怪物力量；在首次铸造时击晕怪物并使其获得无实体。

`[Cards]` 用于控制新增或修改的卡牌：

- `GeneticSnakeBite`：启用遗传蛇咬。
- `RegentStartingDeckHasSnakeBite`：将遗传蛇咬加入储君初始牌组，并移除初始牌组中的打击。此设置需要同时启用 `GeneticSnakeBite`。

配置值支持以下布尔写法：

- 开启：`true`、`1`、`yes`、`y`、`on`
- 关闭：`false`、`0`、`no`、`n`、`off`

INI 支持空行，以及以 `;` 或 `#` 开头的整行注释。无法识别的配置值会回退到模组默认值。

## 合作者

- [cakeko](https://space.bilibili.com/61979921)
- [MuyeNaiNai](https://space.bilibili.com/269519870)
- Medicine_Doll
  - [bilibili](https://space.bilibili.com/14396838)
  - [twitter](https://x.com/Medicine_Doll51)
