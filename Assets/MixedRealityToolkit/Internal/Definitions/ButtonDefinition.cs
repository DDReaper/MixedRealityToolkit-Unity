﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace MixedRealityToolkit.Internal.Definitions
{
    /// <summary>
    /// A ButtonDefinition maps the capabilities of a selected controllers buttons, one definition should exist for each button profile.
    /// </summary>
    public struct ButtonDefinition
    {
        /// <summary>
        /// The ID assigned to the Button
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The input type of the button, e.g. Analogue, Digital, etc.
        /// </summary>
        public ControllerInputType ButtonInputType { get; set; }

        /// <summary>
        /// The primary action of the button as defined by the controller SDK.
        /// </summary>
        public ButtonAction ButtonAction { get; set; }
    }
}