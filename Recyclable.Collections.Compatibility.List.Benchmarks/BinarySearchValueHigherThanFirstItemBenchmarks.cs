using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class BinarySearchValueHigherThanFirstItemBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_BinarySearch_ValueHigherThanFirstItem";

		public void Array_BinarySearch_ValueHigherThanFirstItem()
		{
			var list = TestObjects;
			DoNothing.With(Array.BinarySearch(list, int.MaxValue));
		}

		public void List_BinarySearch_ValueHigherThanFirstItem()
		{
			var list = TestObjectsAsList;
			DoNothing.With(list.BinarySearch(int.MaxValue));
		}

		public void PooledList_BinarySearch_ValueHigherThanFirstItem()
		{
			var list = TestObjectsAsPooledList;
			DoNothing.With(list.BinarySearch(int.MaxValue));
		}

		public void RecyclableList_BinarySearch_ValueHigherThanFirstItem()
		{
			var list = TestObjectsAsRecyclableList;
			DoNothing.With(list.BinarySearch(int.MaxValue));
		}

		public void RecyclableLongList_BinarySearch_ValueHigherThanFirstItem()
		{
			var list = TestObjectsAsRecyclableLongList;
			DoNothing.With(list.BinarySearch(int.MaxValue));
		}
	}
}
