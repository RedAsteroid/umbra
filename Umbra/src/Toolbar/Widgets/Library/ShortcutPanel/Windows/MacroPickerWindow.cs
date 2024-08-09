﻿using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using System.Linq;
using Umbra.Common;

namespace Umbra.Widgets.Library.ShortcutPanel.Windows;

internal abstract class MacroPickerWindow : PickerWindowBase
{
    protected override string Title  { get; } = I18N.Translate("Widget.ShortcutPanel.PickerWindow.Macro.Title");
    protected override string TypeId => _isIndividual ? "IM" : "SM";

    private readonly bool _isIndividual;

    protected unsafe MacroPickerWindow(bool isIndividual)
    {
        _isIndividual = isIndividual;

        var macros = isIndividual
            ? RaptureMacroModule.Instance()->Individual
            : RaptureMacroModule.Instance()->Shared;

        int index = -1;

        foreach (RaptureMacroModule.Macro macro in macros) {
            index++;
            if (!macro.IsNotEmpty()) continue;

            int i = index;

            AddItem(
                macro.Name.ToString(),
                string.Join(';', macro.Lines.ToArray().Where(l => !string.IsNullOrEmpty(l.ToString()))),
                macro.IconId,
                () => {
                    Logger.Info($"Clicked macro index: {i}");
                    SetPickedItemId((uint)i);
                    Close();
                }
            );
        }
    }
}

internal sealed class IndividualMacroPickerWindow() : MacroPickerWindow(true);
internal sealed class SharedMacroPickerWindow() : MacroPickerWindow(false);