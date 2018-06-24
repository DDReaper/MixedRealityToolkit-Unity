﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using System;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Internal.Definitions.Devices
{
    /// <summary>
    /// Maps the capabilities of controllers, linking the Physical inputs of a controller to a Logical construct in a runtime project<para/>
    /// <remarks>One definition should exist for each physical device input, such as buttons, triggers, joysticks, dpads, and more.</remarks>
    /// </summary>
    [Serializable]
    public class MixedRealityInteractionMapping<TReadingType> : IMixedRealityInteractionMapping
    {
        /// <summary>
        /// The constructor for a new Interaction Mapping definition
        /// </summary>
        /// <param name="id">Identity for mapping</param>
        /// <param name="axisType">The axis that the mapping operates on, also denotes the data type for the mapping</param>
        /// <param name="inputType">The physical input device / control</param>
        /// <param name="inputAction">The logical InputAction that this input performs</param>
        public MixedRealityInteractionMapping(uint id, AxisType axisType, DeviceInputType inputType, IMixedRealityInputAction inputAction)
        {
            this.id = id;
            this.axisType = axisType;
            this.inputType = inputType;
            this.inputAction = inputAction;
            rawData = null;
            boolData = false;
            floatData = 0f;
            vector2Data = Vector2.zero;
            positionData = Vector3.zero;
            rotationData = Quaternion.identity;
            sixDofData = SixDof.ZeroIdentity;
            changed = false;

            // Initialize currentReading.
            switch (axisType)
            {
                case AxisType.None:
                    break;
                case AxisType.Raw:
                    currentReading = (TReadingType)rawData;
                    break;
                case AxisType.Digital:
                    currentReading = (TReadingType)(object)boolData;
                    break;
                case AxisType.SingleAxis:
                    currentReading = (TReadingType)(object)floatData;
                    break;
                case AxisType.DualAxis:
                    currentReading = (TReadingType)(object)vector2Data;
                    break;
                case AxisType.ThreeDofPosition:
                    currentReading = (TReadingType)(object)positionData;
                    break;
                case AxisType.ThreeDofRotation:
                    currentReading = (TReadingType)(object)rotationData;
                    break;
                case AxisType.SixDof:
                    currentReading = (TReadingType)(object)sixDofData;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisType), axisType, null);
            }
        }

        #region Interaction Properties

        [SerializeField]
        private uint id;

        /// <inheritdoc/>
        public uint Id => id;

        [SerializeField]
        [Tooltip("The axis type of the button, e.g. Analogue, Digital, etc.")]
        private AxisType axisType;

        /// <inheritdoc/>
        public AxisType AxisType => axisType;

        [SerializeField]
        [Tooltip("The primary action of the input as defined by the controller SDK.")]
        private DeviceInputType inputType;

        /// <inheritdoc/>
        public DeviceInputType InputType => inputType;

        [SerializeField]
        [Tooltip("Action to be raised to the Input Manager when the input data has changed.")]
        private IMixedRealityInputAction inputAction;

        /// <inheritdoc/>
        public IMixedRealityInputAction InputAction => inputAction;

        private TReadingType currentReading;

        private bool changed;

        /// <inheritdoc/>
        public bool Changed
        {
            get
            {
                bool returnValue = changed;

                if (changed)
                {
                    changed = false;
                }

                return returnValue;
            }
            private set
            {
                changed = value;
            }
        }

        #endregion Interaction Properties

        #region Definition Data items

        private object rawData;

        private bool boolData;

        private float floatData;

        private Vector2 vector2Data;

        private Vector3 positionData;

        private Quaternion rotationData;

        private SixDof sixDofData;

        #endregion Definition Data items

        #region Generic Operators

        /// <summary>
        /// Get the value of the Interaction Mapping data
        /// </summary>
        /// <returns>The current reading for the mapping</returns>
        public TReadingType GetValue() => currentReading;

        /// <summary>
        /// Get the value of the Interaction Mapping data
        /// </summary>
        /// <param name="reading">Output parameter for the reading value, returns the current reading for the mapping</param>
        public void GetValue(out TReadingType reading)
        {
            reading = currentReading;
        }

        /// <summary>
        /// Set the value of the Interaction Mapping data
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(TReadingType newValue)
        {
            Changed = (currentReading != null) ? !currentReading.Equals(newValue) : (newValue != null);
            currentReading = newValue;
        }

        #endregion Generic Operators

        #region Unique Get Operators

        public object GetRawValue()
        {
            return rawData;
        }

        public bool GetBooleanValue()
        {
            return boolData;
        }

        public float GetFloatValue()
        {
            return floatData;
        }

        public Vector2 GetVector2Value()
        {
            return vector2Data;
        }

        public Vector3 GetPositionValue()
        {
            return positionData;
        }

        public Quaternion GetRotationValue()
        {
            return rotationData;
        }

        public SixDof GetSixDofValue()
        {
            return sixDofData;
        }

        #endregion Unique Get Operators

        #region Unique Set Operators

        public void SetRawValue(object newValue)
        {
            if (AxisType != AxisType.Raw)
            {
                Debug.LogError("SetRawValue(object) is only valid for AxisType.Raw InteractionMappings");
            }

            Changed = rawData != newValue;
            rawData = newValue;
        }

        public void SetBoolValue(bool newValue)
        {
            if (AxisType != AxisType.Digital)
            {
                Debug.LogError("SetRawValue(bool) is only valid for AxisType.Digital InteractionMappings");
            }

            Changed = boolData != newValue;
            boolData = newValue;
        }

        public void SetFloatValue(float newValue)
        {
            if (AxisType != AxisType.SingleAxis)
            {
                Debug.LogError("SetRawValue(float) is only valid for AxisType.SingleAxis InteractionMappings");
            }

            Changed = !floatData.Equals(newValue);
            floatData = newValue;
        }

        public void SetVector2Value(Vector2 newValue)
        {
            if (AxisType != AxisType.DualAxis)
            {
                Debug.LogError("SetRawValue(Vector2) is only valid for AxisType.DualAxis InteractionMappings");
            }

            Changed = vector2Data != newValue;
            vector2Data = newValue;
        }

        public void SetPositionValue(Vector3 newValue)
        {
            if (AxisType != AxisType.ThreeDofPosition)
            {
                {
                    Debug.LogError("SetRawValue(Vector3) is only valid for AxisType.ThreeDoFPosition InteractionMappings");
                }
            }

            Changed = positionData != newValue;
            positionData = newValue;
        }

        public void SetRotationValue(Quaternion newValue)
        {
            if (AxisType != AxisType.ThreeDofRotation)
            {
                Debug.LogError("SetRawValue(Quaternion) is only valid for AxisType.ThreeDoFRotation InteractionMappings");
            }

            Changed = rotationData != newValue;
            rotationData = newValue;
        }

        public void SetSixDofValue(SixDof newValue)
        {
            if (AxisType != AxisType.SixDof)
            {
                Debug.LogError("SetRawValue(SixDof) is only valid for AxisType.SixDoF InteractionMappings");
            }

            Changed = sixDofData != newValue;

            sixDofData = newValue;
            positionData = sixDofData.Position;
            rotationData = sixDofData.Rotation;
        }

        #endregion Unique Set Operators
    }
}