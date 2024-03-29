﻿// Ignore Spelling: zzz

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
			if (list._longCount == 0)
			{
				return default;
			}

			int blockIndex = 0,
				blockSize = list._blockSize,
				itemIndex = 0,
				lastBlockWithData = list._lastBlockWithData;

			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[0];
			while (blockIndex < lastBlockWithData)
			{
				if (match(sourceMemoryBlockSpan[itemIndex]))
				{
					return sourceMemoryBlockSpan[itemIndex];
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
					if (match(sourceMemoryBlockSpan[itemIndex]))
					{
						return sourceMemoryBlockSpan[itemIndex];
					}

					itemIndex++;
				}
			}

			return default;
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
			if (list._longCount == 0 || count == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int itemIndex = startIndex & list._blockSizeMinus1,
				blockIndex = startIndex >> list._blockSizePow2BitShift,
				blockSize = list._blockSize,
				lastItemIndex = (startIndex + count) & list._blockSizeMinus1,
				lastBlockWithData = (startIndex + count) >> list._blockSizePow2BitShift;

			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[blockIndex];
			while (blockIndex < lastBlockWithData)
			{
				if (match(sourceMemoryBlockSpan[itemIndex]))
				{
					return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
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
				lastBlockWithData = lastItemIndex > 0 ? lastItemIndex : blockSize;
				while (itemIndex < lastBlockWithData)
				{
					if (match(sourceMemoryBlockSpan[itemIndex]))
					{
						return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
					}

					itemIndex++;
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}

		public static int FindIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int itemIndex = startIndex & list._blockSizeMinus1,
				blockIndex = startIndex >> list._blockSizePow2BitShift,
				blockSize = list._blockSize,
				lastItemIndex = (int)(list._longCount & list._blockSizeMinus1),
				lastBlockWithData = list._lastBlockWithData;

			ReadOnlySpan<T[]> sourceMemoryBlocksSpan = list._memoryBlocks;
			ReadOnlySpan<T> sourceMemoryBlockSpan = sourceMemoryBlocksSpan[blockIndex];
			while (blockIndex < lastBlockWithData)
			{
				if (match(sourceMemoryBlockSpan[itemIndex]))
				{
					return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
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
				lastBlockWithData = lastItemIndex > 0 ? lastItemIndex : blockSize;
				while (itemIndex < lastBlockWithData)
				{
					if (match(sourceMemoryBlockSpan[itemIndex]))
					{
						return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
					}

					itemIndex++;
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}
		
		public static int FindIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
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
					return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
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
					if (match(sourceMemoryBlockSpan[itemIndex]))
					{
						return (blockIndex << list._blockSizePow2BitShift) + itemIndex;
					}

					itemIndex++;
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}

		public static T? FindLast<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return default;
			}

			int blockSizeMinus1 = list._blockSizeMinus1,
				itemIndex;

			ReadOnlySpan<T[]> memoryBlocksSpan = new(list._memoryBlocks);
			ReadOnlySpan<T> sourceSpan = memoryBlocksSpan[list._lastBlockWithData];
			for (itemIndex = list._nextItemIndex == 0 ? blockSizeMinus1 : list._nextItemIndex - 1; itemIndex >= 0; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return sourceSpan[itemIndex];
				}
			}

			for (var blockIndex = list._lastBlockWithData - 1; blockIndex >= 0; blockIndex--)
			{
				sourceSpan = memoryBlocksSpan[blockIndex];
				for (itemIndex = blockSizeMinus1; itemIndex >= 0; itemIndex--)
				{
					if (match(sourceSpan[itemIndex]))
					{
						return sourceSpan[itemIndex];
					}
				}
			}

			return default;
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, int count, Predicate<T> match)
		{
			if (count == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_Index();
				}

				return RecyclableDefaults.ItemNotFoundIndex;
			}
			
			if (count < 0 || startIndex - count + 1 < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_Count();
			}

			if (startIndex < 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int blockSizeMinus1 = list._blockSizeMinus1,
				firstBlockIndex = startIndex >> list._blockSizePow2BitShift,
				firstItemIndex = startIndex & blockSizeMinus1,
				blockIndex = (startIndex + count) >> list._blockSizePow2BitShift,
				itemIndex;

			ReadOnlySpan<T[]> memoryBlocksSpan = new(list._memoryBlocks);
			ReadOnlySpan<T> sourceSpan = memoryBlocksSpan[blockIndex];
			for (itemIndex = ((startIndex + count) & blockSizeMinus1) - 1; itemIndex >= 0; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return itemIndex;
				}
			}

			while(blockIndex > firstBlockIndex)
			{
				sourceSpan = memoryBlocksSpan[blockIndex];
				for (itemIndex = blockSizeMinus1; itemIndex >= 0; itemIndex--)
				{
					if (match(sourceSpan[itemIndex]))
					{
						return itemIndex;
					}
				}

				blockIndex--;
			}

			sourceSpan = memoryBlocksSpan[blockIndex];
			for (itemIndex = blockSizeMinus1; itemIndex >= firstItemIndex; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return itemIndex;
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, int startIndex, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_Index();
				}

				return RecyclableDefaults.ItemNotFoundIndex;
			}
			
			if (startIndex < 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int blockSizeMinus1 = list._blockSizeMinus1,
				firstBlockIndex = startIndex >> list._blockSizePow2BitShift,
				firstItemIndex = startIndex & blockSizeMinus1,
				blockIndex = (int)(list._longCount >> list._blockSizePow2BitShift),
				itemIndex;

			ReadOnlySpan<T[]> memoryBlocksSpan = new(list._memoryBlocks);
			ReadOnlySpan<T> sourceSpan = memoryBlocksSpan[blockIndex];
			for (itemIndex = list._nextItemIndex > 0 ? list._nextItemIndex - 1 : blockSizeMinus1; itemIndex >= 0; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return itemIndex;
				}
			}

			for (; blockIndex > firstBlockIndex; blockIndex--)
			{
				sourceSpan = memoryBlocksSpan[blockIndex];
				for (itemIndex = blockSizeMinus1; itemIndex >= 0; itemIndex--)
				{
					if (match(sourceSpan[itemIndex]))
					{
						return itemIndex;
					}
				}
			}

			sourceSpan = memoryBlocksSpan[blockIndex];
			for (itemIndex = blockSizeMinus1; itemIndex >= firstItemIndex; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return itemIndex;
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}

		public static int FindLastIndex<T>(this RecyclableLongList<T> list, Predicate<T> match)
		{
			if (list._longCount == 0)
			{
				return RecyclableDefaults.ItemNotFoundIndex;
			}

			int blockSizeMinus1 = list._blockSizeMinus1;
			ReadOnlySpan<T[]> memoryBlocksSpan = new(list._memoryBlocks);

			ReadOnlySpan<T> sourceSpan = memoryBlocksSpan[list._lastBlockWithData];
			int itemIndex;
			for (itemIndex = list._nextItemIndex == 0 ? blockSizeMinus1 : list._nextItemIndex - 1; itemIndex >= 0; itemIndex--)
			{
				if (match(sourceSpan[itemIndex]))
				{
					return itemIndex;
				}
			}

			for (var blockIndex = list._lastBlockWithData - 1; blockIndex >= 0; blockIndex--)
			{
				sourceSpan = memoryBlocksSpan[blockIndex];
				for (itemIndex = blockSizeMinus1; itemIndex >= 0; itemIndex--)
				{
					if (match(sourceSpan[itemIndex]))
					{
						return itemIndex;
					}
				}
			}

			return RecyclableDefaults.ItemNotFoundIndex;
		}
	}
}
