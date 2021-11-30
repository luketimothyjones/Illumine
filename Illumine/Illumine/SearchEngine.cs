using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Illumine
{
    internal static class Everything
	{
		public static class StatusCodes
		{
			public const int OK = 0,
							 ERROR_MEMORY = 1,
							 ERROR_IPC = 2,
							 ERROR_REGISTERCLASSEX = 3,
							 ERROR_CREATEWINDOW = 4,
							 ERROR_CREATETHREAD = 5,
							 ERROR_INVALIDINDEX = 6,
							 ERROR_INVALIDCALL = 7;
			
			private static readonly Dictionary<uint, string> errorTranslator = new Dictionary<uint, string>()
			{
				{ OK, "The operation completed successfully." },
				{ ERROR_MEMORY, "Failed to allocate memory for the search query." },
				{ ERROR_IPC, "IPC is not available." },
				{ ERROR_REGISTERCLASSEX, "Failed to register the search query window class." },
				{ ERROR_CREATEWINDOW, "Failed to create the search query window." },
				{ ERROR_CREATETHREAD, "Failed to create the search query thread." },
				{ ERROR_INVALIDINDEX, "Invalid index. The index must be greater or equal to 0 and less than the number of visible results." },
				{ ERROR_INVALIDCALL, "Invalid call." }
			};

			public static string TranslateStatusCode(uint code)
            {
				return errorTranslator[code];
            }
		}

		public static class RequestSettings
		{
			public const int FILE_NAME = 0x00000001,
							 PATH = 0x00000002,
							 FULL_PATH_AND_FILE_NAME = 0x00000004,
							 EXTENSION = 0x00000008,
							 SIZE = 0x00000010,
							 DATE_CREATED = 0x00000020,
							 DATE_MODIFIED = 0x00000040,
							 DATE_ACCESSED = 0x00000080,
							 ATTRIBUTES = 0x00000100,
							 FILE_LIST_FILE_NAME = 0x00000200,
							 RUN_COUNT = 0x00000400,
							 DATE_RUN = 0x00000800,
							 DATE_RECENTLY_CHANGED = 0x00001000,
							 HIGHLIGHTED_FILE_NAME = 0x00002000,
							 HIGHLIGHTED_PATH = 0x00004000,
							 HIGHLIGHTED_FULL_PATH_AND_FILE_NAME = 0x00008000;
		}

		public static class SortSettings
		{
			public const int NAME_ASCENDING = 1,
							 NAME_DESCENDING = 2,
							 PATH_ASCENDING = 3,
							 PATH_DESCENDING = 4,
							 SIZE_ASCENDING = 5,
							 SIZE_DESCENDING = 6,
							 EXTENSION_ASCENDING = 7,
							 EXTENSION_DESCENDING = 8,
							 TYPE_NAME_ASCENDING = 9,
							 TYPE_NAME_DESCENDING = 10,
							 DATE_CREATED_ASCENDING = 11,
							 DATE_CREATED_DESCENDING = 12,
							 DATE_MODIFIED_ASCENDING = 13,
							 DATE_MODIFIED_DESCENDING = 14,
							 ATTRIBUTES_ASCENDING = 15,
							 ATTRIBUTES_DESCENDING = 16,
							 FILE_LIST_FILENAME_ASCENDING = 17,
							 FILE_LIST_FILENAME_DESCENDING = 18,
							 RUN_COUNT_ASCENDING = 19,
							 RUN_COUNT_DESCENDING = 20,
							 DATE_RECENTLY_CHANGED_ASCENDING = 21,
							 DATE_RECENTLY_CHANGED_DESCENDING = 22,
							 DATE_ACCESSED_ASCENDING = 23,
							 DATE_ACCESSED_DESCENDING = 24,
							 DATE_RUN_ASCENDING = 25,
							 DATE_RUN_DESCENDING = 26;
		}

		#region Everything DLL Imports

		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern UInt32 Everything_SetSearchW(string lpSearchString);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetMatchPath(bool bEnable);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetMatchCase(bool bEnable);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetMatchWholeWord(bool bEnable);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetRegex(bool bEnable);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetMax(UInt32 dwMax);
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetOffset(UInt32 dwOffset);

		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetMatchPath();
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetMatchCase();
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetMatchWholeWord();
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetRegex();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetMax();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetOffset();
		[DllImport("Everything64.dll")]
		public static extern IntPtr Everything_GetSearchW();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetLastError();

		[DllImport("Everything64.dll")]
		public static extern bool Everything_QueryW(bool bWait);

		[DllImport("Everything64.dll")]
		public static extern void Everything_SortResultsByPath();

		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetNumFileResults();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetNumFolderResults();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetNumResults();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetTotFileResults();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetTotFolderResults();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetTotResults();
		[DllImport("Everything64.dll")]
		public static extern bool Everything_IsVolumeResult(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_IsFolderResult(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_IsFileResult(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern IntPtr Everything_GetResultPath(UInt32 nIndex);
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern void Everything_GetResultFullPathName(UInt32 nIndex, StringBuilder lpString, UInt32 nMaxCount);
		[DllImport("Everything64.dll")]
		public static extern void Everything_Reset();

		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultFileName(UInt32 nIndex);

		// Everything 1.4
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetSort(UInt32 dwSortType);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetSort();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetResultListSort();
		[DllImport("Everything64.dll")]
		public static extern void Everything_SetRequestFlags(UInt32 dwRequestFlags);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetRequestFlags();
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetResultListRequestFlags();
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultExtension(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultSize(UInt32 nIndex, out long lpFileSize);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultDateCreated(UInt32 nIndex, out long lpFileTime);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultDateModified(UInt32 nIndex, out long lpFileTime);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultDateAccessed(UInt32 nIndex, out long lpFileTime);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetResultAttributes(UInt32 nIndex);
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultFileListFileName(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetResultRunCount(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultDateRun(UInt32 nIndex, out long lpFileTime);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_GetResultDateRecentlyChanged(UInt32 nIndex, out long lpFileTime);
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedFileName(UInt32 nIndex);
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedPath(UInt32 nIndex);
		[DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr Everything_GetResultHighlightedFullPathAndFileName(UInt32 nIndex);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_GetRunCountFromFileName(string lpFileName);
		[DllImport("Everything64.dll")]
		public static extern bool Everything_SetRunCountFromFileName(string lpFileName, UInt32 dwRunCount);
		[DllImport("Everything64.dll")]
		public static extern UInt32 Everything_IncRunCountFromFileName(string lpFileName);

        #endregion
    }

    public class SearchResult : IEquatable<SearchResult>, IComparable<SearchResult>
    {
		public string fileName;
		public string filePath;
		public string extension;
		public int _distance = int.MinValue;

        public int CompareTo(SearchResult other)
        {
			if (_distance == int.MinValue)
            {
				Console.WriteLine("WARN: Comparison run against SearchResult before distance was calculated");
				return fileName.Length == other.fileName.Length ? 0 : (fileName.Length < other.fileName.Length ? -1 : 1);
			}
			
			return _distance == other._distance ? 0 : (_distance < other._distance ? -1 : 1);
        }

        public bool Equals(SearchResult other)
        {
			if (_distance == int.MinValue)
			{
				Console.WriteLine("WARN: Comparison run against SearchResult before distance was calculated");
				return fileName == other.fileName;
			}

			return _distance == other._distance;
        }
    }

	public static class ResultSortEngine
    {
		public static void Sort(string search, List<SearchResult> toSort)
        {
			foreach (SearchResult item in toSort)
			{
				item._distance = DamerauLevenshteinDistance(search, item.fileName);
			}

			toSort.Sort();
        }

		private static void Swap<T>(ref T arg1, ref T arg2)
		{
			T temp = arg1;
			arg1 = arg2;
			arg2 = temp;
		}

		/// <summary>
		/// Computes the Damerau-Levenshtein Distance between two strings.
		/// Includes an optional threshhold which can be used to indicate the maximum allowable distance.
		/// </summary>
		/// <param name="source">The first string to be compared</param>
		/// <param name="target">The second string to be compared</param>
		/// <returns>Int.MaxValue if threshhold exceeded; otherwise the Damerau-Leveshteim distance between the strings</returns>
		private static int DamerauLevenshteinDistance(string source, string target)
		{
			/// <param name="threshold">Maximum allowable distance</param>
			int threshold = int.MaxValue;

			int length1 = source.Length;
			int length2 = target.Length;

			// Return trivial case - difference in string lengths exceeds threshhold
			if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

			// Ensure arrays [i] / length1 use shorter length 
			if (length1 > length2)
			{
				Swap(ref target, ref source);
				Swap(ref length1, ref length2);
			}

			int maxi = length1;
			int maxj = length2;

			int[] dCurrent = new int[maxi + 1];
			int[] dMinus1 = new int[maxi + 1];
			int[] dMinus2 = new int[maxi + 1];
			int[] dSwap;

			for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

			int jm1 = 0, im1, im2;

			for (int j = 1; j <= maxj; j++)
			{
				// Rotate
				dSwap = dMinus2;
				dMinus2 = dMinus1;
				dMinus1 = dCurrent;
				dCurrent = dSwap;

				// Initialize
				int minDistance = int.MaxValue;
				dCurrent[0] = j;
				im1 = 0;
				im2 = -1;

				for (int i = 1; i <= maxi; i++)
				{
					int cost = source[im1] == target[jm1] ? 0 : 1;

					int del = dCurrent[im1] + 1;
					int ins = dMinus1[i] + 1;
					int sub = dMinus1[im1] + cost;

					//Fastest execution for min value of 3 integers
					int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

					if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
						min = Math.Min(min, dMinus2[im2] + cost);

					dCurrent[i] = min;
					if (min < minDistance) { minDistance = min; }
					im1++;
					im2++;
				}
				jm1++;
				if (minDistance > threshold) { return int.MaxValue; }
			}

			int result = dCurrent[maxi];
			return (result > threshold) ? int.MaxValue : result;
		}
	}

    class SearchEngine
    {
		public uint searchLimit = 300;

		// This gets passed in from EverythingSearchbar.Searchbar so that the SearchInput
		// can manage cancellation
		public CancellationTokenSource cancelationHandler = null;

		public delegate void ResultsCallback(ValueTuple<long, List<SearchResult>> results);

		private static readonly int cacheItemDuration = 10;  // seconds
		private readonly Dictionary<string, ValueTuple<long, List<SearchResult>>> cache;
		private SpinLock everythingSpinLock;

		public SearchEngine()
        {
			cache = new Dictionary<string, ValueTuple<long, List<SearchResult>>> { };
			everythingSpinLock = new SpinLock();
		}

		public async void DoSearch(string searchQuery, ResultsCallback callback)
        {
			try
			{
				await Task.Delay(100, cancelationHandler.Token);
			}
			catch (TaskCanceledException)
            {
				return;
            }

			if (cache.ContainsKey(searchQuery) && (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - cache[searchQuery].Item1) <= cacheItemDuration)
            {
				callback(cache[searchQuery]);
				return;
            }

			(long, List<SearchResult>) results = (DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new List<SearchResult>());
			bool gotLock = false;

			try
			{
				await Task.Run(() =>
				{
					// This is a little redunant given the CancellationTokenSource, but better safe than sorry
					everythingSpinLock.Enter(ref gotLock);

					// Results configuration
					Everything.Everything_SetMax(searchLimit);
					Everything.Everything_SetRequestFlags(Everything.RequestSettings.FILE_NAME | Everything.RequestSettings.PATH);
					Everything.Everything_SetSort(Everything.SortSettings.NAME_ASCENDING);
					Everything.Everything_SetMatchWholeWord(true);

					Everything.Everything_SetSearchW(searchQuery);
					Everything.Everything_QueryW(true);

					string fileName, filePath;
					string[] fileNameParts;
					for (uint i = 0; i < Everything.Everything_GetNumResults(); i++)
					{
						fileName = Marshal.PtrToStringUni(Everything.Everything_GetResultFileName(i));
						filePath = Marshal.PtrToStringUni(Everything.Everything_GetResultPath(i));

						if (fileName == null || filePath == null) {
							// One of the above functions failed for some reason
							uint errorCode = Everything.Everything_GetLastError();
							Console.WriteLine("Error occurred in SearchEngine.DoSearch while calling Everything: '" + Everything.StatusCodes.TranslateStatusCode(errorCode) + "'");
							continue;
						}

						fileNameParts = fileName.Split('.');

						results.Item2.Add(new SearchResult()
						{
							fileName = fileName,
							filePath = filePath.Length > 0 ? filePath + "\\" : "",
                            extension = string.Join(".", fileNameParts, 1, fileNameParts.Length - 1)
						});

						if (cancelationHandler.IsCancellationRequested)
                        {
							break;
                        }
					}

					if (gotLock)
					{
						everythingSpinLock.Exit();
						gotLock = false;
					}

					ResultSortEngine.Sort(searchQuery, results.Item2);

				}, cancelationHandler.Token);
			}

			catch (TaskCanceledException)
			{
				return;
			}

			finally
            {
				// Ensure we don't deadlock
				if (gotLock)
				{
					everythingSpinLock.Exit();
				}
			}

			cache[searchQuery] = results;
			callback(results);
		}

		public async void ClearSearch()
        {
			// Free up memory allocated by Everything for the current search
			await Task.Run(Everything.Everything_Reset);
        }
    }
}
