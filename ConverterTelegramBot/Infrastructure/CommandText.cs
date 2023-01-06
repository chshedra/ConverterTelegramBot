namespace ConverterTelegramBot.Infrastructure;

public static class CommandText
{
    public const string UnsupportedFileMessage =
        "Неподдерживаемый формат файла. Бот умеет конвертировать текст и изображения";

    public const string ConvertMessage = "Конвертировать в PDF";

    public const string SeparateMessage = "Разделить PDF";

    public const string CommandChoiceMessage = "Выберите необходимое действие";

    public const string SeparateFileMessage = "Введите диапазон страниц";
}
