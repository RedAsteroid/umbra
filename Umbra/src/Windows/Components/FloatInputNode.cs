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

using System;
using System.Numerics;
using ImGuiNET;
using Una.Drawing;

namespace Umbra.Windows.Components;

public class FloatInputNode : Node
{
    public event Action<float>? OnValueChanged;

    public float Value {
        get => _value;
        set {
            if (_value == value) return;
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    public string Label {
        get => (string)(LabelNode.NodeValue ?? "");
        set => LabelNode.NodeValue = value;
    }

    public string? Description {
        get => (string?)DescriptionNode.NodeValue;
        set => DescriptionNode.NodeValue = value;
    }

    public float MinValue { get; set; }
    public float MaxValue { get; set; }

    private float _value;

    public FloatInputNode(
        string  id,
        float   value,
        float   min,
        float   max,
        string  label,
        string? description = null,
        int     leftMargin  = 32
    )
    {
        _value   = value;
        MinValue = min;
        MaxValue = max;

        Id         = id;
        ClassList  = ["input"];
        Stylesheet = InputStylesheet;
        Style      = new() { Padding = new() { Left = leftMargin } };

        ChildNodes = [
            new() {
                ClassList = ["input--label"],
                NodeValue = label,
            },
            new() {
                ClassList = ["input--box"],
            },
            new() {
                ClassList = ["input--description"],
                NodeValue = description,
            },
        ];

        BeforeReflow += _ => {
            int maxWidth = ParentNode!.Bounds.ContentSize.Width - ParentNode!.ComputedStyle.Padding.HorizontalSize;
            int padding  = ComputedStyle.Gap + (int)(leftMargin * ScaleFactor);
            int width    = (int)((maxWidth - padding) / ScaleFactor);
            int labelHeight;

            if (LabelNode.Style.Size?.Width == width && DescriptionNode.Style.Size?.Width == width) {
                return false;
            }

            if (string.IsNullOrEmpty((string?)DescriptionNode.NodeValue)) {
                DescriptionNode.Style.IsVisible = false;
                labelHeight                     = 24;
            } else {
                DescriptionNode.Style.IsVisible = true;
                labelHeight                     = 0;
            }

            LabelNode.Style.Size       = new(width, labelHeight);
            DescriptionNode.Style.Size = new(width, 0);
            SelectBoxNode.Style.Size   = new(width, 26);

            return true;
        };
    }

    protected override void OnDraw(ImDrawListPtr drawList)
    {
        var bounds = QuerySelector(".input--box")!.Bounds;
        ImGui.SetCursorScreenPos(bounds.ContentRect.TopLeft);

        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding,  new Vector2(8, 4));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(8, 4));
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 5);
        ImGui.PushStyleVar(ImGuiStyleVar.PopupRounding, 5);

        ImGui.PushStyleColor(ImGuiCol.Text,           new Color("Input.Text").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.PopupBg,        new Color("Input.Background").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.Button,         new Color("Input.Background").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered,  new Color("Input.Background").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.FrameBg,        new Color("Input.Background").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new Color("Input.Background").ToUInt());
        ImGui.PushStyleColor(ImGuiCol.FrameBgActive,  new Color("Input.Background").ToUInt());

        ImGui.SetNextItemWidth(bounds.ContentSize.Width);

        float v = _value;

        if (ImGui.InputFloat($"##{Id}", ref v, 0.1f, 1, null, ImGuiInputTextFlags.EnterReturnsTrue)) {
            Value = Math.Clamp(v, MinValue, MaxValue);
        }

        ImGui.PopStyleVar(4);
        ImGui.PopStyleColor(7);
    }

    private Node SelectBoxNode   => QuerySelector(".input--box")!;
    private Node LabelNode       => QuerySelector(".input--label")!;
    private Node DescriptionNode => QuerySelector(".input--description")!;

    private static Stylesheet InputStylesheet { get; } = new(
        [
            new(
                ".input",
                new() {
                    Flow    = Flow.Vertical,
                    Gap     = 6,
                    Padding = new() { Left = 32 }
                }
            ),
            new(
                ".input--box",
                new() {
                    Size = new(0, 26),
                }
            ),
            new(
                ".input--label",
                new() {
                    FontSize     = 14,
                    Color        = new("Window.Text"),
                    TextOverflow = false,
                    WordWrap     = false,
                }
            ),
            new(
                ".input--description",
                new() {
                    FontSize     = 11,
                    Color        = new("Window.TextMuted"),
                    TextOverflow = false,
                    WordWrap     = true,
                    LineHeight   = 1.5f,
                }
            ),
        ]
    );
}
