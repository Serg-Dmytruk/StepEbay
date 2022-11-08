namespace StepEbay.Common.Constans
{
    public static class ProductCategoryConstant
    {
        public static readonly string TELEPHONE = "Телефоны";
        public static readonly string CLOTH = "Одяг";
        public static readonly string BUATY = "Краса";
        public static readonly string SPORT = "Спорт";
        public static readonly string TOY = "Іграшки";
    }

    public static class ProductCategoryIdConstant
    {
        public static string TELEPHONE = "1";
        public static string CLOTH = "2";
        public static string BUATY = "4";
        public static string SPORT = "3";
        public static string TOY = "5";
        public static void setConstants(Dictionary<string, int> categorys)
        {
            ProductCategoryIdConstant.TELEPHONE = categorys[ProductCategoryConstant.TELEPHONE].ToString();
            ProductCategoryIdConstant.CLOTH = categorys[ProductCategoryConstant.CLOTH].ToString();
            ProductCategoryIdConstant.BUATY = categorys[ProductCategoryConstant.BUATY].ToString();
            ProductCategoryIdConstant.SPORT = categorys[ProductCategoryConstant.SPORT].ToString();
            ProductCategoryIdConstant.TOY = categorys[ProductCategoryConstant.TOY].ToString();
        }
    }

    public static class ProductListConstant
    {
        public static readonly int MAXONPAGE = 10;
    }
}
