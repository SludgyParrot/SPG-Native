﻿using System;
using UnityEngine;

namespace Native.UI
{
    /// <summary>
    /// Implements the <see cref="IVirtualKeyboard"/> interface to provide a virtual keyboard using Unity's TouchScreenKeyboard.
    /// </summary>
    /// <remarks>
    /// This class wraps Unity's <see cref="UnityEngine.TouchScreenKeyboard"/> to provide an interface for displaying a virtual keyboard. 
    /// For more information on Unity's TouchScreenKeyboard, refer to the 
    /// <a href="https://docs.unity3d.com/Manual/script-TouchKeyboard.html">Unity Touch Keyboard Documentation</a>.
    /// </remarks>
    public class TouchScreenKeyboard : IVirtualKeyboard,  IDisposable
    {
        private UnityEngine.TouchScreenKeyboard virtualKeyboard;

        private bool isShowing;

        /// <summary>
        /// Displays the virtual keyboard with various customization options.
        /// </summary>
        /// <param name="prompt">
        /// The prompt text to display to the user. If <c>null</c>, no prompt will be shown.
        /// </param>
        /// <param name="keyboardType">
        /// The type of keyboard to display. Defaults to <see cref="TouchScreenKeyboardType.Default"/>.
        /// For more options, refer to the 
        /// <a href="https://docs.unity3d.com/ScriptReference/TouchScreenKeyboardType.html">TouchScreenKeyboardType Documentation</a>.
        /// </param>
        /// <param name="enableAutoCorrection">
        /// Whether to enable auto-correction. Defaults to <c>false</c>.
        /// </param>
        /// <param name="useMultiLines">
        /// Whether to allow multiple lines of input. Defaults to <c>false</c>.
        /// </param>
        /// <param name="secure">
        /// Whether to use secure input mode (e.g., for password input). Defaults to <c>false</c>.
        /// </param>
        /// <param name="enableAlert">
        /// Whether to enable an alert message. Defaults to <c>false</c>.
        /// </param>
        /// <param name="initialText">
        /// The initial text to display in the input field. If <c>null</c>, no initial text will be shown.
        /// </param>
        /// <param name="maxTextLength">
        /// The maximum length of the input text. Defaults to <c>300</c>.
        /// </param>
        /// <remarks>
        /// If the virtual keyboard is already showing, this method will not create a new instance.
        /// </remarks>
        public void Show(string prompt = null, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, bool enableAutoCorrection = false, bool useMultiLines = false, bool secure = false, bool enableAlert = false, string initialText = null, int maxTextLength = 300)
        {
            if (virtualKeyboard != null)
            {
                return;
            }

            virtualKeyboard = new UnityEngine.TouchScreenKeyboard(prompt, keyboardType, enableAutoCorrection, useMultiLines, secure, enableAlert, initialText, maxTextLength);
        }

        /// <summary>
        /// Disposes of the virtual keyboard resources.
        /// </summary>
        /// <remarks>
        /// The current implementation throws <see cref="NotImplementedException"/>. You should provide a proper implementation
        /// to handle the disposal of resources and cleanup.
        /// </remarks>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finalizer for the <see cref="TouchScreenKeyboard"/> class.
        /// </summary>
        /// <remarks>
        /// This finalizer is not currently implemented. Consider implementing proper cleanup if needed.
        /// </remarks>
        ~TouchScreenKeyboard()
        {
            // Finalization logic can be implemented here if needed.
        }
    }
}