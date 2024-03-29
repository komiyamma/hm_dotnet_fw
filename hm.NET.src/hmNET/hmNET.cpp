/*
 * Copyright (c) 2017 Akitsugu Komiyama
 * under the MIT License
 */

#include <windows.h>
#include <string>
#include "string_converter.h"
#include "hidemaruexe_export.h"
#include "hmNET.h"
#include "hmNETStatlcLib.h"


using namespace std;
using namespace System;
using namespace System::Reflection;
using namespace System::Collections::Generic;

// 上の手動のBindDllHandleを自動で行う。秀丸8.66以上
// １回だけ実行すれば良いわけではない。dllが読み込まれている間にもdll値が変わってしまうかもしれないため。(将来の実装では)
// よって、CallMethodの時を契機に更新する。
static bool BindDllHandle() {
	// 秀丸8.66以上
	if (Hidemaru_GetDllFuncCalledType) {
		int dll = Hidemaru_GetDllFuncCalledType(-1); // 自分のdllの呼ばれ方をチェック

		// Hidemaru_GetDllFuncCalledTypeについては、別の存在でありながら成り立たせるために、呼ばれ方情報だけは結びつけています。
		// 0x80000001みたいな、32bitの最上位ビットが立ったIDにしていて、マクロ側loaddllのIDと被らないようにしています。10進だとマイナス値でわかりにくいですね。
		if ((dll & 0x80000000) != 0) {
			wstring errmsg = L"「jsmode」の「hidemaru.loadDll(...)」からの呼び出しを検知しました。\n「jsmode」の「loadDll経由の呼び出し」には対応していません。\n"
				L"「jsmode」から呼び出すには、「hidemaruCompat.loaddll(...)」を利用してください。\n"
				L"https://秀丸マクロ.net/?page=nobu_tool_hm_jsmode_hidemarucompat\n";
			MessageBox(NULL, errmsg.c_str(), L"「hm.NET.dll」の「jsmode」からの呼び出し", NULL);
			OutputDebugStringW(errmsg.c_str());
		}

		INETStaticLib::BindDllHandle((IntPtr)dll);
		return true;
	}
	return false;
}


// 秀丸の変数が文字列か数値かの判定用
MACRO_DLL intHM_t SetTmpVar(const void* dynamic_value) {
	int param_type = Hidemaru_GetDllFuncCalledType(1); // 1番目の引数の型。
	if (param_type == DLLFUNCPARAM_WCHAR_PTR) {
		return (intHM_t)INETStaticLib::SetTmpVar(gcnew String((wchar_t *)dynamic_value));
	}
	else {
		return (intHM_t)INETStaticLib::SetTmpVar((intHM_t)dynamic_value);
	}
}

MACRO_DLL intHM_t PopNumVar() {
	intHM_t num = (intHM_t)INETStaticLib::PopTmpVar();
	return num;
}

static wstring strvarsopop;
MACRO_DLL const TCHAR * PopStrVar() {
	auto var = (String ^)INETStaticLib::PopTmpVar();
	strvarsopop = String_to_wstring(var);
	return strvarsopop.data();
}


intHM_t ConvertObjectToIntPtr(Object^ o) {
	// Boolean型であれば、True:1 Flase:0にマッピングする
	if (o->GetType()->Name == "Boolean")
	{
		if ((Boolean)o == true)
		{
			return (intHM_t)1;
		}
		else
		{
			return (intHM_t)0;
		}
	}

	// 32bit
	if (IntPtr::Size == 4)
	{
		// まずは整数でトライ
		Int32 itmp = 0;
		bool success = Int32::TryParse(o->ToString(), itmp);

		if (success == true)
		{
			return (intHM_t)itmp;
		}

		else
		{
			// 次に浮動少数でトライ
			Double dtmp = 0;
			success = Double::TryParse(o->ToString(), dtmp);
			if (success)
			{
				return (intHM_t)(Int32)Math::Floor(dtmp);
			}
			else
			{
				return (intHM_t)0;
			}
		}
	}

	// 64bit
	else
	{
		// まずは整数でトライ
		Int64 itmp = 0;
		bool success = Int64::TryParse(o->ToString(), itmp);

		if (success == true)
		{
			return (intHM_t)itmp;
		}

		else
		{
			// 次に浮動少数でトライ
			Double dtmp = 0;
			success = Double::TryParse(o->ToString(), dtmp);
			if (success)
			{
				return (intHM_t)(Int64)Math::Floor(dtmp);
			}
			else
			{
				return (intHM_t)0;
			}
		}
	}
	return (intHM_t)0;
}

