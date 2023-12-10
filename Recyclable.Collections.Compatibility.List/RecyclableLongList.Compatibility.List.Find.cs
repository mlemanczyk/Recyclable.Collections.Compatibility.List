// Ignore Spelling: zzz

namespace Recyclable.Collections
{
#pragma warning disable IDE1006 // This intentional so that it shows at the end of IntelliSense list
	public static class zzzRecyclableLongListCompatibilityListFind
#pragma warning restore IDE1006 // Naming Styles
	{
		public static bool Exists<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return false;
			}

			int blockIndex = 0,
				blockSize = list._blockSize;
			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[0];
			var itemIndex = 0;
			int lastBlockWithData = list._lastBlockWithData;
			while (blockIndex < lastBlockWithData)
			{
				if (match(sourceMemoryBlockSpan[itemIndex]))
				{
					return true;
				}

				if (itemIndex + 1 == blockSize)
				{
					itemIndex = 0;
					blockIndex++;
					sourceMemoryBlockSpan = sourceMemoryBlocksSpan[blockIndex];

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
					if (match(sourceMemoryBlockSpan[itemIndex++]))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static T? Find<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return default;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return sourceSpan[itemIndex];
			//	}
			//}

			//return default;

			throw new NotImplementedException();
		}

		public static RecyclableList<T> FindAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return new();
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//RecyclableList<T> result = new(sourceItemsCount >> 3);
			//Span<T> resultSpan = result._memoryBlock;
			//int capacity = result._capacity,
			//	resultItemsCount = 0;

			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		if (resultItemsCount + 1 > capacity)
			//		{
			//			capacity = checked((int)BitOperations.RoundUpToPowerOf2((uint)resultItemsCount + 1));
			//			_ = RecyclableListHelpers<T>.EnsureCapacity(result, resultItemsCount, capacity);
			//			resultSpan = result._memoryBlock;
			//		}

			//		resultSpan[resultItemsCount++] = sourceSpan[itemIndex];
			//	}
			//}

			//result._capacity = capacity;
			//result._count = resultItemsCount;
			//return result;

			throw new NotImplementedException();
		}
		public static RecyclableList<int> FindAllIndexes<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return new();
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//RecyclableList<int> result = new(sourceItemsCount >> 3);
			//Span<int> resultSpan = result._memoryBlock;
			//int capacity = result._capacity,
			//	resultItemsCount = 0;

			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		if (resultItemsCount + 1 > capacity)
			//		{
			//			capacity = checked((int)BitOperations.RoundUpToPowerOf2((uint)resultItemsCount + 1));
			//			_ = RecyclableListHelpers<int>.EnsureCapacity(result, resultItemsCount, capacity);
			//			resultSpan = result._memoryBlock;
			//		}

			//		resultSpan[resultItemsCount++] = itemIndex;
			//	}
			//}

			//result._capacity = capacity;
			//result._count = resultItemsCount;
			//return result;

			throw new NotImplementedException();
		}

		public static int FindIndex<T>(this RecyclableLongList<T> list, int startIndex, int count, Predicate<T> match)
		{
			//int sourceItemsCount = Math.Min(startIndex + count, list._count);
			//if (sourceItemsCount == 0 || startIndex >= sourceItemsCount)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = startIndex; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}

		public static int FindIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0 || startIndex >= sourceItemsCount)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//sourceItemsCount = Math.Min(sourceItemsCount, startIndex + sourceItemsCount);
			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = startIndex; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}
		
		public static int FindIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//int sourceItemsCount = list._count;
			//if (sourceItemsCount == 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = 0; itemIndex < sourceItemsCount; itemIndex++)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}

		public static T? FindLast<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//if (list._count == 0)
			//{
			//	return default;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = list._count - 1; itemIndex >= 0; itemIndex--)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return sourceSpan[itemIndex];
			//	}
			//}

			//return default;

			throw new NotImplementedException();
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, int count, Predicate<T> match)
		{
			//if (count == 0)
			//{
			//	if (startIndex != -1)
			//	{
			//		ThrowHelper.ThrowArgumentOutOfRangeException_Index();
			//	}
			//}
			//else  if (count < 0 || startIndex - count + 1 < 0)
			//{
			//	ThrowHelper.ThrowArgumentOutOfRangeException_Count();
			//}

			//if (startIndex < 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//int sourceItemsCount = count; // startIndex - count + 1 >= 0 ? count : startIndex + 1;
			//if (sourceItemsCount == 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//int lastItemIndex = startIndex - sourceItemsCount;
			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = startIndex; itemIndex > lastItemIndex; itemIndex--)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
		{
			//if (list._count == 0 || startIndex < 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = startIndex; itemIndex >= 0; itemIndex--)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			//if (list._count == 0)
			//{
			//	return RecyclableDefaults.ItemNotFoundIndex;
			//}

			//ReadOnlySpan<T> sourceSpan = list._memoryBlock;
			//for (var itemIndex = list._count - 1; itemIndex >= 0; itemIndex--)
			//{
			//	if (match(sourceSpan[itemIndex]))
			//	{
			//		return itemIndex;
			//	}
			//}

			//return RecyclableDefaults.ItemNotFoundIndex;

			throw new NotImplementedException();
		}
	}
}
