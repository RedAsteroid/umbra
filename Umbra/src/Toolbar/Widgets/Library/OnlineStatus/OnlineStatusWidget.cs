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
using Dalamud.Utility;
using Umbra.Common;
using Umbra.Game;
using Umbra.Widgets.System;
using Una.Drawing;

namespace Umbra.Widgets;

public partial class OnlineStatusWidget(
    WidgetInfo                  info,
    string?                     guid         = null,
    Dictionary<string, object>? configValues = null
) : DefaultToolbarWidget(info, guid, configValues)
{
    private IPlayer _player = null!;
    private uint?   _statusId;
    private bool?   _showName;
    private uint?   _iconId;

    /// <inheritdoc/>
    protected override void Initialize()
    {
        Node.OnRightClick += _ => OpenSearchInfoWindow();
        Node.Tooltip      =  I18N.Translate("Widget.OnlineStatus.Tooltip");

        Popup.UseGrayscaleIcons = false;

        _player = Framework.Service<IPlayer>();
    }

    /// <inheritdoc/>
    protected override void OnUpdate()
    {
        var showName = GetConfigValue<bool>("ShowName");

        if (_statusId != _player.OnlineStatusId || _showName != showName) {
            _statusId = _player.OnlineStatusId;
            _showName = showName;
            UpdateStatus();
        }

        SetPopupStatusOption(47, true);
        SetPopupStatusOption(17, true);
        SetPopupStatusOption(12, true);
        SetPopupStatusOption(22, true);
        SetPopupStatusOption(27, _player.IsMentor);
        SetPopupStatusOption(28, _player.IsBattleMentor);
        SetPopupStatusOption(30, _player.IsBattleMentor);
        SetPopupStatusOption(29, _player.IsTradeMentor);

        SetGhost(!GetConfigValue<bool>("Decorate"));

        switch (GetConfigValue<string>("IconLocation")) {
            case "Left":
                SetLeftIcon(_iconId);
                SetRightIcon(null);
                break;
            case "Right":
                SetLeftIcon(null);
                SetRightIcon(_iconId);
                break;
        }

        LabelNode.Style.TextOffset      = new(0, GetConfigValue<int>("TextYOffset"));
        LeftIconNode.Style.ImageOffset  = new(0, GetConfigValue<int>("IconYOffset"));
        RightIconNode.Style.ImageOffset = new(0, GetConfigValue<int>("IconYOffset"));
    }

    private void UpdateStatus()
    {
        uint statusId = _player.OnlineStatusId == 0 ? 47 : _player.OnlineStatusId;
        var  status   = GetStatusById(statusId);

        SetLabel(GetConfigValue<bool>("ShowName") ? status.Name.ToDalamudString().TextValue : null);

        _iconId = status.Icon;

        LabelNode.Style.IsVisible = GetConfigValue<bool>("ShowName");

        foreach (Node node in Node.QuerySelectorAll(".icon")) {
            node.Style.Margin = new();
        }
    }
}
