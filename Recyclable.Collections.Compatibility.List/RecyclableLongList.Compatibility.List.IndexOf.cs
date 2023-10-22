// Ignore Spelling: zzz

namespace Recyclable.Collections
{
#pragma warning disable IDE1006 // This intentional so that it shows at the end of IntelliSense list
	public static class zzzRecyclableLongListCompatibilityListIndexOf
#pragma warning restore IDE1006 // Naming Styles
	{
		public static int IndexOf<T>(this RecyclableLongList<T> list, T item, int index)
			//=> list._count != 0
			//	? Array.IndexOf(list._memoryBlock, item, index, list._count - index)
			//	: RecyclableDefaults.ItemNotFoundIndex;
			=> throw new NotImplementedException();

		public static int IndexOf<T>(this RecyclableLongList<T> list, T item, int index, int count)
			//=> list._count != 0
			//	? Array.IndexOf(list._memoryBlock, item, index, count)
			//	: RecyclableDefaults.ItemNotFoundIndex;
			=> throw new NotImplementedException();

		public static int LastIndexOf<T>(this RecyclableLongList<T> list, T item)
			//=> list._count != 0
			//	? Array.LastIndexOf(list._memoryBlock, item, list._count - 1)
			//	: RecyclableDefaults.ItemNotFoundIndex;
			=> throw new NotImplementedException();

		public static int LastIndexOf<T>(this RecyclableLongList<T> list, T item, int index)
			//=> list._count != 0
			//	? Array.LastIndexOf(list._memoryBlock, item, index)
			//	: RecyclableDefaults.ItemNotFoundIndex;
			=> throw new NotImplementedException();

		public static int LastIndexOf<T>(this RecyclableLongList<T> list, T item, int index, int count)
			//=> list._count != 0
			//	? Array.LastIndexOf(list._memoryBlock, item, index, count)
			//	: RecyclableDefaults.ItemNotFoundIndex;
			=> throw new NotImplementedException();
	}
}
