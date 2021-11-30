using System;
using System.Windows.Forms;

namespace GlobalHotkeys
{
    [Flags]
    public enum Modifiers
    {
        NoMod = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        Shift = 0x0004,
        Win = 0x0008
    }
    public static class Constants
    {
        //modifiers
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }

    public class GlobalHotkeys : IDisposable
    {
        public Modifiers Modifier { get; private set; }
        public int Key { get; private set; }
        public int Id { get; private set; }

        private readonly IntPtr hWnd;
        private bool registered;

        /// <summary>
        /// Creates a GlobalHotkey object.
        /// </summary>
        /// <param name="modifier">Hotkey modifier keys</param>
        /// <param name="key">Hotkey</param>
        /// <param name="window">The Window that the hotkey should be registered to</param>
        /// <param name="registerImmediately"> </param>
        public GlobalHotkeys(Modifiers modifier, Keys key, IWin32Window window, bool registerImmediately = false)
        {
            if (window == null) throw new ArgumentNullException("window", "You must provide a form or window to register the hotkey against.");
            Modifier = modifier;
            Key = (int)key;
            hWnd = window.Handle;
            Id = GetHashCode();
            if (registerImmediately) Register();
        }

        /// <summary>
        /// Registers the current hotkey with Windows.
        /// Note! You must override the WndProc method in your window that registers the hotkey, or you will not receive any hotkey notifications.
        /// </summary>
        public void Register()
        {
            if (!SystemMethods.RegisterHotKey(hWnd, Id, (int)Modifier, Key))
                throw new GlobalHotkeysException("Hotkey failed to register.");
            registered = true;
        }

        /// <summary>
        /// Unregisters the current hotkey with Windows.
        /// </summary>
        public void Unregister()
        {
            if (!registered) return;
            if (!SystemMethods.UnregisterHotKey(hWnd, Id))
                throw new GlobalHotkeysException("Hotkey failed to unregister.");
            registered = false;
        }

        #region IDisposable Members / Finalizer

        public void Dispose()
        {
            Unregister();
            GC.SuppressFinalize(this);
        }

        ~GlobalHotkeys()
        {
            Unregister();
        }

        #endregion

        #region Overrides

        public override sealed int GetHashCode()
        {
            return (int)Modifier ^ Key ^ hWnd.ToInt32();
        }

        #endregion
    }

    public class GlobalHotkeysException : Exception
    {
        public GlobalHotkeysException(string message) : base(message) { }
        public GlobalHotkeysException(string message, Exception inner) : base(message, inner) { }
    }


}
