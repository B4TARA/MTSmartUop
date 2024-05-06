namespace MtSmart.BLL.BusinessModels
{
    public static class TermManager
    {
        public static bool IsEqualMonths(DateOnly date1, DateOnly date2)
        {
            if(date1.Year == date2.Year && date1.Month == date2.Month)
            {
                return true;
            }

            return false;
        }

        public static DateOnly GetMin()
        {
            return GetDate();
        }

        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "Январь";
                case 2: return "Ферваль";
                case 3: return "Март";
                case 4: return "Апрель";
                case 5: return "Май";
                case 6: return "Июнь";
                case 7: return "Июль";
                case 8: return "Август";
                case 9: return "Сентябрь";
                case 10: return "Октябрь";
                case 11: return "Ноябрь";
                case 12: return "Декабрь";
            }

            return "Январь";
        }

        public static DateOnly GetDate()
        {
            return DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
