using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class FindLastIndexBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_FindLastIndex_BestAndWorstCases";

		public void Array_FindLastIndex_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.FindLastIndex(list, 0, (x) => x == data[i]));
				DoNothing.With(Array.FindLastIndex(list, 0, (x) => x == data[^(i + 1)]));
			}
		}

		public void List_FindLastIndex_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.FindLastIndex(0, (x) => x == data[i]));
				DoNothing.With(list.FindLastIndex(0, (x) => x == data[^(i + 1)]));
			}
		}

		public void RecyclableList_FindLastIndex_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.FindLastIndex(0, (x) => x == data[i]));
				DoNothing.With(list.FindLastIndex(0, (x) => x == data[^(i + 1)]));
			}
		}

		//public void RecyclableLongList_FindLastIndex_BestAndWorstCases()
		//{
		//	var data = TestObjects;
		//	var list = TestObjectsAsRecyclableLongList;
		//	var dataCount = TestObjectCountForSlowMethods;
		//	for (var i = 0; i < dataCount; i++)
		//	{
		//		DoNothing.With(list.FindLastIndex(0, (x) => x == data[i]));
		//		DoNothing.With(list.FindLastIndex(0, (x) => x == data[^(i + 1)]));
		//	}
		//}
	}
}
