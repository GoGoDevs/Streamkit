using System;
using System.Collections;
using System.Collections.Generic;

namespace Streamkit.Utils {
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