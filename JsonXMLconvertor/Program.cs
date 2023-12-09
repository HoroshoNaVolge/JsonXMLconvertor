using System.Text.Json;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        // Входной JSON
        string json = "{\"name\": \"Инакентий\", \"age\": 25, \"city\": \"Москва\"}";

        // Конвертация JSON в XML
        string xml = ConvertJsonToXml(json);

        // Вывод результата
        Console.WriteLine("XML Result:");
        Console.WriteLine(xml);
    }

    static string ConvertJsonToXml(string jsonString)
    {
        // Разбор JSON с использованием JsonDocument
        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

        // Создание корневого элемента XML
        XElement rootElement = new XElement("root");

        // Рекурсивное преобразование JSON в XML
        ConvertJsonToXmlRecursive(jsonDocument.RootElement, rootElement);

        // Возвращение результата в виде строки XML
        return rootElement.ToString();
    }

    static void ConvertJsonToXmlRecursive(JsonElement jsonElement, XElement parentElement)
    {
        foreach (JsonProperty property in jsonElement.EnumerateObject())
        {
            // Обработка значений различных типов
            switch (property.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    XElement childElement = new XElement(property.Name);
                    ConvertJsonToXmlRecursive(property.Value, childElement);
                    parentElement.Add(childElement);
                    break;

                case JsonValueKind.Array:
                    XElement arrayElement = new XElement(property.Name);
                    foreach (JsonElement arrayItem in property.Value.EnumerateArray())
                    {
                        XElement itemElement = new XElement("item");
                        ConvertJsonToXmlRecursive(arrayItem, itemElement);
                        arrayElement.Add(itemElement);
                    }
                    parentElement.Add(arrayElement);
                    break;

                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    XElement valueElement = new XElement(property.Name, property.Value.ToString());
                    parentElement.Add(valueElement);
                    break;

                case JsonValueKind.Null:
                    // Обработка случая, когда значение равно null
                    XElement nullElement = new XElement(property.Name, "null");
                    parentElement.Add(nullElement);
                    break;
            }
        }
    }
}