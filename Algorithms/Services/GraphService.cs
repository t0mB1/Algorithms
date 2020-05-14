using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SkiaSharp;
using Entry = Microcharts.Entry;

namespace Algorithms.Services
{
    public class GraphService
    {
        public List<Entry> GetBestCaseEntries(int minVal, int maxVal, int searchItem)
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
            return entries;
        }

        public List<Entry> GetWostCaseEntries(int minVal, int maxVal)
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

        public List<Entry> GetRandomEntries(int minVal, int maxVal, int SearchItem)
        {
            List<Entry> entries = GetBestCaseEntries(minVal, maxVal, SearchItem);
            return ShuffleList(entries);
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


        private List<Entry> ShuffleList(List<Entry> entries)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = entries.Count;
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
