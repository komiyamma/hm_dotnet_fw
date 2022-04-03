/*
 * Copyright (c) 2017 Akitsugu Komiyama
 * under the MIT License
 */

using System;
using System.Runtime.InteropServices;


// ★秀丸クラス
internal sealed partial class hmNETDynamicLib
{
    internal sealed partial class Hidemaru
    {
        // 途中でエラーが出るかもしれないので、相応しいUnlockやFreeが出来るように内部管理用
        private enum HGlobalStatus { None, Lock, Unlock, Free };
        private static string HGlobalToString(IntPtr hGlobal)
        {
            String retstr = "";
            HGlobalStatus hgs = HGlobalStatus.None;
            if (hGlobal != IntPtr.Zero)
            {
                try
                {
                    IntPtr ret = GlobalLock(hGlobal);
                    hgs = HGlobalStatus.Lock;
                    retstr = Marshal.PtrToStringUni(ret);
                    GlobalUnlock(hGlobal);
                    hgs = HGlobalStatus.Unlock;
                    GlobalFree(hGlobal);
                    hgs = HGlobalStatus.Free;
                }
                catch (Exception e)
                {
                    OutputDebugStream(e.Message);
                }
                finally
                {
                    switch (hgs)
                    {
                        // ロックだけ成功した
                        case HGlobalStatus.Lock:
                            {
                                GlobalUnlock(hGlobal);
                                GlobalFree(hGlobal);
                                break;
                            }
                        // アンロックまで成功した
                        case HGlobalStatus.Unlock:
                            {
                                GlobalFree(hGlobal);
                                break;
                            }
                        // フリーまで成功した
                        case HGlobalStatus.Free:
                            {
                                break;
                            }
                    }
                }
            }
            return retstr;
        }
    }
}
