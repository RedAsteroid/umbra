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
using FFXIVClientStructs.FFXIV.Client.System.Framework;

namespace Umbra.Widgets;

public partial class ClockWidget
{
    /// <summary>
    /// Returns a <see cref="DateTime"/> object based on the configured time
    /// source.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private DateTime GetTime()
    {
        var timeSource = GetConfigValue<string>("TimeSource");

        return timeSource switch {
            "LT" => DateTime.Now,
            "ST" => GetServerTime(),
            "ET" => GetEorzeaTime(),
            _    => throw new NotImplementedException($"Unknown time source: {timeSource}")
        };
    }

    /// <summary>
    /// Returns the current server time.
    /// </summary>
    /// <returns></returns>
    private static DateTime GetServerTime()
    {
        long serverTime = Framework.GetServerTime();
        long hours      = serverTime / 3600 % 24;
        long minutes    = serverTime / 60   % 60;
        long seconds    = serverTime        % 60;

        return new(1, 1, 1, (int)hours, (int)minutes, (int)seconds);
    }

    /// <summary>
    /// Returns the current Eorzea time.
    /// </summary>
    /// <returns></returns>
    private static unsafe DateTime GetEorzeaTime()
    {
        var fw = Framework.Instance();

        if (fw == null) {
            return DateTime.MinValue;
        }

        long eorzeaTime = fw->ClientTime.EorzeaTime;
        long hours      = eorzeaTime / 3600 % 24;
        long minutes    = eorzeaTime / 60   % 60;
        long seconds    = eorzeaTime        % 60;

        return new(1, 1, 1, (int)hours, (int)minutes, (int)seconds);
    }
}
