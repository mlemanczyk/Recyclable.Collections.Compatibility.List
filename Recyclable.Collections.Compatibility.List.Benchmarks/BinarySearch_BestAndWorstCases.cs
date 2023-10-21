﻿using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Benchmarks
{
	public class BinarySearchBenchmarks<T> : CollectionsBenchmarksBase<T>
	{
		public void Array_BinarySearch_BestAndWorstCases()
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

		public void List_BinarySearch_BestAndWorstCases()
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

		public void PooledList_BinarySearch_BestAndWorstCases()
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

		public void RecyclableList_BinarySearch_BestAndWorstCases()
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

		// public void RecyclableLongList_BinarySearch_BestAndWorstCases()
		// {
		// 	var data = TestObjects;
		// 	var list = TestObjectsAsRecyclableLongList;
		// 	var dataCount = TestObjectCountForSlowMethods;
		// 	for (var i = 0; i < dataCount; i++)
		// 	{
		// 		DoNothing(list.BinarySearch(data[i]));
		// 		DoNothing(list.BinarySearch(data[^(i + 1)]));
		// 	}
		// }
	}
}
