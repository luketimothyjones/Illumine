using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Illumine
{
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
