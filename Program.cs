using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;
using System.Linq;



namespace RestApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }
        static void Run()
        {

            string url_address = "";
                string userName = "admin";
                string password = "";

            //EA.Repository repository = new EA.Repository();
            //repository.OpenFile("C:\\Users\\User\\Downloads\\test for stereotype.qea");
            DataTable dt = Getresponse(url_address, userName, password);

            //string label = dt.Rows[0]["QWE"].ToString();

            //EA.Package model = repository.Models.GetAt(0);
            //EA.Package package = model.Packages.AddNew(QWE, "Package");
            //package.Update();




        }

        static DataTable Getresponse(string Url, string Username, string Password)
        {

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Username + ":" + Password);
            string val = System.Convert.ToBase64String(plainTextBytes);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));


                client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);


                HttpResponseMessage response = client.GetAsync(Url).Result;
                string content = string.Empty;
                using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    XmlDocument xDoc = new XmlDocument();
                    content = stream.ReadToEnd();

                    xDoc.LoadXml(content);
                    XmlReader xmlReader = new XmlNodeReader(xDoc);
                    
                    
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xmlReader);
                    return dataSet.Tables[0];

                }
            }
        }
    }
}