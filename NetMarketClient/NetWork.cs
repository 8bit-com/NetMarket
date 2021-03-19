using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using ShowcaseLib;

namespace NetMarketClient
{
    public static class NetWork
    {
        private static HttpClient client = new HttpClient();

        private static IndexMenu _index = new IndexMenu();

        private static EditShowcase _editShowcase = new EditShowcase();

        private static EditProduct _editProduct = new EditProduct();

        public static List<string> Get()
        {
            HttpResponseMessage respons = new HttpResponseMessage();

            try
            {
                respons = client.GetAsync("http://localhost:51433/").Result;
            }
            catch (Exception)
            {
                Console.WriteLine("Удалённый сервер не отвечает");

                Console.ReadKey(true);

                Environment.Exit(0);
            }

            return GetList(respons);
        }

        public static List<string> PostFirstMenu(int index)
        {
            string json = JsonConvert.SerializeObject(index);

            StringContent jsonContent = new StringContent(json);

            HttpResponseMessage respons = client.PostAsync("http://localhost:51433/Get", jsonContent).Result;

            return GetList(respons);
        }

        public static List<string> EditFirstMenu(int index)
        {
            _editShowcase.name = EitName(index);

            _editShowcase.volume = EditVolume(index);

            _editShowcase.index = index;

            string json = JsonConvert.SerializeObject(_editShowcase);

            StringContent jsonContent = new StringContent(json);

            HttpResponseMessage respons = client.PostAsync("http://localhost:51433/EditFirst", jsonContent).Result;

            return GetList(respons);
        }

        public static List<string> EditSecondMenu(int firstIndex, int secondIndex)
        {
            _editProduct.name = EitName(secondIndex);

            _editProduct.volume = EditVolume(secondIndex);

            _editProduct.price = EditPrice(secondIndex);

            _editProduct.firstIndex = firstIndex;

            _editProduct.secondIndex = secondIndex;

            string json = JsonConvert.SerializeObject(_editProduct);

            StringContent jsonContent = new StringContent(json);

            HttpResponseMessage respons = client.PostAsync("http://localhost:51433/EditSecond", jsonContent).Result;

            return GetList(respons);
        }

        public static List<string> PUTFirstMenu()
        {
            HttpResponseMessage respons = client.PutAsync("http://localhost:51433/FirstMenu", null).Result;

            return GetList(respons);
        }

        public static List<string> PUTSecondMenu(int index)
        {
            StringContent stringContent = new StringContent(index.ToString());

            HttpResponseMessage respons = client.PutAsync("http://localhost:51433/SecondMenu", stringContent).Result;

            return GetList(respons);
        }

        public static List<string> DelFirstMenu(int index)
        {
            if (IsNotEmpty(index))
            {
                Message();

                return Get();
            }
            else
            {
                StringContent jsonContent = new StringContent(index.ToString());

                HttpResponseMessage respons = client.PatchAsync("http://localhost:51433/FirstMenu", jsonContent).Result;

                return GetList(respons);
            }
            
        }

        public static List<string> DelSecondMenu(int firstIndex, int secondIndex)
        {
            if (IsNotEmpty(firstIndex))
            {
                _index.i1 = firstIndex;

                _index.i2 = secondIndex;

                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(_index));

                HttpResponseMessage respons = client.PatchAsync("http://localhost:51433/SecondMenu", jsonContent).Result;

                return GetList(respons);
            }
            else return PostFirstMenu(firstIndex);
        }

        private static List<string> GetList(HttpResponseMessage respons)
        {
            string content = respons.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<string>>(content);
        }

        private static string EitName(int index)
        {
            Console.SetCursorPosition(9, index + 2);

            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Write("            ");

            Console.SetCursorPosition(9, index + 2);
            
            Console.CursorVisible = true;

            string name = Console.ReadLine();

            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;

            return name;
        }

        private static int EditVolume(int index)
        {
            Console.SetCursorPosition(33, index + 2);

            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Write("        ");

            Console.SetCursorPosition(33, index + 2);

            Console.CursorVisible = true;

            int volume = 100;

            try
            {
                volume = Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                volume = 100;
            }

            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;

            return volume;
        }

        private static int EditPrice(int index)
        {
            Console.SetCursorPosition(48, index + 2);

            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Write("        ");

            Console.SetCursorPosition(48, index + 2);

            Console.CursorVisible = true;

            int price = 100;

            try
            {
                price = Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                price = 100;
            }

            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;

            return price;
        }

        private static bool IsNotEmpty(int index)
        {
            if (PostFirstMenu(index).Count > 0)
            {
                return true;
            }
            else return false;
        }

        private static void Message()
        {
            Console.Clear();

            Console.SetCursorPosition(10, 5);

            Console.WriteLine(@"
                                ######################################
                                #                                    # 
                                #      Ошибка прилавок не пуст!      #
                                #                                    # 
                                ######################################");

            Console.ReadKey(true);

            Console.SetCursorPosition(10, 5);

            Console.WriteLine(@"
                                                                      
                                                                       
                                                                      
                                                                        
                                                                       ");
        }
    }
}