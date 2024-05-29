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
using Umbra.Widgets;
using Umbra.Widgets.System;

namespace Umbra.Windows.Settings.Modules;

public partial class WidgetsModule : SettingsModule
{
    public override string Id   => "WidgetsModule";
    public override string Name { get; } = I18N.Translate("Settings.WidgetsModule.Name");

    public WidgetsModule()
    {
        var wm = Framework.Service<WidgetManager>();
        wm.OnWidgetCreated   += OnWidgetInstanceCreated;
        wm.OnWidgetRemoved   += OnWidgetInstanceRemoved;
        wm.OnWidgetRelocated += OnWidgetInstanceRelocated;

        Node.QuerySelector("#Left")!.AppendChild(CreateAddNewButton("Left"));
        Node.QuerySelector("#Center")!.AppendChild(CreateAddNewButton("Center"));
        Node.QuerySelector("#Right")!.AppendChild(CreateAddNewButton("Right"));

        foreach (var widget in wm.GetWidgetInstances()) OnWidgetInstanceCreated(widget);
    }

    public override void OnUpdate()
    {
        UpdateNodeSizes();

        // Update widget instance names.
        foreach (var widget in Framework.Service<WidgetManager>().GetWidgetInstances()) {
            var node = GetColumn(widget.Location).QuerySelector($"#widget-{widget.Id}");
            if (node == null) continue;

            node.QuerySelector(".widget-instance--name")!.NodeValue = widget.GetInstanceName();
        }
    }

    private void OnWidgetInstanceCreated(ToolbarWidget widget)
    {
        var column = GetColumn(widget.Location);
        column.AppendChild(CreateWidgetInstanceNode(widget));
    }

    private void OnWidgetInstanceRemoved(ToolbarWidget widget)
    {
        var column = GetColumn(widget.Location);
        var node   = column.QuerySelector($"#widget-{widget.Id}");

        if (node != null) column.RemoveChild(node);
    }

    private void OnWidgetInstanceRelocated(ToolbarWidget widget, string previousLocation)
    {
        var oldColumn = GetColumn(previousLocation);
        var newColumn = GetColumn(widget.Location);
        var node      = oldColumn.QuerySelector($"#widget-{widget.Id}");

        if (node != null) {
            oldColumn.RemoveChild(node);
            newColumn.AppendChild(node);
        }
    }
}
