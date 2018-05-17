﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Internal.EventDatum.Input
{
    /// <summary>
    /// Describes an Input Event with voice dictation.
    /// </summary>
    public class DictationEventData : BaseInputEventData
    {
        /// <summary>
        /// String result of the current dictation.
        /// </summary>
        public string DictationResult { get; private set; }

        /// <summary>
        /// Audio Clip of the last Dictation recording Session.
        /// </summary>
        public AudioClip DictationAudioClip { get; private set; }

        /// <inheritdoc />
        public DictationEventData(UnityEngine.EventSystems.EventSystem eventSystem) : base(eventSystem) { }

        /// <summary>
        /// Used to initialize/reset the event and populate the data.
        /// </summary>
        /// <param name="inputSource"></param>
        /// <param name="dictationResult"></param>
        /// <param name="dictationAudioClip"></param>
        public void Initialize(IMixedRealityInputSource inputSource, string dictationResult, AudioClip dictationAudioClip = null)
        {
            BaseInitialize(inputSource);
            DictationResult = dictationResult;
            DictationAudioClip = dictationAudioClip;
        }
    }
}
