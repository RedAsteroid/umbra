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
using Umbra.Common;

namespace Umbra.Widgets;

public partial class DtrBarWidget
{
    protected override IEnumerable<IWidgetConfigVariable> GetConfigVariables()
    {
        return [
            new BooleanWidgetConfigVariable(
                "PlainText",
                I18N.Translate("Widget.DtrBar.Config.PlainText.Name"),
                I18N.Translate("Widget.DtrBar.Config.PlainText.Description"),
                false
            ),
            new BooleanWidgetConfigVariable(
                "HideNative",
                I18N.Translate("Widget.DtrBar.Config.HideNative.Name"),
                I18N.Translate("Widget.DtrBar.Config.HideNative.Description"),
                true
            ),
            new SelectWidgetConfigVariable(
                "DecorateMode",
                I18N.Translate("Widget.DtrBar.Config.DecorateMode.Name"),
                I18N.Translate("Widget.DtrBar.Config.DecorateMode.Description"),
                "Always",
                new() {
                    { "Always", I18N.Translate("Widget.DtrBar.Config.DecorateMode.Option.Always") },
                    { "Never", I18N.Translate("Widget.DtrBar.Config.DecorateMode.Option.Never") },
                    { "Auto", I18N.Translate("Widget.DtrBar.Config.DecorateMode.Option.Auto") },
                }
            ),
            new IntegerWidgetConfigVariable(
                "ItemSpacing",
                I18N.Translate("Widget.DtrBar.Config.ItemSpacing.Name"),
                I18N.Translate("Widget.DtrBar.Config.ItemSpacing.Description"),
                6,
                0,
                64
            ),
            new IntegerWidgetConfigVariable(
                "TextYOffset",
                I18N.Translate("Widget.DtrBar.Config.TextYOffset.Name"),
                I18N.Translate("Widget.DtrBar.Config.TextYOffset.Description"),
                1,
                -5,
                5
            )
        ];
    }
}
