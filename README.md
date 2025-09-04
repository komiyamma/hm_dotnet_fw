# hm.NET

![latest release](https://img.shields.io/github/v/release/komiyamma/hm_dotnet_fw?label=hm.NET&color=6479ff)
[![MIT](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](LICENSE)
![Hidemaru 8.73 x86](https://img.shields.io/badge/Hidemaru-v8.73-6479ff.svg)
![.NET Framework 4.5 － 4.8](https://img.shields.io/badge/.NET_Framework-v4.5_－_v4.8-6479ff.svg)

## hm.NETとは？

`hm.NET` は、高機能テキストエディタ「秀丸エディタ」のマクロ機能と、C#などの.NET Frameworkをシームレスに連携させるためのライブラリです。

秀丸マクロの`loaddll`命令を用いて.NETで作成したライブラリ（DLL）を呼び出し、C#の持つ強力な機能や豊富なライブラリをマクロから手軽に利用できるようにします。また、C#側から秀丸エディタ本体を操作するためのAPIも提供します。

より詳しい情報は、作者のウェブサイトを参照してください。  
[https://秀丸マクロ.net/?page=nobu_tool_hm_dotnet](https://秀丸マクロ.net/?page=nobu_tool_hm_dotnet)

## 主な機能

*   秀丸マクロからC#で書かれたメソッドを呼び出し
*   C#から秀丸エディタの各種機能（テキストの取得・設定、コマンド実行など）を操作
*   32bit版秀丸エディタ(x86)と64bit版秀丸エディタ(x64)の両方に対応
*   NuGetパッケージによる簡単な導入

## プロジェクト構成

このソリューションは、主に以下のプロジェクトから構成されています。

*   `hmNET` (C++/CLI)
    *   秀丸エディタの`loaddll`から直接呼び出されるネイティブDLLのプロジェクトです。C#の実行環境であるCLR（共通言語ランタイム）をホストし、`hmNETStaticLib`を呼び出す役割を担います。
*   `hmNETStaticLib` (C#)
    *   C#開発者が利用するメインのライブラリです。秀丸エディタを操作するためのAPI (`hm.NET.Hidemaru`など) を提供します。

## ビルド方法

1.  hmNETStaticLib をコンパイル
2.  その後hmNETをコンパイル
