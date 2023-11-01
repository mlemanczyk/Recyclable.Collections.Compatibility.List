using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class BinarySearchBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_BinarySearch";

		public void Array_BinarySearch()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.BinarySearch(list, data[i]));
				DoNothing.With(Array.BinarySearch(list, data[^(i + 1)]));
			}
		}

		public void List_BinarySearch()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(data[i]));
				DoNothing.With(list.BinarySearch(data[^(i + 1)]));
			}
		}

		public void PooledList_BinarySearch()
		{
			var data = TestObjects;
			var list = TestObjectsAsPooledList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(data[i]));
				DoNothing.With(list.BinarySearch(data[^(i + 1)]));
			}
		}

		public void RecyclableList_BinarySearch()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(data[i]));
				DoNothing.With(list.BinarySearch(data[^(i + 1)]));
			}
		}

		public void RecyclableLongList_BinarySearch()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableLongList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(data[i]));
				DoNothing.With(list.BinarySearch(data[^(i + 1)]));
			}
		}
	}
}
