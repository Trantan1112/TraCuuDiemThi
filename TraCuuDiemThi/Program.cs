using System;
using xNet;
using System.Threading;
using Newtonsoft.Json;
using System.Text;

namespace TraCuuDiemThi
{
    class Program
    {
        static string SBD;
        const string url = "https://diemthi.tuoitre.vn/search-thpt-score";
        static void Main()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            while (true)
            {
                try
                {
                    Console.Title = "TRA CỨU ĐIỂM THI THPTQG 2022 | Trần Tân";
                    Console.WriteLine("TRA CỨU ĐIỂM THI THPTQG 2022");
                    Console.WriteLine("--------------------------------");
                    Thread.Sleep(1000);
                    Console.Write("Nhập SBD: ");
                    SBD = Console.ReadLine();
                    if (int.Parse(SBD) <= 0 || SBD.ToString().Length != 8)
                        throw new Exception();

                    else
                        break;
                }
                catch
                {
                    Console.WriteLine("Vui lòng nhập lại");
                    Thread.Sleep(860);
                    Console.Clear();
                }
            }
            Console.Clear();
            string req = Request(SBD);
            Console.WriteLine("Kết quả của thí sinh " + SBD);
            Console.WriteLine("--------------------------------");
            XuLy(req);          
            Thread.Sleep(200);
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Ấn Enter để tiếp tục tra cứu");
            Console.ReadLine();
            Main();

        }
        static string Request(string sbd)
        {
            try
            {
                using (var request = new HttpRequest(url))
                {
                    request.ConnectTimeout = 10000;
                    request.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.122 Safari/537.36";                    
                    string param = ("{\"data\":\"" + sbd + "\",\"code\":\"\"}");
                    return request.Post(url, param, "application/json").ToString();
                }
            }
            catch(Exception e)
            {
                return "Lỗi! | " + e.Message;
            }
        }
        static void XuLy(string input)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                if (input.Contains("\"examination_id\":\"1\""))
                {
                    var score = JsonConvert.DeserializeObject<Parse_Json>(input);
                    string specific_score = score.score;
                    if (specific_score.Contains(';'))
                    {
                        string[] array = specific_score.Split(';');
                        foreach(var lines in array)
                        {
                            if(!string.IsNullOrEmpty(lines.Split(':')[1]))
                            {
                                Console.WriteLine(lines);
                            }
                        }
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    Console.WriteLine("Lỗi!");
                }
            }
            catch { }
        }
    }
    class Parse_Json
    {
        public string score { get; set; }
    }
}
