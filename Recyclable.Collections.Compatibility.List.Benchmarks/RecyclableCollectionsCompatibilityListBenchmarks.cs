using BenchmarkDotNet.Attributes;
using Collections.Benchmarks.Core;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{

	public partial class RecyclableCollectionsCompatibilityListBenchmarks : DataDrivenBenchmarksBase<CollectionsBenchmarksSource>
	{
		[Params(CollectionsBenchmarksSource.Array)]
		public override CollectionsBenchmarksSource BaseDataType { get => base.BaseDataType; set => base.BaseDataType = value; }

		[Params(
			//CollectionsBenchmarksSource.List,
			//CollectionsBenchmarksSource.PooledList,
			//CollectionsBenchmarksSource.RecyclableList,
			CollectionsBenchmarksSource.RecyclableLongList
		)]
		public override CollectionsBenchmarksSource DataType { get => base.DataType; set => base.DataType = value; }

		//[Params(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 32, 40, RecyclableDefaults.MinPooledArrayLength - 1, RecyclableDefaults.MinPooledArrayLength, 40, 50, 60, 70, 80, 90, 100, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, RecyclableDefaults.MinItemsCountForParallelization)]
		[Params(RecyclableDefaults.MinPooledArrayLength, RecyclableDefaults.MinItemsCountForParallelization)]
		public override int TestObjectCount { get => base.TestObjectCount; set => base.TestObjectCount = value; }
	}
}
