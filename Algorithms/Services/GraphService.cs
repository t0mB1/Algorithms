using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using SkiaSharp;
using Entry = Microcharts.Entry;

namespace Algorithms.Services
{
    public class GraphService
    {
        public IEnumerable<Entry> GetBestCaseEntries(int minVal, int maxVal, int searchItem)
        {
            List<Entry> entries = new List<Entry>();
            for (int i = minVal; i < maxVal + 1; i++)
            {
                if (i == searchItem)
                {
                    entries.Add(GenerateSearchItemEntry(i));
                }
                else
                {
                    entries.Add(GenerateEntry(i));
                }
            }
            return entries.ToArray();
        }

        public IEnumerable<Entry> GetBestCaseEntries(int minVal, int maxVal)
        {
            List<Entry> entries = new List<Entry>();
            for (int i = minVal; i < maxVal + 1; i++)
            {
                entries.Add(GenerateEntry(i));
            }
            return entries.ToArray();
        }

        public IEnumerable<Entry> GetWostCaseEntriesForSearch(int minVal, int maxVal)
        {
            List<Entry> entries = new List<Entry>();
            for (int i = minVal; i < maxVal + 1; i++)
            {
                if (i == 20)
                {
                    entries.Add(GenerateSearchItemEntry(i));
                }
                else
                {
                    entries.Add(GenerateEntry(i));
                }
            }
            return entries;
        }

        public IEnumerable<Entry> GetWostCaseEntriesForSort(int minVal, int maxVal)
        {
            List<Entry> entries = new List<Entry>();
            for (int i = maxVal; i > minVal-1; i--)
            {
                entries.Add(GenerateEntry(i));
            }
            return entries.ToArray();
        }

        public IEnumerable<Entry> GetRandomEntries(int minVal, int maxVal, int SearchItem)
        {
            IEnumerable<Entry> entries = GetBestCaseEntries(minVal, maxVal, SearchItem);
            return ShuffleList(entries.ToArray());
        }

        public IEnumerable<Entry> GetRandomEntries(int minVal, int maxVal)
        {
            IEnumerable<Entry> entries = GetBestCaseEntries(minVal, maxVal);
            return ShuffleList(entries.ToArray());
        }

        public Entry GenerateEntry(int value)
        {
            return new Entry(value)
            {
                Color = SKColor.Parse("#FF1493")
            };
        }

        public Entry GenerateSearchItemEntry(int value)
        {
            return new Entry(value)
            {
                Color = SKColor.Parse("#0000FF")
            };
        }

        private IEnumerable<Entry> ShuffleList(Entry[] entries)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = entries.Length;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                int k = box[0] % n;
                n--;
                Entry value = entries[k];
                entries[k] = entries[n];
                entries[n] = value;
            }
            return entries;
        }
    }
}
