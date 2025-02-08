# GEnum - High-Performance Enum Utility

## Overview

GEnum is a library that provides extension methods to enhance the performance and usability of C# enums. Utilizing **Source Generators**, this library generates optimized code at compile time, offering methods that operate at speeds **comparable to bitwise operations**.

## Installation

You can install it via NuGet:

```sh
dotnet add package GEnum
```

Alternatively, search for `GEnum` in Visual Studio's NuGet Package Manager and install it.

## Features

- **Bitwise Operation-Level Speed**: Methods like `Contains` and `Add` utilize efficient bitwise operations.
- **Convenient Extension Methods**: Provides flag operations (`Add`, `Remove`, etc.), name retrieval (`GetDisplayName`), value checking (`Is{Xxx}`), and more to improve enum usability.
- **Zero Allocation**: All operations are performed without heap allocation, reducing GC overhead.
- **Source Generator-Based**: Generates all methods at compile time for high-speed execution without reflection.

## Usage

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
flag.IsXxx();                 // Methods are generated based on the enum definition

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

## Provided Methods

### FlagsExtensions

| Method     | Description                         |
|------------|-------------------------------------|
| `Contains` | Checks if the flag contains a value |
| `Add`      | Adds a flag                         |
| `Remove`   | Removes a flag                      |
| `Clear`    | Clears all flags                    |

### MatchingExtensions

| Method    | Description                                     |
|-----------|-------------------------------------------------|
| `Is{Xxx}` | Auto-generated `IsXxx` methods based on enum definition |

### DisplayingExtensions

| Method            | Description                              |
|-------------------|------------------------------------------|
| `GetDefineName`   | Retrieves the defined name               |
| `GetDisplayName`  | Retrieves the value of the `DisplayName` attribute |

## Benchmarks

Each function was benchmarked by executing it 100 times.

### FlagsExtensions

| Method         | Mean     | Error     | StdDev   | Allocated |
|----------------|---------:|----------:|---------:|----------:|
| And            | 33.08 ns | 10.255 ns | 0.562 ns |         - |
| HasFlag        | 58.67 ns |  9.717 ns | 0.533 ns |         - |
| GEnum_Contains | 58.09 ns |  5.011 ns | 0.275 ns |         - |
| Or             | 31.02 ns |  7.747 ns | 0.425 ns |         - |
| GEnum_Add      | 30.84 ns |  1.866 ns | 0.102 ns |         - |
| GEnum_Remove   | 31.42 ns |  5.458 ns | 0.299 ns |         - |
| GEnum_Clear    | 30.62 ns |  1.432 ns | 0.078 ns |         - |

### MatchingExtensions

| Method   | Mean     | Error     | StdDev   | Allocated |
|----------|---------:|----------:|---------:|----------:|
| Equals   | 35.31 ns | 11.898 ns | 0.652 ns |         - |
| GEnum_Is | 35.33 ns |  1.713 ns | 0.094 ns |         - |

### DisplayingExtensions

| Method               | Mean       | Error     | StdDev   | Gen0   | Allocated |
|----------------------|-----------:|----------:|---------:|-------:|----------:|
| Enum_ToString        | 1,521.7 ns | 529.45 ns | 29.02 ns | 0.3853 |    2424 B |
| GEnum_GetDefineName  |   223.6 ns |  72.53 ns |  3.98 ns |      - |         - |
| GEnum_GetDisplayName |   250.6 ns |  93.97 ns |  5.15 ns |      - |         - |

