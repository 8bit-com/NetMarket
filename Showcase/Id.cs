namespace ShowcaseLib
{
    public enum Type { Product, Showcase}

    public struct EditProduct
    {
        public string name;
        public int volume;
        public int price;
        public int firstIndex;
        public int secondIndex;
    }

    public struct IndexMenu
    {
        public int i1;
        public int i2;
    }

    public struct EditShowcase
    {
        public string name;
        public int volume;
        public int index;
    }

    public static class Id
    {
        private static int IdProduct  = 1;
        private static int IdShowcase = 1;

        public static int GetId(Type type)
        {
            switch (type)
            {
                case Type.Product:
                    return IdProduct++;

                case Type.Showcase:
                    return IdShowcase++;

                default:
                    return 0;
            }
        }
    }
}
