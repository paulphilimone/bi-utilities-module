using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace mz.betainteractive.utilities.module.General {
    public class DateUtilities {
        public static DayOfWeek FirstDayOfWeek = /*DayOfWeek.Monday; */Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        private List<WeekDateBounds> _weeks;
        private List<MonthDateBounds> _months;
        private List<QuarterDateBounds> _quarters;
        private List<HalfDateBounds> _halfs;

        public List<WeekDateBounds> Weeks { get { return _weeks; } }
        public List<MonthDateBounds> Months { get { return _months; } }
        public List<QuarterDateBounds> Quarters { get { return _quarters; } }
        public List<HalfDateBounds> Halfs { get { return _halfs; } }

        public DateUtilities(int dateYear) {
            this._weeks = GenerateWeeksOfYear(dateYear);
            this._months = GenerateMonthsOfYear(dateYear);
            this._quarters = GenerateQuartersOfYear(dateYear);
            this._halfs = GenerateHalfsOfYear(dateYear);
        }

        public DateUtilities(DateTime date) : this(date.Year) { 

        }

        public WeekDateBounds GetWeekBounds(DateTime date) {
            
            foreach (var dateBounds in Weeks) {
                if (dateBounds.Contains(date)) {
                    return dateBounds;
                }
            }

            return null;
        }

        public MonthDateBounds GetMonthBounds(DateTime date) {

            foreach (var dateBounds in Months) {
                if (dateBounds.Contains(date)) {
                    return dateBounds;
                }
            }

            return null;
        }

        public QuarterDateBounds GetQuarterBounds(DateTime date) {

            foreach (var dateBounds in Quarters) {
                if (dateBounds.Contains(date)) {
                    return dateBounds;
                }
            }

            return null;
        }

        public HalfDateBounds GetHalfBounds(DateTime date) {
            
            foreach (var dateBounds in Halfs) {
                if (dateBounds.Contains(date)) {
                    return dateBounds;
                }
            }

            return null;
        }

        private int GetWeekDay(DayOfWeek day) {
            switch (day) {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
            }
            return 0;
        }

        private int[] GetIncreaseDay(DayOfWeek firstDayOfWeek) {

            int first = GetWeekDay(firstDayOfWeek);

            //Console.WriteLine("First: " + first);

            int[] inc = new int[7];
            int[] v1 = { 6, 5, 4, 3, 2, 1, 0 };

            //first fill
            for (int i = 0, x = 6 - (first - 2); i < first - 1; i++, x++) {
                inc[i] = v1[x];
            }

            //second fill
            for (int i = first - 1, x = 0; i < 7; i++, x++) {
                inc[i] = v1[x];
            }

            return inc;
        }

        private List<WeekDateBounds> GenerateWeeksOfYear(int year) {
            return GenerateWeeksOfYear(year, FirstDayOfWeek);
        }
        
        /*
         * Retorna uma arrayList de SemanaAno, representando todas o
         * intervalo de dias de todas as semanas do ano
         */
        private List<WeekDateBounds> GenerateWeeksOfYear(int year, DayOfWeek firstDayOfWeek) {

            List<WeekDateBounds> arr = new List<WeekDateBounds>();

            DateTime date = new DateTime(year, 1, 1).Date;
            int yr = date.Year;
            DayOfWeek weekDay = date.DayOfWeek;

            int week = 1;
            int daysToIncrease = 0;

            int[] daysIncs = GetIncreaseDay(firstDayOfWeek);

            switch (weekDay) {
                case DayOfWeek.Monday: daysToIncrease = daysIncs[0]; break;
                case DayOfWeek.Tuesday: daysToIncrease = daysIncs[1]; break;
                case DayOfWeek.Wednesday: daysToIncrease = daysIncs[2]; break;
                case DayOfWeek.Thursday: daysToIncrease = daysIncs[3]; break;
                case DayOfWeek.Friday: daysToIncrease = daysIncs[4]; break;
                case DayOfWeek.Saturday: daysToIncrease = daysIncs[5]; break;
                case DayOfWeek.Sunday: daysToIncrease = daysIncs[6]; break;
            }

            while (date.Year != yr + 1) {
                WeekDateBounds semana = new WeekDateBounds();

                semana.Order = week;
                //semana.Ano = date.Year;
                semana.SetFirstDate(date.Date);
                date = date.AddDays(daysToIncrease);
                semana.SetLastDate(date.Date);

                date = date.AddDays(1);

                if (date.Year == yr + 1) {
                    semana.SetLastDate(new DateTime(year, 12, 31));
                }

                arr.Add(semana);

                daysToIncrease = 6;
                week++;
            }

            return arr;
        }

        private List<MonthDateBounds> GenerateMonthsOfYear(int year) {

            List<MonthDateBounds> arr = new List<MonthDateBounds>();

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            for (int mes = 1; mes <= 12; mes++) {

                int lastday = calendar.GetDaysInMonth(year, mes);
                DateTime firstDayOfMonth = new DateTime(year, mes, 1);
                DateTime lastDayOfMonth = new DateTime(year, mes, lastday);

                MonthDateBounds mesAno = new MonthDateBounds();

                mesAno.Order = mes;
                mesAno.Month = mes;
                mesAno.Year = year;
                mesAno.SetFirstDate(firstDayOfMonth.Date);
                mesAno.SetLastDate(lastDayOfMonth.Date);

                arr.Add(mesAno);
            }

            return arr;
        }

        private List<QuarterDateBounds> GenerateQuartersOfYear(int year) {

            List<QuarterDateBounds> arr = new List<QuarterDateBounds>();

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            int order = 1;
            for (int mes = 0; mes <= 11; mes+=3) {

                MonthDateBounds mes1 = this.Months[mes];
                MonthDateBounds mes2 = this.Months[mes + 1];
                MonthDateBounds mes3 = this.Months[mes + 2];

                QuarterDateBounds quarter = new QuarterDateBounds();
                
                quarter.Order = order;
                quarter.Month1 = mes1.Month;
                quarter.Month2 = mes2.Month;
                quarter.Month3 = mes3.Month;
                quarter.Year = mes1.Year;
                quarter.SetFirstDate(mes1.First);
                quarter.SetLastDate(mes3.Last);

                arr.Add(quarter);

                order++;
            }

            return arr;
        }

        private List<HalfDateBounds> GenerateHalfsOfYear(int year) {

            List<HalfDateBounds> arr = new List<HalfDateBounds>();

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;

            int order = 1;
            for (int mes = 0; mes <= 11; mes += 6) {

                MonthDateBounds mes1 = this.Months[mes];
                MonthDateBounds mes2 = this.Months[mes + 1];
                MonthDateBounds mes3 = this.Months[mes + 2];
                MonthDateBounds mes4 = this.Months[mes + 3];
                MonthDateBounds mes5 = this.Months[mes + 4];
                MonthDateBounds mes6 = this.Months[mes + 5];

                HalfDateBounds half = new HalfDateBounds();

                half.Order = order;
                
                half.Month1 = mes1.Month;
                half.Month2 = mes2.Month;
                half.Month3 = mes3.Month;
                half.Month4 = mes4.Month;
                half.Month5 = mes5.Month;
                half.Month6 = mes6.Month;

                half.Year = mes1.Year;
                half.SetFirstDate(mes1.First);
                half.SetLastDate(mes6.Last);

                arr.Add(half);

                order++;
            }

            return arr;
        }
                
        public List<DateBounds> GetListOfWeekBounds() {
            return _weeks.ToList<DateBounds>();
        }

        public List<DateBounds> GetListOfMonthBounds() {
            return _months.ToList<DateBounds>();
        }

        public List<DateBounds> GetListOfQuarterBounds() {
            return _quarters.ToList<DateBounds>();
        }
        
        public List<DateBounds> GetListOfHalfBounds() {
            return _halfs.ToList<DateBounds>();
        }


        //statics
        public static int GetMonthDays(DateTime date) {
            CultureInfo ci = new CultureInfo("pt-PT");
            Calendar calendar = ci.Calendar;
            return calendar.GetDaysInMonth(date.Year, date.Month);
        }

        //Internationalization
        public static String GetPeriodo(DateBounds bounds) {
            CultureInfo ci = new CultureInfo("pt-PT");
            String dataInicial = bounds.First.ToString("dd MMMM", ci);
            String dataFinal = bounds.Last.ToString("dd MMMM", ci);

            return dataInicial + " á " + dataFinal;
        }

        public static String GetPeriodoShort(DateBounds bounds) {
            CultureInfo ci = new CultureInfo("pt-PT");
            String dataInicial = bounds.First.ToString("dd MMM", ci);
            String dataFinal = bounds.Last.ToString("dd MMM", ci);

            return dataInicial + " á " + dataFinal;
        }

        public static DateBounds GetMonthDateBound(DateTime startDate) {
            CultureInfo ci = new CultureInfo("pt-PT");

            int days = DateUtilities.GetMonthDays(startDate);            
            DateTime endDate = startDate.AddDays(days - 1);

            int monthNumber = endDate.Month;
            string monthName = endDate.ToString("MMMM", ci);

            var dateBound = new DateBounds();
            dateBound.SetDateBounds(monthNumber, endDate.Year, monthName, startDate, endDate);

            return dateBound;
        }

        public static List<DateBounds> GetMonthsList(DateTime startDate, int nrYears) {
            CultureInfo ci = new CultureInfo("pt-PT");
            List<DateBounds> bounds = new List<DateBounds>();

            int days = DateUtilities.GetMonthDays(startDate);
            DateTime endDate = startDate.AddDays(days - 1);
            int monthNumber = endDate.Month;
            string monthName = "";

            if (monthNumber > 1) {  //> Jan                
                while (monthNumber-- > 1) {
                    endDate = startDate.AddDays(-1);
                    days = DateUtilities.GetMonthDays(endDate.AddDays(-1 * (DateUtilities.GetMonthDays(endDate))));
                    startDate = endDate.AddDays(-1 * days + 1);
                    //Console.WriteLine("in: "+inDate+", en: "+ltDate + ", mth: "+monthNumber);                    
                }
            }

            do {
                while (monthNumber < 12) {
                    days = DateUtilities.GetMonthDays(startDate);
                    endDate = startDate.AddDays(days - 1);

                    monthNumber = endDate.Month;
                    monthName = StringUtilities.Capitalize(endDate.ToString("MMMM", ci));

                    var dateBound = new DateBounds();
                    dateBound.SetDateBounds(monthNumber, endDate.Year, monthName, startDate, endDate);

                    bounds.Add(dateBound);

                    startDate = endDate.AddDays(1);
                }
                monthNumber = 0;
            } while (--nrYears > 0);


            return bounds;
        }
    }
    
    public class DateBounds {            
        public int Order { get; set; }
        public int Year { get; set; }
        private string _name;
        private DateTime _first;
        private DateTime _last;

        public string Name { get { return _name; } }
        public DateTime First { get { return _first; } }
        public DateTime Last { get { return _last; } }
        public int Days { get { return (int) (_last - _first).TotalDays+1; } }

        public void SetDateBounds(System.DateTime first, System.DateTime last) {
            this._first = new DateTime(first.Year, first.Month, first.Day);
            this._last = new DateTime(last.Year, last.Month, last.Day, 23, 59, 59);
        }

        public void SetDateBounds(int order, int year, string name, System.DateTime first, System.DateTime last) {
            this._first = new DateTime(first.Year, first.Month, first.Day);
            this._last = new DateTime(last.Year, last.Month, last.Day, 23, 59, 59);
            this._name = name;
            this.Order = order;
            this.Year = year;
        }

        public void SetFirstDate(System.DateTime first) {
            this._first = new DateTime(first.Year, first.Month, first.Day);            
        }

        public void SetLastDate(System.DateTime last) {            
            this._last = new DateTime(last.Year, last.Month, last.Day, 23, 59, 59);
        }

        public void SetName(string name) { 
            this._name = name;
        }
        
        public bool Contains(DateTime date) {
            return date >= this.First && date <= this.Last;
        }

        public override string ToString() {
            return Order+": " + First.ToShortDateString()+" -> " +Last.ToShortDateString();
        }
    }

    public class WeekDateBounds : DateBounds {
        public int Year { get; set; }        
    }

    public class MonthDateBounds : DateBounds {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class QuarterDateBounds : DateBounds {
        public int Year { get; set; }
        public int Month1 { get; set; }
        public int Month2 { get; set; }
        public int Month3 { get; set; }
    }

    public class HalfDateBounds : DateBounds {
        public int Year { get; set; }
        public int Month1 { get; set; }
        public int Month2 { get; set; }
        public int Month3 { get; set; }
        public int Month4 { get; set; }
        public int Month5 { get; set; }
        public int Month6 { get; set; }
    }

}
