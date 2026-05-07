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

    private const bool RegentStartingDeckHasSnakeBiteDefault = false;

    public static ConfigManager? Instance;

    public bool MonsterIntangiblePerTurn { get; private set; } = MonsterIntangiblePerTurnDefault;

    public bool UpgradeToReplay { get; private set; } = UpgradeToReplayDefault;

    public bool ForgeGiveMonsterStrength { get; private set; } = ForgeGiveMonsterStrengthDefault;

    public bool GeneticSnakeBite { get; private set; } = GeneticSnakeBiteDefault;

    public bool RegentStartingDeckHasSnakeBite { get; private set; } = RegentStartingDeckHasSnakeBiteDefault;

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

        Dictionary<string, Dictionary<string, string>> config = ParseIniConfig(dir);
        MonsterIntangiblePerTurn = ReadBool(config, GameplaySection, nameof(MonsterIntangiblePerTurn),
            MonsterIntangiblePerTurnDefault);
        UpgradeToReplay = ReadBool(config, GameplaySection, nameof(UpgradeToReplay), UpgradeToReplayDefault);
        ForgeGiveMonsterStrength = ReadBool(config, GameplaySection, nameof(ForgeGiveMonsterStrength),
            ForgeGiveMonsterStrengthDefault);
        GeneticSnakeBite = ReadBool(config, CardsSection, nameof(GeneticSnakeBite), GeneticSnakeBiteDefault);
        RegentStartingDeckHasSnakeBite = ReadBool(config, CardsSection, nameof(RegentStartingDeckHasSnakeBite),
            RegentStartingDeckHasSnakeBiteDefault);
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
            $"{nameof(RegentStartingDeckHasSnakeBite)}={RegentStartingDeckHasSnakeBiteDefault.ToString().ToLowerInvariant()}"
        ];

        File.WriteAllLines(configPath, lines);
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
