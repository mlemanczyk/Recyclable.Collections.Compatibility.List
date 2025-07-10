# Recyclable.Collections.Compatibility.List
`Recyclable.Collections.Compatibility.List` is an open source add-on for [Recyclable.Collections](https://github.com/mlemanczyk/Recyclable.Collections). It provides compatibility APIs that mirror `List<T>` and larger list implementations while reusing memory through recyclable pools.

## Included compatibility packs
- `RecyclableList<T>`

## Milestones
1. 👉 Implement compatibility pack
        1. ✅ `RecyclableList<T>`
        1. 👉 `RecyclableLongList<T>`
        1. 🅿️ `RecyclableQueue<T>`
        1. 🅿️ `RecyclableSortedList<T>`
        1. 🅿️ `RecyclableStack<T>`
        1. 🅿️ `RecyclableUnorderedList<T>`
1. 🅿️ Review performance
1. 🅿️ Optimize performance

## Characteristics of the classes
All classes use [Recyclable.Collections](https://github.com/mlemanczyk/Recyclable.Collections) internally to minimize memory allocations.

### Current collections
- `RecyclableList<T>` – drop-in replacement for `List<T>`.
- `RecyclableLongList<T>` – supports capacities greater than `int.MaxValue`; currently partially implemented.

### Planned collections
The following types are planned but not yet implemented. Method signatures are subject to change and will mirror their .NET equivalents.
- `RecyclableQueue<T>` – equivalent to `Queue<T>`.
- `RecyclableSortedList<T>` – equivalent to `SortedList<TKey,TValue>`.
- `RecyclableStack<T>` – equivalent to `Stack<T>`.
- `RecyclableUnorderedList<T>` – unordered list similar to `HashSet<T>`.

### Standard .NET collections not provided here
This repository does not currently include recyclable versions of:
- `LinkedList<T>`
- `Dictionary<TKey,TValue>`
- `HashSet<T>`
- `ObservableCollection<T>`

## Extension classes and methods
The compatibility API is implemented through a set of extension classes. The lists below show the available and planned methods.

### `zzzRecyclableListCompatibilityList`
File: `RecyclableList.Compatibility.List.cs`
```csharp
ReadOnlyCollection<T> AsReadOnly<T>(this RecyclableList<T> list)
int BinarySearch<T>(this RecyclableList<T> list, T item)
int BinarySearch<T>(this RecyclableList<T> list, T item, IComparer<T>? comparer)
int BinarySearch<T>(this RecyclableList<T> list, int index, int count, T item, IComparer<T>? comparer)
RecyclableList<TOutput> ConvertAll<T, TOutput>(this RecyclableList<T> list, Converter<T, TOutput> converter)
void CopyTo<T>(this RecyclableList<T> list, T[] array)
void CopyTo<T>(this RecyclableList<T> list, int index, T[] array, int arrayIndex, int count)
void ForEach<T>(this RecyclableList<T> list, Action<T> action)
RecyclableList<T> GetRange<T>(this RecyclableList<T> list, int index, int count)
int RemoveAll<T>(this RecyclableList<T> list, Predicate<T> match)
void RemoveRange<T>(this RecyclableList<T> list, int index, int count)
void Reverse<T>(this RecyclableList<T> list)
void Reverse<T>(this RecyclableList<T> list, int index, int count)
void Sort<T>(this RecyclableList<T> list)
void Sort<T>(this RecyclableList<T> list, Comparison<T> comparison)
void Sort<T>(this RecyclableList<T> list, IComparer<T>? comparer)
void Sort<T>(this RecyclableList<T> list, int index, int count, IComparer<T>? comparer)
T[] ToArray<T>(this RecyclableList<T> list)
void TrimExcess<T>(this RecyclableList<T> list)
bool TrueForAll<T>(this RecyclableList<T> list, Predicate<T> match)
```

### `zzzRecyclableListCompatibilityListInsertRange`
File: `RecyclableList.Compatibility.List.InsertRange.cs`
```csharp
void InsertRange<T>(this RecyclableList<T> list, int index, in Array items)
void InsertRange<T>(this RecyclableList<T> list, int index, in T[] items)
void InsertRange<T>(this RecyclableList<T> list, int index, ReadOnlySpan<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, Span<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, List<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, ICollection items)
void InsertRange<T>(this RecyclableList<T> list, int index, ICollection<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, RecyclableList<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, RecyclableLongList<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, IEnumerable items, int growByCount = RecyclableDefaults.MinPooledArrayLength)
void InsertRange<T>(this RecyclableList<T> list, int index, IReadOnlyList<T> items)
void InsertRange<T>(this RecyclableList<T> list, int index, IEnumerable<T> items, int growByCount = RecyclableDefaults.MinPooledArrayLength)
```

### `zzzRecyclableListCompatibilityListIndexOf`
File: `RecyclableList.Compatibility.List.IndexOf.cs`
```csharp
int LastIndexOf<T>(this RecyclableList<T> list, T item)
int LastIndexOf<T>(this RecyclableList<T> list, T item, int index)
int LastIndexOf<T>(this RecyclableList<T> list, T item, int index, int count)
```

### `zzzRecyclableListCompatibilityListFind`
File: `RecyclableList.Compatibility.List.Find.cs`
```csharp
bool Exists<T>(this RecyclableList<T> list, Predicate<T> match)
T? Find<T>(this RecyclableList<T> list, Predicate<T> match)
RecyclableList<T> FindAll<T>(this RecyclableList<T> list, Predicate<T> match)
RecyclableList<int> FindAllIndexes<T>(this RecyclableList<T> list, Predicate<T> match)
int FindIndex<T>(this RecyclableList<T> list, int startIndex, int count, Predicate<T> match)
int FindIndex<T>(this RecyclableList<T> list, int startIndex, Predicate<T> match)
int FindIndex<T>(this RecyclableList<T> list, Predicate<T> match)
T? FindLast<T>(this RecyclableList<T> list, Predicate<T> match)
int FindLastIndex<T>(this RecyclableList<T> list, int startIndex, int count, Predicate<T> match)
int FindLastIndex<T>(this RecyclableList<T> list, int startIndex, Predicate<T> match)
int FindLastIndex<T>(this RecyclableList<T> list, Predicate<T> match)
```

### `zzzRecyclableLongListCompatibilityList`
File: `RecyclableLongList.Compatibility.List.cs`
```csharp
ReadOnlyCollection<T> AsReadOnly<T>(this RecyclableLongList<T> list)
int BinarySearch<T>(this RecyclableLongList<T> list, T item)
int BinarySearch<T>(this RecyclableLongList<T> list, T item, IComparer<T>? comparer)
int BinarySearch<T>(this RecyclableLongList<T> list, int index, int count, T item, IComparer<T>? comparer)
RecyclableLongList<TOutput> ConvertAll<T, TOutput>(this RecyclableLongList<T> list, Converter<T, TOutput> converter)
void CopyTo<T>(this RecyclableLongList<T> list, T[] array)
void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex)
void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex, int count)
void ForEach<T>(this RecyclableLongList<T> list, Action<T> action)
RecyclableLongList<T> GetRange<T>(this RecyclableLongList<T> list, int startIndex, int count)
```
Internal methods still to be implemented:
```csharp
int RemoveAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
void RemoveRange<T>(this RecyclableLongList<T> list, int index, int count)
void Reverse<T>(this RecyclableLongList<T> list)
void Reverse<T>(this RecyclableLongList<T> list, int index, int count)
void Sort<T>(this RecyclableLongList<T> list)
void Sort<T>(this RecyclableLongList<T> list, Comparison<T> comparison)
void Sort<T>(this RecyclableLongList<T> list, IComparer<T>? comparer)
void Sort<T>(this RecyclableLongList<T> list, int index, int count, IComparer<T>? comparer)
T[] ToArray<T>(this RecyclableLongList<T> list)
void TrimExcess<T>(this RecyclableLongList<T> list)
bool TrueForAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
```

### `zzzRecyclableLongListCompatibilityListFind`
File: `RecyclableLongList.Compatibility.List.Find.cs`
```csharp
bool Exists<T>(this RecyclableLongList<T> list, Predicate<T> match)
T? Find<T>(this RecyclableLongList<T> list, Predicate<T> match)
RecyclableLongList<T> FindAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
RecyclableLongList<long> FindAllIndexes<T>(this RecyclableLongList<T> list, Predicate<T> match)
int FindIndex<T>(this RecyclableLongList<T> list, int startIndex, int count, Predicate<T> match)
int FindIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
int FindIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
T? FindLast<T>(this RecyclableLongList<T> list, Predicate<T> match)
int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, int count, Predicate<T> match)
int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
int FindLastIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
```

### `zzzRecyclableLongListCompatibilityListIndexOf`
File: `RecyclableLongList.Compatibility.List.IndexOf.cs`
Internal methods planned:
```csharp
int LastIndexOf<T>(this RecyclableLongList<T> list, T item)
int LastIndexOf<T>(this RecyclableLongList<T> list, T item, int index)
int LastIndexOf<T>(this RecyclableLongList<T> list, T item, int index, int count)
```

### `zzzRecyclableLongListCompatibilityListInsertRange`
File: `RecyclableLongList.Compatibility.List.InsertRange.cs`
All methods are internal placeholders and currently throw `NotImplementedException`:
```csharp
void InsertRange<T>(this RecyclableLongList<T> list, int index, in Array items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, in T[] items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, ReadOnlySpan<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, Span<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, List<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, ICollection items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, ICollection<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, RecyclableList<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, RecyclableLongList<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, IEnumerable items, int growByCount = RecyclableDefaults.MinPooledArrayLength)
void InsertRange<T>(this RecyclableLongList<T> list, int index, IReadOnlyList<T> items)
void InsertRange<T>(this RecyclableLongList<T> list, int index, IEnumerable<T> items, int growByCount = RecyclableDefaults.MinPooledArrayLength)
```

## Testing
The unit tests are written with **xUnit** and **FluentAssertions**. Projects and tests target **.NET 8** by default and also support **.NET 9**, **.NET 7**, and **.NET 6**.
