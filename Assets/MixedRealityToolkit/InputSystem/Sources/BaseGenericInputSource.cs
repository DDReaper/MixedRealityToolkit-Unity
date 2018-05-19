﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.InputSystem.Gaze;
using Microsoft.MixedReality.Toolkit.Internal.Definitions;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Microsoft.MixedReality.Toolkit.InputSystem.Sources
{
    /// <summary>
    /// Base class for input sources that don't inherit from MonoBehaviour.
    /// </summary>
    public class BaseGenericInputSource : IMixedRealityInputSource
    {
        private static IMixedRealityInputSystem inputSystem = null;
        public static IMixedRealityInputSystem InputSystem => inputSystem ?? (inputSystem = MixedRealityManager.Instance.GetManager<IMixedRealityInputSystem>());

        public BaseGenericInputSource(string name, InteractionDefinition[] interactions, IMixedRealityPointer[] pointers = null)
        {
            SourceId = InputSystem.GenerateNewSourceId();
            SourceName = name;
            Pointers = pointers ?? new[] { GazeProvider.GazePointer };
            Interactions = new Dictionary<InputType, InteractionDefinition>();
            for (uint i = 0; i < interactions.Length; i++)
            {
                Interactions.Add(interactions[i].InputType, new InteractionDefinition(i, interactions[i].AxisType, interactions[i].InputType));
            }
        }

        public uint SourceId { get; }

        public string SourceName { get; }

        public IMixedRealityPointer[] Pointers { get; private set; }

        public InputSourceState InputSourceState { get; }

        public Handedness Handedness { get; } = Handedness.None;

        public Dictionary<InputType, InteractionDefinition> Interactions { get; }

        public virtual void SetupInputSource(InteractionSourceState interactionSourceState) { }

        public virtual void UpdateInputSource(InteractionSourceState interactionSourceState) { }

        public bool SupportsInputCapabilities(InputType[] capabilities)
        {
            for (int j = 0; j < capabilities.Length; j++)
            {
                if (Interactions.ContainsKey(capabilities[j]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool SupportsInputCapability(InputType capability)
        {
            return Interactions.ContainsKey(capability);
        }

        public virtual void RegisterPointers(IMixedRealityPointer[] pointers)
        {
            if (pointers == null) { throw new System.ArgumentNullException(nameof(pointers)); }

            Pointers = pointers;
        }

        public virtual void AddPointer(IMixedRealityPointer pointer)
        {
            for (int i = 0; i < Pointers.Length; i++)
            {
                if (Pointers[i].PointerId == pointer.PointerId)
                {
                    Debug.LogWarning($"This pointer has already been added to {SourceName}.");
                    return;
                }
            }

            var newPointers = new IMixedRealityPointer[Pointers.Length + 1];

            // Set our new pointer at the end.
            newPointers[newPointers.Length - 1] = pointer;

            // Reverse loop and set our existing pointers.
            for (int i = newPointers.Length - 2; i >= 0; i--)
            {
                newPointers[i] = Pointers[i];
            }
        }

        public virtual void RemovePointer(IMixedRealityPointer pointer)
        {
            var oldPointerList = new List<IMixedRealityPointer>(Pointers.Length);

            for (int i = 0; i < Pointers.Length; i++)
            {
                if (Pointers[i].PointerId != pointer.PointerId)
                {
                    oldPointerList.Add(Pointers[i]);
                }
            }

            Pointers = oldPointerList.ToArray();
        }

        public virtual bool TryGetPointerPosition(IMixedRealityPointer pointer, out Vector3 position)
        {
            foreach (var sourcePointer in Pointers)
            {
                if (sourcePointer.PointerId == pointer.PointerId)
                {
                    return sourcePointer.TryGetPointerPosition(out position);
                }
            }

            position = Vector3.zero;
            return false;
        }

        public virtual bool TryGetPointingRay(IMixedRealityPointer pointer, out Ray pointingRay)
        {
            foreach (var sourcePointer in Pointers)
            {
                if (sourcePointer.PointerId == pointer.PointerId)
                {
                    return sourcePointer.TryGetPointingRay(out pointingRay);
                }
            }

            pointingRay = default(Ray);
            return false;
        }

        public virtual bool TryGetPointerRotation(IMixedRealityPointer pointer, out Quaternion rotation)
        {
            foreach (var sourcePointer in Pointers)
            {
                if (sourcePointer.PointerId == pointer.PointerId)
                {
                    return sourcePointer.TryGetPointerRotation(out rotation);
                }
            }

            rotation = Quaternion.identity;
            return false;
        }

        #region IEquality Implementation

        public static bool Equals(IMixedRealityInputSource left, IMixedRealityInputSource right)
        {
            return left.Equals(right);
        }

        bool IEqualityComparer.Equals(object left, object right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            return Equals((IMixedRealityInputSource)obj);
        }

        private bool Equals(IMixedRealityInputSource other)
        {
            return other != null && SourceId == other.SourceId && string.Equals(SourceName, other.SourceName);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                hashCode = (hashCode * 397) ^ (int)SourceId;
                hashCode = (hashCode * 397) ^ (SourceName != null ? SourceName.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion IEquality Implementation
    }
}