using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinTrinhLibrary;
using System.Text.RegularExpressions;
using System.IO;

namespace watch_crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string fpath = "D:\\result.txt";
            FileStream fs = new FileStream(fpath, FileMode.Create);           
            StreamWriter swriter = new StreamWriter(fs, Encoding.UTF8);
            //GET : HTTPS://www.adayroi.com/
            TinTrinhLibrary.WebClient client = new TinTrinhLibrary.WebClient(); //tạo kết nối
            for (int i = 1; i <= 2; i++) //vòng lặp tăng số trang lên
            {
                string html = client.Get("https://www.adayroi.com/dong-ho-nam-m101?p=" + i, "https://www.adayroi.com/", ""); //đi tới trang ưeb
                MatchCollection listLink = Regex.Matches(html, "post-title\"><a href=\"/(.*?)title=.*?>"); //danh sách link từng sản phẩm
                foreach (Match link in listLink) //vòng lặp để vào từng sản phẩm
                {
                    string html1 = client.Get("https://www.adayroi.com/" + link.Groups[1].Value, "https://www.adayroi.com/", "");//tạo đường link đến từng sản phẩm
                    Match watch_name = Regex.Match(html1, "item-title\">(.*?)</h1>"); //lấy tên sản phẩm
                    Match watch_category = Regex.Match(html1, "text-blue\".*?>(.*?)</a>"); //lấy danh mục sản phẩm
                    Match watch_price = Regex.Match(html1, "item-price.*?>(.*?)<span", RegexOptions.Singleline); //lấy giá sản phẩm
                    Match watch_desc = Regex.Match(html1, "itemprop=\"description\">.*?<p>(.*?)</p>", RegexOptions.Singleline); //lấy mô tả về sản phẩm
                    swriter.WriteLine("Tên đồng hồ: " + watch_name.Groups[1].Value); //in ra 
                    swriter.WriteLine("Danh muc: " + watch_category.Groups[1].Value);
                    swriter.WriteLine("Giá: " + watch_price.Groups[1].Value.Trim());
                    swriter.WriteLine("Mô tả: " + watch_desc.Groups[1].Value);
                    swriter.WriteLine("\n====================================\n");
                }
            }
            swriter.Flush();
            fs.Close();
        }
    }
}
