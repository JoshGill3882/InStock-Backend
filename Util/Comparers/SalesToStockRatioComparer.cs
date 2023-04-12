namespace instock_server_application.Util.Comparers;

public class SalesToStockRatioComparer : IComparer<string>
{
    public int Compare(string ratio1, string ratio2)
    {
        // Extract sales and stock values from the ratio strings
        string[] ratio1Values = ratio1.Split(':');
        int ratio1Sales = int.Parse(ratio1Values[0]);
        int ratio1Stock = int.Parse(ratio1Values[1]);
        string[] ratio2Values = ratio2.Split(':');
        int ratio2Sales = int.Parse(ratio2Values[0]);
        int ratio2Stock = int.Parse(ratio2Values[1]);
        
        // compare ratio amounts
        double ratio1Total = (double)ratio1Sales / ratio1Stock;
        double ratio2Total = (double)ratio2Sales / ratio2Stock;
        int result = ratio1Total.CompareTo(ratio2Total);
        if (result == 0)
        {
            // If the ratios are equal, compare the keys themselves
            result = ratio1.CompareTo(ratio2);
        }
        return result;
    }
}