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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umbra.Common;
using Umbra.Game;
using Una.Drawing;

namespace Umbra.Widgets;

public sealed partial class GearsetSwitcherPopup
{
    private Dictionary<GearsetCategory, List<Gearset>> GearsetsByCategory { get; } = [];
    private Dictionary<GearsetCategory, Node>          RoleContainers     { get; } = [];
    private Dictionary<Gearset, GearsetNode>           NodeByGearset      { get; } = [];

    /// <summary>
    /// Invoked when a gearset has been created.
    /// </summary>
    private void OnGearsetCreated(Gearset gearset)
    {
        AssignGearsetToDataLookupTables(gearset);

        GearsetNode node = new(_gearsetRepository, gearset);
        GetGearsetListNodeFor(gearset.Category).AppendChild(NodeByGearset[gearset] = node);
    }

    /// <summary>
    /// Invoked when a gearset has been removed.
    /// </summary>
    private void OnGearsetRemoved(Gearset gearset)
    {
        Node gearsetNode = NodeByGearset[gearset];

        gearsetNode.Remove();

        NodeByGearset.Remove(gearset);
        RemoveGearsetFromDataLookupTables(gearset);
    }

    /// <summary>
    /// Invoked when a gearset has been changed.
    /// </summary>
    private void OnGearsetChanged(Gearset gearset)
    {
        if (!NodeByGearset.TryGetValue(gearset, out GearsetNode? gearsetNode)) {
            OnGearsetCreated(gearset);
            return;
        }

        AssignGearsetToDataLookupTables(gearset);
        GetGearsetListNodeFor(gearset.Category).AppendChild(gearsetNode);
        SetBackgroundGradientFor(gearset.Category);
    }

    /// <summary>
    /// Adds the given gearset to the lookup tables.
    /// </summary>
    private void AssignGearsetToDataLookupTables(Gearset gearset)
    {
        // Remove from previous category.
        foreach ((GearsetCategory category, List<Gearset> gearsets) in GearsetsByCategory) {
            if (category != gearset.Category && gearsets.Contains(gearset)) {
                gearsets.Remove(gearset);
            }
        }

        if (!GearsetsByCategory.ContainsKey(gearset.Category)) {
            GearsetsByCategory[gearset.Category] = [gearset];
            return;
        }

        if (!GearsetsByCategory[gearset.Category].Contains(gearset)) {
            GearsetsByCategory[gearset.Category].Add(gearset);
        }
    }

    /// <summary>
    /// Removes the given gearset from the lookup tables.
    /// </summary>
    private void RemoveGearsetFromDataLookupTables(Gearset gearset)
    {
        foreach ((GearsetCategory category, List<Gearset> gearsets) in GearsetsByCategory) {
            if (gearsets.Contains(gearset)) {
                gearsets.Remove(gearset);
                Logger.Debug($"Gearset {gearset.Id} removed from category {category}");
            }
        }

        NodeByGearset.Remove(gearset);
    }
}