static wstring strcallmethod;
MACRO_DLL intHM_t CallMethod(const wchar_t* assm_path, const wchar_t* class_name, wchar_t* method_name, void *arg0, void *arg1, void *arg2, void *arg3) {

	// 自分自身のCallMethodを別から再度呼ぶと値が崩れるので、いち早く取得
	int rty = Hidemaru_GetDllFuncCalledType(0);
	int pt0 = Hidemaru_GetDllFuncCalledType(4); // 引数４番目
	int pt1 = Hidemaru_GetDllFuncCalledType(5); // 引数５番目
	int pt2 = Hidemaru_GetDllFuncCalledType(6); // 引数６番目
	int pt3 = Hidemaru_GetDllFuncCalledType(7); // 引数７番目

	try {

		strcallmethod.clear();

		BindDllHandle();

		List<Object^>^ args = gcnew List<Object^>();
		int pt = 0;

		for (int i = 4; true; i++) {

			void *arg = nullptr;
			int pt = 0;
			switch (i) {
			case 4: {
				arg = arg0;
				pt = pt0;
				break;
			}
			case 5: {
				arg = arg1;
				pt = pt1;
				break;
			}
			case 6: {
				arg = arg2;
				pt = pt2;
				break;
			}
			case 7: {
				arg = arg3;
				pt = pt3;
				break;
			}
			}

			// arg0のチェック
			if (pt == DLLFUNCPARAM_NOPARAM) {
				break;
			}
			else if (pt == DLLFUNCPARAM_INT) {
				args->Add((IntPtr)arg);
			}
			else if (pt == DLLFUNCPARAM_WCHAR_PTR) {
				args->Add(gcnew String((wchar_t *)arg));
			}
		}

		INETStaticLib::CallMethod(L"init");

		Object^ o = INETStaticLib::SubCallMethod(wstring_to_String(assm_path), wstring_to_String(class_name), wstring_to_String(method_name), args, "normal_func_mode");

		GC::Collect();

		if (rty == DLLFUNCRETURN_INT) {
			// System::Diagnostics::Trace::WriteLine("数値リターン");
			if (o == nullptr) {
				return (intHM_t)0;
			}
			try {
				intHM_t rtn = ConvertObjectToIntPtr(o);
				return rtn;
			}
			catch (Exception^ e) {
				System::Diagnostics::Trace::WriteLine(e->GetType());
				System::Diagnostics::Trace::WriteLine(e->Message);
			}
			return (intHM_t)0;

		}
		else if (rty == DLLFUNCRETURN_WCHAR_PTR) {
			// System::Diagnostics::Trace::WriteLine("文字列リターン");
			strcallmethod = String_to_wstring(o->ToString());
			return (intHM_t)strcallmethod.data();

		}

		return false;

	}
	catch (Exception^ ex) {
		INETStaticLib::TraceExceptionInfo(ex);
		if (rty == DLLFUNCRETURN_INT) {
			return (intHM_t)0;
		}
		else if (rty == DLLFUNCRETURN_WCHAR_PTR) {
			// System::Diagnostics::Trace::WriteLine("文字列リターン");
			strcallmethod = L"";
			return (intHM_t)strcallmethod.data();

		}

		return false;
	}
}

struct DETATH_FUNC {
	wstring assm_path;
	wstring class_name;
	wstring method_name;
};

static vector<DETATH_FUNC> detach_func_list;
MACRO_DLL intHM_t SetDetachMethod(const wchar_t* assm_path, const wchar_t* class_name, wchar_t* method_name) {
	bool is_exist = false;
	for each(auto v in detach_func_list) {
		if (v.assm_path == assm_path && v.class_name == class_name && v.method_name == method_name) {
			is_exist = true;
		}
	}
	if (!is_exist) {
		DETATH_FUNC f = { assm_path, class_name, method_name };
		detach_func_list.push_back(f);
	}
	return true;
}

MACRO_DLL intHM_t DetachScope(intHM_t n) {
	intHM_t ret = 0;

	try {
		strcallmethod.clear();
		BindDllHandle();

		List<Object^>^ args = gcnew List<Object^>();
		args->Add((IntPtr)n); // 終了時のパラメータを付け足し
		for each(auto v in detach_func_list) {
			INETStaticLib::SubCallMethod(wstring_to_String(v.assm_path), wstring_to_String(v.class_name), wstring_to_String(v.method_name), args, "detach_func_mode");
		}

		ret = (intHM_t)INETStaticLib::DetachScope(System::IntPtr(n));

		GC::Collect();
	}

	catch (Exception^ ex) {
		INETStaticLib::TraceExceptionInfo(ex);
	}

	return ret;
}

MACRO_DLL intHM_t DllDetachFunc_After_Hm866(intHM_t n) {

	intHM_t ret = DetachScope(n);
	// v8.77未満だと、nは常に0
	if (n == 0) {
		// INETStaticLib::OutputDebugStream(L"v8.66未満\n");
	}
	else if (n == 1) {
		MessageBox(NULL, L"hm.NET.dllをマクロ制御下で解放をしてはいけません。\n"
			L"(freedllによる解放エラー)", L"エラー", MB_ICONERROR);
		// freedll
	}
	else if (n == 2) {
		MessageBox(NULL, L"hm.NET.dllをマクロ制御下で解放をしてはいけません。\n"
			L"「loaddll文」ではなく、「loaddll関数」を利用してください。\n"
			L"(新たなloaddll文による、hm.NET.dllの暗黙的解放エラー)", L"エラー", MB_ICONERROR);
		// loaddll文による入れ替え
	}
	else if (n == 3) {
		//  INETStaticLib::OutputDebugStream(L"プロセス終了時\n");
		// プロセス終了時
	}
	else if (n == 4) {
		//  INETStaticLib::OutputDebugStream(L"keepdll解放指定登録とエラーが発生を理由としたマクロ終了に伴うdllが解放時\n");
		// keepdll #dll, 0; のエラー発生時解放が指定してあり、エラーが発生した理由によりマクロが終了し、dllが解放されようとしている時
	}
	else if (n == 5) {
		//  INETStaticLib::OutputDebugStream(L"keepdll #dll, 3を指定している時、ファイルを閉じて無題になるなど「プロセスが残った」状態でファイルが切り替わると呼び出される\n");
		// keepdll #dll, 3; が指定してあること。
	}
	else {
		// 未知の数
	}

	return ret;
}
