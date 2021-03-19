using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ShowcaseLib;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

namespace NetMarketServer
{
    class Server
    {
        static List<Showcase> showcase = new List<Showcase>();

        private static void ListInit()
        {
            for (int i = 0; i < 10; i++)
            {
                showcase.Add(new Showcase());

                for (int j = 0; j < 5; j++)
                {
                    showcase[i].list.Add(new Product());
                }
            }
        }

        static void Main(string[] args)
        {
            ListInit();

            //Для выхода
            Thread backgroundThread = new Thread(new ThreadStart(Exit));

            backgroundThread.Start();

            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:51433/");

            listener.Start();

            do
            {    
                var context = listener.GetContext();

                var inStream = context.Request.InputStream;

                var bytes = new byte[context.Request.ContentLength64];

                inStream.Read(bytes, 0, (int)context.Request.ContentLength64);

                var content = Encoding.UTF8.GetString(bytes);

                inStream.Close();

                var request = context.Request;

                var outStream = context.Response.OutputStream;

                string[] obj = null;

                switch (request.HttpMethod)
                {
                    case "POST":
                        if (request.Url.PathAndQuery == "/Get")
                        {
                            obj = showcase[Int32.Parse(content)].list.Select(x => x.Id + " |" + x.Name + " |" + x.Volume + " |" + x.Price + " |").ToArray();
                        }
                        if (request.Url.PathAndQuery == "/EditFirst")
                        {
                            var _editShowcase = JsonConvert.DeserializeObject<EditShowcase>(content);

                            showcase[_editShowcase.index].Name = _editShowcase.name;

                            showcase[_editShowcase.index].Volume = _editShowcase.volume;

                            obj = GetList(ShowcaseLib.Type.Showcase, 0);
                        }
                        if (request.Url.PathAndQuery == "/EditSecond")
                        {
                            var _editProduct = JsonConvert.DeserializeObject<EditProduct>(content);

                            showcase[_editProduct.firstIndex].list[_editProduct.secondIndex].Name = _editProduct.name;

                            showcase[_editProduct.firstIndex].list[_editProduct.secondIndex].Volume = _editProduct.volume;

                            showcase[_editProduct.firstIndex].list[_editProduct.secondIndex].Price = _editProduct.price;

                            obj = GetList(ShowcaseLib.Type.Product, _editProduct.firstIndex);
                        }
                        break;

                    case "GET":
                        obj = GetList(ShowcaseLib.Type.Showcase, 0);
                        break;

                    case "PUT":
                        if (request.Url.PathAndQuery == "/FirstMenu")
                        {
                            showcase.Add(new Showcase());

                            obj = GetList(ShowcaseLib.Type.Showcase, 0);
                        }
                        if (request.Url.PathAndQuery == "/SecondMenu")
                        {
                            showcase[Int32.Parse(content)].list.Add(new Product());

                            obj = GetList(ShowcaseLib.Type.Product, Int32.Parse(content));
                        }
                        break;

                    case "PATCH":
                        if (request.Url.PathAndQuery == "/FirstMenu")
                        {
                            showcase.RemoveAt(Int32.Parse(content));

                            obj = GetList(ShowcaseLib.Type.Showcase, 0);
                        }
                        if (request.Url.PathAndQuery == "/SecondMenu")
                        {
                            var _index = JsonConvert.DeserializeObject<IndexMenu>(content);

                            showcase[_index.i1].list.RemoveAt(_index.i2);

                            obj = GetList(ShowcaseLib.Type.Product, _index.i1);
                        }
                        break;

                    default:
                        break;
                }

                var json = JsonConvert.SerializeObject(obj);

                bytes = Encoding.UTF8.GetBytes(json);

                outStream.Write(bytes, 0, bytes.Length);

                outStream.Close();

                context.Response.Close();

            } while (true);
        }

        private static string[] GetList(ShowcaseLib.Type type, int index)
        {
            if (type == ShowcaseLib.Type.Product)
            {
                return showcase[index].list.Select(x => x.Id + " |" + x.Name + " |" + x.Volume + " |" + x.Price + " |").ToArray();
            }
            else
            {
                return showcase.Select(x => x.Id + " |" + x.Name + " |" + x.Volume + " |" + x.DateCreate).ToArray();
            }
            
        }
        public static void Exit()
        {
            do
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape) 
                        Environment.Exit(0);
                }
            } while (true);
        }
    }
}