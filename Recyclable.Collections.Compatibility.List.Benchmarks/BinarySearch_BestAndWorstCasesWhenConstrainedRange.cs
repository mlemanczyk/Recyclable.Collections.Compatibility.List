using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class BinarySearchBenchmarksBestAndWorstCasesWhenConstrainedRange : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_BinarySearchWhenConstrainedRange";
		private readonly static Comparer<int> _comparer = Comparer<int>.Default;

		public void Array_BinarySearchWhenConstrainedRange()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.BinarySearch(list, 0, dataCount, data[i], _comparer));
				DoNothing.With(Array.BinarySearch(list, 0, dataCount, data[^(i + 1)], _comparer));
			}
		}

		public void List_BinarySearchWhenConstrainedRange()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(0, dataCount, data[i], _comparer));
				DoNothing.With(list.BinarySearch(0, dataCount, data[^(i + 1)], _comparer));
			}
		}

		public void PooledList_BinarySearchWhenConstrainedRange()
		{
			var data = TestObjects;
			var list = TestObjectsAsPooledList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(0, dataCount, data[i], _comparer));
				DoNothing.With(list.BinarySearch(0, dataCount, data[^(i + 1)], _comparer));
			}
		}

		public void RecyclableList_BinarySearchWhenConstrainedRange()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(0, dataCount, data[i], _comparer));
				DoNothing.With(list.BinarySearch(0, dataCount, data[^(i + 1)], _comparer));
			}
		}

		public void RecyclableLongList_BinarySearchWhenConstrainedRange()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableLongList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.BinarySearch(0, dataCount, data[i], _comparer));
				DoNothing.With(list.BinarySearch(0, dataCount, data[^(i + 1)], _comparer));
			}
		}
	}
}
