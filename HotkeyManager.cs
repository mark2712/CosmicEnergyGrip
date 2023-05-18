using System.Runtime.InteropServices;

namespace HotkeyManager
{
    public class HotkeyManager1
    {
        private const int WM_HOTKEY = 0x0312;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int MOD_ALT = 0x0001;
        private const int MOD_WIN = 0x0008;
        public int HotkeyId;
        public IntPtr Handle;

        public event EventHandler? HotkeyPressed;

        public void HotkeyManagerStart(Form form)
        {
            Handle = form.Handle;
            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            HotkeyId = 1; // Уникальный идентификатор горячей клавиши

            // Регистрация горячей клавиши P
            //RegisterHotKey(Handle, HotkeyId, 0x0000 | 0x4000, (int)Keys.P);

            // Регистрация горячей клавиши Ctrl+Shift+E
            RegisterHotKey(Handle, HotkeyId, MOD_CONTROL | MOD_SHIFT | 0x4000, (int)Keys.E);
        }


        public void FormClosing()
        {
            UnregisterHotKey(Handle, HotkeyId);
        }

        public void ProcessHotkey(Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HotkeyId)
            {
                HotkeyPressed?.Invoke(this, EventArgs.Empty);
            }
        }


        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}