namespace Viasoft.Analytics.UserBehaviour.Domain.Consts
{
    public static class ElasticSearchUtils
    {
        public static string GetCamelCaseField(string fieldName)
        {
            return char.ToLowerInvariant(fieldName[0]) + fieldName.Substring(1);
        }

        public static string GetPascalCaseField(string fieldName)
        {
            return char.ToUpperInvariant(fieldName[0]) + fieldName.Substring(1);
        }

        public static string GetEsFieldKeyword(string fieldName)
        {
            return $"{GetCamelCaseField(fieldName)}.keyword";
        }

        public static string GetEsFieldRaw(string fieldName)
        {
            return $"{GetCamelCaseField(fieldName)}.raw";
        }
    }
}