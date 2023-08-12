namespace DatingApi.Extentions
{
    public static class DateTimeExtenstion
    {
        public static int CalculateAge(this DateTime birthdate)
        {
            DateTime today = DateTime.Today; // Today's date

            int age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age)) age--;
            return age;
        }
    }
}
