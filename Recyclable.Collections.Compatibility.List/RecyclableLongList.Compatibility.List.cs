// Ignore Spelling: zzz

using Recyclable.Collections.Compatibility.List.Properties;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Recyclable.Collections
{

#pragma warning disable IDE1006 // This intentional so that it shows at the end of IntelliSense list
	public static class zzzRecyclableLongListCompatibilityList
#pragma warning restore IDE1006 // Naming Styles
	{
		public static ReadOnlyCollection<T> AsReadOnly<T>(this RecyclableLongList<T> list) => new(list);

		public static int BinarySearch<T>(this RecyclableLongList<T> list, T item)
		{
			long high = list._longCount - 1;
			if (high < 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			var comparer = Comparer<T>.Default;
			byte blockShift = list._blockSizePow2BitShift;
			int blockSizeMinus1 = list._blockSizeMinus1,
				compareResult;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;

			long low = 0;
			if (high >= 0)
			{
				compareResult = comparer.Compare(memoryBlocksSpan[(int)low][low], item);
				if (compareResult == 0)
				{
					return (int)low;
				}
				else if (compareResult > 0)
				{
					return RecyclableDefaults.ItemNotFoundIndex;
				}

				compareResult = comparer.Compare(memoryBlocksSpan[list._lastBlockWithData][(int)(high & blockSizeMinus1)], item);
				if (compareResult == 0)
				{
					return checked((int)high);
				}
				else if (compareResult < 0)
				{
					return checked((int)~list._longCount);
				}
			}

			while (low <= high)
			{
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
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int BinarySearch<T>(this RecyclableLongList<T> list, T item, IComparer<T>? comparer)
		{
			long high = list._longCount - 1;
			if (high < 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			comparer ??= Comparer<T>.Default;
			byte blockShift = list._blockSizePow2BitShift;
			int blockSizeMinus1 = list._blockSizeMinus1,
				compareResult;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;

			long low = 0;
			if (high >= 0)
			{
				compareResult = comparer.Compare(memoryBlocksSpan[(int)low][low], item);
				if (compareResult == 0)
				{
					return (int)low;
				}
				else if (compareResult > 0)
				{
					return RecyclableDefaults.ItemNotFoundIndex;
				}

				compareResult = comparer.Compare(memoryBlocksSpan[list._lastBlockWithData][(int)(high & blockSizeMinus1)], item);
				if (compareResult == 0)
				{
					return checked((int)high);
				}
				else if (compareResult < 0)
				{
					return checked((int)~list._longCount);
				}
			}

			while (low <= high)
			{
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
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int BinarySearch<T>(this RecyclableLongList<T> list, int index, int count, T item, IComparer<T>? comparer)
		{
			// Kept strictly for compatibility with List's behavior, including error messages.
			if (index < 0 || count < 0)
				throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count), Exceptions.ArgumentOutOfRange_NeedNonNegNum);
			if (list._longCount - index < count)
				throw new ArgumentException(Exceptions.Argument_InvalidOffLen);

			Contract.EndContractBlock();
			
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			long low = index;
			long high = index + count - 1;

			comparer ??= Comparer<T>.Default;
			Span<T[]> memoryBlocksSpan = list._memoryBlocks;
			byte blockShift = list._blockSizePow2BitShift;
			int blockSizeMinus1 = list._blockSizeMinus1;

			int compareResult;
			if (low <= high)
			{
				compareResult = comparer.Compare(memoryBlocksSpan[(int)(low >> blockShift)][(int)(low & blockSizeMinus1)], item);
				if (compareResult == 0)
				{
					return checked((int)low);
				}
				else if (compareResult > 0)
				{
					return checked((int)~low);
				}

				if (low != high)
				{
					compareResult = comparer.Compare(memoryBlocksSpan[(int)(high >> blockShift)][(int)(high & blockSizeMinus1)], item);
					if (compareResult == 0)
					{
						return checked((int)high);
					}
					else if (compareResult < 0)
					{
						return checked((int)~(high + 1));
					}
				}
			}

			while (low <= high)
			{
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
		}

		public static RecyclableLongList<TOutput> ConvertAll<T, TOutput>(this RecyclableLongList<T> list, Converter<T, TOutput> converter)
		{
			if (list._longCount == 0)
			{
				return new();
			}

			int blockIndex = 0,
				blockSize = list._blockSize;
			RecyclableLongList<TOutput> result = new(blockSize, list._longCount);
			ReadOnlySpan<TOutput[]> resultMemoryBlocksSpan = result._memoryBlocks;
			Span<TOutput> resultMemoryBlockSpan = resultMemoryBlocksSpan[0];
			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[0];
			var itemIndex = 0;
			int lastBlockWithData = list._lastBlockWithData;
			while (blockIndex < lastBlockWithData)
			{
				resultMemoryBlockSpan[itemIndex] = converter(sourceMemoryBlockSpan[itemIndex]);
				if (itemIndex + 1 == blockSize)
				{
					itemIndex = 0;
					blockIndex++;
					sourceMemoryBlockSpan = sourceMemoryBlocksSpan[blockIndex];
					resultMemoryBlockSpan = resultMemoryBlocksSpan[blockIndex];

					if (blockIndex == lastBlockWithData)
					{
						break;
					}
				}
				else
				{
					itemIndex++;
				}
			}

			if (blockIndex == lastBlockWithData)
			{
				// We're re-using another variable for better performance
				lastBlockWithData = list._nextItemIndex > 0 ? list._nextItemIndex : blockSize;
				while (itemIndex < lastBlockWithData)
				{
					resultMemoryBlockSpan[itemIndex] = converter(sourceMemoryBlockSpan[itemIndex++]);
				}
			}

			result._longCount = list._longCount;
			result._nextItemBlockIndex = list._nextItemBlockIndex;
			result._nextItemIndex = list._nextItemIndex;
			result._lastBlockWithData = list._lastBlockWithData;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void CopyTo<T>(this RecyclableLongList<T> list, T[] array) => DoCopyTo(list._memoryBlocks, 0, list._blockSize, list._longCount, array, 0);

		private static void DoCopyTo<T>(T[][] sourceMemoryBlocks, long startingIndex, int blockSize, long itemsCount, Array destinationArray, int destinationArrayIndex)
		{
			if (itemsCount <= 0)
			{
				return;
			}

			// TODO: Replace with bit-shifting
			int startingItemIndex = (int)(startingIndex % blockSize);
			int memoryBlockIndex = (int)(startingIndex / blockSize);
			int lastItemIndex;

			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = new(sourceMemoryBlocks);
			// We're using lastItemIndex as temp storage for copied count, so that we can move the destination index. That's to avoid additional var.
			Array.Copy(sourceMemoryBlocks[memoryBlockIndex], startingItemIndex, destinationArray, destinationArrayIndex, lastItemIndex = (int)Math.Min(itemsCount, blockSize - startingItemIndex));
			memoryBlockIndex++;
			destinationArrayIndex += lastItemIndex;
			// TODO: Replace with bit-shifting
			lastItemIndex = (int)((startingIndex + itemsCount) % blockSize);
			int lastBlockIndex = (int)Math.Min((startingIndex + itemsCount) / blockSize, sourceMemoryBlocks.Length - 1);

			while(memoryBlockIndex < lastBlockIndex)
			{
				Array.Copy(sourceMemoryBlocksSpan[memoryBlockIndex], 0, destinationArray, destinationArrayIndex, blockSize);
				destinationArrayIndex += blockSize;
				memoryBlockIndex++;
			}

			if (destinationArrayIndex < destinationArray.Length && memoryBlockIndex == lastBlockIndex)
			{
				Array.Copy(sourceMemoryBlocksSpan[lastBlockIndex], 0, destinationArray, destinationArrayIndex, lastItemIndex > 0 ? lastItemIndex : blockSize);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		//public static void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex, int count) => RecyclableLongList<T>.Helpers.CopyTo(list._memoryBlocks, index, list._blockSize, count, array, arrayIndex);
		public static void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex) => DoCopyTo(list._memoryBlocks, index, list._blockSize, list._longCount - index, array, arrayIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void CopyTo<T>(this RecyclableLongList<T> list, int index, T[] array, int arrayIndex, int count)
		{

			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_Index();
			}

			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_Count();
			}

			if (arrayIndex < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex), "Parameter `arrayIndex` has invalid value. It must be within the no. of elements in the collection");
			}

			if (index + count > list._longCount || arrayIndex + count > list._longCount)
			{
				ThrowHelper.ThrowArgumentException_Count();
			}

			DoCopyTo(list._memoryBlocks, index, list._blockSize, count, array, arrayIndex);

		}		
			
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
