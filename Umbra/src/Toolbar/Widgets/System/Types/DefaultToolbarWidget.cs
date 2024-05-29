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
using Dalamud.Game.Text.SeStringHandling;
using Umbra.Style;
using Umbra.Widgets.System;
using Una.Drawing;

namespace Umbra.Widgets;

public abstract class DefaultToolbarWidget(
    WidgetInfo                  info,
    string?                     guid         = null,
    Dictionary<string, object>? configValues = null
)
    : ToolbarWidget(info, guid, configValues)
{
    public sealed override Node Node { get; } = new() {
        Stylesheet = WidgetStyles.DefaultWidgetStylesheet,
        ClassList  = ["toolbar-widget-default"],
        ChildNodes = [
            new() {
                Id          = "LeftIcon",
                ClassList   = ["icon"],
                InheritTags = true,
                Style = new() {
                    Margin    = new() { Left = -2 },
                    IsVisible = false
                }
            },
            new() {
                Id          = "Label",
                NodeValue   = "",
                InheritTags = true,
                ChildNodes = [
                    new() { Id = "TopLabel", InheritTags    = true, },
                    new() { Id = "BottomLabel", InheritTags = true, }
                ],
                Style = new() {
                    Padding = new(),
                }
            },
            new() {
                Id          = "RightIcon",
                ClassList   = ["icon"],
                InheritTags = true,
                Style = new() {
                    Margin    = new() { Right = -2 },
                    IsVisible = false
                }
            },
        ],
        BeforeDraw = node => {
            Node leftIconNode    = node.QuerySelector("#LeftIcon")!;
            Node rightIconNode   = node.QuerySelector("#RightIcon")!;
            Node labelNode       = node.QuerySelector("#Label")!;

            bool hasLabelValue = labelNode.NodeValue is not null;

            leftIconNode.Style.IsVisible  = leftIconNode.Style.IconId is not null;
            rightIconNode.Style.IsVisible = rightIconNode.Style.IconId is not null;

            bool leftIconVisible  = leftIconNode.Style.IsVisible ?? false;
            bool rightIconVisible = rightIconNode.Style.IsVisible ?? false;

            labelNode.Style.Padding = new(
                0,
                !hasLabelValue || rightIconVisible ? 0 : 6,
                0,
                !hasLabelValue || leftIconVisible ? 0 : 6
            );
        }
    };

    protected override void OnUpdate() { }

    protected void SetGhost(bool isGhost)
    {
        switch (isGhost) {
            case true when !Node.TagsList.Contains("ghost"):
                Node.TagsList.Add("ghost");
                break;
            case false when Node.TagsList.Contains("ghost"):
                Node.TagsList.Remove("ghost");
                break;
        }
    }

    protected void SetLeftIcon(uint? iconId)
    {
        Node.QuerySelector("LeftIcon")!.Style.IconId = iconId;
    }

    protected void SetRightIcon(uint? iconId)
    {
        Node.QuerySelector("RightIcon")!.Style.IconId = iconId;
    }

    protected void SetIconSize(int size)
    {
        Node.QuerySelector("LeftIcon")!.Style.Size  = new(size, size);
        Node.QuerySelector("RightIcon")!.Style.Size = new(size, size);
    }

    protected void SetLabel(string? label)
    {
        LabelNode.NodeValue       = label;
        LabelNode.Style.IsVisible = !string.IsNullOrEmpty(label);

        TopLabelNode.NodeValue          = null;
        BottomLabelNode.NodeValue       = null;
        TopLabelNode.Style.IsVisible    = false;
        BottomLabelNode.Style.IsVisible = false;
    }

    protected void SetLabel(SeString? label)
    {
        LabelNode.NodeValue = label;

        TopLabelNode.NodeValue          = null;
        BottomLabelNode.NodeValue       = null;
        TopLabelNode.Style.IsVisible    = false;
        BottomLabelNode.Style.IsVisible = false;
    }

    protected void SetTwoLabels(string? topLabel, string? bottomLabel)
    {
        LabelNode.NodeValue = null;

        TopLabelNode.NodeValue    = topLabel;
        BottomLabelNode.NodeValue = bottomLabel;

        TopLabelNode.Style.IsVisible    = !string.IsNullOrEmpty(topLabel);
        BottomLabelNode.Style.IsVisible = !string.IsNullOrEmpty(bottomLabel);
        LabelNode.Style.IsVisible       = !string.IsNullOrEmpty(topLabel) || !string.IsNullOrEmpty(bottomLabel);
    }

    protected void SetLabelMaxWidth(int maxWidth)
    {
        // TODO: Make an actual "maxWidth" style option.
        Node.QuerySelector("Label")!.Style.Size!.Width = maxWidth;
    }

    protected void SetDisabled(bool isDisabled)
    {
        Node.IsDisabled    = isDisabled;
        Node.Style.Opacity = isDisabled ? .66f : 1f;

        Node.QuerySelector("LeftIcon")!.Style.ImageGrayscale  = isDisabled;
        Node.QuerySelector("RightIcon")!.Style.ImageGrayscale = isDisabled;
    }

    protected void SetTextAlignLeft()
    {
        LabelNode.Style.TextAlign       = Anchor.MiddleLeft;
        TopLabelNode.Style.TextAlign    = Anchor.MiddleLeft;
        BottomLabelNode.Style.TextAlign = Anchor.MiddleLeft;
    }

    protected void SetTextAlignRight()
    {
        LabelNode.Style.TextAlign       = Anchor.MiddleRight;
        TopLabelNode.Style.TextAlign    = Anchor.MiddleRight;
        BottomLabelNode.Style.TextAlign = Anchor.MiddleRight;
    }

    protected void SetTextAlignCenter()
    {
        LabelNode.Style.TextAlign       = Anchor.MiddleCenter;
        TopLabelNode.Style.TextAlign    = Anchor.MiddleCenter;
        BottomLabelNode.Style.TextAlign = Anchor.MiddleCenter;
    }

    protected Node LabelNode    => Node.QuerySelector("#Label")!;
    protected Node TopLabelNode => Node.QuerySelector("#TopLabel")!;
    protected Node BottomLabelNode => Node.QuerySelector("#BottomLabel")!;
    protected Node LeftIconNode  => Node.QuerySelector("#LeftIcon")!;
    protected Node RightIconNode => Node.QuerySelector("#RightIcon")!;
}
