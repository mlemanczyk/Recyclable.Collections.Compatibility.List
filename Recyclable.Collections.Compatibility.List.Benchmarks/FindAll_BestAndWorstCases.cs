using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class FindAllBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_FindAll_BestAndWorstCases";

		public void Array_FindAll_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(Array.FindAll(list, (item) => item == data[i]));
				DoNothing.With(Array.FindAll(list, (item) => item == data[^(i + 1)]));
			}
		}

		public void List_FindAll_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				DoNothing.With(list.FindAll((item) => item == data[i]));
				DoNothing.With(list.FindAll((item) => item == data[^(i + 1)]));
			}
		}

		public void PooledList_FindAll_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsPooledList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				var temp = list.FindAll((item) => item == data[i]);
				DoNothing.With(temp);
				temp.Dispose();

				temp = list.FindAll((item) => item == data[^(i + 1)]);
				DoNothing.With(temp);
				temp.Dispose();
			}
		}

		public void RecyclableList_FindAll_BestAndWorstCases()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			for (var i = 0; i < dataCount; i++)
			{
				var temp = list.FindAll((item) => item == data[i]);
				DoNothing.With(temp);
				temp.Dispose();

				temp = list.FindAll((item) => item == data[^(i + 1)]);
				DoNothing.With(temp);
				temp.Dispose();
			}
		}

		//public void RecyclableLongList_FindAll_BestAndWorstCases()
		//{
		//	var data = TestObjects;
		//	var list = TestObjectsAsRecyclableLongList;
		//	var dataCount = TestObjectCountForSlowMethods;
		//	for (var i = 0; i < dataCount; i++)
		//	{
		//		var temp = list.FindAll((item) => item == data[i]);
		//		DoNothing.With(temp);
		//		temp.Dispose();

		//		temp = list.FindAll((item) => item == data[^(i + 1)]);
		//		DoNothing.With(temp);
		//		temp.Dispose();
		//	}
		//}
	}
}
