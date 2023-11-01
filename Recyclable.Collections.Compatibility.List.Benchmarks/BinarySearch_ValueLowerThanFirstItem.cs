using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class BinarySearchValueLowerThanFirstItemBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_BinarySearch_ValueLowerThanFirstItem";

		public void Array_BinarySearch_ValueLowerThanFirstItem()
		{
			var list = TestObjects;
			DoNothing.With(Array.BinarySearch(list, -1));
		}

		public void List_BinarySearch_ValueLowerThanFirstItem()
		{
			var list = TestObjectsAsList;
			DoNothing.With(list.BinarySearch(-1));
		}

		public void PooledList_BinarySearch_ValueLowerThanFirstItem()
		{
			var list = TestObjectsAsPooledList;
			DoNothing.With(list.BinarySearch(-1));
		}

		public void RecyclableList_BinarySearch_ValueLowerThanFirstItem()
		{
			var list = TestObjectsAsRecyclableList;
			DoNothing.With(list.BinarySearch(-1));
		}

		public void RecyclableLongList_BinarySearch_ValueLowerThanFirstItem()
		{
			var list = TestObjectsAsRecyclableLongList;
			DoNothing.With(list.BinarySearch(-1));
		}
	}
}
