using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Recyclable.Collections.Compatibility.List.Benchmarks
{
	public static class Program
	{
		static IConfig BenchmarkConfig { get; } = ManualConfig.Create(DefaultConfig.Instance)
			.WithOptions(ConfigOptions.DisableOptimizationsValidator | ConfigOptions.JoinSummary);

		static void Main()
		{
			BenchmarkRunner.Run(new[]
			{
				//typeof(BinarySearchBenchmarks),
				typeof(BinarySearchBenchmarksBestAndWorstCasesWhenConstrainedRange),
				//typeof(BinarySearchValueLowerThanFirstItemBenchmarks),
				//typeof(BinarySearchValueHigherThanFirstItemBenchmarks),
				//typeof(ConvertAllBenchmarks),
				//typeof(ExistsBenchmarks),
				//typeof(FindBenchmarks),
				//typeof(FindAllBenchmarks),
				//typeof(FindLastBenchmarks),
				//typeof(FindLastIndexBenchmarks)
			}, BenchmarkConfig);
		}
	}
}