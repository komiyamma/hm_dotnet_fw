/*
 * Copyright (c) 2017 Akitsugu Komiyama
 * under the MIT License
 */

#include <windows.h>
#include "hidemaruexe_export.h"


#if DOTNET_EXPORTS
using namespace System;
using namespace System::Collections::Generic;

System::Reflection::Assembly^ CurrentDomain_AssemblyResolve(Object^ sender, ResolveEventArgs^ args) {

	String^ nameall = args->Name;
	if (nameall->Contains(L"hm.NET") && nameall->Contains(L".Reference")) {
		HMODULE hHideExe = GetModuleHandle(NULL);
		wchar_t szHidemaruFullPath[512] = L"";
		GetModuleFileName(hHideExe, szHidemaruFullPath, _countof(szHidemaruFullPath));
		String^ hmexepath = gcnew String(szHidemaruFullPath);
		String^ dir = System::IO::Path::GetDirectoryName(hmexepath);
		
		int findcolon_ix = nameall->IndexOf(",");
		String^name = nameall->Remove(findcolon_ix);
		String^ fullpath = dir + "\\" + name + ".dll";

		if (System::IO::File::Exists(fullpath)) {
			return System::Reflection::Assembly::LoadFile(fullpath);
		}
	}

	System::Diagnostics::Trace::WriteLine(args->Name + "‚ª—v‹‚³‚ê‚Ü‚µ‚½‚ªA”­Œ©‚Å‚«‚Ü‚¹‚ñ‚Å‚µ‚½B");

	return nullptr;
}
#endif

PFNGetDllFuncCalledType Hidemaru_GetDllFuncCalledType = NULL;

struct CHidemaruExeExporter {
	CHidemaruExeExporter() {
		// GŠÛ 8.66ˆÈã
		HMODULE hHideExe = GetModuleHandle(NULL);
		if (hHideExe) {
			Hidemaru_GetDllFuncCalledType = (PFNGetDllFuncCalledType)GetProcAddress(hHideExe, "Hidemaru_GetDllFuncCalledType");
		}

#if DOTNET_EXPORTS
		System::AppDomain::CurrentDomain->AssemblyResolve += gcnew System::ResolveEventHandler(&CurrentDomain_AssemblyResolve);
#endif
	}
};





CHidemaruExeExporter init;