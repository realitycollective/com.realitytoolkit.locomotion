// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace RealityToolkit.Locomotion.Definitions
{
    /// <summary>
    /// Possible validation outcomes by the <see cref="LocomotionSystem.Interfaces.ITeleportValidationProvider"/>.
    /// </summary>
    [Serializable]
    public enum TeleportValidationResult
    {
        None = 0,
        Valid,
        Invalid,
        Anchor,
    }
}
