# hm.NET

![hm.NET v2.0.8](https://img.shields.io/badge/hm.NET-v2.0.8-6479ff.svg)
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

## 使い方

### 1. C#でライブラリを作成する

1.  Visual Studioで「クラスライブラリ (.NET Framework)」プロジェクトを新規作成します。ターゲットフレームワークは **.NET Framework 4.5** 以上を選択してください。
2.  NuGetパッケージマネージャから、秀丸エディタのビット数に合わせて `hmNET-x86` または `hmNET-x64` をインストールします。
3.  以下のように、`public static`なクラスとメソッドを定義します。

```csharp
using System;

public static class MyHidemaruLib
{
    // 秀丸マクロから呼び出すメソッド
    public static void HelloWorld()
    {
        // C#側から秀丸エディタに文字列を挿入する
        hm.NET.Hidemaru.Edit.File.InsertText("Hello from C# world!");
    }
}
```

### 2. 秀丸マクロから呼び出す

作成したC#ライブラリをビルドし、生成されたDLL（例： `MyHidemaruLib.dll`）を秀丸マクロから`loaddll`で呼び出します。

```mac
// C#で作成したDLLのパス
$dll_path = "C:\\path\\to\\your\\MyHidemaruLib.dll";

// loaddllでC#ライブラリを読み込む
loaddll $dll_path;

// loaddllしたライブラリの関数を呼び出す
// callmethod("クラス名", "メソッド名");
callmethod "MyHidemaruLib", "HelloWorld";

// 使い終わったら解放
freedll;
```

## プロジェクト構成

このソリューションは、主に以下のプロジェクトから構成されています。

*   `hmNET` (C++/CLI)
    *   秀丸エディタの`loaddll`から直接呼び出されるネイティブDLLのプロジェクトです。C#の実行環境であるCLR（共通言語ランタイム）をホストし、`hmNETStaticLib`を呼び出す役割を担います。
*   `hmNETStaticLib` (C#)
    *   C#開発者が利用するメインのライブラリです。秀丸エディタを操作するためのAPI (`hm.NET.Hidemaru`など) を提供します。
*   `hmHandleLegacy` (C++)
    *   古い秀丸エディタとの互換性を保つための補助的なモジュールです。

## ビルド方法

1.  `hm.NET.src/hm.NET.sln` を Visual Studio 2017 で開きます。
2.  ソリューションをビルドします。

必要なライブラリや依存関係はNuGetによって管理されています。