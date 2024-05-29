﻿using Una.Drawing;

namespace Umbra.Windows.Settings;

public abstract class SettingsModule
{
    /// <summary>
    /// A unique identifier for this module.
    /// </summary>
    public abstract string Id { get; }

    /// <summary>
    /// The display name of this module.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The root node of this module.
    /// </summary>
    public abstract Node Node { get; }

    /// <summary>
    /// Invoked when the module is opened.
    /// </summary>
    public virtual void OnOpen() { }

    /// <summary>
    /// Invoked when the module is closed.
    /// </summary>
    public virtual void OnClose() { }

    /// <summary>
    /// Invoked on every frame.
    /// </summary>
    public virtual void OnUpdate() { }
}
