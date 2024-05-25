namespace UmbracoBlazorHeadless.Frontend.Extensions
{
    public static class UmbracoExtensions
    {
        private const string ContentTypeModelSuffix = "ContentResponseModel";
        private const string BlockContentTypeModelSuffix = "ElementModel";

        public static string GetContentTypeAlias(this Type type)
        {
            var name = type.Name
                .TrimEnd(ContentTypeModelSuffix)
                .FirstToLower();

            return name;
        }


        public static string GetContentTypeName(this Type type)
        {
            var name = type.Name
                .TrimEnd(ContentTypeModelSuffix);

            return name;
        }

        public static string GetContentTypeName(this string type)
        {
            var name = type
                .TrimEnd(ContentTypeModelSuffix);

            return name.Split(".").Last();
        }

        public static string GetBlockContentTypeName(this Type type)
        {
            var name = type.Name
                .TrimEnd(BlockContentTypeModelSuffix);

            return name;
        }

        public static bool BoolIsTrue(this IDictionary<string, object> dictionary, string key) {
            if (!dictionary.ContainsKey(key))
            {
                return false;
            }

            var value = dictionary[key];
            if (value == null)
            {
                return false;
            }

            bool.TryParse(value.ToString(), out bool returnValue);

            return returnValue;
        }
    }
}
