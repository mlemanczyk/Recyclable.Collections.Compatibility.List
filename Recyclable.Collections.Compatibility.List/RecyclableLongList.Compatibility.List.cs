// Ignore Spelling: zzz

using System.Collections.ObjectModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Recyclable.Collections.Pools;

namespace Recyclable.Collections
{

#pragma warning disable IDE1006 // This intentional so that it shows at the end of IntelliSense list
	public static class zzzRecyclableLongListCompatibilityList
#pragma warning restore IDE1006 // Naming Styles
	{
		public static ReadOnlyCollection<T> AsReadOnly<T>(this RecyclableLongList<T> list) => new(list);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int BinarySearch<T>(this RecyclableLongList<T> list, T item)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int blockIndex = 0,
				blockSize = list._blockSize,
				foundItemIndex;
			var comparer = Comparer<T>.Default;
			var lastFullBlockIndex = list._lastBlockWithData;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;
			while (blockIndex < lastFullBlockIndex)
			{
				foundItemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, blockSize).BinarySearch(item, comparer);
				if (foundItemIndex >= 0)
				{
					return (blockIndex << list._blockSizePow2BitShift) + foundItemIndex;
				}
				else if (-foundItemIndex <= blockSize  && foundItemIndex <= RecyclableDefaults.ItemNotFoundIndex)
				{
					return foundItemIndex - (blockIndex << list._blockSizePow2BitShift);
				}

				blockIndex++;
			}

			foundItemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, list._nextItemIndex != 0 ? list._nextItemIndex : blockSize).BinarySearch(item, comparer);
			return foundItemIndex >= 0 ? (blockIndex << list._blockSizePow2BitShift) + foundItemIndex : foundItemIndex - (blockIndex << list._blockSizePow2BitShift);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int BinarySearch<T>(this RecyclableLongList<T> list, T item, IComparer<T>? comparer)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			comparer ??= Comparer<T>.Default;
			int blockIndex = 0,
				blockSize = list._blockSize,
				foundItemIndex;
			var lastFullBlockIndex = list._lastBlockWithData;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;
			while (blockIndex < lastFullBlockIndex)
			{
				foundItemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, blockSize).BinarySearch(item, comparer);
				if (foundItemIndex >= 0)
				{
					return (blockIndex << list._blockSizePow2BitShift) + foundItemIndex;
				}

				blockIndex++;
			}

			foundItemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, list._nextItemIndex != 0 ? list._nextItemIndex : blockSize).BinarySearch(item, comparer);
			return foundItemIndex >= 0 ? (blockIndex << list._blockSizePow2BitShift) + foundItemIndex : RecyclableDefaults.ItemNotFoundIndex;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int BinarySearch<T>(this RecyclableLongList<T> list, int index, int count, T item, IComparer<T>? comparer)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			comparer ??= Comparer<T>.Default;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;
			byte blockShift = list._blockSizePow2BitShift;
			int blockSizeMinus1 = list._blockSizeMinus1;
			long low = index,
				high = index + count - 1;

			int compareResult = comparer.Compare(memoryBlocksSpan[(int)(low >> blockShift)][(int)(low & blockSizeMinus1)], item);
			if (compareResult == 0)
			{
				return checked((int)low);
			}
			else if (compareResult > 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			while (low <= high)
			{
				// i might overflow if lo and hi are both large positive numbers.
				long med = low + ((high - low) >> 1);

				compareResult = comparer.Compare(memoryBlocksSpan[(int)(med >> blockShift)][(int)(med & blockSizeMinus1)], item);
				if (compareResult == 0)
				{
					return checked((int)med);
				}

				if (compareResult < 0)
				{
					low = med + 1;
				}
				else
				{
					high = med - 1;
				}
			}

			return checked((int)~low);

			// ATTEMPT 2
			//int blockSizeMinus1 = list._blockSizeMinus1;
			//long low = index,
			//	high = Math.Min(index + (count != 0 ? count : list._longCount) - 1, list._longCount - 1);
			//byte blockShift = list._blockSizePow2BitShift;

			//comparer ??= Comparer<T>.Default;
			//Span<T[]> memoryBlocksSpan = list._memoryBlocks;

			//while (low <= high)
			//{
			//	long mid = low + ((high - low) / 2);

			//	// Compare the target with the middle element of the array
			//	int comparisonResult = comparer.Compare(item, memoryBlocksSpan[(int)mid >> blockShift][mid & blockSizeMinus1]);
			//	if (comparisonResult == 0)
			//	{
			//		return (int)mid;
			//	}
			//	else if (comparisonResult < 0)
			//	{
			//		low = mid + 1;
			//	}
			//	else
			//	{
			//		high = mid - 1;
			//		low++;
			//	}
			//}

			//return low != 0 ? (int)-low : RecyclableDefaults.ItemNotFoundIndex ;


			// ATTEMPT 3
			//if (list._longCount == 0 || (count == 0 && index == 0))
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//comparer ??= Comparer<T>.Default;
			//byte blockSizePow2BitShift = list._blockSizePow2BitShift;
			//int blockSize = list._blockSize,
			//	startItemIndex = index & list._blockSizeMinus1,
			//	blockIndex = index >> blockSizePow2BitShift,
			//	lastItemIndex = (index + count) & list._blockSizeMinus1,
			//	lastFullBlockIndex = ((index + count) >> blockSizePow2BitShift) - (lastItemIndex != 0 /*|| count == 1*/ ? 1 : 0);

			//Span<T[]> memoryBlocksSpan = list._memoryBlocks;
			//if (comparer.Compare(item, memoryBlocksSpan[blockIndex][startItemIndex]) < 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//int itemIndex =
			//	new Span<T>
			//	(
			//		memoryBlocksSpan[blockIndex], 
			//		startItemIndex,
			//		Math.Max(0, Math.Min(blockSize, (int)Math.Min(count, list._longCount - startItemIndex)))
			//	)
			//	.BinarySearch(item, comparer);

			//if (itemIndex >= 0)
			//{
			//	return (blockIndex << blockSizePow2BitShift) + itemIndex;
			//}
			//else if ((itemIndex < RecyclableDefaults.ItemNotFoundIndex || blockIndex > 0) && -itemIndex <= blockSize)
			//{
			//	return itemIndex - (blockIndex << blockSizePow2BitShift);
			//}
			////else if (blockIndex == 0)
			////{
			////	return itemIndex;
			////}

			//blockIndex++;
			//while (blockIndex <= lastFullBlockIndex)
			//{
			//	itemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, blockSize).BinarySearch(item, comparer);
			//	if (itemIndex >= 0)
			//	{
			//		return (blockIndex << blockSizePow2BitShift) + itemIndex;
			//	}
			//	else if (itemIndex <= RecyclableDefaults.ItemNotFoundIndex)
			//	{
			//		return itemIndex - (blockIndex << blockSizePow2BitShift);
			//	}

			//	blockIndex++;
			//}

			//if (blockIndex <= list._lastBlockWithData)
			//{
			//	itemIndex = new Span<T>(memoryBlocksSpan[blockIndex], 0, lastItemIndex + 1).BinarySearch(item, comparer);
			//	switch (itemIndex)
			//	{
			//		case >= 0: return (blockIndex << blockSizePow2BitShift) + itemIndex;
			//		case < RecyclableDefaults.ItemNotFoundIndex: return itemIndex - (blockIndex << blockSizePow2BitShift);
			//		case RecyclableDefaults.ItemNotFoundIndex: return -(blockIndex << blockSizePow2BitShift) - (blockSize > 1 ? 1 : 0);
			//	}
			//}

			//return checked((int)-list._longCount);
		}

		public static RecyclableList<TOutput> ConvertAll<T, TOutput>(this RecyclableLongList<T> list, Converter<T, TOutput> converter)
		{
			//int itemsCount = list._count;
			//if (itemsCount == 0)
			//{
			//	return new();
			//}

			//RecyclableList<TOutput> result = new(itemsCount);
			//TOutput[] resultSpan = result._memoryBlock;			
			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (int bufferIndex = 0; bufferIndex < itemsCount; bufferIndex++)
			//{
			//	resultSpan[bufferIndex] = converter(sourceSpan[bufferIndex]);
			//}

			//result._count = itemsCount;
			//return result;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void CopyTo<T>(this RecyclableLongList<T> list, T[] array)
			//=> Array.Copy(list._memoryBlock, array, list._count);
			=> throw new NotImplementedException();

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex, int count)
			//=> Array.Copy(list._memoryBlock, index, array, arrayIndex, count);
			=> throw new NotImplementedException();

		public static void ForEach<T>(this RecyclableLongList<T> list, Action<T> action)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	action(sourceSpan[itemIndex]);
			//}

			throw new NotImplementedException();
		}

		public static RecyclableList<T> GetRange<T>(this RecyclableLongList<T> list, int index, int count)
		{
			//int sourceItemsCount = index + count <= list._count ? count : list._count - index;
			//if (sourceItemsCount <= 0)
			//{
			//	return new();
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//RecyclableList<T> result = new(sourceItemsCount);
			//Span<T> targetSpan = result._memoryBlock;
			//int resultItemsCount = 0;
			//sourceItemsCount += index;
			//for (var itemIndex = index; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	targetSpan[resultItemsCount++] = sourceSpan[itemIndex];
			//}

			//result._count = resultItemsCount;
			//return result;

			throw new NotImplementedException();
		}

		public static int RemoveAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//using var allIndexes = zzzRecyclableListCompatibilityListFind.FindAllIndexes(list, match);
			//int foundItemsCount = allIndexes._count;
			//if (foundItemsCount == 0)
			//{
			//	return foundItemsCount;
			//}

			//for (int removed = 0; removed < foundItemsCount; removed++)
			//{
			//	var indexToRemove = allIndexes[removed] - removed;
			//	Array.Copy(list._memoryBlock, indexToRemove + 1, list._memoryBlock, indexToRemove, list._count - indexToRemove - removed - 1);
			//}

			//if (RecyclableList<T>._needsClearing)
			//{
			//	Array.Clear(list._memoryBlock, list._count - foundItemsCount, foundItemsCount);
			//}

			//list._count -= foundItemsCount;
			//list._version++;
			//return foundItemsCount;

			throw new NotImplementedException();
		}

		public static void RemoveRange<T>(this RecyclableLongList<T> list, int index, int count)
		{
			//int oldCount = list._count;
			//count = count - index + 1 <= oldCount ? count : oldCount - index + 1;
			//if (count == 0)
			//{
			//	return;
			//}

			//if (index + count < oldCount)
			//{
			//	Array.Copy(list._memoryBlock, index + count, list._memoryBlock, index, oldCount - count - index);
			//}

			//if (RecyclableList<T>._needsClearing)
			//{
			//	Array.Clear(list._memoryBlock, oldCount - count, count);
			//}

			//list._count -= count;
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Reverse<T>(this RecyclableLongList<T> list)
		{
			//Array.Reverse(list._memoryBlock, 0, list._count);
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Reverse<T>(this RecyclableLongList<T> list, int index, int count)
		{
			//Array.Reverse(list._memoryBlock, index, count);
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Sort<T>(this RecyclableLongList<T> list)
		{
			//Array.Sort(list._memoryBlock, 0, list._count);
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Sort<T>(this RecyclableLongList<T> list, Comparison<T> comparison)
		{
			//Array.Sort(list._memoryBlock, 0, list._count, new ComparisonToComparerAdapter<T>(comparison));
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Sort<T>(this RecyclableLongList<T> list, IComparer<T>? comparer)
		{
			//Array.Sort(list._memoryBlock, 0, list._count, comparer);
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void Sort<T>(this RecyclableLongList<T> list, int index, int count, IComparer<T>? comparer)
		{
			//Array.Sort(list._memoryBlock, index, count, comparer);
			//list._version++;

			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static T[] ToArray<T>(this RecyclableLongList<T> list)
		{
			//T[] result = new T[list._count];
			//new ReadOnlySpan<T>(list._memoryBlock, 0, list._count).CopyTo(result);
			//return result;

			throw new NotImplementedException();
		}

		public static void TrimExcess<T>(this RecyclableLongList<T> list)
		{
			//var requiredCapacity = list._count >= RecyclableDefaults.InitialCapacity ? list._count : RecyclableDefaults.InitialCapacity;

			//T[] oldMemoryBlock = list._memoryBlock;
			//T[] result = requiredCapacity >= RecyclableDefaults.MinPooledArrayLength && BitOperations.IsPow2(requiredCapacity)
			//	? RecyclableArrayPool<T>.RentShared(requiredCapacity)
			//	: new T[requiredCapacity];

			//new ReadOnlySpan<T>(oldMemoryBlock, 0, list._count).CopyTo(result);

			//if (oldMemoryBlock.Length >= RecyclableDefaults.MinPooledArrayLength)
			//{
			//	RecyclableArrayPool<T>.ReturnShared(oldMemoryBlock, RecyclableList<T>._needsClearing);
			//}

			//list._memoryBlock = result;
			//list._capacity = requiredCapacity;

			throw new NotImplementedException();
		}

		public static bool TrueForAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return false;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (!match(sourceSpan[itemIndex]))
			//	{
			//		return false;
			//	}
			//}

			//return true;

			throw new NotImplementedException();
		}
	}
}
