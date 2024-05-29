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

using System.Collections.Generic;
using Dalamud.Interface;
using Dalamud.Interface.GameFonts;
using Umbra.Common;
using Umbra.Widgets.System;
using Una.Drawing;

namespace Umbra.Widgets;

public partial class MailIndicatorWidget(
    WidgetInfo                  info,
    string?                     guid         = null,
    Dictionary<string, object>? configValues = null
)
    : IconToolbarWidget(info, guid, configValues)
{
    /// <inheritdoc/>
    public override WidgetPopup? Popup => null;

    /// <inheritdoc/>
    protected override void Initialize()
    {
        SetIcon(FontAwesomeIcon.Envelope);
    }

    /// <inheritdoc/>
    protected override void OnUpdate()
    {
        uint unreadMailCount = GetUnreadMailCount();

        SetIconYOffset(GetConfigValue<int>("IconYOffset"));
        SetGhost(!GetConfigValue<bool>("Decorate"));
        SetDisabled(unreadMailCount == 0);

        Node.Tooltip         = I18N.Translate($"Widget.MailIndicator.Tooltip.{(unreadMailCount == 1 ? "Singular" : "Plural")}", unreadMailCount.ToString());
        Node.Style.IsVisible = GetConfigValue<bool>("AlwaysShow") || (unreadMailCount > 0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<IWidgetConfigVariable> GetConfigVariables()
    {
        return [
            new BooleanWidgetConfigVariable(
                "Decorate",
                I18N.Translate("Widget.MailIndicator.Config.Decorate.Name"),
                I18N.Translate("Widget.MailIndicator.Config.Decorate.Description"),
                true
            ),
            new BooleanWidgetConfigVariable(
                "AlwaysShow",
                I18N.Translate("Widget.MailIndicator.Config.AlwaysShow.Name"),
                I18N.Translate("Widget.MailIndicator.Config.AlwaysShow.Description"),
                false
            ),
            new IntegerWidgetConfigVariable(
                "IconYOffset",
                I18N.Translate("Widget.MailIndicator.Config.IconYOffset.Name"),
                I18N.Translate("Widget.MailIndicator.Config.IconYOffset.Description"),
                -1,
                -5,
                5
            )
        ];
    }
}
