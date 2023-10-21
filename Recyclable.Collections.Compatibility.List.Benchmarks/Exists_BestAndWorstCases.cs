using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Benchmarks
{
	public partial class RecyclableCollectionsCompatibilityListBenchmarks
	{
		public void Array_Exists_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.Exists(list, (x) => x == data[i]));
				DoNothing.With(Array.Exists(list, (x) => x == data[^(i + 1)]));
			}
		}

		public void List_Exists_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.Exists((x) => x == data[i]));
				DoNothing.With(list.Exists((x) => x == data[^(i + 1)]));
			}
		}

		public void PooledList_Exists_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsPooledList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.Exists((x) => x == data[i]));
				DoNothing.With(list.Exists((x) => x == data[^(i + 1)]));
			}
		}

		public void RecyclableList_Exists_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.Exists((x) => x == data[i]));
				DoNothing.With(list.Exists((x) => x == data[^(i + 1)]));
			}
		}

		//public void RecyclableLongList_Exists_BestAndWorstCases()
		//{
		//	var data = TestObjects;
		//	var list = TestObjectsAsRecyclableLongList;
		//	var dataCount = TestObjectCountForSlowMethods;
		//	for (var i = 0; i < dataCount; i++)
		//	{
		//		DoNothing.With(list.Exists((x) => x == data[i]));
		//		DoNothing.With(list.Exists((x) => x == data[^(i + 1)]));
		//	}
		//}
	}
}
