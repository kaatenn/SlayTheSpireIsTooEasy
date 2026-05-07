# SlayTheSpireIsTooEasy

## 用户设置

模组会在模组目录下读取 `config.ini`。如果文件不存在，模组会在启动时自动创建一份默认配置。

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

`[Gameplay]` 用于游戏机制调整：

- `MonsterIntangiblePerTurn`：怪物每回合获得无形。
- `UpgradeToReplay`：升级相关效果改为重新打出。
- `ForgeGiveMonsterStrength`：锻造相关效果给予怪物力量。

`[Cards]` 用于新增或修改卡牌：

- `GeneticSnakeBite`：启用遗传蛇咬。
- `RegentStartingDeckHasSnakeBite`：将遗传蛇咬加入观者初始牌组，并移除初始牌组中的打击。此设置需要同时启用 `GeneticSnakeBite`。

配置值支持以下布尔写法：

- 开启：`true`、`1`、`yes`、`y`、`on`
- 关闭：`false`、`0`、`no`、`n`、`off`

INI 支持空行，以及以 `;` 或 `#` 开头的整行注释。无法识别的配置值会使用模组默认值。

## 开发

## 构建

## 许可
