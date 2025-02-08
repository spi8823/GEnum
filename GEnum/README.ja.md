# GEnum - 高性能な列挙型ユーティリティ

## 概要

GEnum は、C# の列挙型 (enum) のパフォーマンスと利便性を向上させる拡張メソッドを提供するライブラリです。本ライブラリは **Source Generator** を活用してコンパイル時に最適化されたコードを生成し、**ビット演算等とほぼ同等の速度で動作する** メソッドを提供します。

## インストール

NuGet からインストールできます:

```sh
dotnet add package GEnum
```

または、Visual Studio の「NuGet パッケージマネージャ」から `GEnum` を検索してインストールしてください。

## 特徴

- **ビット演算と同等の速度**: `Contains` や `Add` などの操作を、高速なビット演算を用いて実現。
- **便利な拡張メソッド**: フラグ操作 (`Add`、`Remove` など)、表示名取得 (`GetDisplayName`)、値チェック (`Is{Xxx}`) など、列挙型をより扱いやすくするメソッドを提供。
- **ゼロアロケーション**: すべての処理をヒープアロケーションなしで実行し、GC の負荷を抑制。
- **Source Generator を活用**: すべてのメソッドをコンパイル時に最適化し、リフレクションを使用せずに高速に動作。

## 使い方

```csharp
using GEnum;

var flag = StatusFlag.A;

// FlagsExtensions
flag.Contains(StatusFlag.A);  // true
flag.Add(StatusFlag.B);       // flag is A | B
flag.Remove(StatusFlag.A);    // flag is B
flag.Clear();                 // flag is None

// MatchingExtensions
flag = StatusFlag.A;
flag.IsXxx();                 // 定義に応じたメソッドが生成される

// DisplayingExtensions
StatusFlag.None.GetDefineName();                     // "None"
StatusFlag.A.GetDefineName();                        // "A"
(StatusFlag.A | StatusFlag.B).GetDefineName();       // "Undefined"

StatusFlag.None.GetDisplayName();                    // "None"
StatusFlag.A.GetDisplayName();                       // "DisplayA"
(StatusFlag.A | StatusFlag.B).GetDisplayName();      // "DisplayA | DisplayB"

[FlagsExtensions]       // Add to use FlagsExtensions
[MatchingExtensions]    // Add to use MatchingExtensions
[DisplayingExtensions]  // Add to use DisplayingExtensions
public enum StatusFlag
{
    None = 0,
    [DisplayName("DisplayA")] A = 1 << 0,
    [DisplayName("DisplayB")] B = 1 << 1,
    C = 1 << 2,
}
```

## 提供されるメソッド

### FlagsExtensions

| メソッド       | 説明               |
| ---------- | ---------------- |
| `Contains` | 指定したフラグを含んでいるか判定 |
| `Add`      | 指定したフラグを追加       |
| `Remove`   | 指定したフラグを削除       |
| `Clear`    | すべてのフラグをクリア      |

### MatchingExtensions

| メソッド      | 説明                             |
| --------- | ------------------------------ |
| `Is{Xxx}` | 列挙型の定義に応じた `Is〇〇` メソッドが自動生成される |

### DisplayingExtensions

| メソッド             | 説明                    |
| ---------------- | --------------------- |
| `GetDefineName`  | 定義名を取得                |
| `GetDisplayName` | `DisplayName` 属性の値を取得 |

## ベンチマーク

各関数を100回実行する関数でベンチマーク

### FlagsExtensions

| Method          | Mean     | Error     | StdDev   | Allocated |
| --------------- | -------- | --------- | -------- | --------- |
| And             | 33.08 ns | 10.255 ns | 0.562 ns | -         |
| HasFlag         | 58.67 ns | 9.717 ns  | 0.533 ns | -         |
| GEnum\_Contains | 58.09 ns | 5.011 ns  | 0.275 ns | -         |
| Or              | 31.02 ns | 7.747 ns  | 0.425 ns | -         |
| GEnum\_Add      | 30.84 ns | 1.866 ns  | 0.102 ns | -         |
| GEnum\_Remove   | 31.42 ns | 5.458 ns  | 0.299 ns | -         |
| GEnum\_Clear    | 30.62 ns | 1.432 ns  | 0.078 ns | -         |

### MatchingExtensions

| Method    | Mean     | Error     | StdDev   | Allocated |
| --------- | -------- | --------- | -------- | --------- |
| Equals    | 35.31 ns | 11.898 ns | 0.652 ns | -         |
| GEnum\_Is | 35.33 ns | 1.713 ns  | 0.094 ns | -         |

### DisplayingExtensions

| Method                | Mean       | Error     | StdDev   | Gen0   | Allocated |
| --------------------- | ---------- | --------- | -------- | ------ | --------- |
| Enum\_ToString        | 1,521.7 ns | 529.45 ns | 29.02 ns | 0.3853 | 2424 B    |
| GEnum\_GetDefineName  | 223.6 ns   | 72.53 ns  | 3.98 ns  | -      | -         |
| GEnum\_GetDisplayName | 250.6 ns   | 93.97 ns  | 5.15 ns  | -      | -         |

