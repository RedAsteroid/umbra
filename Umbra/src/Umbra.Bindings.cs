using Dalamud.Game.Text.SeStringHandling;
using Umbra.AuxBar;
using Umbra.Widgets.System;
using Umbra.Windows;
using Umbra.Windows.Library.Installer;
using Umbra.Windows.Settings;

namespace Umbra;

[Service]
internal sealed class UmbraBindings : IDisposable
{
    [ConfigVariable("IsFirstTimeStart.V3")]
    public static bool IsFirstTimeStart { get; set; } = true;
    
    [ConfigVariable("General.UiScale", "General", null, 50, 250)]
    public static int UiScale { get; set; } = 100;

    [ConfigVariable("General.UseGameMouseCursor", "General")]
    public static bool UseGameMouseCursor { get; set; } = false;

    [ConfigVariable("General.ShowInputControlDescriptions", "General")]
    public static bool ShowInputControlDescriptions { get; set; } = true;
    
    private ICommandManager _commandManager;

    private readonly IChatGui        _chatGui;
    private readonly WindowManager   _windowManager;
    private readonly WidgetManager   _widgetManager;

    public UmbraBindings(
        IChatGui chatGui,
        ICommandManager commandManager,
        WindowManager windowManager,
        WidgetManager widgetManager
    )
    {
        _chatGui        = chatGui;
        _commandManager = commandManager;
        _windowManager  = windowManager;
        _widgetManager  = widgetManager;
        
        _commandManager.AddHandler(
            "/umbra",
            new(HandleUmbraCommand) {
                HelpMessage = "打开 Umbra 设置窗口",
                ShowInHelp  = true,
            }
        );

        _commandManager.AddHandler(
            "/umbra-aux",
            new(HandleUmbraCommand) {
                HelpMessage = "显示或隐藏一个辅助栏\n            用法: /umbra-aux <show|hide|toggle> [名称]",
                ShowInHelp  = true,
            }
        );
        
        _commandManager.AddHandler(
            "/umbra-toggle",
            new(HandleUmbraCommand) {
                HelpMessage = "切换指定的 Umbra 设置\n            用法: /umbra -toggle <设置>\n            如需查看设置列表请使用不带参数的命令: /umbra-toggle",
                ShowInHelp  = true,
            }
        );

        _commandManager.AddHandler(
            "/umbra-toolbar-profile",
            new(HandleUmbraCommand) {
                HelpMessage = "切换工具栏配置\n           用法: /umbra-toolbar-profile <配置名称>",
                ShowInHelp  = true,
            }
        );

        Framework.DalamudPlugin.UiBuilder.OpenConfigUi += OpenSettingsWindow;
        Framework.DalamudPlugin.UiBuilder.OpenMainUi   += OpenSettingsWindow;

        Node.ScaleFactor = 1.0f;

        // #if DEBUG
        // _windowManager.Present("UmbraSettings", new SettingsWindow());
        // #endif

        if (IsFirstTimeStart) {
            _windowManager.Present("Installer", new InstallerWindow());
        }
    }

    public void Dispose()
    {
        _commandManager.RemoveHandler("/umbra");
        _commandManager.RemoveHandler("/umbra-toggle");
        _commandManager.RemoveHandler("/umbra-toolbar-profile");
        _commandManager.RemoveHandler("/umbra-aux");

        _commandManager = null!;

        Framework.DalamudPlugin.UiBuilder.OpenConfigUi -= OpenSettingsWindow;
        Framework.DalamudPlugin.UiBuilder.OpenMainUi   -= OpenSettingsWindow;
    }

    private void OpenSettingsWindow()
    {
        _windowManager.Present("UmbraSettings", new SettingsWindow());
    }

    [OnTick(interval: 60)]
    private void OnTick()
    {
        Node.ScaleFactor = (float)Math.Round(Math.Clamp(UiScale / 100f, 0.5f, 3.0f), 2);

        Framework.DalamudPlugin.UiBuilder.OverrideGameCursor = !UseGameMouseCursor;
    }

    private void HandleUmbraCommand(string command, string args)
    {
        switch (command.ToLower()) {
            case "/umbra":
                _windowManager.Present("UmbraSettings", new SettingsWindow());
                break;
            case "/umbra-toggle":
                string arg  = args.Trim();

                if (arg == string.Empty) {
                    ShowCvarToggleHelp();
                    return;
                }

                Cvar?  cvar = ConfigManager.GetCvar(arg);

                if (cvar is not { Default: bool }) {
                    _chatGui.PrintError($"Invalid setting: \"{arg}\".");
                    return;
                }

                ConfigManager.Set(cvar.Id, !(bool)cvar.Value!);
                break;
            case "/umbra-toolbar-profile":
                string profile = args.Trim();

                if (profile == string.Empty || !_widgetManager.GetProfileNames().Contains(profile)) {
                    _chatGui.PrintError("Usage: /umbra-toolbar-profile <profile name>.");
                    _chatGui.Print($"Available profiles: {String.Join(", ", _widgetManager.GetProfileNames())}");
                    return;
                }

                _widgetManager.ActivateProfile(profile);
                break;
            case "/umbra-aux":
                var parts  = args.Trim().Split(' ');
                var cmd = parts.FirstOrDefault();
                var name = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : string.Empty;
                
                if (cmd != "show" && cmd != "hide" && cmd != "toggle") {
                    _chatGui.PrintError("Usage: /umbra-aux <show|hide|toggle> [name].");
                    return;
                }

                var am = Framework.Service<AuxBarManager>();
                if (!am.NameExists(name)) {
                    _chatGui.PrintError($"Invalid auxiliary toolbar name: \"{name}\".");
                    _chatGui.Print($"Available toolbars: {String.Join(", ", am.All.Select(a => a.Name))}");
                    return;
                }
                
                am.ToggleByName(name, cmd == "toggle" ? null : cmd == "show");
                break;
        }
    }

    private void ShowCvarToggleHelp()
    {
        SeStringBuilder builder = new();

        foreach (string category in ConfigManager.GetCategories()) {
            if (!I18N.Has($"CVAR.Group.{category}")) continue;

            var cvars = ConfigManager.GetVariablesFromCategory(category).Where(c => c.Default is bool && I18N.Has($"CVAR.{c.Id}.Name")).ToList();
            if (cvars.Count == 0) continue;

            builder.AddText("Category: ").AddUiForeground(I18N.Translate($"CVAR.Group.{category}"), 42).AddText("\n");

            foreach (var cvar in cvars) {
                builder.AddText("    \"").AddUiForeground(cvar.Id, 32).AddText("\" - ").AddUiForeground(I18N.Translate($"CVAR.{cvar.Id}.Name"), 4).AddText("\n");
            }
        }

        builder.AddText("Usage: ").AddUiForeground("/umbra-toggle <setting>", 32);
        _chatGui.Print(builder.Build());
    }
}
