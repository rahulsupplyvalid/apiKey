namespace masterapi.Helper
{
    public class RandomCode
    {
        // Generate country code as "2-letter reference + DDMMYYYY"
        public static string GenerateCode(string start, DateTime? staticdate = null)
        {
            var random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            DateTime date = staticdate ?? DateTime.UtcNow;
            //string code = $"{start}00{randomNumber}";
            string code = $"{start.ToUpper()}{date.Year}{date.Month:D2}{date.Day:D2}" +
                $"{date.Hour:D2}{date.Minute:D2}{date.Second:D2}{randomNumber}";
            return code;
        }
    }
}
