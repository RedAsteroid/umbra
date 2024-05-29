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
using Umbra.Windows.Components;
using Una.Drawing;

namespace Umbra.Windows.Library.WidgetConfig;

public partial class WidgetConfigWindow
{
    protected sealed override Node Node { get; } = new() {
        Stylesheet = WidgetConfigWindowStylesheet,
        ClassList  = ["widget-config-window"],
        ChildNodes = [
            new() {
                ClassList = ["widget-config-list--wrapper"],
                Overflow  = false,
                ChildNodes = [
                    new() {
                        ClassList = ["widget-config-list"],
                    }
                ]
            },
            new() {
                ClassList = ["widget-config-footer"],
                ChildNodes = [
                    new() {
                        ClassList = ["widget-config-footer--buttons"],
                        ChildNodes = [
                            new ButtonNode("CloseButton", I18N.Translate("Close")),
                        ]
                    },
                ]
            }
        ]
    };

    private void UpdateNodeSizes()
    {
        Node.Style.Size = ContentSize;

        Node.QuerySelector(".widget-config-list--wrapper")!.Style.Size =
            new(ContentSize.Width, ContentSize.Height - 50);

        Node.QuerySelector(".widget-config-footer")!.Style.Size = new(ContentSize.Width, 50);

        foreach (var widgetNode in Node.QuerySelectorAll(".control")) {
            widgetNode.Style.Size = new(ContentSize.Width - 30, 0);
        }
    }

    private Node ControlsListNode => Node.QuerySelector(".widget-config-list")!;
}
