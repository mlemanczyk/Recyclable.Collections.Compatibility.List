using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public class ConvertAllBenchmarks : RecyclableCollectionsCompatibilityListBenchmarks
	{
		public override string MethodNameSuffix => "_ConvertAll";

		public void Array_ConvertAll()
		{
			var data = TestObjects;
			var list = TestObjects;
			var dataCount = TestObjectCountForSlowMethods;
			DoNothing.With(Array.ConvertAll(list, static (item) => (long)item));
		}

		public void List_ConvertAll()
		{
			var data = TestObjects;
			var list = TestObjectsAsList;
			var dataCount = TestObjectCountForSlowMethods;
			DoNothing.With(list.ConvertAll(static (item) => (long)item));
		}

		public void PooledList_ConvertAll()
		{
			var data = TestObjects;
			var list = TestObjectsAsPooledList;
			var dataCount = TestObjectCountForSlowMethods;
			using var converted = list.ConvertAll(static (item) => (long)item);
			DoNothing.With(converted);
		}

		public void RecyclableList_ConvertAll()
		{
			var data = TestObjects;
			var list = TestObjectsAsRecyclableList;
			var dataCount = TestObjectCountForSlowMethods;
			using RecyclableList<long> converted = list.ConvertAll(static (item) => (long)item);
			DoNothing.With(converted);
		}

		// public void RecyclableLongList_ConvertAll()
		// {
		// 	var data = TestObjects;
		// 	var list = TestObjectsAsRecyclableLongList;
		// 	var dataCount = TestObjectCountForSlowMethods;
		// 	using RecyclableList<long> converted = list.ConvertAll(static (item) => (long)item);
		// 	DoNothing(converted);
		// }
	}
}
