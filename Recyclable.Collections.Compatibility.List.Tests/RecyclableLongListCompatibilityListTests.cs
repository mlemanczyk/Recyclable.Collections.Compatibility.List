using FluentAssertions;
using Recyclable.Collections;
using Recyclable.Collections.TestData;
using Recyclable.Collections.Pools;
using System.Numerics;
using System.Collections;

#pragma warning disable xUnit1026, RCS1163, IDE0039, IDE0060, RCS1235

namespace Recyclable.CollectionsTests
{
	public class RecyclableLongListCompatibilityListTests
	{
		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldFindAllItems(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.BinarySearch(expectedItem);

				// Act
				var actual = list.BinarySearch(expectedItem);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldFindAllItemsWhenConstrainedRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				TestWith(list, comparer, expectedData, itemIndex, rangedItemsCount, expectedData[(int)itemIndex]);
				TestWith(list, comparer, expectedData, itemIndex, rangedItemsCount, expectedData[Math.Min((int)(itemIndex + rangedItemsCount), itemsCount - 1)]);
				TestWith(list, comparer, expectedData, itemIndex, rangedItemsCount, expectedData[(int)(itemIndex + (rangedItemsCount >> 1))]);
			}

			static void TestWith(RecyclableLongList<long> list, Comparer<long> comparer, List<long> expectedData, long itemIndex, long rangedItemsCount, long expectedItem)
			{
				var expected = expectedData.BinarySearch((int)itemIndex, (int)rangedItemsCount, expectedItem, comparer);

				// Act
				var actual = list.BinarySearch((int)itemIndex, (int)rangedItemsCount, expectedItem, comparer);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldFindAllItemsWhenWithCustomComparer(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.BinarySearch(expectedItem, comparer);

				// Act
				var actual = list.BinarySearch(expectedItem, comparer);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldNotFindNonExistingItems(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
				list.RemoveAt((int)itemIndex);
				var expectedItem = expectedData[(int)itemIndex];
				var expectedRangedData = testData.ToList();
				expectedRangedData.RemoveAt((int)itemIndex);
				var expected = expectedRangedData.BinarySearch(expectedItem);

				// Act
				var actual = list.BinarySearch(expectedItem);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldNotFindNonExistingItemsWhenConstrainedRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
				list.RemoveAt(itemIndex);
				var expectedItem = expectedData[(int)itemIndex];
				var expectedRangedData = testData.ToList();
				expectedRangedData.RemoveAt((int)itemIndex);
				var expectedRangeSize = itemIndex + rangedItemsCount < expectedRangedData.Count ? rangedItemsCount : expectedRangedData.Count - itemIndex;
				// Now we have 1 element less on the lists
				var expected = expectedRangedData.BinarySearch((int)itemIndex, (int)expectedRangeSize, expectedItem, comparer);

				// Act
				var actual = list.BinarySearch((int)itemIndex, (int)expectedRangeSize, expectedItem, comparer);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldNotFindNonExistingItemsWhenOutsideRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				var expectedItem = expectedData[(int)itemIndex];

				// Act && Validate
				var expected = expectedData.BinarySearch(0, (int)itemIndex, expectedItem, comparer);
				_ = list.BinarySearch(0, (int)itemIndex, expectedItem, comparer).Should().Be(expected);

				expected = expectedData.BinarySearch((int)itemIndex + 1, itemsCount - (int)itemIndex - 1, expectedItem, comparer);
				_ = list.BinarySearch((int)itemIndex + 1, itemsCount - (int)itemIndex - 1, expectedItem, comparer).Should().Be(expected);

				expectedItem--;
				expected = expectedData.BinarySearch((int)itemIndex, (int)rangedItemsCount, expectedItem, comparer);
				_ = list.BinarySearch((int)itemIndex, (int)rangedItemsCount, expectedItem, comparer).Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldNotFindNonExistingItemsWhenWithCustomComparer(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			var comparer = Comparer<long>.Default;
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
				list.RemoveAt((int)itemIndex);
				var expectedItem = expectedData[(int)itemIndex];
				var expectedRangedData = testData.ToList();
				expectedRangedData.RemoveAt((int)itemIndex);
				var expected = expectedRangedData.BinarySearch(expectedItem, comparer);

				// Act
				var actual = list.BinarySearch(expectedItem, comparer);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void BinarySearchShouldThrowArgumentOutOfRangeWhenOutsideRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];

				// Act && Validate
				Assert.Throws<ArgumentOutOfRangeException>(() => _ = expectedData.BinarySearch(-1, (int)itemIndex, expectedItem, comparer));
				Assert.Throws<ArgumentOutOfRangeException>(() => _ = list.BinarySearch(-1, (int)itemIndex, expectedItem, comparer));

				Assert.Throws<ArgumentException>(() => _ = expectedData.BinarySearch((int)itemIndex + 1, itemsCount, expectedItem, comparer));
				Assert.Throws<ArgumentException>(() => _ = list.BinarySearch((int)itemIndex + 1, itemsCount, expectedItem, comparer));

				Assert.Throws<ArgumentOutOfRangeException>(() => _ = expectedData.BinarySearch((int)itemIndex + 1, -1, expectedItem, comparer));
				Assert.Throws<ArgumentOutOfRangeException>(() => _ = list.BinarySearch((int)itemIndex + 1, -1, expectedItem, comparer));
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ConvertAllShouldConvertAllItems(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var expectedData = testData.ToList().ConvertAll(item => item + 0.1);

			// Act
			using var actual = list.ConvertAll(item => item + 0.1);

			// Validate
			_ = actual.Count.Should().Be(itemsCount);
			_ = actual.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void CopyToShouldCopyAllItemsInTheCorrectOrderWhenConstrainedIndex(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemIndexRanges)
		{
			// Prepare
			var expectedData = testData.ToList();
			foreach (var (itemIndex, _) in itemIndexRanges)
			{
				using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
				long[] expectedItems = new long[expectedData.Count];
				expectedData.CopyTo((int)itemIndex, expectedItems, (int)itemIndex, (int)(expectedData.Count - itemIndex));
				long[] actualItems = new long[expectedData.Count];

				// Act
				list.CopyTo((int)itemIndex, actualItems, (int)itemIndex);

				// Validate
				_ = actualItems.Should().Equal(expectedItems);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void CopyToShouldCopyAllItemsInTheCorrectOrderWhenConstrainedRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemIndexRanges)
		{
			// Prepare
			var expectedData = testData.ToList();
			foreach (var (itemIndex, rangedItemsCount) in itemIndexRanges)
			{
				using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
				long[] expectedItems = new long[rangedItemsCount + itemIndex];
				expectedData.CopyTo((int)itemIndex, expectedItems, (int)itemIndex, (int)rangedItemsCount);
				long[] actualItems = new long[rangedItemsCount + itemIndex];

				// Act
				list.CopyTo((int)itemIndex, actualItems, (int)itemIndex, (int)rangedItemsCount);

				// Validate
				_ = actualItems.Should().Equal(expectedItems);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void CopyToShouldCopyAllItemsInTheCorrectOrderWhenFromStart(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			long[] copiedItems = new long[itemsCount];

			// Act
			list.CopyTo(copiedItems);

			// Validate
			long[] expectedData = new long[itemsCount];
			testData.ToList().CopyTo(expectedData);

			_ = copiedItems.Should().Equal(testData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void CopyToShouldThrowArgumentOutOfRangeWhenOutsideRange(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, minBlockSize, initialCapacity: itemsCount);
			var comparer = Comparer<long>.Default;
			var expectedData = testData.ToList();
			var actual = new long[itemsCount];

			foreach (var itemIndex in itemIndexes)
			{

				// Act && Validate
				Assert.Throws<ArgumentOutOfRangeException>(() => expectedData.CopyTo(-1, actual, 0, (int)itemIndex));
				Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(-1, actual, 0, (int)itemIndex));

				Assert.Throws<ArgumentOutOfRangeException>(() => expectedData.CopyTo(0, actual, -1, (int)itemIndex));
				Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(0, actual, -1, (int)itemIndex));

				Assert.Throws<ArgumentException>(() => expectedData.CopyTo(0, actual, (int)itemIndex + 1, itemsCount));
				Assert.Throws<ArgumentException>(() => list.CopyTo(0, actual, (int)itemIndex + 1, itemsCount));

				Assert.Throws<ArgumentOutOfRangeException>(() => expectedData.CopyTo(0, actual, (int)itemIndex + 1, -1));
				Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(0, actual, (int)itemIndex + 1, -1));
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ExistsShouldFindAllItems(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.Exists(item => item == expectedItem);

				// Act
				var actual = list.Exists(item => item == expectedItem);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ExistsShouldNotFindNonExistingItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.Exists(item => item == -expectedItem);

				// Act
				var actual = list.Exists(item => item == -expectedItem);

				// Validate				
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindAllIndexesShouldReturnCorrectIndexes(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			//* There is no equivalent of .FindAllIndexes in List<T>, so no comparison here.

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);

			foreach (var itemIndex in itemIndexes)
			{
				// Act
				using var actual = list.FindAllIndexes(item => item == itemIndex + 1);

				// Validate
				_ = actual.Count.Should().Be(1);
				if (itemsCount > 0)
				{
					_ = actual.Count.Should().Be(1);
					_ = actual.Should().AllSatisfy(x => x.Should().Be((int)itemIndex));
				}
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.ItemsCountTestCases), MemberType = typeof(RecyclableLongListTestData))]
		public void FindAllShouldNotFindNonExistingItems(int itemsCount)
		{
			const int ExpectedItem = 1;

			// Prepare
			var testData = Enumerable.Range(0, itemsCount).Select(_ => ExpectedItem);
			using var list = new RecyclableLongList<int>(testData, initialCapacity: itemsCount);

			// Act
			using var foundItems = list.FindAll(x => x == -ExpectedItem);

			// Validate
			var expectedData = testData.ToList().FindAll(x => x == -ExpectedItem);

			_ = foundItems.Count.Should().Be(expectedData.Count);
			_ = foundItems.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.ItemsCountTestCases), MemberType = typeof(RecyclableLongListTestData))]
		public void FindAllShouldReturnCorrectIndexes(int itemsCount)
		{
			const int ExpectedItem = 1;

			// Prepare
			var testData = Enumerable.Range(0, itemsCount).Select(value => value % 2 == 0 ? ExpectedItem : -ExpectedItem);
			using var list = new RecyclableLongList<int>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList().FindAll(x => x == ExpectedItem);

			// Act
			using var actual = list.FindAll(x => x == ExpectedItem);

			// Validate
			_ = actual.Count.Should().Be(expectedData.Count);
			_ = actual.Should().Equal(expectedData);
			if (expectedData.Count > 0)
			{
				_ = actual.Should().AllSatisfy(x => x.Should().Be(ExpectedItem)).And.Equal(expectedData);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindIndexShouldReturnCorrectIndexes(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];

				// Act
				var actual = list.FindIndex(item => item == expectedItem);

				// Validate
				var expected = expectedData.FindIndex(item => item == expectedItem);

				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindIndexShouldReturnCorrectIndexesWhenConstrainedCount(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				TestWith(itemsCount, list, expectedData, itemIndex, rangedItemsCount, expectedData[(int)itemIndex]);
				TestWith(itemsCount, list, expectedData, itemIndex, rangedItemsCount, expectedData[Math.Min((int)(itemIndex + rangedItemsCount), itemsCount - 1)]);
				TestWith(itemsCount, list, expectedData, itemIndex, rangedItemsCount, expectedData[(int)(itemIndex + (rangedItemsCount >> 1))]);
			}

			static void TestWith(int itemsCount, RecyclableLongList<long> list, List<long> expectedData, long itemIndex, long rangedItemsCount, long expectedItem)
			{
				var expected = expectedData.FindIndex((int)itemIndex, (int)rangedItemsCount, item => item == expectedItem);

				// Act
				var actual = list.FindIndex((int)itemIndex, (int)rangedItemsCount, item => item == expectedItem);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindIndexShouldNotAnythingWhenRangeExcludesItem(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				TestWith(itemsCount, list, expectedData, itemIndex, rangedItemsCount, expectedData[Math.Max((int)(itemIndex - 1), 0)]);
				TestWith(itemsCount, list, expectedData, itemIndex, rangedItemsCount, expectedData[Math.Min((int)(itemIndex + rangedItemsCount), itemsCount - 1)]);
			}

			static void TestWith(int itemsCount, RecyclableLongList<long> list, List<long> expectedData, long itemIndex, long rangedItemsCount, long expectedItem)
			{
				var expected = expectedData.FindIndex((int)itemIndex, (int)rangedItemsCount, item => item == expectedItem);

				// Act
				var actual = list.FindIndex((int)itemIndex, (int)rangedItemsCount, item => item == expectedItem);

				// Validate
				_ = actual.Should().Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindIndexShouldReturnCorrectIndexesWhenConstrainedIndex(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.FindIndex(Math.Min(1, (int)itemIndex), item => item == expectedItem);

				// Act
				var actual = list.FindIndex(Math.Min(1, (int)itemIndex), item => item == expectedItem);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindLastIndexShouldReturnCorrectIndexes(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.FindLastIndex(x => x == expectedItem);

				// Act
				var actual = list.FindLastIndex(x => x == expectedItem);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithBlockSizeWithItemIndexWithRangeVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindLastIndexShouldReturnCorrectIndexesWhenConstrainedCount(string testCase, IEnumerable<long> testData, int itemsCount, int minBlockSize, in (long ItemIndex, long RangedItemsCount)[] itemRanges)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var (itemIndex, rangedItemsCount) in itemRanges)
			{
				// Act & Validate
				TestWith(itemsCount, list, expectedData, itemIndex, expectedData[(int)itemIndex]);
				TestWith(itemsCount, list, expectedData, itemIndex, expectedData[Math.Min((int)(itemIndex + rangedItemsCount), itemsCount - 1)]);
			}

			static void TestWith(int itemsCount, RecyclableLongList<long> list, List<long> expectedData, long itemIndex, long expectedItem)
			{
				var expected = expectedData.FindLastIndex(itemsCount - 1, itemsCount, x => x == expectedItem);
				_ = list.FindLastIndex(itemsCount - 1, itemsCount, x => x == expectedItem).Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindLastIndexShouldReturnCorrectIndexesWhenConstrainedIndex(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.FindLastIndex(itemsCount - 1, x => x == expectedItem);

				// Act
				var actual = list.FindLastIndex(itemsCount - 1, x => x == expectedItem);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindLastIndexShouldThrowArgumentOutOfRange(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				int expected = int.MinValue;

				// Act & Validate
				if (itemsCount - 1 != -1)
				{
					Assert.Throws<ArgumentOutOfRangeException>(() => expected = expectedData.FindLastIndex(itemsCount - 1, itemsCount + 3, x => x == expectedItem));
					Assert.Throws<ArgumentOutOfRangeException>(() => _ = list.FindLastIndex(itemsCount - 1, itemsCount + 3, x => x == expectedItem).Should().Be((int)itemIndex).And.Be(expected));
				}
				else
				{
					expected = expectedData.FindLastIndex(itemsCount - 1, itemsCount + 3, x => x == expectedItem);
					_ = list.FindLastIndex(itemsCount - 1, itemsCount + 3, x => x == expectedItem).Should().Be((int)itemIndex).And.Be(expected);
				}
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindLastShouldReturnCorrectItem(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.FindLast(item => item == expectedItem);

				// Act
				var actual = list.FindLast(item => item == expectedItem);

				// Validate
				_ = actual.Should().Be(expectedData[(int)itemIndex]).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindShouldFindAllItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				long expectedItem = expectedData[(int)itemIndex];
				var expected = expectedData.Find(item => item == expectedItem);

				// Act
				var actual = list.Find(item => item == expectedData[(int)itemIndex]);

				// Validate
				_ = actual.Should().Be(expectedData[(int)itemIndex]);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void FindShouldNotFindNonExistingItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.Find(item => item == -expectedData[(int)itemIndex]);

				// Act
				var actual = list.Find(item => item == -expectedData[(int)itemIndex]);

				// Validate
				actual.Should().Be(0).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ForEachShouldGiveItemsInCorrectOrder(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();
			var expected = new List<long>(itemsCount);
			expectedData.ForEach(expected.Add);

			// Act
			var yieldedItems = new List<long>(itemsCount);
			list.ForEach(yieldedItems.Add);

			// Validate
			_ = yieldedItems.Should().Equal(testData).And.Equal(expected);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.EmptySourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ForEachShouldDoNothingWhenEmptyList(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();
			var expected = new List<long>(itemsCount);
			expectedData.ForEach(expected.Add);

			// Act
			var yieldedItems = new List<long>(itemsCount);
			list.ForEach(item => yieldedItems.Add(item));

			// Validate
			_ = yieldedItems.Should().BeEmpty().And.Equal(expected);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void GetRangeShouldReturnCorrectItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				int startingIndex = (int)Math.Min(1, itemIndex);
				var expectedRangeItems = expectedData.GetRange(startingIndex, (int)itemIndex);

				// Act
				using var rangeItems = list.GetRange(startingIndex, (int)itemIndex);

				// Validate
				_ = rangeItems.Count.Should().Be(expectedRangeItems.Count);
				_ = rangeItems.Should().Equal(expectedRangeItems);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void IndexOfShouldReturnCorrectIndexesWhenConstrainedCount(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.IndexOf(expectedData[(int)itemIndex], (int)itemIndex, (int)(itemsCount - itemIndex));

				// Act
				var actual = list.IndexOf(expectedData[(int)itemIndex], (int)itemIndex, (int)(itemsCount - itemIndex));

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void IndexOfShouldReturnCorrectIndexesWhenConstrainedIndex(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.IndexOf(expectedData[(int)itemIndex], (int)itemIndex);

				// Act
				var actual = list.IndexOf(expectedData[(int)itemIndex], (int)itemIndex);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void InsertRangeShouldAddItemsInTheRightPosition(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			var expectedData = testData.Reverse().ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);
				var expected = expectedData.ToList();
				expected.InsertRange((int)itemIndex, testData);

				if (testCase.Contains("Array[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (Array)testData);
				}
				else if (testCase.Contains("ICollection[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (ICollection)testData);
				}
				else if (testCase.Contains("ICollection<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (ICollection<long>)testData);
				}
				else if (testCase.Contains("IEnumerable[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (IEnumerable)testData);
				}
				else if (testCase.Contains("IReadOnlyList<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (IReadOnlyList<long>)testData);
				}
				else if (testCase.Contains("ReadOnlySpan<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, new ReadOnlySpan<long>((long[])testData));
				}
				else if (testCase.Contains("Span<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, new Span<long>((long[])testData));
				}
				else if (testData is long[] testDataArray)
				{
					list.InsertRange((int)itemIndex, testDataArray);
				}
				else if (testData is List<long> testDataList)
				{
					list.InsertRange((int)itemIndex, testDataList);
				}
				else if (testData is RecyclableList<long> testDataRecyclableList)
				{
					list.InsertRange((int)itemIndex, testDataRecyclableList);
				}
				else if (testData is RecyclableLongList<long> testDataRecyclableLongList)
				{
					list.InsertRange((int)itemIndex, testDataRecyclableLongList);
				}
				else if (testData is IList<long> testDataIList)
				{
					list.InsertRange((int)itemIndex, testDataIList);
				}
				else if (testData is IEnumerable<long> testDataIEnumerable)
				{
					list.InsertRange((int)itemIndex, testDataIEnumerable);
				}
				else
				{
					throw new InvalidCastException("Unknown type of test data");
				}

				// Validate
				list.Count.Should().Be(itemsCount << 1);
				_ = list.Should().Equal(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void LastIndexOfShouldReturnCorrectIndexes(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.LastIndexOf(expectedData[(int)itemIndex]);

				// Act
				var actual = list.LastIndexOf(expectedData[(int)itemIndex]);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void LastIndexOfShouldReturnCorrectIndexesWhenConstrainedCount(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1, itemsCount);

				// Act
				var actual = list.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1, itemsCount);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void LastIndexOfShouldReturnCorrectIndexesWhenConstrainedIndex(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				var expected = expectedData.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1);

				// Act
				var actual = list.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1);

				// Validate
				_ = actual.Should().Be((int)itemIndex).And.Be(expected);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void LastIndexOfShouldThrowArgumentOutOfRangeWhenCountTooBig(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();

			// Act & Validate
			foreach (var itemIndex in itemIndexes)
			{
				_ = Assert.Throws<ArgumentOutOfRangeException>(() => _ = expectedData.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1, itemsCount + 3).Should().Be((int)itemIndex));
				_ = Assert.Throws<ArgumentOutOfRangeException>(() => _ = list.LastIndexOf(expectedData[(int)itemIndex], itemsCount - 1, itemsCount + 3).Should().Be((int)itemIndex));
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void InsertRangeShouldMoveItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			foreach (var itemIndex in itemIndexes)
			{
				// Prepare
				using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);
				var expectedItems = testData.Reverse().ToList();
				var item = expectedItems[^1];
				expectedItems.InsertRange((int)itemIndex, testData);

				// Act
				if (testCase.Contains("Array[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (Array)testData);
				}
				else if (testCase.Contains("ICollection[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (ICollection)testData);
				}
				else if (testCase.Contains("ICollection<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (ICollection<long>)testData);
				}
				else if (testCase.Contains("IEnumerable[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (IEnumerable)testData);
				}
				else if (testCase.Contains("IReadOnlyList<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, (IReadOnlyList<long>)testData);
				}
				else if (testCase.Contains("ReadOnlySpan<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, new ReadOnlySpan<long>((long[])testData));
				}
				else if (testCase.Contains("Span<T>[", StringComparison.OrdinalIgnoreCase))
				{
					list.InsertRange((int)itemIndex, new Span<long>((long[])testData));
				}
				else if (testData is long[] testDataArray)
				{
					list.InsertRange((int)itemIndex, testDataArray);
				}
				else if (testData is List<long> testDataList)
				{
					list.InsertRange((int)itemIndex, testDataList);
				}
				else if (testData is RecyclableList<long> testDataRecyclableList)
				{
					list.InsertRange((int)itemIndex, testDataRecyclableList);
				}
				else if (testData is RecyclableLongList<long> testDataRecyclableLongList)
				{
					list.InsertRange((int)itemIndex, testDataRecyclableLongList);
				}
				else if (testData is IList<long> testDataIList)
				{
					list.InsertRange((int)itemIndex, testDataIList);
				}
				else if (testData is IEnumerable<long> testDataIEnumerable)
				{
					list.InsertRange((int)itemIndex, testDataIEnumerable);
				}
				else
				{
					throw new InvalidCastException("Unknown type of test data");
				}

				// Validate
				_ = list[(int)itemIndex].Should().Be(item);
				_ = list.Count.Should().Be(itemsCount << 1);
				_ = list.Capacity.Should().BeGreaterThanOrEqualTo(itemsCount << 1);
				_ = list.Should().Equal(expectedItems);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void RemoveAllShouldRemoveTheCorrectItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var itemsToRemove = itemIndexes.Select(x => list[(int)x]).ToArray();
			var expectedItems = testData.ToList();
			var expectedResult = expectedItems.RemoveAll(itemsToRemove.Contains);

			// Act
			var actualResult = list.RemoveAll(itemsToRemove.Contains);

			// Validate
			_ = actualResult.Should().Be(expectedResult);
			_ = list.Capacity.Should().BeGreaterThanOrEqualTo(itemsCount);
			_ = list.Count.Should().Be(itemsCount - itemsToRemove.Length);
			_ = list.Should().Equal(expectedItems);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void RemoveRangeShouldRemoveCorrectItems(string testCase, IEnumerable<long> testData, int itemsCount, in long[] itemIndexes)
		{
			_ = testData.Any().Should().BeTrue("we need items on the list that we can look for");

			foreach (var itemIndex in itemIndexes)
			{
				// Prepare
				int startingIndex = (int)Math.Min(1, itemIndex);
				var expectedData = testData.ToList();
				expectedData.RemoveRange(startingIndex, (int)itemIndex);

				// Act
				using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
				list.RemoveRange(startingIndex, (int)itemIndex);

				// Validate
				_ = list.Count.Should().Be(expectedData.Count);
				_ = list.Should().Equal(expectedData);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ReverseShouldSwapItemsInCorrectOrder(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedData = testData.ToList();
			expectedData.Reverse();

			// Act
			list.Reverse();

			// Validate
			_ = list.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void ReverseShouldSwapItemsInCorrectOrderWhenConstrained(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			var expectedData = testData.ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				// Prepare
				using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
				var startingItemIndex = (int)Math.Min(1, itemIndex);
				var expectedRangeItems = expectedData.ToList();
				expectedRangeItems.Reverse(startingItemIndex, (int)itemIndex);

				// Act
				list.Reverse(startingItemIndex, (int)itemIndex);

				// Validate
				_ = list.Should().Equal(expectedRangeItems);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void SortShouldSortItemsInCorrectOrder(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);
			var expectedData = testData.Reverse().ToList();
			expectedData.Sort();

			// Act
			list.Sort();

			// Validate
			_ = list.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void SortShouldSortItemsInCorrectOrderWhenWithComparer(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);
			IComparer<long> comparer = Comparer<long>.Default;
			var expectedData = testData.Reverse().ToList();
			expectedData.Sort(comparer);

			// Act
			list.Sort(comparer);

			// Validate
			_ = list.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void SortShouldSortItemsInCorrectOrderWhenWithComparison(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);

			static int comparison(long x, long y)
			{
				return (x - y) switch
				{
					> 0 => 1,
					< 0 => -1,
					_ => 0,
				};
			}

			var expectedData = testData.Reverse().ToList();
			expectedData.Sort(comparison);

			// Act
			list.Sort(comparison);

			// Validate
			_ = list.Should().Equal(expectedData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void SortShouldSortItemsInCorrectOrderWhenConstrained(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			IComparer<long> comparer = Comparer<long>.Default;
			var expectedData = testData.Reverse().ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				// Prepare
				using var list = new RecyclableLongList<long>(testData.Reverse(), initialCapacity: itemsCount);
				var startingIndex = (int)Math.Min(1, itemIndex);

				var expectedSortedData = expectedData.ToList();
				expectedSortedData.Sort(startingIndex, (int)itemIndex, comparer);

				// Act
				list.Sort(startingIndex, (int)itemIndex, comparer);

				// Validate
				_ = list.Should().Equal(expectedSortedData);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldDecreaseCapacity(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedCapacity = itemsCount >= RecyclableDefaults.InitialCapacity ? itemsCount : RecyclableDefaults.InitialCapacity;

			// Act
			list.TrimExcess();

			// Validate
			_ = list.Capacity.Should().Be(expectedCapacity);
			_ = list.Count.Should().Be(itemsCount);
			_ = list.Should().Equal(testData);
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakAdd(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedCapacity = (int)BitOperations.RoundUpToPowerOf2((uint)itemsCount + 1);
			expectedCapacity = expectedCapacity >= RecyclableDefaults.InitialCapacity ? expectedCapacity : RecyclableDefaults.InitialCapacity;

			// Act
			list.TrimExcess();
			list.Add(-1);

			// Validate
			_ = list.Capacity.Should().Be(expectedCapacity);
			_ = list.Count.Should().Be(itemsCount + 1);
			_ = list.Should().Equal(testData.Append(-1));
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakAddRange(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
			var expectedCapacity = (int)BitOperations.RoundUpToPowerOf2(checked((uint)itemsCount << 1));
			expectedCapacity = expectedCapacity >= RecyclableDefaults.InitialCapacity ? expectedCapacity : RecyclableDefaults.InitialCapacity;

			// Act
			list.TrimExcess();
			list.AddRange(testData);

			// Validate
			_ = list.Capacity.Should().BeGreaterThanOrEqualTo(expectedCapacity);
			_ = list.Count.Should().Be(itemsCount << 1);
			_ = list.Should().Equal(testData.Concat(testData));
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakClear(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);

			// Act
			list.TrimExcess();
			list.Clear();

			// Validate
			_ = list.Capacity.Should().Be(Math.Max(RecyclableDefaults.InitialCapacity, itemsCount));
			_ = list.Count.Should().Be(0);
			_ = list.Should().BeEmpty();
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakInsert(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			var expectedCapacity = (int)BitOperations.RoundUpToPowerOf2((uint)itemsCount + 1);
			expectedCapacity = expectedCapacity >= RecyclableDefaults.InitialCapacity ? expectedCapacity : RecyclableDefaults.InitialCapacity;
			var expectedData = testData.ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
				var expectedItemsAfterInsert = expectedData.ToList();
				expectedItemsAfterInsert.Insert((int)itemIndex, -3);

				// Act
				list.TrimExcess();
				list.Insert((int)itemIndex, -3);

				// Validate
				_ = list.Capacity.Should().BeGreaterThanOrEqualTo(expectedCapacity);
				_ = list.Count.Should().Be(expectedItemsAfterInsert.Count);
				_ = list.Should().Equal(expectedItemsAfterInsert);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakRemove(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			var expectedData = testData.ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);
				var expectedItemsAfterInsert = expectedData.ToList();
				expectedItemsAfterInsert.RemoveAt((int)itemIndex);

				// Act
				list.TrimExcess();
				list.RemoveAt((int)itemIndex);

				// Validate
				_ = list.Capacity.Should().BeGreaterThanOrEqualTo(itemsCount);
				_ = list.Count.Should().Be(expectedItemsAfterInsert.Count);
				_ = list.Should().Equal(expectedItemsAfterInsert);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimExcessShouldNotBreakIndexOf(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			var expectedData = testData.ToArray();

			foreach (var itemIndex in itemIndexes)
			{
				using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);

				// Act
				list.TrimExcess();
				var actual = list.IndexOf(expectedData[(int)itemIndex]);

				// Validate
				_ = actual.Should().Be((int)itemIndex);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrimmedArraysShouldNotBePooled(string testCase, IEnumerable<long> testData, int itemsCount)
		{
			// Prepare
			using var list = new RecyclableLongList<long>(testData, initialCapacity: itemsCount);

			// Act
			list.TrimExcess();
			var trimmedMemoryBlock = list.AsArray;

			list.Dispose();

			// Validate
			_ = list.Capacity.Should().Be(0);
			_ = list.Count.Should().Be(0);
			_ = list.Should().BeEmpty();

			if (itemsCount >= RecyclableDefaults.MinPooledArrayLength)
			{
				var newMemoryBlock = RecyclableArrayPool<long[]>.RentShared(itemsCount);
				_ = BitOperations.IsPow2(itemsCount)
					? newMemoryBlock.Should().BeSameAs(trimmedMemoryBlock)
					: newMemoryBlock.Should().NotBeSameAs(trimmedMemoryBlock);
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrueForAllShouldBeFalseWhenNoMatch(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			var expectedData = testData.ToList();

			foreach (var itemIndex in itemIndexes)
			{
				using RecyclableList<long> list = new(testData, initialCapacity: itemsCount);
				Predicate<long> condition = item => item == itemIndex;
				var expected = expectedData.TrueForAll(condition);

				// Act
				var actual = list.TrueForAll(condition);

				// Validate
				_ = actual.Should().Be(expected).And.BeFalse();
			}
		}

		[Theory]
		[MemberData(nameof(RecyclableLongListTestData.SourceDataWithItemIndexVariants), MemberType = typeof(RecyclableLongListTestData))]
		public void TrueForAllShouldBeTrueWhenMatched(string testCase, IEnumerable<long> testData, int itemsCount, long[] itemIndexes)
		{
			// Prepare
			using RecyclableList<long> list = new(testData, initialCapacity: itemsCount);
			int index = 0;
			Predicate<long> condition = item => item == ++index;
			var expected = testData.ToList().TrueForAll(condition);

			index = 0;

			// Act
			var actual = list.TrueForAll(condition);

			// Validate
			_ = actual.Should().Be(expected).And.BeTrue();
		}
	}
}