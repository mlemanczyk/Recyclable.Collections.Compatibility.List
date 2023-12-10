// Ignore Spelling: zzz

using System.Numerics;

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

		public static RecyclableLongList<T> FindAll<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return new();
			}

			int sourceBlockIndex = 0,
				blockSize = list._blockSize,
				targetBlockIndex = 0;
			var targetCapacity = Math.Max(list._longCount >> 3, RecyclableDefaults.InitialCapacity);
			if (!BitOperations.IsPow2(targetCapacity))
			{
				targetCapacity = checked((long)BitOperations.RoundUpToPowerOf2((ulong)targetCapacity));
			}

			RecyclableLongList<T> result = new(blockSize, targetCapacity);
			ReadOnlySpan<T[]> resultMemoryBlocksSpan = result._memoryBlocks;
			Span<T> resultMemoryBlockSpan = resultMemoryBlocksSpan[0];
			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[0];
			var sourceItemIndex = 0;
			int lastBlockWithData = list._lastBlockWithData;
			var targetItemIndex = 0;
			var fullBlocksItemsCount = 0L;
			while (sourceBlockIndex < lastBlockWithData)
			{
				var item = sourceMemoryBlockSpan[sourceItemIndex];
				if (match(item))
				{
					if (fullBlocksItemsCount + targetItemIndex >= targetCapacity)
					{
						targetCapacity = RecyclableLongList<T>.Helpers.Resize(result, blockSize, result._blockSizePow2BitShift, targetCapacity << 1);
						resultMemoryBlocksSpan = new(result._memoryBlocks);
					}
					
					if (targetItemIndex == blockSize)
					{
						targetItemIndex = 0;
						resultMemoryBlockSpan = resultMemoryBlocksSpan[++targetBlockIndex];
						fullBlocksItemsCount += blockSize;
					}

					resultMemoryBlockSpan[targetItemIndex++] = item;
				}

				if (sourceItemIndex + 1 == blockSize)
				{
					sourceItemIndex = 0;
					sourceBlockIndex++;
					sourceMemoryBlockSpan = sourceMemoryBlocksSpan[sourceBlockIndex];

					if (sourceBlockIndex == lastBlockWithData)
					{
						break;
					}
				}
				else
				{
					sourceItemIndex++;
				}
			}

			if (sourceBlockIndex == lastBlockWithData)
			{
				// We're re-using another variable for better performance
				lastBlockWithData = list._nextItemIndex > 0 ? list._nextItemIndex : blockSize;
				while (sourceItemIndex < lastBlockWithData)
				{
					var item = sourceMemoryBlockSpan[sourceItemIndex++];
					if (match(item))
					{
						if (fullBlocksItemsCount + targetItemIndex >= targetCapacity)
						{
							targetCapacity = RecyclableLongList<T>.Helpers.Resize(result, blockSize, result._blockSizePow2BitShift, targetCapacity << 1);
							resultMemoryBlocksSpan = new(result._memoryBlocks);
						}
					
						if (targetItemIndex == blockSize)
						{
							targetItemIndex = 0;
							resultMemoryBlockSpan = resultMemoryBlocksSpan[++targetBlockIndex];
							fullBlocksItemsCount += blockSize;
						}

						resultMemoryBlockSpan[targetItemIndex++] = item;
					}
				}
			}

			if (targetItemIndex == blockSize)
			{
				result._nextItemBlockIndex = targetBlockIndex + 1;
				result._nextItemIndex = 0;
				result._lastBlockWithData = targetBlockIndex;
			}
			else
			{
				result._nextItemBlockIndex = targetBlockIndex;
				result._nextItemIndex = targetItemIndex;
				result._lastBlockWithData = targetBlockIndex;
			}

			result._longCount = fullBlocksItemsCount + targetItemIndex;
			result._capacity = targetCapacity;
			return result;
		}

		public static RecyclableLongList<long> FindAllIndexes<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return new();
			}

			int sourceBlockIndex = 0,
				blockSize = list._blockSize,
				targetBlockIndex = 0;
			var targetCapacity = Math.Max(list._longCount >> 3, RecyclableDefaults.InitialCapacity);
			if (!BitOperations.IsPow2(targetCapacity))
			{
				targetCapacity = checked((long)BitOperations.RoundUpToPowerOf2((ulong)targetCapacity));
			}

			RecyclableLongList<long> result = new(blockSize, targetCapacity);
			ReadOnlySpan<long[]> resultMemoryBlocksSpan = result._memoryBlocks;
			Span<long> resultMemoryBlockSpan = resultMemoryBlocksSpan[0];
			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[0];
			var sourceItemIndex = 0;
			int lastBlockWithData = list._lastBlockWithData;
			var targetItemIndex = 0;
			long sourceFullBlocksItemsCount = 0L,
				targetFullBlocksItemsCount = 0L;
			while (sourceBlockIndex < lastBlockWithData)
			{
				var item = sourceMemoryBlockSpan[sourceItemIndex];
				if (match(item))
				{
					if (targetFullBlocksItemsCount + targetItemIndex >= targetCapacity)
					{
						targetCapacity = RecyclableLongList<long>.Helpers.Resize(result, blockSize, result._blockSizePow2BitShift, targetCapacity << 1);
						resultMemoryBlocksSpan = new(result._memoryBlocks);
					}
					
					if (targetItemIndex == blockSize)
					{
						targetItemIndex = 0;
						resultMemoryBlockSpan = resultMemoryBlocksSpan[++targetBlockIndex];
						targetFullBlocksItemsCount += blockSize;
					}

					resultMemoryBlockSpan[targetItemIndex++] = sourceFullBlocksItemsCount + sourceItemIndex;
				}

				if (sourceItemIndex + 1 == blockSize)
				{
					sourceItemIndex = 0;
					sourceBlockIndex++;
					sourceMemoryBlockSpan = sourceMemoryBlocksSpan[sourceBlockIndex];
					sourceFullBlocksItemsCount += blockSize;

					if (sourceBlockIndex == lastBlockWithData)
					{
						break;
					}
				}
				else
				{
					sourceItemIndex++;
				}
			}

			if (sourceBlockIndex == lastBlockWithData)
			{
				// We're re-using another variable for better performance
				lastBlockWithData = list._nextItemIndex > 0 ? list._nextItemIndex : blockSize;
				while (sourceItemIndex < lastBlockWithData)
				{
					var item = sourceMemoryBlockSpan[sourceItemIndex];
					if (match(item))
					{
						if (targetFullBlocksItemsCount + targetItemIndex >= targetCapacity)
						{
							targetCapacity = RecyclableLongList<long>.Helpers.Resize(result, blockSize, result._blockSizePow2BitShift, targetCapacity << 1);
							resultMemoryBlocksSpan = new(result._memoryBlocks);
						}
					
						if (targetItemIndex == blockSize)
						{
							targetItemIndex = 0;
							resultMemoryBlockSpan = resultMemoryBlocksSpan[++targetBlockIndex];
							targetFullBlocksItemsCount += blockSize;
						}

						resultMemoryBlockSpan[targetItemIndex++] = sourceFullBlocksItemsCount + sourceItemIndex;
					}

					sourceItemIndex++;
				}
			}

			if (targetItemIndex == blockSize)
			{
				result._nextItemBlockIndex = targetBlockIndex + 1;
				result._nextItemIndex = 0;
				result._lastBlockWithData = targetBlockIndex;
			}
			else
			{
				result._nextItemBlockIndex = targetBlockIndex;
				result._nextItemIndex = targetItemIndex;
				result._lastBlockWithData = targetBlockIndex;
			}

			result._longCount = targetFullBlocksItemsCount + targetItemIndex;
			result._capacity = targetCapacity;
			return result;
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
