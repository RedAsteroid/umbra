﻿/* Umbra | (c) 2024 by Una              ____ ___        ___.
 * Licensed under the terms of AGPL-3  |    |   \ _____ \_ |__ _______ _____
 *                                     |    |   //     \ | __ \\_  __ \\__  \
 * https://github.com/una-xiv/umbra    |    |  /|  Y Y  \| \_\ \|  | \/ / __ \_
 *                                     |______//__|_|  /____  /|__|   (____  /
 *     Umbra is free software: you can redistribute  \/     \/             \/
 *     it and/or modify it under the terms of the GNU Affero General Public
 *     License as published by the Free Software Foundation, either version 3
 *     of the License, or (at your option) any later version.
 *
 *     Umbra UI is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU Affero General Public License for more details.
 */

using Umbra.Common;
using Umbra.Widgets.System;

namespace Umbra.Widgets;

[Service]
internal class DefaultWidgets
{
    public DefaultWidgets(WidgetManager wm)
    {
        wm.RegisterWidget<ClockWidget>(
            new(
                "Clock",
                I18N.Translate("Widget.Clock.Name"),
                I18N.Translate("Widget.Clock.Description")
            )
        );

        wm.RegisterWidget<DtrBarWidget>(
            new(
                "DtrBar",
                I18N.Translate("Widget.DtrBar.Name"),
                I18N.Translate("Widget.DtrBar.Description")
            )
        );

        wm.RegisterWidget<SpacerWidget>(
            new(
                "Spacer",
                I18N.Translate("Widget.Spacer.Name"),
                I18N.Translate("Widget.Spacer.Description")
            )
        );

        wm.RegisterWidget<MailIndicatorWidget>(
            new(
                "MailIndicator",
                I18N.Translate("Widget.MailIndicator.Name"),
                I18N.Translate("Widget.MailIndicator.Description")
            )
        );

        wm.RegisterWidget<MainMenuWidget>(
            new(
                "MainMenu",
                I18N.Translate("Widget.MainMenu.Name"),
                I18N.Translate("Widget.MainMenu.Description")
            )
        );

        wm.RegisterWidget<WeatherWidget>(
            new(
                "Weather",
                I18N.Translate("Widget.Weather.Name"),
                I18N.Translate("Widget.Weather.Description")
            )
        );

        wm.RegisterWidget<WorldNameWidget>(
            new(
                "WorldName",
                I18N.Translate("Widget.WorldName.Name"),
                I18N.Translate("Widget.WorldName.Description")
            )
        );

        wm.RegisterWidget<LocationWidget>(
            new(
                "Location",
                I18N.Translate("Widget.Location.Name"),
                I18N.Translate("Widget.Location.Description")
            )
        );

        wm.RegisterWidget<CurrenciesWidget>(
            new(
                "Currencies",
                I18N.Translate("Widget.Currencies.Name"),
                I18N.Translate("Widget.Currencies.Description")
            )
        );

        wm.RegisterWidget<OnlineStatusWidget>(
            new(
                "OnlineStatus",
                I18N.Translate("Widget.OnlineStatus.Name"),
                I18N.Translate("Widget.OnlineStatus.Description")
            )
        );

        wm.RegisterWidget<MarkerControlWidget>(new(
            "MarkerControl",
            I18N.Translate("Widget.MarkerControl.Name"),
            I18N.Translate("Widget.MarkerControl.Description")
        ));

        wm.RegisterWidget<AccessibilityWidget>(
            new(
                "Accessibility",
                I18N.Translate("Widget.Accessibility.Name"),
                I18N.Translate("Widget.Accessibility.Description")
            )
        );

        wm.RegisterWidget<CustomButtonWidget>(
            new(
                "CustomButton",
                I18N.Translate("Widget.CustomButton.Name"),
                I18N.Translate("Widget.CustomButton.Description")
            )
        );

        wm.RegisterWidget<FlagWidget>(
            new(
                "Flag",
                I18N.Translate("Widget.Flag.Name"),
                I18N.Translate("Widget.Flag.Description")
            )
        );

        wm.RegisterWidget<WalkingIndicatorWidget>(
            new(
                "WalkingIndicator",
                I18N.Translate("Widget.WalkingIndicator.Name"),
                I18N.Translate("Widget.WalkingIndicator.Description")
            )
        );

        wm.RegisterWidget<CompanionWidget>(
            new(
                "Companion",
                I18N.Translate("Widget.Companion.Name"),
                I18N.Translate("Widget.Companion.Description")
            )
        );

        wm.RegisterWidget<ItemButtonWidget>(new(
            "ItemButton",
            I18N.Translate("Widget.ItemButton.Name"),
            I18N.Translate("Widget.ItemButton.Description")
        ));

        wm.RegisterWidget<BattleEffectsWidget>(
            new(
                "BattleEffects",
                I18N.Translate("Widget.BattleEffects.Name"),
                I18N.Translate("Widget.BattleEffects.Description")
            )
        );

        wm.RegisterWidget<ToolbarPinWidget>(
            new(
                "ToolbarPin",
                I18N.Translate("Widget.ToolbarPin.Name"),
                I18N.Translate("Widget.ToolbarPin.Description")
            )
        );

        wm.RegisterWidget<GearsetSwitcherWidget>(
            new(
                "GearsetSwitcher",
                I18N.Translate("Widget.GearsetSwitcher.Name"),
                I18N.Translate("Widget.GearsetSwitcher.Description")
            )
        );

        wm.RegisterWidget<VolumeWidget>(
            new(
                "Volume",
                I18N.Translate("Widget.Volume.Name"),
                I18N.Translate("Widget.Volume.Description")
            )
        );
    }
}
