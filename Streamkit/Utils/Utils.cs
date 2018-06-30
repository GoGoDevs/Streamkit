using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Streamkit.Utils {
    public static class ByteConverter {
        public static byte[] ToBytes(string str) {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ToString(byte[] bytes) {
            return Encoding.UTF8.GetString(bytes);
        }
    }


    public static class Base64 {
        public static string Encode(byte[] bytes) {
            return Convert.ToBase64String(bytes);
        }

        public static string Encode(string str) {
            return Encode(ByteConverter.ToBytes(str));
        }

        public static byte[] DecodeToBytes(string base64) {
            return Convert.FromBase64String(base64);
        }

        public static string DecodeToString(string base64) {
            return ByteConverter.ToString(DecodeToBytes(base64));
        }
    }


    /// <summary>
    /// Class for iterating over a range of dates.
    /// </summary>
    public class DateRange : IEnumerable<DateTime> {
        private DateTime start;
        private DateTime end;

        public DateRange(DateTime start, DateTime end) {
            this.start = start.Date;
            this.end = end.Date;
        }

        public DateTime Start {
            get { return this.start; }
        }

        public DateTime End {
            get { return this.end; }
        }

        public List<DateTime> FindAll(Predicate<DateTime> predicate) {
            List<DateTime> found = new List<DateTime>();
            foreach (DateTime date in this) {
                if (predicate(date)) {
                    found.Add(date);
                }
            }
            return found;
        }

        public IEnumerator<DateTime> GetEnumerator() {
            DateTime currDate = start;
            while (currDate <= end.Date) {
                yield return currDate;
                currDate.AddDays(1);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}