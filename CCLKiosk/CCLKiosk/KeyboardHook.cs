using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CCLKiosk
{
    public class KeyboardHook : IDisposable
    {
        public delegate void LocalKeyEventHandler(Keys key);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;

        public delegate int CallbackDelegate(int Code, int W, int L);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);

        public enum HookType : int
        {
            WH_KEYBOARD = 2,
            WH_KEYBOARD_LL = 13,
        }

        private int HookID = 0;
        CallbackDelegate TheHookCB = null;

        //Start hook
        public KeyboardHook()
        {
            TheHookCB = new CallbackDelegate(KeybHookProc);
            HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, TheHookCB, 0, 0);
        }

        //The listener that will trigger events
        private int KeybHookProc(int Code, int W, int L)
        {
            if (Code < 0) return CallNextHookEx(HookID, Code, W, L);
            try
            {
                KeyEvents kEvent = (KeyEvents)W;
                Int32 vkCode = Marshal.ReadInt32((IntPtr)L);
                if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown) KeyDown?.Invoke((Keys)vkCode);
                if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp) KeyUp?.Invoke((Keys)vkCode);
            }
            catch { }

            return CallNextHookEx(HookID, Code, W, L);
        }

        public enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        bool IsFinalized = false;
        ~KeyboardHook()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
    }
}
