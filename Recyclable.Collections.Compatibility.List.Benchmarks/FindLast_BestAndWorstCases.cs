using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class FindLastBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_FindLast_BestAndWorstCases";

		public void Array_FindLast_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.FindLast(list, (x) => x == data[i]));
				DoNothing.With(Array.FindLast(list, (x) => x == data[^(i + 1)]));
			}
		}

		public void List_FindLast_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.FindLast((x) => x == data[i]));
				DoNothing.With(list.FindLast((x) => x == data[^(i + 1)]));
			}
		}

		public void RecyclableList_FindLast_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.FindLast((x) => x == data[i]));
				DoNothing.With(list.FindLast((x) => x == data[^(i + 1)]));
			}
		}

		//public void RecyclableLongList_FindLast_BestAndWorstCases()
		//{
		//	var data = TestObjects;
		//	var list = TestObjectsAsRecyclableLongList;
		//	var dataCount = TestObjectCountForSlowMethods;
		//	for (var i = 0; i < dataCount; i++)
		//	{
		//		DoNothing.With(list.FindLast((x) => x == data[i]));
		//		DoNothing.With(list.FindLast((x) => x == data[^(i + 1)]));
		//	}
		//}
	}
}
