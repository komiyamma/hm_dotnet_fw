﻿/*
 * Copyright (c) 2017 Akitsugu Komiyama
 * under the MIT License
 */


using System;
using System.Text;
using System.Runtime.InteropServices;



// ★秀丸クラス
internal sealed partial class hmNETDynamicLib
{
    internal sealed partial class Hidemaru
    {
        public sealed class Edit
        {
            static Edit()
            {
                SetUnManagedDll();
            }

            public static bool CheckQueueStatus
            {
                get { return pCheckQueueStatus() != 0; }
            }

            // 座標型。Point型では、System.Drawingを読み込まないとダメなので負荷がある。また、x, yは秀丸に別値として存在するので、
            // あくまでも、マクロのcolumnとlinenoと一致しているという主張。なお、x, yはワープロ的な座標を拾ってくる。
            // columnやlinenoはエディタ的な座標である。
            public struct HmCursurPos : global::Hidemaru.Hm.Edit.ICursorPos
            {
                private int m_lineno;
                private int m_column;
                public HmCursurPos(int lineno, int column)
                {
                    this.m_lineno = lineno;
                    this.m_column = column;
                }
                public int Column { get { return m_column; } }
                public int LineNo { get { return m_lineno; } }
            }


            public class HmMousePos : global::Hidemaru.Hm.Edit.IMousePos
            {
                private int m_lineno;
                private int m_column;
                private int m_x;
                private int m_y;
                public HmMousePos(int x, int y, int lineno, int column)
                {
                    this.m_lineno = lineno;
                    this.m_column = column;
                    this.m_x = x;
                    this.m_y = y;
                }
                public int Column { get { return m_column; } }
                public int LineNo { get { return m_lineno; } }
                public int X { get { return m_x; } }
                public int Y { get { return m_y; } }
            }


            /// <summary>
            ///  CursorPos
            /// </summary>
            public static HmCursurPos CursorPos
            {
                get
                {
                    return GetCursorPos();
                }
            }

            public static HmMousePos MousePos
            {
                get
                {
                    return GetCursorPosFromMousePos();
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int X;
                public int Y;
            }
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool GetCursorPos(out POINT lpPoint);


            // columnやlinenoはエディタ的な座標である。
            private static HmCursurPos GetCursorPos()
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return new HmCursurPos(-1, -1);
                }

                int column = -1;
                int lineno = -1;
                int success = pGetCursorPosUnicode(ref lineno, ref column);
                if (success > 0)
                {
                    HmCursurPos p = new HmCursurPos(lineno, column);
                    return p;
                }

                return new HmCursurPos(-1, -1);
            }

            // columnやlinenoはエディタ的な座標である。
            private static HmMousePos GetCursorPosFromMousePos()
            {
                if (version < 873)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed873);
                    return new HmMousePos(-1, -1, -1, -1);
                }

