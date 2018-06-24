﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Microsoft.MixedReality.Toolkit.Internal.Devices.WindowsMixedReality
{
    public class WindowsMixedRealityController : BaseController
    {
        public WindowsMixedRealityController(ControllerState controllerState, Handedness controllerHandedness, IMixedRealityInputSource inputSource = null, IMixedRealityInteractionMapping[] interactions = null)
                : base(controllerState, controllerHandedness, inputSource, interactions)
        {
            IsControllerTracked = false;
        }

        /// <summary>
        /// The last updated source state reading for this Windows Mixed Reality Controller.
        /// </summary>
        public InteractionSourceState LastSourceStateReading { get; private set; }

        public bool IsControllerTracked { get; private set; }

        private SixDof currentPointerData = new SixDof(Vector3.zero, Quaternion.identity);
        private SixDof currentGripData = new SixDof(Vector3.zero, Quaternion.identity);

        /// <inheritdoc/>
        public override void LoadConfiguration()
        {
            MixedRealityControllerMapping controllerMapping = Managers.MixedRealityManager.Instance.ActiveProfile.GetControllerMapping(typeof(WindowsMixedRealityController), ControllerHandedness);

            if (controllerMapping.Interactions?.Length > 0)
            {
                SetupFromMapping(controllerMapping.Interactions);
            }
            else
            {
                SetupControllerDefaults();
            }
        }

        #region Setup and Update functions

        /// <summary>
        /// Load the Interaction mappings for this controller from the configured Controller Mapping profile
        /// </summary>
        /// <param name="mappings">Configured mappings from a controller mapping profile</param>
        private void SetupFromMapping(IMixedRealityInteractionMapping[] mappings)
        {
            var interactions = new List<IMixedRealityInteractionMapping>();
            for (int i = 0; i < mappings.Length; i++)
            {
                switch (mappings[i].AxisType)
                {
                    case AxisType.Digital:
                        interactions.Add(new MixedRealityInteractionMapping<bool>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.SingleAxis:
                        interactions.Add(new MixedRealityInteractionMapping<float>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.DualAxis:
                        interactions.Add(new MixedRealityInteractionMapping<Vector2>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.ThreeDofPosition:
                        interactions.Add(new MixedRealityInteractionMapping<Vector3>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.ThreeDofRotation:
                        interactions.Add(new MixedRealityInteractionMapping<Quaternion>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.SixDof:
                        interactions.Add(new MixedRealityInteractionMapping<SixDof>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    case AxisType.None:
                    case AxisType.Raw:
                        interactions.Add(new MixedRealityInteractionMapping<object>((uint)i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Interactions = interactions.ToArray();
        }

        /// <summary>
        /// Create Interaction mappings from a device specific default set of action mappings
        /// </summary>
        private void SetupControllerDefaults()
        {
            var interactions = new List<IMixedRealityInteractionMapping>();

            InputAction[] inputActions = Managers.MixedRealityManager.Instance.ActiveProfile.InputActionsProfile.InputActions;
            if (inputActions == null)
            {
                return;
            }
            //Add the Controller Pointer
            interactions.Add(new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.SpatialPointer, new InputAction(1, "Select")));

            // Add the Controller trigger
            interactions.Add(new MixedRealityInteractionMapping<float>(2, AxisType.SingleAxis, DeviceInputType.Trigger, new InputAction(1, "Select")));

            // If the controller has a Grip / Grasp button, add it to the controller capabilities
            interactions.Add(new MixedRealityInteractionMapping<SixDof>(3, AxisType.SixDof, DeviceInputType.SpatialGrip, new InputAction(2, "Grip")));

            interactions.Add(new MixedRealityInteractionMapping<bool>(4, AxisType.Digital, DeviceInputType.GripPress, new InputAction(3, "Grab")));

            // If the controller has a menu button, add it to the controller capabilities
            interactions.Add(new MixedRealityInteractionMapping<bool>(5, AxisType.Digital, DeviceInputType.Menu, new InputAction(4, "Menu")));

            // If the controller has a Thumbstick, add it to the controller capabilities
            interactions.Add(new MixedRealityInteractionMapping<Vector2>(6, AxisType.DualAxis, DeviceInputType.ThumbStick, ControllerHandedness == Handedness.Left ? new InputAction(5, "Walk") : new InputAction(6, "Look")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(7, AxisType.Digital, DeviceInputType.ThumbStickPress, new InputAction(7, "Interact")));

            // If the controller has a Touchpad, add it to the controller capabilities
            interactions.Add(new MixedRealityInteractionMapping<Vector2>(8, AxisType.DualAxis, DeviceInputType.Touchpad, new InputAction(8, "Inventory")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(9, AxisType.Digital, DeviceInputType.TouchpadTouch, new InputAction(9, "Pickup")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(10, AxisType.Digital, DeviceInputType.TouchpadPress, new InputAction(9, "Pickup")));

            Interactions = interactions.ToArray();
        }

        #region Update data functions

        /// <summary>
        /// Update the controller data from the provided platform state
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        public void UpdateController(InteractionSourceState interactionSourceState)
        {
            UpdateControllerData(interactionSourceState);

            for (int i = 0; i < Interactions.Length; i++)
            {
                switch (Interactions[i].InputType)
                {
                    case DeviceInputType.SpatialPointer:
                    case DeviceInputType.PointerPosition:
                    case DeviceInputType.PointerRotation:
                        UpdatePointerData(interactionSourceState, Interactions[i]);
                        break;
                    case DeviceInputType.Select:
                    case DeviceInputType.Trigger:
                    case DeviceInputType.TriggerPress:
                    case DeviceInputType.PointerClick:
                        UpdateTriggerData(interactionSourceState, Interactions[i]);
                        break;
                    case DeviceInputType.SpatialGrip:
                    case DeviceInputType.GripPosition:
                    case DeviceInputType.GripRotation:
                    case DeviceInputType.GripPress:
                        UpdateGripData(interactionSourceState, Interactions[i]);
                        break;
                    case DeviceInputType.ThumbStick:
                    case DeviceInputType.ThumbStickPress:
                        UpdateThumbStickData(interactionSourceState, Interactions[i]);
                        break;
                    case DeviceInputType.Touchpad:
                    case DeviceInputType.TouchpadTouch:
                    case DeviceInputType.TouchpadPress:
                        UpdateTouchPadData(interactionSourceState, Interactions[i]);
                        break;
                    case DeviceInputType.Menu:
                    case DeviceInputType.ButtonPress:
                        {
                            Interactions[i].SetValue(interactionSourceState.menuPressed);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Update the "Controller" input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        private void UpdateControllerData(InteractionSourceState interactionSourceState)
        {
            LastSourceStateReading = interactionSourceState;

            // Get Controller tracked state
            ControllerState = IsControllerTracked ? ControllerState.Tracked : ControllerState.NotTracked;
        }

        /// <summary>
        /// Update the "Spatial Pointer" input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdatePointerData(InteractionSourceState interactionSourceState, IMixedRealityInteractionMapping interactionMapping)
        {
            Vector3 position;
            Quaternion rotation;

            interactionSourceState.sourcePose.TryGetPosition(out position, InteractionSourceNode.Pointer);
            interactionSourceState.sourcePose.TryGetRotation(out rotation, InteractionSourceNode.Pointer);

            if (CameraCache.Main.transform.parent != null)
            {
                currentPointerData.Position = CameraCache.Main.transform.parent.TransformPoint(position);
                currentPointerData.Rotation = Quaternion.Euler(CameraCache.Main.transform.parent.TransformDirection(rotation.eulerAngles));
            }

            //Update the interaction data source
            interactionMapping.SetValue(currentPointerData);

            //Raise input system Event if it enabled
            InputSystem?.Raise6DofInputChanged(InputSource, ControllerHandedness, interactionMapping.InputAction, currentPointerData);
        }

        /// <summary>
        /// Update the "Spatial Grip" input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateGripData(InteractionSourceState interactionSourceState, IMixedRealityInteractionMapping interactionMapping)
        {
            Vector3 position;
            Quaternion rotation;

            interactionSourceState.sourcePose.TryGetPosition(out position, InteractionSourceNode.Grip);
            interactionSourceState.sourcePose.TryGetRotation(out rotation, InteractionSourceNode.Grip);

            if (CameraCache.Main.transform.parent != null)
            {
                position = CameraCache.Main.transform.parent.TransformPoint(position);
                rotation = Quaternion.Euler(CameraCache.Main.transform.parent.TransformDirection(rotation.eulerAngles));
            }

            currentGripData.Position = position;
            currentGripData.Rotation = rotation;

            InputSystem?.Raise6DofInputChanged(InputSource, ControllerHandedness, interactionMapping.InputAction, currentGripData);
            //Interactions.SetDictionaryValue(DeviceInputType.SpatialGrip, currentGripData);
        }

        /// <summary>
        /// Update the Touchpad input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateTouchPadData(InteractionSourceState interactionSourceState, IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.TouchpadTouch:
                    {
                        interactionMapping.SetValue(interactionSourceState.touchpadTouched);

                        if (interactionSourceState.touchpadTouched)
                        {
                            InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        else
                        {
                            InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        break;
                    }
                case DeviceInputType.TouchpadPress:
                    {
                        interactionMapping.SetValue(interactionSourceState.touchpadPressed);

                        if (interactionSourceState.touchpadPressed)
                        {
                            InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        else
                        {
                            InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        break;
                    }
                case DeviceInputType.Touchpad:
                    {
                        interactionMapping.SetValue(interactionSourceState.touchpadPosition);
                        InputSystem?.Raise2DoFInputChanged(InputSource, ControllerHandedness, interactionMapping.InputAction, interactionSourceState.touchpadPosition);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Update the Thumbstick input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateThumbStickData(InteractionSourceState interactionSourceState, IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.ThumbStickPress:
                    {
                        interactionMapping.SetValue(interactionSourceState.thumbstickPressed);

                        if (interactionSourceState.thumbstickPressed)
                        {
                            InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        else
                        {
                            InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        break;
                    }
                case DeviceInputType.ThumbStick:
                    {
                        interactionMapping.SetValue(interactionSourceState.thumbstickPosition);
                        InputSystem?.Raise2DoFInputChanged(InputSource, ControllerHandedness, interactionMapping.InputAction, interactionSourceState.thumbstickPosition);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Update the Trigger input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateTriggerData(InteractionSourceState interactionSourceState, IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.Select:
                case DeviceInputType.TriggerPress:
                    {
                        interactionMapping.SetValue(interactionSourceState.selectPressed);

                        if (interactionSourceState.selectPressed)
                        {
                            InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        else
                        {
                            InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.InputAction);
                        }
                        break;
                    }
                case DeviceInputType.Trigger:
                    {
                        interactionMapping.SetValue(interactionSourceState.selectPressedAmount);
                        InputSystem?.RaiseOnInputPressed(InputSource, ControllerHandedness, interactionMapping.InputAction, interactionSourceState.selectPressedAmount);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        #endregion Update data functions

        #endregion Setup and Update functions
    }
}