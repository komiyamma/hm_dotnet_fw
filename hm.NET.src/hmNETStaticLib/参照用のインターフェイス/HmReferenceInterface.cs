/*
 * Copyright (c) 2017 Akitsugu Komiyama
 * under the MIT License
 */

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Hidemaru
{
    public static class Hm
    {
        // メインのウィンドウ
        public static IntPtr WindowHandle
        {
            get
            {
                return hmNETDynamicLib.Hidemaru.WindowHandle;
            }
        }

        // バージョン
        public static Double Version
        {
            get
            {
                return hmNETDynamicLib.Hidemaru.version;
            }
        }

        // マクロ空間
        public static class Macro
        {
            // 実行中か
            public static bool IsExecuting
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Macro.IsExecuting;
                }
            }

            // マクロでの問い合わせ結果系
            public interface IResult
            {
                int Result { get; }
                String Message { get; }
                Exception Error { get; }
            }

            // 問い合わせ結果系の実態。外から見えないように
            private class TResult : IResult
            {
                public int Result { get; set; }
                public string Message { get; set; }
                public Exception Error { get; set; }

                public TResult(int Result, String Message, Exception Error)
                {
                    this.Result = Result;
                    this.Message = Message;
                    this.Error = Error;
                }
            }

            // マクロでの問い合わせ結果系
            public interface IStatementResult
            {
                int Result { get; }
                String Message { get; }
                Exception Error { get; }
                List<Object> Args { get; }
            }


            private class TStatementResult : IStatementResult
            {
                public int Result { get; set; }
                public string Message { get; set; }
                public Exception Error { get; set; }
                public List<Object> Args { get; set; }

                public TStatementResult(int Result, String Message, Exception Error, List<Object> Args)
                {
                    this.Result = Result;
                    this.Message = Message;
                    this.Error = Error;
                    this.Args = new List<Object>(Args); // コピー渡し
                }
            }

            // マクロでの問い合わせ結果系
            public interface IFunctionResult
            {
                object Result { get; }
                String Message { get; }
                Exception Error { get; }
                List<Object> Args { get; }
            }

            private class TFunctionResult : IFunctionResult
            {
                public object Result { get; set; }
                public string Message { get; set; }
                public Exception Error { get; set; }
                public List<Object> Args { get; set; }

                public TFunctionResult(object Result, String Message, Exception Error, List<Object> Args)
                {
                    this.Result = Result;
                    this.Message = Message;
                    this.Error = Error;
                    this.Args = new List<Object>(Args); // コピー渡し
                }
            }

            // 実行系
            public interface IExec
            {
                IResult File(String filepath);
                IResult Eval(String expression);
                IResult Method(string message_parameter, string dllfullpath, string typefullname, string methodname);
                IResult Method(string message_parameter, Delegate delegate_method);
            }

            // 実行系の実態
            private class TExec : IExec
            {
                public IResult Eval(string expression)
                {
                    var ret = hmNETDynamicLib.Hidemaru.Macro.ExecEval(expression);
                    var result = new TResult(ret.Result, ret.Message, ret.Error);
                    return result;
                }

                public IResult File(string filepath)
                {
                    var ret = hmNETDynamicLib.Hidemaru.Macro.ExecFile(filepath);
                    var result = new TResult(ret.Result, ret.Message, ret.Error);
                    return result;
                }

                public IResult Method(string message_parameter, string dllfullpath, string typefullname, string methodname)
                {
                    var ret = hmNETDynamicLib.Hidemaru.Macro.BornMacroScopeMethod(message_parameter, dllfullpath, typefullname, methodname);
                    var result = new TResult(ret.Result, ret.Message, ret.Error);
                    return result;
                }

                public IResult Method(string message_parameter, Delegate delegate_method)
                {
                    if (delegate_method.Method.IsStatic && delegate_method.Method.IsPublic)
                    {
                        var ret = hmNETDynamicLib.Hidemaru.Macro.BornMacroScopeMethod(message_parameter, delegate_method.Method.DeclaringType.Assembly.Location, delegate_method.Method.DeclaringType.FullName, delegate_method.Method.Name);
                        var result = new TResult(ret.Result, ret.Message, ret.Error);
                        return result;
                    }
                    else if (!delegate_method.Method.IsStatic)
                    {

                        string message_no_static = delegate_method.Method.DeclaringType.FullName + "." + delegate_method.Method.Name + " is not 'STATIC' in " + delegate_method.Method.DeclaringType.Assembly.Location;
                        var result_no_static = new TResult(0, "", new MissingMethodException(message_no_static));
                        System.Diagnostics.Trace.WriteLine(message_no_static);
                        return result_no_static;
                    }
                    else if (!delegate_method.Method.IsPublic)
                    {
                        string message_no_public = delegate_method.Method.DeclaringType.FullName + "." + delegate_method.Method.Name + " is not 'PUBLIC' in " + delegate_method.Method.DeclaringType.Assembly.Location;
                        var result_no_public = new TResult(0, "", new MissingMethodException(message_no_public));
                        System.Diagnostics.Trace.WriteLine(message_no_public);
                        return result_no_public;
                    }
                    string message_missing = delegate_method.Method.DeclaringType.FullName + "." + delegate_method.Method.Name + "is 'MISSING' access in " + delegate_method.Method.DeclaringType.Assembly.Location;
                    var result_missing = new TResult(0, "", new MissingMethodException(delegate_method.Method.Name + " is missing access"));
                    System.Diagnostics.Trace.WriteLine(result_missing);
                    return result_missing;
                }

            }

            public static IExec Exec = new TExec();


            public static IStatementResult Statement(string funcname, params object[] args)
            {
                var ret = hmNETDynamicLib.Hidemaru.Macro.AsStatementTryInvokeMember(funcname, args);
                IStatementResult result = new TStatementResult(ret.Result, ret.Message, ret.Error, ret.Args);
                return result;
            }

            public static IFunctionResult Function(string funcname, params object[] args)
            {
                var ret = hmNETDynamicLib.Hidemaru.Macro.AsFunctionTryInvokeMember<Object>(funcname, args);
                IFunctionResult result = new TFunctionResult(ret.Result, ret.Message, ret.Error, ret.Args);
                return result;
            }

            public static IFunctionResult Function<T>(string funcname, params object[] args)
            {
                var ret = hmNETDynamicLib.Hidemaru.Macro.AsFunctionTryInvokeMember<T>(funcname, args);
                IFunctionResult result = new TFunctionResult(ret.Result, ret.Message, ret.Error, ret.Args);
                return result;
            }

            public static IResult Eval(String expression)
            {
                var ret = hmNETDynamicLib.Hidemaru.Macro.Eval(expression);
                var result = new TResult(ret.Result, ret.Message, ret.Error);
                return result;
            }

            public static IVar Var = new TVar();
            public interface IVar
            {
                Object this[String name] { get; set; }
            }
            private class TVar : IVar
            {
                public Object this[String name]
                {
                    get
                    {
                        return hmNETDynamicLib.Hidemaru.Macro.Var[name];
                    }
                    set
                    {
                        hmNETDynamicLib.Hidemaru.Macro.Var[name] = value;
                    }
                }
            }

            public static IStaticVar StaticVar = new TStaticVar();
            public interface IStaticVar
            {
                string this[String name, int sharedflag] { get; set; }
                string Get(string name, int sharedflag);
                bool Set(string name, string value, int sharedflag);
            }

            private class TStaticVar : IStaticVar
            {
                public String this[String name, int sharedflag]
                {
                    get
                    {
                        return hmNETDynamicLib.Hidemaru.Macro.GetStaticVariable(name, sharedflag);
                    }
                    set
                    {
                        var ret = hmNETDynamicLib.Hidemaru.Macro.SetStaticVariable(name, value, sharedflag);
                    }
                }

                public string Get(string name, int sharedflag)
                {
                    return hmNETDynamicLib.Hidemaru.Macro.GetStaticVariable(name, sharedflag);
                }

                public bool Set(string name, string value, int sharedflag)
                {
                    var ret = hmNETDynamicLib.Hidemaru.Macro.SetStaticVariable(name, value, sharedflag);
                    if (ret != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public static class Flags
            {
                public static class Encode
                {
                    //OPENFILE等のENCODE相当
                    public static int Sjis { get { return 0x01; } }
                    public static int Utf16 { get { return 0x02; } }
                    public static int Euc { get { return 0x03; } }
                    public static int Jis { get { return 0x04; } }
                    public static int Utf7 { get { return 0x05; } }
                    public static int Utf8 { get { return 0x06; } }
                    public static int Utf16_be { get { return 0x07; } }
                    public static int Euro { get { return 0x08; } }
                    public static int Gb2312 { get { return 0x09; } }
                    public static int Big5 { get { return 0x0a; } }
                    public static int Euckr { get { return 0x0b; } }
                    public static int Johab { get { return 0x0c; } }
                    public static int Easteuro { get { return 0x0d; } }
                    public static int Baltic { get { return 0x0e; } }
                    public static int Greek { get { return 0x0f; } }
                    public static int Russian { get { return 0x10; } }
                    public static int Symbol { get { return 0x11; } }
                    public static int Turkish { get { return 0x12; } }
                    public static int Hebrew { get { return 0x13; } }
                    public static int Arabic { get { return 0x14; } }
                    public static int Thai { get { return 0x15; } }
                    public static int Vietnamese { get { return 0x16; } }
                    public static int Mac { get { return 0x17; } }
                    public static int Oem { get { return 0x18; } }
                    public static int Default { get { return 0x19; } }
                    public static int Utf32 { get { return 0x1b; } }
                    public static int Utf32_be { get { return 0x1c; } }
                    public static int Binary { get { return 0x1a; } }
                    public static int LF { get { return 0x40; } }
                    public static int CR { get { return 0x80; } }

                    //SAVEASの他のオプションの数値指定
                    public static int Bom { get { return 0x0600; } }
                    public static int NoBom { get { return 0x0400; } }
                    public static int Selection { get { return 0x2000; } }

                    //OPENFILEの他のオプションの数値指定
                    public static int NoAddHist { get { return 0x0100; } }
                    public static int WS { get { return 0x0800; } }
                    public static int WB { get { return 0x1000; } }
                }

                public static class SearchOption
                {
                    //searchoption(検索関係)
                    public static int Word { get { return 0x00000001; } }
                    public static int Casesense { get { return 0x00000002; } }
                    public static int NoCasesense { get { return 0x00000000; } }
                    public static int Regular { get { return 0x00000010; } }
                    public static int NoRegular { get { return 0x00000000; } }
                    public static int Fuzzy { get { return 0x00000020; } }
                    public static int Hilight { get { return 0x00003800; } }
                    public static int NoHilight { get { return 0x00002000; } }
                    public static int LinkNext { get { return 0x00000080; } }
                    public static int Loop { get { return 0x01000000; } }

                    //searchoption(マスク関係)
                    public static int MaskComment { get { return 0x00020000; } }
                    public static int MaskIfdef { get { return 0x00040000; } }
                    public static int MaskNormal { get { return 0x00010000; } }
                    public static int MaskScript { get { return 0x00080000; } }
                    public static int MaskString { get { return 0x00100000; } }
                    public static int MaskTag { get { return 0x00200000; } }
                    public static int MaskOnly { get { return 0x00400000; } }
                    public static int FEnableMaskFlags { get { return 0x00800000; } }

                    //searchoption(置換関係)
                    public static int FEnableReplace { get { return 0x00000004; } }
                    public static int Ask { get { return 0x00000008; } }
                    public static int NoClose { get { return 0x02000000; } }

                    //searchoption(grep関係)
                    public static int SubDir { get { return 0x00000100; } }
                    public static int Icon { get { return 0x00000200; } }
                    public static int Filelist { get { return 0x00000040; } }
                    public static int FullPath { get { return 0x00000400; } }
                    public static int OutputSingle { get { return 0x10000000; } }
                    public static int OutputSameTab { get { return 0x20000000; } }

                    //searchoption(grepして置換関係)
                    public static int BackUp { get { return 0x04000000; } }
                    public static int Preview { get { return 0x08000000; } }

                    // searchoption2を使うよ、というフラグ。なんと、int32_maxを超えているので、特殊な処理が必要。
                    public static long FEnableSearchOption2
                    {
                        get
                        {
                            if (IntPtr.Size == 4)
                            {
                                return -0x80000000;
                            }
                            else
                            {
                                return 0x80000000;
                            }
                        }
                    }
                }

                public static class SearchOption2
                {
                    //searchoption2
                    public static int UnMatch { get { return 0x00000001; } }
                    public static int InColorMarker { get { return 0x00000002; } }
                    public static int FGrepFormColumn { get { return 0x00000008; } }
                    public static int FGrepFormHitOnly { get { return 0x00000010; } }
                    public static int FGrepFormSortDate { get { return 0x00000020; } }
                }
            }
        }

        // エディット系
        public static class Edit
        {
            public static String FilePath
            {
                get
                {
                    String file_name = hmNETDynamicLib.Hidemaru.Edit.FileName;
                    if (String.IsNullOrEmpty(file_name))
                    {
                        return null;
                    }
                    else
                    {
                        return file_name;
                    }
                }
            }

            public static bool QueueStatus
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Edit.CheckQueueStatus;
                }
            }

            public static String TotalText
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Edit.TotalText;
                }
                set
                {
                    hmNETDynamicLib.Hidemaru.Edit.TotalText = value;
                }
            }

            public static String SelectedText
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Edit.SelectedText;
                }
                set
                {
                    hmNETDynamicLib.Hidemaru.Edit.SelectedText = value;
                }
            }

            public static String LineText
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Edit.LineText;
                }

                set
                {
                    hmNETDynamicLib.Hidemaru.Edit.LineText = value;
                }

            }

            public interface ICursorPos
            {
                int LineNo { get; }
                int Column { get; }
            }
            public static ICursorPos CursorPos
            {
                get
                {
                    var pos = hmNETDynamicLib.Hidemaru.Edit.CursorPos;
                    return pos;
                }
            }

            public interface IMousePos
            {
                int LineNo { get; }
                int Column { get; }
                int X { get; }
                int Y { get; }
            }
            public static IMousePos MousePos
            {
                get
                {
                    var pos = hmNETDynamicLib.Hidemaru.Edit.MousePos;
                    return pos;
                }
            }

            public static int UpdateCount
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.Edit.UpdateCount;
                }
            }

            public static int InputStates { 
                get 
                {
                    return hmNETDynamicLib.Hidemaru.Edit.InputStates;
                }
            }

        }


        public static class File
        {
            public interface IHidemaruEncoding
            {
                int HmEncode { get; }
            }
            public interface IMicrosoftEncoding
            {
                int MsCodePage { get; }
            }

            public interface IEncoding : IHidemaruEncoding, IMicrosoftEncoding
            {
            }

            public interface IHidemaruStreamReader : IDisposable
            {
                IEncoding Encoding { get; }
                String Read();
                String FilePath { get; }
                void Close();
            }

            public static IHidemaruStreamReader Open(String filepath, int hm_encode = -1)
            {
                return hmNETDynamicLib.Hidemaru.File.Open(filepath, hm_encode);
            }

            public static IEncoding GetEncoding(String filepath)
            {
                return hmNETDynamicLib.Hidemaru.File.GetEncoding(filepath);
            }
        }

        // アウトプットペイン系
        public static class OutputPane
        {
            public static int Output(String message)
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.Output(message);
            }

            public static int Push()
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.Push();
            }

            public static int Pop()
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.Pop();
            }

            public static int Clear()
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.Clear();
            }

            public static IntPtr WindowHandle
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.OutputPane.WindowHandle;
                }
            }

            public static IntPtr SendMessage(int command_id)
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.SendMessage(command_id);
            }

            public static int SetBaseDir(String dirpath)
            {
                return hmNETDynamicLib.Hidemaru.OutputPane.SetBaseDir(dirpath);
            }
        }

        // ファイルマネージャ系
        public static class ExplorerPane
        {
            public static int SetMode(int mode)
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.SetMode(mode);
            }

            public static int GetMode()
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.GetMode();
            }

            public static int LoadProject(string filepath)
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.LoadProject(filepath);
            }

            public static int SaveProject(string filepath)
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.SaveProject(filepath);
            }

            public static int GetUpdated()
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.GetUpdated();
            }

            public static string GetProject()
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.GetProject();
            }

            public static string GetCurrentDir()
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.GetCurrentDir();
            }

            public static IntPtr WindowHandle
            {
                get
                {
                    return hmNETDynamicLib.Hidemaru.ExplorerPane.WindowHandle;
                }
            }

            public static IntPtr SendMessage(int command_id)
            {
                return hmNETDynamicLib.Hidemaru.ExplorerPane.SendMessage(command_id);
            }

        }
    }
}

