using System.Text.RegularExpressions;

namespace SparkChange.Resources.Validators
{
    public static class Helpers
    {
        public static bool IsGuid(Guid guid)
        {

            var regex = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";

            if (guid != Guid.Empty && Regex.IsMatch(guid.ToString(), regex))
            {
                return true;
            }

            return false;
        }
    }
}
