namespace Borigran.OneData.Domain.Values
{
    public enum CItemTypes
    {
        Pedestal = 10,  // Тумба
        Garden = 20,    // Цветник
        Stella = 30,    // Стелла
        Tombstone = 40, // Надгробие
        Boder = 50,     // Ограда
        Tip = 60,       // Наконечник
        Bench = 70,     // Скамья
        Vase = 80,      // Ваза
        Lampada = 90    // Лампада
    }

    public static class CItemTypesExtensions
    {
        public static string ToItemName(this CItemTypes itemType)
        {
            switch(itemType)
            {
                case CItemTypes.Pedestal:
                    return "Тумба";
                case CItemTypes.Garden:
                    return "Скамья";
                case CItemTypes.Stella:
                    return "Стелла";
                case CItemTypes.Tombstone:
                    return "Надгробие";
                case CItemTypes.Boder:
                    return "Ограда";
                case CItemTypes.Tip:
                    return "Наконечник";
                case CItemTypes.Bench:
                    return "Скамья";
                case CItemTypes.Vase:
                    return "Ваза";
                case CItemTypes.Lampada:
                    return "Лампада";
            }

            return string.Empty;
        }

        public static bool PositionCanBeChanged(this CItemTypes itemType)
        {
            switch (itemType)
            {
                case CItemTypes.Pedestal:
                case CItemTypes.Garden:
                case CItemTypes.Stella:
                case CItemTypes.Tombstone:
                    return true;
            }

            return false;
        }
    }
}
