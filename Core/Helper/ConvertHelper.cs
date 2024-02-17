namespace Core.Helper
{
    public static class ConvertHelper
    {
        public static int ConvertCode(this string[] code)
        {
			try
			{
                return Convert.ToInt32(string.Join("", code));
			}
			catch (Exception)
			{
                return 0;
			}
        }
    }
}
