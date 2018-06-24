// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Internal.Extensions;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces;
using NUnit.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Microsoft.MixedReality.Toolkit.Tests
{
    public class InteractionDefinitionTests
    {
        private const int SpeedTestIterations = 10000000;

        #region objects

        [Test]
        public void Test01_TestObjectChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue1 = (object)1f;
            var testValue2 = (object)false;

            var initialValue = inputDef.GetRawValue();

            Assert.IsNull(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRawValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetRawValue();

            Assert.IsNotNull(setValue1);
            Assert.AreEqual(setValue1, testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRawValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetRawValue();

            Assert.IsNotNull(setValue2);
            Assert.AreEqual(setValue2, testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test02_TestObjectNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue = new object();

            var initialValue = inputDef.GetRawValue();

            Assert.IsNull(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRawValue(testValue);

            Assert.IsTrue(inputDef.Changed);

            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRawValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);

            //Check setting the value twice with the same value produces no change
            var newValue = inputDef.GetRawValue();

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed, newValue.ToString());

            // Make sure setting again after query, we query again it's false
            inputDef.SetRawValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);

        }

        [Test]
        public void Test03_TestObjectGenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue1 = (object)1f;
            var testValue2 = (object)false;

            var initialValue = inputDef.GetValue();

            Assert.IsNull(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.IsNotNull(setValue1);
            Assert.AreEqual(setValue1, testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.IsNotNull(setValue2);
            Assert.AreEqual(setValue2, testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test04_TestObjectGenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue = (object)1f;

            var initialValue = inputDef.GetValue();

            Assert.IsNull(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsFalse(inputDef.Changed);
            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test05_TestObjectDirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue1 = (object)1f;
            var testValue2 = (object)false;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetRawValue(testValue);
                inputDef.GetRawValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            Debug.Log($"Object Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.GreaterOrEqual(genericTime, directTime);
        }

        #endregion objects

        #region bools

        [Test]
        public void Test06_TestBoolChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue1 = true;
            var testValue2 = false;

            var initialValue = inputDef.GetBooleanValue();

            Assert.IsFalse(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetBoolValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetBooleanValue();

            Assert.IsTrue(setValue1);
            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetBoolValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetBooleanValue();

            Assert.IsFalse(setValue2);
            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test07_TestBoolNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue = true;

            var initialValue = inputDef.GetBooleanValue();

            Assert.IsFalse(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetBoolValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetBoolValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }
        [Test]
        public void Test08_TestBoolGenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue1 = true;
            var testValue2 = false;

            var initialValue = inputDef.GetValue();

            Assert.IsFalse(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.IsTrue(setValue1);
            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.IsFalse(setValue2);
            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test09_TestBoolGenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue = true;

            var initialValue = inputDef.GetValue();

            Assert.IsFalse(initialValue);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test10_TestBoolDirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue1 = true;
            var testValue2 = false;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetBoolValue(testValue);
                inputDef.GetBooleanValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            Debug.Log($"Bool Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion bools

        #region float

        [Test]
        public void Test11_TestFloatChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = 1f;
            var testValue2 = 9001f;

            var initialValue = inputDef.GetFloatValue();

            Assert.AreEqual(initialValue, 0d, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetFloatValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetFloatValue();

            Assert.AreEqual(setValue1, testValue1, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetFloatValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetFloatValue();

            Assert.AreEqual(setValue2, testValue2, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test12_TestFloatNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue = 1f;

            var initialValue = inputDef.GetFloatValue();

            Assert.AreEqual(initialValue, 0d, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetFloatValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetFloatValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test13_TestFloatGenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = 1f;
            var testValue2 = 9001f;

            var initialValue = inputDef.GetValue();

            Assert.AreEqual(initialValue, 0d, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.AreEqual(setValue1, testValue1, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.AreEqual(setValue2, testValue2, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test14_TestFloatGenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue = 1f;

            var initialValue = inputDef.GetValue();

            Assert.AreEqual(initialValue, 0d, double.Epsilon);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }
        [Test]
        public void Test15_TestFloatDirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = 1f;
            var testValue2 = 9001f;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetFloatValue(testValue);
                inputDef.GetFloatValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            UnityEngine.Debug.Log($"Float Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion float

        #region Vector2

        [Test]
        public void Test16_TestVector2Changed()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector2.one;
            var testValue2 = Vector2.zero;

            var initialValue = inputDef.GetVector2Value();

            Assert.True(initialValue == Vector2.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetVector2Value(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetVector2Value();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetVector2Value(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetVector2Value();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test17_TestVector2NoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue = Vector2.one;

            var initialValue = inputDef.GetVector2Value();

            Assert.True(initialValue == Vector2.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetVector2Value(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetVector2Value(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test18_TestVector2GenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector2.one;
            var testValue2 = Vector2.zero;

            var initialValue = inputDef.GetValue();

            Assert.True(initialValue == Vector2.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test19_TestVector2GenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue = Vector2.one;

            var initialValue = inputDef.GetValue();

            Assert.True(initialValue == Vector2.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test20_TestVector2DirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector2.one;
            var testValue2 = Vector2.zero;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetVector2Value(testValue);
                inputDef.GetVector2Value();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            UnityEngine.Debug.Log($"Vector2 Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion Vector2

        #region Vector3

        [Test]
        public void Test21_TestVector3Changed()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector3.one;
            var testValue2 = Vector3.zero;

            var initialValue = inputDef.GetPositionValue();

            Assert.True(initialValue == Vector3.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetPositionValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetPositionValue();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetPositionValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetPositionValue();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test22_TestVector3NoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue = Vector3.one;

            var initialValue = inputDef.GetPositionValue();

            Assert.True(initialValue == Vector3.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetPositionValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetPositionValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test23_TestVector3GenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector3.one;
            var testValue2 = Vector3.zero;

            var initialValue = inputDef.GetValue();

            Assert.True(initialValue == Vector3.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test24_TestVector3GenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue = Vector3.one;

            var initialValue = inputDef.GetValue();

            Assert.True(initialValue == Vector3.zero);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }
        [Test]
        public void Test25_TestVector3DirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector3.one;
            var testValue2 = Vector3.zero;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetPositionValue(testValue);
                inputDef.GetPositionValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            UnityEngine.Debug.Log($"Vector3 Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion Vector3

        #region Quaternion

        [Test]
        public void Test26_TestQuaternionChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue1 = Quaternion.Euler(45f, 45f, 45f);
            var testValue2 = Quaternion.identity;

            var initialValue = inputDef.GetRotationValue();

            Assert.IsTrue(initialValue == Quaternion.identity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRotationValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetRotationValue();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRotationValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetRotationValue();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test27_TestQuaternionNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue = Quaternion.Euler(45f, 45f, 45f);

            var initialValue = inputDef.GetRotationValue();

            Assert.IsTrue(initialValue == Quaternion.identity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRotationValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetRotationValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test28_TestQuaternionGenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue1 = Quaternion.Euler(45f, 45f, 45f);
            var testValue2 = Quaternion.identity;

            var initialValue = inputDef.GetValue();

            Assert.IsTrue(initialValue == Quaternion.identity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.True(setValue1 == testValue1);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.True(setValue2 == testValue2);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test29_TestQuaternionGenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue = Quaternion.Euler(45f, 45f, 45f);

            var initialValue = inputDef.GetValue();

            Assert.IsTrue(initialValue == Quaternion.identity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);
            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test30_TestQuaternionDirectVsGenericSpeed()
        {
            var inputDef = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue1 = Quaternion.Euler(45f, 45f, 45f);
            var testValue2 = Quaternion.identity;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetValue(testValue);
                inputDef.GetValue();
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetRotationValue(testValue);
                inputDef.GetRotationValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            UnityEngine.Debug.Log($"Quaternion Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion Quaternion

        #region SixDof

        [Test]
        public void Test31_TestSixDofChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            var testValue1 = new SixDof(Vector3.up, Quaternion.identity);
            var testValue2 = new SixDof(Vector3.one, new Quaternion(45f, 45f, 45f, 45f));

            var initialValue = inputDef.GetSixDofValue();

            Assert.IsTrue(initialValue == SixDof.ZeroIdentity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetSixDofValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetSixDofValue();

            Assert.IsTrue(setValue1 == testValue1);
            Assert.IsTrue(setValue1.Position == testValue1.Position);
            Assert.IsTrue(setValue1.Rotation == testValue1.Rotation);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetSixDofValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetSixDofValue();

            Assert.IsTrue(setValue2 == testValue2);
            Assert.IsTrue(setValue2.Position == testValue2.Position);
            Assert.IsTrue(setValue2.Rotation == testValue2.Rotation);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test32_TestSixDofNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            var testValue = new SixDof(Vector3.up, Quaternion.identity);

            var initialValue = inputDef.GetSixDofValue();

            Assert.IsTrue(initialValue == SixDof.ZeroIdentity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetSixDofValue(testValue);

            Assert.IsTrue(inputDef.Changed);

            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetSixDofValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test33_TestSixDofGenericChanged()
        {
            var inputDef = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            var testValue1 = new SixDof(Vector3.up, Quaternion.identity);
            var testValue2 = new SixDof(Vector3.one, new Quaternion(45f, 45f, 45f, 45f));

            var initialValue = inputDef.GetValue();

            Assert.IsTrue(initialValue == SixDof.ZeroIdentity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue1);

            Assert.IsTrue(inputDef.Changed);

            var setValue1 = inputDef.GetValue();

            Assert.IsTrue(setValue1 == testValue1);
            Assert.IsTrue(setValue1.Position == testValue1.Position);
            Assert.IsTrue(setValue1.Rotation == testValue1.Rotation);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue2);

            Assert.IsTrue(inputDef.Changed);

            var setValue2 = inputDef.GetValue();

            Assert.IsTrue(setValue2 == testValue2);
            Assert.IsTrue(setValue2.Position == testValue2.Position);
            Assert.IsTrue(setValue2.Rotation == testValue2.Rotation);
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test34_TestSixDofGenericNoChange()
        {
            var inputDef = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            var testValue = new SixDof(Vector3.up, Quaternion.identity);

            var initialValue = inputDef.GetValue();

            Assert.IsTrue(initialValue == SixDof.ZeroIdentity);
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            Assert.IsTrue(inputDef.Changed);

            // Make sure the second time we query it's false
            Assert.IsFalse(inputDef.Changed);

            inputDef.SetValue(testValue);

            // Make sure if we set the same value it's false
            Assert.IsFalse(inputDef.Changed);
        }

        [Test]
        public void Test35_TestSixDofSpeed()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            var inputDef = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            interactions[0] = inputDef;

            var testValue1 = new SixDof(Vector3.up, Quaternion.identity);
            var testValue2 = new SixDof(Vector3.one, new Quaternion(45f, 45f, 45f, 45f));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                var interaction = interactions[0] as MixedRealityInteractionMapping<SixDof>;
                interaction.SetValue(testValue);
                interaction.GetValue(out testValue);
            }

            var genericTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();

            for (int i = 0; i < SpeedTestIterations; i++)
            {
                var testValue = i % 2 == 0 ? testValue1 : testValue2;
                inputDef.SetSixDofValue(testValue);
                inputDef.GetSixDofValue();
            }

            var directTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Stop();

            Debug.Log($"SixDof Speed Test Results | Generic Time: {genericTime} | Direct Time: {directTime}");
            Assert.Greater(genericTime, directTime);
        }

        #endregion SixDof

        #region Interaction Dictionary Tests

        [Test]
        public void Test36_InteractionDictionaryObject()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None));
            var testValue1 = (object)1f;
            var testValue2 = (object)false;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<object>;

            Assert.IsNotNull(interaction);
            Assert.IsNull(interaction.GetValue());
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction.GetValue());
            Assert.AreEqual(interaction.GetValue(), testValue1);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction.GetValue());
            Assert.AreEqual(interaction.GetValue(), testValue2);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test37_InteractionDictionaryBool()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None));
            var testValue1 = true;
            var testValue2 = false;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<bool>;

            Assert.IsNotNull(interaction);
            Assert.IsFalse(interaction.GetValue());
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.IsTrue(interaction.GetValue());
            Assert.IsTrue(interaction.GetValue() == testValue1);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.IsFalse(interaction.GetValue());
            Assert.IsTrue(interaction.GetValue() == testValue2);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test38_InteractionDictionaryFloat()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None));
            var testValue1 = 1f;
            var testValue2 = 9001f;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<float>;

            Assert.IsNotNull(interaction);
            Assert.AreEqual(interaction.GetValue(), 0d, double.Epsilon);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.AreEqual(interaction.GetValue(), testValue1, double.Epsilon);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.AreEqual(interaction.GetValue(), testValue2, double.Epsilon);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test39_InteractionDictionaryVector2()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None));
            var testValue1 = Vector2.one;
            var testValue2 = Vector2.zero;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<Vector2>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Vector2.zero);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test40_InteractionDictionaryVector3()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None));
            var testValue1 = Vector3.one;
            var testValue2 = Vector3.zero;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<Vector3>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Vector3.zero);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test41_InteractionDictionaryQuaternion()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None));
            var testValue1 = Quaternion.Euler(45f, 45f, 45f);
            var testValue2 = Quaternion.identity;

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<Quaternion>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Quaternion.identity);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        [Test]
        public void Test42_InteractionDictionarySixDof()
        {
            var interactions = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();
            interactions.Add(DeviceInputType.None, new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None));
            var testValue1 = new SixDof(Vector3.up, Quaternion.identity);
            var testValue2 = new SixDof(Vector3.one, new Quaternion(45f, 45f, 45f, 45f));

            var interaction = interactions[DeviceInputType.None] as MixedRealityInteractionMapping<SixDof>;

            Assert.IsNotNull(interaction);

            SixDof initialValue;
            interaction.GetValue(out initialValue);

            Assert.IsTrue(initialValue == SixDof.ZeroIdentity);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue1);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.IsTrue(interaction.GetValue() == testValue1);
            Assert.IsTrue(interaction.GetValue().Position == testValue1.Position);
            Assert.IsTrue(interaction.GetValue().Rotation == testValue1.Rotation);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            interactions.SetDictionaryValue(DeviceInputType.None, testValue2);

            Assert.IsTrue(interactions.GetDictionaryValueChanged(DeviceInputType.None));

            Assert.IsNotNull(interaction);
            Assert.IsTrue(interaction.GetValue() == testValue2);
            Assert.IsTrue(interaction.GetValue().Position == testValue2.Position);
            Assert.IsTrue(interaction.GetValue().Rotation == testValue2.Rotation);
            Assert.IsFalse(interactions.GetDictionaryValueChanged(DeviceInputType.None));
        }

        #endregion Interaction Dictionary

        #region Interaction Array Tests

        [Test]
        public void Test42_InteractionArrayObject()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<object>(1, AxisType.Raw, DeviceInputType.None, InputAction.None);
            var testValue1 = (object)1f;
            var testValue2 = (object)false;

            var interaction = interactions[0] as MixedRealityInteractionMapping<object>;

            Assert.IsNotNull(interaction);
            Assert.IsNull(interaction.GetValue());
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.IsNotNull(interaction.GetValue());
            Assert.AreEqual(interaction.GetValue(), testValue1);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.IsNotNull(interaction.GetValue());
            Assert.AreEqual(interaction.GetValue(), testValue2);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test43_InteractionArrayBool()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<bool>(1, AxisType.Digital, DeviceInputType.None, InputAction.None);
            var testValue1 = true;
            var testValue2 = false;

            var interaction = interactions[0] as MixedRealityInteractionMapping<bool>;

            Assert.IsNotNull(interaction);
            Assert.IsFalse(interaction.GetValue());
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.IsTrue(interaction.GetValue());
            Assert.IsTrue(interaction.GetValue() == testValue1);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.IsFalse(interaction.GetValue());
            Assert.IsTrue(interaction.GetValue() == testValue2);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test44_InteractionArrayFloat()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<float>(1, AxisType.SingleAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = 1f;
            var testValue2 = 9001f;

            var interaction = interactions[0] as MixedRealityInteractionMapping<float>;

            Assert.IsNotNull(interaction);
            Assert.AreEqual(interaction.GetValue(), 0d, double.Epsilon);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.AreEqual(interaction.GetValue(), testValue1, double.Epsilon);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.AreEqual(interaction.GetValue(), testValue2, double.Epsilon);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test45_InteractionArrayVector2()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<Vector2>(1, AxisType.DualAxis, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector2.one;
            var testValue2 = Vector2.zero;

            var interaction = interactions[0] as MixedRealityInteractionMapping<Vector2>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Vector2.zero);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test46_InteractionArrayVector3()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<Vector3>(1, AxisType.ThreeDofPosition, DeviceInputType.None, InputAction.None);
            var testValue1 = Vector3.one;
            var testValue2 = Vector3.zero;

            var interaction = interactions[0] as MixedRealityInteractionMapping<Vector3>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Vector3.zero);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test47_InteractionArrayQuaternion()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<Quaternion>(1, AxisType.ThreeDofRotation, DeviceInputType.None, InputAction.None);
            var testValue1 = Quaternion.Euler(45f, 45f, 45f);
            var testValue2 = Quaternion.identity;

            var interaction = interactions[0] as MixedRealityInteractionMapping<Quaternion>;

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == Quaternion.identity);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue1);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.IsNotNull(interaction);
            Assert.True(interaction.GetValue() == testValue2);
            Assert.IsFalse(interaction.Changed);
        }

        [Test]
        public void Test48_InteractionArraySixDof()
        {
            var interactions = new IMixedRealityInteractionMapping[1];
            interactions[0] = new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.None, InputAction.None);
            var testValue1 = new SixDof(Vector3.up, Quaternion.identity);
            var testValue2 = new SixDof(Vector3.one, new Quaternion(45f, 45f, 45f, 45f));

            var interaction = interactions[0] as MixedRealityInteractionMapping<SixDof>;

            Assert.IsNotNull(interaction);
            SixDof initialSixDofValue = interaction.GetValue();

            Assert.IsTrue(initialSixDofValue == SixDof.ZeroIdentity);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue1);

            Assert.IsTrue(interaction.Changed);

            Assert.IsTrue(interaction.GetValue() == testValue1);
            Assert.IsTrue(interaction.GetValue().Position == testValue1.Position);
            Assert.IsTrue(interaction.GetValue().Rotation == testValue1.Rotation);
            Assert.IsFalse(interaction.Changed);

            interaction.SetValue(testValue2);

            Assert.IsTrue(interaction.Changed);

            Assert.IsTrue(interaction.GetValue() == testValue2);
            Assert.IsTrue(interaction.GetValue().Position == testValue2.Position);
            Assert.IsTrue(interaction.GetValue().Rotation == testValue2.Rotation);
            Assert.IsFalse(interaction.Changed);
        }

        #endregion Interaction Array Tests

        #region Performance Tests
        //Tests show the array lookup is faster.  Keeping here for reference until merge

        [Test]
        public void Test49_InteractionDictionaryPerformance()
        {
            var interactionsDictionary = new Dictionary<DeviceInputType, IMixedRealityInteractionMapping>();

            //Setup
            interactionsDictionary.Add(DeviceInputType.SpatialPointer, new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.SpatialPointer, new InputAction(1, "Select")));
            interactionsDictionary.Add(DeviceInputType.Trigger, new MixedRealityInteractionMapping<float>(2, AxisType.SingleAxis, DeviceInputType.Trigger, new InputAction(1, "Select")));
            interactionsDictionary.Add(DeviceInputType.SpatialGrip, new MixedRealityInteractionMapping<SixDof>(3, AxisType.SixDof, DeviceInputType.SpatialGrip, new InputAction(2, "Grip")));
            interactionsDictionary.Add(DeviceInputType.GripPress, new MixedRealityInteractionMapping<bool>(4, AxisType.Digital, DeviceInputType.GripPress, new InputAction(3, "Grab")));
            interactionsDictionary.Add(DeviceInputType.Menu, new MixedRealityInteractionMapping<bool>(5, AxisType.Digital, DeviceInputType.Menu, new InputAction(4, "Menu")));
            interactionsDictionary.Add(DeviceInputType.ThumbStick, new MixedRealityInteractionMapping<Vector2>(6, AxisType.DualAxis, DeviceInputType.ThumbStick, new InputAction(5, "Walk")));
            interactionsDictionary.Add(DeviceInputType.ThumbStickPress, new MixedRealityInteractionMapping<bool>(7, AxisType.Digital, DeviceInputType.ThumbStickPress, new InputAction(6, "Interact")));
            interactionsDictionary.Add(DeviceInputType.Touchpad, new MixedRealityInteractionMapping<Vector2>(8, AxisType.DualAxis, DeviceInputType.Touchpad, new InputAction(7, "Inventory")));
            interactionsDictionary.Add(DeviceInputType.TouchpadTouch, new MixedRealityInteractionMapping<bool>(9, AxisType.Digital, DeviceInputType.TouchpadTouch, new InputAction(8, "Pickup")));
            interactionsDictionary.Add(DeviceInputType.TouchpadPress, new MixedRealityInteractionMapping<bool>(10, AxisType.Digital, DeviceInputType.TouchpadPress, new InputAction(8, "Pickup")));

            //Test
            for (int i = 0; i < 500; i++)
            {
                UpdateFromInteractionSource(interactionsDictionary);
            }
        }

        #region InteractionDictionaryPerformance Update data functions

        /// <summary>
        /// Update the controller data from the provided platform state
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        public void UpdateFromInteractionSource(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            //Debug.Assert(interactionSourceState.source.id == SourceId, "An UpdateSourceState call happened with mismatched source ID.");
            // TODO - Do we need Kind?
            //Debug.Assert(interactionSourceState.source.kind == sourceData.Source.kind, "An UpdateSourceState call happened with mismatched source kind.");

            // Update Controller
            // TODO - Controller currently not accepted by InputSystem, only InteractionState captured
            // TODO - May need to be more granular with checks if we are allowing user to configure :S  
            // TODO - Need to think of a better way to validate options, multiple Contains aren't good, maybe an extension?
            UpdateControllerDataDictionary();

            // Update Pointer
            if (interactions.ContainsKey(DeviceInputType.SpatialPointer)) UpdatePointerData(interactions);

            // Update Grip
            if (interactions.ContainsKey(DeviceInputType.SpatialGrip)) UpdateGripData(interactions);

            // Update Touchpad
            if (interactions.ContainsKey(DeviceInputType.Touchpad) || interactions.ContainsKey(DeviceInputType.TouchpadTouch)) UpdateTouchPadData(interactions);

            // Update Thumbstick
            if (interactions.ContainsKey(DeviceInputType.Thumb)) UpdateThumbStickData(interactions);

            // Update Trigger
            if (interactions.ContainsKey(DeviceInputType.Trigger)) UpdateTriggerData(interactions);
        }


        /// <summary>
        /// Update the "Controller" input from the device
        /// </summary>
        private void UpdateControllerDataDictionary()
        {
            // Get Controller start position and tracked state
            var controllerTracked = true;
            var controllerState = controllerTracked ? ControllerState.Tracked : ControllerState.NotTracked;
            Assert.AreNotEqual(ControllerState.None, controllerState);
        }

        /// <summary>
        /// Update the "Spatial Pointer" input from the device
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        private void UpdatePointerData(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            Vector3 currentControllerPosition = Vector3.left;
            Quaternion currentControllerRotation = Quaternion.identity;

            interactions.SetDictionaryValue(DeviceInputType.SpatialPointer, new SixDof(currentControllerPosition, currentControllerRotation));
        }

        /// <summary>
        /// Update the "Spatial Grip" input from the device
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        private void UpdateGripData(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            Vector3 currentGripPosition = Vector3.left;
            Quaternion currentGripRotation = Quaternion.identity;

            interactions.SetDictionaryValue(DeviceInputType.SpatialGrip, new SixDof(currentGripPosition, currentGripRotation));
        }

        /// <summary>
        /// Update the Touchpad input from the device
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        private void UpdateTouchPadData(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            if (interactions.ContainsKey(DeviceInputType.TouchpadTouch)) interactions.SetDictionaryValue(DeviceInputType.TouchpadTouch, true);  //Interactions[DeviceInputType.TouchpadTouch].SetRawValue(interactionSourceState.touchpadTouched);
            if (interactions.ContainsKey(DeviceInputType.TouchpadPress)) interactions.SetDictionaryValue(DeviceInputType.TouchpadPress, true);  //Interactions[DeviceInputType.TouchpadPress].SetRawValue(interactionSourceState.touchpadPressed);
            if (interactions.ContainsKey(DeviceInputType.Touchpad)) interactions.SetDictionaryValue(DeviceInputType.Touchpad, Vector2.left);  //Interactions[DeviceInputType.Touchpad].SetRawValue(interactionSourceState.touchpadPosition);
        }

        /// <summary>
        /// Update the Thumbstick input from the device
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        private void UpdateThumbStickData(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            if (interactions.ContainsKey(DeviceInputType.ThumbStickPress)) interactions.SetDictionaryValue(DeviceInputType.ThumbStickPress, true);  //Interactions[DeviceInputType.ThumbStickPress].SetRawValue(interactionSourceState.thumbstickPressed);

            interactions.SetDictionaryValue(DeviceInputType.ThumbStick, Vector2.left);
        }

        /// <summary>
        /// Update the Trigger input from the device
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        private void UpdateTriggerData(Dictionary<DeviceInputType, IMixedRealityInteractionMapping> interactions)
        {
            if (interactions.ContainsKey(DeviceInputType.TriggerPress)) interactions.SetDictionaryValue(DeviceInputType.TriggerPress, true);  //Interactions[DeviceInputType.TriggerPress].SetRawValue(interactionSourceState.selectPressed);

            interactions.SetDictionaryValue(DeviceInputType.Trigger, 1f);
        }

        #endregion InteractionDictionaryPerformance Update data functions

        [Test]
        public void Test50_InteractionArrayPerformance()
        {
            //Setup
            var interactions = new List<IMixedRealityInteractionMapping>();

            interactions.Add(new MixedRealityInteractionMapping<SixDof>(1, AxisType.SixDof, DeviceInputType.SpatialPointer, new InputAction(1, "Select")));
            interactions.Add(new MixedRealityInteractionMapping<float>(2, AxisType.SingleAxis, DeviceInputType.Trigger, new InputAction(1, "Select")));
            interactions.Add(new MixedRealityInteractionMapping<SixDof>(3, AxisType.SixDof, DeviceInputType.SpatialGrip, new InputAction(2, "Grip")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(4, AxisType.Digital, DeviceInputType.GripPress, new InputAction(3, "Grab")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(5, AxisType.Digital, DeviceInputType.Menu, new InputAction(4, "Menu")));
            interactions.Add(new MixedRealityInteractionMapping<Vector2>(6, AxisType.DualAxis, DeviceInputType.ThumbStick, new InputAction(5, "Walk")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(7, AxisType.Digital, DeviceInputType.ThumbStickPress, new InputAction(6, "Interact")));
            interactions.Add(new MixedRealityInteractionMapping<Vector2>(8, AxisType.DualAxis, DeviceInputType.Touchpad, new InputAction(7, "Inventory")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(9, AxisType.Digital, DeviceInputType.TouchpadTouch, new InputAction(8, "Pickup")));
            interactions.Add(new MixedRealityInteractionMapping<bool>(10, AxisType.Digital, DeviceInputType.TouchpadPress, new InputAction(8, "Pickup")));

            IMixedRealityInteractionMapping[] interactionsArray = interactions.ToArray();

            //Test
            for (int i = 0; i < 500; i++)
            {
                UpdateController(interactionsArray);
            }
        }

        #region InteractionArrayPerformance Update data functions

        /// <summary>
        /// Update the controller data from the provided platform state
        /// </summary>
        /// <param name="interactions">The InteractionSourceState retrieved from the platform</param>
        public void UpdateController(IMixedRealityInteractionMapping[] interactions)
        {
            UpdateControllerDataArray();

            for (int i = 0; i < interactions.Length; i++)
            {
                switch (interactions[i].InputType)
                {
                    case DeviceInputType.SpatialPointer:
                    case DeviceInputType.PointerPosition:
                    case DeviceInputType.PointerRotation:
                        UpdatePointerData(interactions[i]);
                        break;
                    case DeviceInputType.Select:
                    case DeviceInputType.Trigger:
                    case DeviceInputType.TriggerPress:
                    case DeviceInputType.PointerClick:
                        UpdateTriggerData(interactions[i]);
                        break;
                    case DeviceInputType.SpatialGrip:
                    case DeviceInputType.GripPosition:
                    case DeviceInputType.GripRotation:
                    case DeviceInputType.GripPress:
                        UpdateGripData(interactions[i]);
                        break;
                    case DeviceInputType.ThumbStick:
                    case DeviceInputType.ThumbStickPress:
                        UpdateThumbStickData(interactions[i]);
                        break;
                    case DeviceInputType.Touchpad:
                    case DeviceInputType.TouchpadTouch:
                    case DeviceInputType.TouchpadPress:
                        UpdateTouchPadData(interactions[i]);
                        break;
                    case DeviceInputType.Menu:
                        {
                            var interaction = interactions[i] as MixedRealityInteractionMapping<bool>;
                            Debug.Assert(interaction != null);
                            interaction.SetBoolValue(true);
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
        private void UpdateControllerDataArray()
        {
            // Get Controller start position and tracked state
            bool isControllerTracked = true;
            var controllerState = isControllerTracked ? ControllerState.Tracked : ControllerState.NotTracked;
            Assert.AreNotEqual(ControllerState.None, controllerState);
        }

        /// <summary>
        /// Update the "Spatial Pointer" input from the device
        /// </summary>
        private void UpdatePointerData(IMixedRealityInteractionMapping interactionMapping)
        {
            Vector3 currentPointerPosition = Vector3.left;
            Quaternion currentPointerRotation = Quaternion.identity;

            var interaction = interactionMapping as MixedRealityInteractionMapping<SixDof>;
            currentPointerData.Position = currentPointerPosition;
            currentPointerData.Rotation = currentPointerRotation;
            Debug.Assert(interaction != null);
            interaction.SetSixDofValue(currentPointerData);
        }

        private SixDof currentPointerData = new SixDof(Vector3.zero, Quaternion.identity);

        /// <summary>
        /// Update the "Spatial Grip" input from the device
        /// </summary>
        /// <param name="interactionMapping"></param>
        private void UpdateGripData(IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.SpatialGrip:
                    {
                        Vector3 currentGripPosition = Vector3.left;
                        Quaternion currentGripRotation = Quaternion.identity;

                        var interaction = interactionMapping as MixedRealityInteractionMapping<SixDof>;
                        var value = new SixDof(currentGripPosition, currentGripRotation);
                        Debug.Assert(interaction != null);
                        interaction.SetSixDofValue(value);
                        break;
                    }
                case DeviceInputType.GripPress:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<bool>;
                        var value = true;
                        Debug.Assert(interaction != null);
                        interaction.SetBoolValue(value);
                        break;
                    }
            }
        }

        /// <summary>
        /// Update the Touchpad input from the device
        /// </summary>
        /// <param name="interactionMapping"></param>
        private void UpdateTouchPadData(IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.TouchpadTouch:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<bool>;
                        Debug.Assert(interaction != null);
                        interaction.SetBoolValue(true);
                        break;
                    }
                case DeviceInputType.TouchpadPress:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<bool>;
                        Debug.Assert(interaction != null);
                        interaction.SetBoolValue(true);
                        break;
                    }
                case DeviceInputType.Touchpad:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<Vector2>;
                        Debug.Assert(interaction != null);
                        interaction.SetVector2Value(Vector2.left);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Update the Thumbstick input from the device
        /// </summary>
        /// <param name="interactionMapping"></param>
        private void UpdateThumbStickData(IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.ThumbStickPress:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<bool>;
                        Debug.Assert(interaction != null);
                        interaction.SetBoolValue(true);
                        break;
                    }
                case DeviceInputType.ThumbStick:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<Vector2>;
                        Debug.Assert(interaction != null);
                        interaction.SetVector2Value(Vector2.down);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Update the Trigger input from the device
        /// </summary>
        /// <param name="interactionMapping"></param>
        private void UpdateTriggerData(IMixedRealityInteractionMapping interactionMapping)
        {
            switch (interactionMapping.InputType)
            {
                case DeviceInputType.Select:
                case DeviceInputType.TriggerPress:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<bool>;
                        Debug.Assert(interaction != null);
                        interaction.SetBoolValue(true);
                        break;
                    }
                case DeviceInputType.Trigger:
                    {
                        var interaction = interactionMapping as MixedRealityInteractionMapping<float>;
                        Debug.Assert(interaction != null);
                        interaction.SetFloatValue(1f);
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        #endregion InteractionArrayPerformance Update data functions

        #endregion Performance Tests
    }
}