using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Illumine
{
    public partial class KeybindSetter : Form
    {
        public HashSet<Keys> chosenKeys;
        public delegate bool KeybindSetCallback(HashSet<Keys> keys);

        private readonly List<KeybindSetCallback> callbacks;

        public KeybindSetter()
        {
            InitializeComponent();

            chosenKeys = new HashSet<Keys>();
            callbacks = new List<KeybindSetCallback>();
        }

        public void RegisterCallback(KeybindSetCallback callback)
        {
            callbacks.Add(callback);
        }

        private void DoCallbacks()
        {
            foreach (KeybindSetCallback callback in callbacks)
            {
                // Something went wrong. Callback is responsible for rolling back to the old keybind.
                if (!callback(chosenKeys)) {
                    chosenKeys.Clear();
                    KeysPressedLabel.Text = "Invalid keybind";
                    break;
                }
            }
        }

        private void KeybindSetter_Shown(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void KeybindSetter_KeyDown(object sender, KeyEventArgs e)
        {
            chosenKeys.Add(e.KeyCode);

            List<(string, string)> replacePairs = new List<(string, string)>()
            {
                { ("Oem", "") }, { ("Key", "") }, { ("Menu", "Alt") }, { ("LWin", "Win") }, { ("RWin", "Win") },
            };

            string outString = string.Join(" + ", chosenKeys);
            foreach ((string, string)replacePair in replacePairs) {
                outString = outString.Replace(replacePair.Item1, replacePair.Item2);
            }

            KeysPressedLabel.Text = outString.ToUpper();

            SetKeybindButton.Enabled = true;
        }

        private void SetKeybindButton_Click(object sender, EventArgs e)
        {
            DoCallbacks();
            SetKeybindButton.Enabled = false;
        }

        private void ResetKeybindButton_Click(object sender, EventArgs e)
        {
            chosenKeys.Clear();
            KeysPressedLabel.Text = "...";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
