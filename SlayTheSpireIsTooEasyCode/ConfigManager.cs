using System.Reflection;

namespace SlayTheSpireIsTooEasy.SlayTheSpireIsTooEasyCode;

public class ConfigManager
{
    private const string ModFolderName = "SlayTheSpireIsTooEasy";

    private const string ConfigFileName = "config.ini";

    private const string GameModFolder = "mods";

    private const string SpecialFolder = "StS2Mods";

    private const string GameplaySection = "Gameplay";

    private const string CardsSection = "Cards";

    private const bool MonsterIntangiblePerTurnDefault = false;

    private const bool UpgradeToReplayDefault = false;

    private const bool ForgeGiveMonsterStrengthDefault = false;

    private const bool GeneticSnakeBiteDefault = false;

    private const bool ApotheoneurosisDefault = false;

    private const bool StartingDeckHasCustomCardsDefault = false;

    private const string CharacterModifyStartingDeckDefault = "Ironclad";

    private const bool RemoveAllStartDeckDefault = false;

    public static ConfigManager? Instance;

    public bool MonsterIntangiblePerTurn { get; private set; } = MonsterIntangiblePerTurnDefault;

    public bool UpgradeToReplay { get; private set; } = UpgradeToReplayDefault;

    public bool ForgeGiveMonsterStrength { get; private set; } = ForgeGiveMonsterStrengthDefault;

    public bool GeneticSnakeBite { get; private set; } = GeneticSnakeBiteDefault;

    public bool Apotheoneurosis { get; private set; } = ApotheoneurosisDefault;

    public bool StartingDeckHasCustomCards { get; private set; } = StartingDeckHasCustomCardsDefault;

    public string CharacterModifyStartingDeck { get; private set; } = CharacterModifyStartingDeckDefault;

    public bool RemoveAllStartDeck { get; private set; } = RemoveAllStartDeckDefault;

    public ConfigManager()
    {
        Instance = this;
        LoadOrCreateConfig();
    }

    private void LoadOrCreateConfig()
    {
        var dir = ResolveModDirectory();
        Directory.CreateDirectory(dir);
        CreateDefaultConfigIfMissing(dir);
        CompleteMissingDefaultConfigEntries(dir);

        Dictionary<string, Dictionary<string, string>> config = ParseIniConfig(dir);
        MonsterIntangiblePerTurn = ReadBool(config, GameplaySection, nameof(MonsterIntangiblePerTurn),
            MonsterIntangiblePerTurnDefault);
        UpgradeToReplay = ReadBool(config, GameplaySection, nameof(UpgradeToReplay), UpgradeToReplayDefault);
        ForgeGiveMonsterStrength = ReadBool(config, GameplaySection, nameof(ForgeGiveMonsterStrength),
            ForgeGiveMonsterStrengthDefault);
        GeneticSnakeBite = ReadBool(config, CardsSection, nameof(GeneticSnakeBite), GeneticSnakeBiteDefault);
        Apotheoneurosis = ReadBool(config, CardsSection, nameof(Apotheoneurosis), ApotheoneurosisDefault);
        StartingDeckHasCustomCards = ReadBool(config, CardsSection, nameof(StartingDeckHasCustomCards),
            StartingDeckHasCustomCardsDefault);
        CharacterModifyStartingDeck = ReadString(config, CardsSection, nameof(CharacterModifyStartingDeck),
            CharacterModifyStartingDeckDefault);
        RemoveAllStartDeck = ReadBool(config, CardsSection, nameof(RemoveAllStartDeck), RemoveAllStartDeckDefault);
    }

    private static void CreateDefaultConfigIfMissing(string rootDir)
    {
        string configPath = Path.Combine(rootDir, ConfigFileName);
        if (File.Exists(configPath))
        {
            return;
        }

        string[] lines =
        [
            $"[{GameplaySection}]",
            $"{nameof(MonsterIntangiblePerTurn)}={MonsterIntangiblePerTurnDefault.ToString().ToLowerInvariant()}",
            $"{nameof(UpgradeToReplay)}={UpgradeToReplayDefault.ToString().ToLowerInvariant()}",
            $"{nameof(ForgeGiveMonsterStrength)}={ForgeGiveMonsterStrengthDefault.ToString().ToLowerInvariant()}",
            string.Empty,
            $"[{CardsSection}]",
            $"{nameof(GeneticSnakeBite)}={GeneticSnakeBiteDefault.ToString().ToLowerInvariant()}",
            $"{nameof(Apotheoneurosis)}={ApotheoneurosisDefault.ToString().ToLowerInvariant()}",
            $"{nameof(StartingDeckHasCustomCards)}={StartingDeckHasCustomCardsDefault.ToString().ToLowerInvariant()}",
            $"{nameof(CharacterModifyStartingDeck)}={CharacterModifyStartingDeckDefault}",
            $"{nameof(RemoveAllStartDeck)}={RemoveAllStartDeckDefault.ToString().ToLowerInvariant()}"
        ];

        File.WriteAllLines(configPath, lines);
    }