                // この関数が存在しないバージョン
                if (pGetCursorPosUnicodeFromMousePos == null)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed873);
                    return new HmMousePos(-1, -1, -1, -1);
                }

                POINT lpPoint;
                bool success = GetCursorPos(out lpPoint);
                if (!success)
                {
                    lpPoint.X = -1;
                    lpPoint.Y = -1;
                }
                int column = -1;
                int lineno = -1;
                pGetCursorPosUnicodeFromMousePos(IntPtr.Zero, ref lineno, ref column);
                HmMousePos p = new HmMousePos(lpPoint.X, lpPoint.Y, lineno, column);
                return p;
            }

            public static String FileName
            {
                get
                {
                    IntPtr hWndHidemaru = WindowHandle;
                    if (hWndHidemaru != IntPtr.Zero) {
                        const int WM_USER = 0x400;
                        const int WM_HIDEMARUINFO = WM_USER + 181;
                        const int HIDEMARUINFO_GETFILEFULLPATH = 4;

                        StringBuilder sb = new StringBuilder(512);
                        bool cwch = SendMessage(hWndHidemaru, WM_HIDEMARUINFO, new IntPtr(HIDEMARUINFO_GETFILEFULLPATH), sb);
                        String filename = sb.ToString();
                        if ( String.IsNullOrEmpty(filename) )
                        {
                            return "";
                        } else
                        {
                            return filename;
                        }
                    }
                    return "";
                }
            }

            public static int UpdateCount
            {
                get
                {
                    if (version < 912.98)
                    {
                        OutputDebugStream(ErrorMsg.MethodNeed912);
                        throw new MissingMethodException("Hidemaru_Edit_UpdateCount");
                    }
                    IntPtr hWndHidemaru = WindowHandle;
                    if (hWndHidemaru != IntPtr.Zero)
                    {
                        const int WM_USER = 0x400;
                        const int WM_HIDEMARUINFO = WM_USER + 181;
                        const int HIDEMARUINFO_GETUPDATECOUNT = 7;

                        IntPtr updatecount = SendMessage(hWndHidemaru, WM_HIDEMARUINFO, HIDEMARUINFO_GETUPDATECOUNT, IntPtr.Zero);
                        return (int)updatecount;
                    }
                    return -1;
                }
            }

            public static int InputStates
            {
                get
                {
                    if (version < 919.11)
                    {
                        OutputDebugStream(ErrorMsg.MethodNeed919);
                        throw new MissingMethodException("Hidemaru_Edit_InputStates");
                    }
                    if (pGetInputStates == null)
                    {
                        OutputDebugStream(ErrorMsg.MethodNeed919);
                        throw new MissingMethodException("Hidemaru_Edit_InputStates");
                    }

                    return pGetInputStates();
                }
            }

            /// <summary>
            /// TotalText
            /// </summary>
            public static String TotalText
            {
                get
                {
                    return GetTotalText();
                }
                set
                {
                    // 935.β6以降は、settotaltext() が実装された。
                    if (version >= 935.06)
                    {
                        SetTotalText2(value);
                    }
                    else
                    {
                        SetTotalText(value);
                    }
                }
            }

            // 現在の秀丸の編集中のテキスト全て。元が何の文字コードでも関係なく秀丸がwchar_tのユニコードで返してくれるので、
            // String^型に入れておけば良い。
            private static String GetTotalText()
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return "";
                }

                IntPtr hGlobal = pGetTotalTextUnicode();
                return HGlobalToString(hGlobal);
            }

            private static void SetTotalText(String value)
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return;
                }

                int dll = iDllBindHandle;

                if (dll == 0)
                {
                    throw new NullReferenceException(ErrorMsg.NoDllBindHandle866);
                }

                SetTmpVar(value);
                String cmd = ModifyFuncCallByDllType(
                    "begingroupundo;\n" +
                    "rangeeditout;\n" +
                    "selectall;\n" +
                    "insert dllfuncstrw( {0} \"PopStrVar\" );\n" +
                    "endgroupundo;\n"
                );
                if (Macro.IsExecuting)
                {
                    Macro.Eval(cmd);
                }
                else
                {
                    Macro.ExecEval(cmd);
                }
                SetTmpVar(null);
            }

            private static void SetTotalText2(String value)
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return;
                }


                int dll = iDllBindHandle;

                if (dll == 0)
                {
                    throw new NullReferenceException(ErrorMsg.NoDllBindHandle866);
                }

                SetTmpVar(value);
                String cmd = ModifyFuncCallByDllType(
                    "settotaltext dllfuncstrw( {0} \"PopStrVar\" );\n"
                );
                if (Macro.IsExecuting)
                {
                    Macro.Eval(cmd);
                }
                else
                {
                    Macro.ExecEval(cmd);
                }
                SetTmpVar(null);
            }

            /// <summary>
            ///  SelecetdText
            /// </summary>
            public static String SelectedText
            {
                get
                {
                    return GetSelectedText();
                }
                set
                {
                    SetSelectedText(value);
                }

            }

            // 現在の秀丸の選択中のテキスト。元が何の文字コードでも関係なく秀丸がwchar_tのユニコードで返してくれるので、
            // String^型に入れておけば良い。
            private static String GetSelectedText()
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return "";
                }

                String curstr = "";
                IntPtr hGlobal = pGetSelectedTextUnicode();
                curstr = HGlobalToString(hGlobal);

                if (curstr == null)
                {
                    curstr = "";
                }
                return curstr;
            }

            private static void SetSelectedText(String value)
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return;
                }

                int dll = iDllBindHandle;

                if (dll == 0)
                {
                    throw new NullReferenceException(ErrorMsg.NoDllBindHandle866);
                }

                SetTmpVar(value);
                String invocate = ModifyFuncCallByDllType("{0}");
                String cmd =
                    "if (selecting) {\n" +
                    "insert dllfuncstrw( " + invocate + " \"PopStrVar\" );\n" +
                    "}\n";
                if (Macro.IsExecuting)
                {
                    Macro.Eval(cmd);
                }
                else
                {
                    Macro.ExecEval(cmd);
                }
                SetTmpVar(null);
            }


            /// <summary>
            /// LineText
            /// </summary>
            public static String LineText
            {
                get
                {
                    return GetLineText();
                }
                set
                {
                    SetLineText(value);
                }
            }

            // 現在の秀丸の編集中のテキストで、カーソルがある行だけのテキスト。
            // 元が何の文字コードでも関係なく秀丸がwchar_tのユニコードで返してくれるので、
            // String^型に入れておけば良い。
            private static String GetLineText()
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return "";
                }

                HmCursurPos p = GetCursorPos();

                IntPtr hGlobal = pGetLineTextUnicode(p.LineNo);
                return HGlobalToString(hGlobal);
            }

            private static void SetLineText(String value)
            {
                if (version < 866)
                {
                    OutputDebugStream(ErrorMsg.MethodNeed866);
                    return;
                }

                int dll = iDllBindHandle;

                if (dll == 0)
                {
                    throw new NullReferenceException(ErrorMsg.NoDllBindHandle866);
                }

                SetTmpVar(value);
                var pos = GetCursorPos();
                String cmd = ModifyFuncCallByDllType(
                    "begingroupundo;\n" +
                    "selectline;\n" +
                    "insert dllfuncstrw( {0} \"PopStrVar\" );\n" +
                    "moveto2 " + pos.Column + ", " + pos.LineNo + ";\n" +
                    "endgroupundo;\n"
                );
                if (Macro.IsExecuting)
                {
                    Macro.Eval(cmd);
                }
                else
                {
                    Macro.ExecEval(cmd);
                }
                SetTmpVar(null);
            }
        }
    }
}

