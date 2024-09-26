using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);
string dashes = "---------------------------------";
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"Sales Summary{Environment.NewLine}");
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{dashes}{Environment.NewLine}");

WriteSummary(salesFiles);
void WriteSummary(IEnumerable<string> saleFiles)
{
    File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"Total Sales: {salesTotal}{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"Details:{Environment.NewLine}");

    foreach (var file in saleFiles)
    {
        var dataJson = File.ReadAllText(file);
        SalesTotal? data = JsonConvert.DeserializeObject<SalesTotal>(dataJson);
        FileInfo info = new(file);
        File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{info.Name}: {data?.Total}{Environment.NewLine}");
    }
}

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}
double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}
record SalesData(double Total);