    private static void CompleteMissingDefaultConfigEntries(string rootDir)
    {
        string configPath = Path.Combine(rootDir, ConfigFileName);
        Dictionary<string, Dictionary<string, string>> config = ParseIniConfig(rootDir);

        List<string> missingGameplayEntries = GetMissingEntries(config, GameplaySection,
        [
            (nameof(MonsterIntangiblePerTurn), MonsterIntangiblePerTurnDefault),
            (nameof(UpgradeToReplay), UpgradeToReplayDefault),
            (nameof(ForgeGiveMonsterStrength), ForgeGiveMonsterStrengthDefault)
        ]);

        List<string> missingCardEntries = GetMissingEntries(config, CardsSection,
        [
            (nameof(GeneticSnakeBite), GeneticSnakeBiteDefault),
            (nameof(Apotheoneurosis), ApotheoneurosisDefault),
            (nameof(StartingDeckHasCustomCards), StartingDeckHasCustomCardsDefault),
            (nameof(RemoveAllStartDeck), RemoveAllStartDeckDefault)
        ]);

        List<string> missingCardStringEntries = GetMissingEntries(config, CardsSection,
        [
            (nameof(CharacterModifyStartingDeck), CharacterModifyStartingDeckDefault)
        ]);

        if (missingGameplayEntries.Count == 0 && missingCardEntries.Count == 0 && missingCardStringEntries.Count == 0)
        {
            return;
        }

        List<string> lines = File.Exists(configPath)
            ? File.ReadAllLines(configPath).ToList()
            : [];

        AddMissingEntries(lines, GameplaySection, missingGameplayEntries);
        AddMissingEntries(lines, CardsSection, missingCardEntries);
        AddMissingEntries(lines, CardsSection, missingCardStringEntries);
        File.WriteAllLines(configPath, lines);
    }

    private static List<string> GetMissingEntries(
        Dictionary<string, Dictionary<string, string>> config,
        string section,
        IEnumerable<(string Key, bool DefaultValue)> defaults)
    {
        config.TryGetValue(section, out Dictionary<string, string>? sectionConfig);

        return defaults
            .Where(entry => sectionConfig == null || !sectionConfig.ContainsKey(entry.Key))
            .Select(entry => $"{entry.Key}={entry.DefaultValue.ToString().ToLowerInvariant()}")
            .ToList();
    }

    private static List<string> GetMissingEntries(
        Dictionary<string, Dictionary<string, string>> config,
        string section,
        IEnumerable<(string Key, string DefaultValue)> defaults)
    {
        config.TryGetValue(section, out Dictionary<string, string>? sectionConfig);

        return defaults
            .Where(entry => sectionConfig == null || !sectionConfig.ContainsKey(entry.Key))
            .Select(entry => $"{entry.Key}={entry.DefaultValue}")
            .ToList();
    }

    private static void AddMissingEntries(List<string> lines, string section, List<string> entries)
    {
        if (entries.Count == 0)
        {
            return;
        }

        int sectionIndex =
            lines.FindIndex(line => line.Trim().Equals($"[{section}]", StringComparison.OrdinalIgnoreCase));
        if (sectionIndex < 0)
        {
            if (lines.Count > 0 && !string.IsNullOrWhiteSpace(lines[^1]))
            {
                lines.Add(string.Empty);
            }

            lines.Add($"[{section}]");
            lines.AddRange(entries);
            return;
        }

        int insertIndex = sectionIndex + 1;
        while (insertIndex < lines.Count)
        {
            string line = lines[insertIndex].Trim();
            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                break;
            }

            insertIndex++;
        }

        lines.InsertRange(insertIndex, entries);
    }

    private static bool ReadBool(
        Dictionary<string, Dictionary<string, string>> config,
        string section,
        string key,
        bool defaultValue)
    {
        if (!config.TryGetValue(section, out Dictionary<string, string>? sectionConfig) ||
            !sectionConfig.TryGetValue(key, out string? rawValue))
        {
            return defaultValue;
        }

        return ParseBool(rawValue, defaultValue);
    }

    private static string ReadString(
        Dictionary<string, Dictionary<string, string>> config,
        string section,
        string key,
        string defaultValue)
    {
        if (!config.TryGetValue(section, out Dictionary<string, string>? sectionConfig) ||
            !sectionConfig.TryGetValue(key, out string? rawValue))
        {
            return defaultValue;
        }

        string value = rawValue.Trim();
        return value.Length == 0 ? defaultValue : value;
    }

    private static bool ParseBool(string value, bool defaultValue)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "true" or "1" or "yes" or "y" or "on" => true,
            "false" or "0" or "no" or "n" or "off" => false,
            _ => defaultValue
        };
    }

    private static Dictionary<string, Dictionary<string, string>> ParseIniConfig(string rootDir)
    {
        Dictionary<string, Dictionary<string, string>> config = new(StringComparer.OrdinalIgnoreCase);
        string configPath = Path.Combine(rootDir, ConfigFileName);
        if (!File.Exists(configPath))
        {
            return config;
        }

        string currentSection = string.Empty;
        config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (string rawLine in File.ReadLines(configPath))
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith(';') || line.StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                currentSection = line[1..^1].Trim();
                if (!config.ContainsKey(currentSection))
                {
                    config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                continue;
            }

            int separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            string key = line[..separatorIndex].Trim();
            string value = line[(separatorIndex + 1)..].Trim();
            if (key.Length == 0)
            {
                continue;
            }

            config[currentSection][key] = value;
        }

        return config;
    }

    private string ResolveModDirectory()
    {
        string assemblyRoot = Assembly.GetExecutingAssembly().Location;
        string? dir = string.IsNullOrWhiteSpace(assemblyRoot) ? null : Path.GetDirectoryName(assemblyRoot);
        if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
        {
            return dir;
        }

        string modRoot = Path.Combine(AppContext.BaseDirectory, GameModFolder, ModFolderName);
        if (Directory.Exists(modRoot))
        {
            return modRoot;
        }

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SpecialFolder,
            ModFolderName);
    }
}