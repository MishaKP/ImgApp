using System;
using System.Diagnostics;
using System.IO;



/* для подключения System.Drawing в своем проекте правой в проекте нажать правой кнопкой по Ссылкам -> Добавить ссылку
    отметить галочкой сборку System.Drawing    */
using System.Drawing;
using System.Drawing.Drawing2D;


namespace IMGapp
{

    class Program
    {
        private static object result { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите название картинки");
            string n = Console.ReadLine();
            Console.WriteLine("Введите название картинки");
            string m = Console.ReadLine();
            try
            {
                using (var img1 = new Bitmap($"..\\..\\{n}.jpg"))

                //открываем картинку     
                {     //блок using используется для корретного высвобождения памяти переменной, которая в нем создается
                      //для типа Bitmap это необходимо.
                      //вне блока using объект, в нем созданный, будет уже не доступен.
                      //Внутри этого блока using нельзя будет сохранить новое изображение в файл in.jpg,
                      //т.к. пока загруженный битмап висит в памяти файл открыт.
                    using (var img2 = new Bitmap($"..\\..\\{m}.jpg"))
                    {
                        Console.WriteLine("Открываю изображение1 " + Directory.GetParent("..\\..\\") + $"\\{n}.jpg");
                        Console.WriteLine("Открываю изображение2 " + Directory.GetParent("..\\..\\") + $"\\{m}.jpg");

                        var w = img1.Width;
                        var h = img1.Height;
                        Console.WriteLine("0.попиксельно сумма\n1.произведение\n2.среднее-арифметическое\n3.min\n4.max\n5.наложить маску");
                        int a = Convert.ToInt32(Console.ReadLine());
                        if ((a > -1) && (a < 6))
                            for (int i = 0; i < h; ++i)
                                for (int j = 0; j < w; ++j)
                                {
                                    var pix1 = img1.GetPixel(j, i);
                                    var pix2 = img2.GetPixel(j, i);
                                    switch (a)
                                    {
                                        case 0:
                                            {
                                                int r = (int)Clamp(pix1.R + pix2.R, 0, 255);
                                                int g = (int)Clamp(pix1.G + pix2.G, 0, 255);
                                                int b = (int)Clamp(pix1.B + pix2.B, 0, 255);
                                                pix1 = Color.FromArgb(r, g, b);
                                                break;
                                            }
                                        case 1:
                                            {
                                                int r = Convert.ToInt32((float)pix1.R / 255 * pix2.R);
                                                int g = Convert.ToInt32((float)pix1.G / 255 * pix2.G);
                                                int b = Convert.ToInt32((float)pix1.B / 255 * pix2.B);
                                                pix1 = Color.FromArgb(r, g, b);
                                                break;
                                            }
                                        case 2:
                                            {
                                                int r = (int)Clamp((pix1.R + pix2.R) / 2, 0, 255);
                                                int g = (int)Clamp((pix1.G + pix2.G) / 2, 0, 255);
                                                int b = (int)Clamp((pix1.B + pix2.B) / 2, 0, 255);
                                                pix1 = Color.FromArgb(r, g, b);
                                                break;
                                            }
                                        case 3:
                                            {
                                                int r = Math.Min(pix1.R, pix2.R);
                                                int g = Math.Min(pix1.G, pix2.G);
                                                int b = Math.Min(pix1.B, pix2.B);
                                                pix1 = Color.FromArgb(r, g, b);
                                                break;
                                            }
                                        case 4:
                                            {
                                                int r = Math.Max(pix1.R, pix2.R);
                                                int g = Math.Max(pix1.G, pix2.G);
                                                int b = Math.Max(pix1.B, pix2.B);
                                                pix1 = Color.FromArgb(r, g, b);
                                                break;
                                            }
                                        case 5:
                                            {
                                                Console.WriteLine("1.круг\n2.квадрат\n3.прямоугольник");
                                                int a1 = Convert.ToInt32(Console.ReadLine());
                                                if ((a1 > -1) && (a1 < 3))
                                                    switch (a1)
                                                    {
                                                        case 1:
                                                            {
                                                                var brightness = Color.FromArgb(pix2.R, pix2.G, pix2.B).GetBrightness();
                                                                int r = (int)Clamp(pix1.R * brightness, 0, 255);
                                                                int g = (int)Clamp(pix1.G * brightness, 0, 255);
                                                                int b = (int)Clamp(pix1.B * brightness, 0, 255);
                                                                pix1 = Color.FromArgb(r, g, b);
                                                                break;
                                                            }
                                                    }
                                                break;
                                            }
                                    }
                                }
                    }
                    Console.WriteLine("Введите название результата без разрешения:");
                    string result_name = Console.ReadLine();
                    img1.Save(("..\\..\\") + result_name + ".jpg");
                    Console.WriteLine("Выходное изображение было сохренено по пути " + ("..\\..\\") + result_name + ".jpg");
                    Console.ReadKey();
                }
            }
            /*   using (var img_out = new Bitmap(w, h))   //создаем пустое изображение размером с исходное для сохранения результата
               {
                   var time1 = DateTime.Now;
                   Stopwatch timer = new Stopwatch();
                   timer.Start();

                   //попиксельно обрабатываем картинку 
                   for (int i = 0; i < h; ++i)
                   {
                       for (int j = 0; j < w; ++j)
                       {

                           //считывыем пиксель картинки и получаем его цвет
                           var pix = img1.GetPixel(j, i);

                           //получаем цветовые компоненты цвета
                           int r = pix.R;
                           int g = pix.G;
                           int b = pix.B;

                           //Увеличим квет каждого пикселя на 1.4
                           //При вычислении пикселей используем функию Clamp (см. ниже Main) чтобы цвет не вылезал за границы [0 255]
                           r = (int)Clamp(r * 1.4, 0, 255);
                           g = (int)Clamp(g * 1.4, 0, 255);
                           b = (int)Clamp(b * 1.4, 0, 255);


                           //записываем пиксель в изображение
                           pix = Color.FromArgb(r, g, b);
                           img_out.SetPixel(j, i, pix);

                           //ц-ции GetPixel и SetPixel работают достаточно медленно, надо стримится к минимизации их использования
                       }
                   }

                   //нарисуем что нибудь на картинке
                   using (var g = Graphics.FromImage(img_out)) //через Using создадим объекет Graphics из нашей выходной картинке
                   {              //Graphics как раз содержит методы для рисования линий, текста и прочих геомиетричсеких примитивов

                       g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                       g.SmoothingMode = SmoothingMode.HighQuality;

                       var p = Pens.Red.Clone() as Pen;  //красная ручка
                       p.Width = 5;        //Так как кисть стандартная, для изменения ее свойств создадим ее копию ф-цией Clone


                       g.FillRectangle(Brushes.White, 10, 10, 340, 50); //белый прямоугольник

                       var f = new Font("Times New Roman", 20, FontStyle.Bold); //шрифт
                       g.DrawString("Выходное изображение:", f, Brushes.Black, 10, 10);

                       g.DrawLine(p, 10, 10, 350, 10); //красная линия  


                       //В завершении, нарисуем зеленую синусоиду на картинке =)m
                       var green_pen = Pens.Green.Clone() as Pen;
                       green_pen.Width = 3;
                       for (int i = 1; i < w; ++i)
                           g.DrawLine(green_pen,
                               (i - 1),
                               h / 2 + (int)(50 * Math.Sin((i - 1) / 50.0)),
                               i,
                               h / 2 + (int)(50 * Math.Sin(i / 50.0)));


                       //нарисуем в нижнем правом углу оригинальное изображение в красной рамочке
                       g.DrawImage(img1, w - 100 - 1, h - 100 - 1, 100, 100);
                       g.DrawRectangle(p, w - 100 - 1, h - 100 - 1, 100, 100);

                       //ручками высвобождаем ресурсы
                       f.Dispose();
                       p.Dispose();
                       green_pen.Dispose();

                   }     //вот тут графикс g удаляется методом g.Dispose()     

                   timer.Stop();

                   Console.WriteLine("Обработал изображение за " + timer.ElapsedMilliseconds + " мс.");

                   //сохраним нашу выходную картинку 
                   img_out.Save("..\\..\\out.jpg");


                   Console.WriteLine("Выходное изображение было сохренено по пути " + Directory.GetParent("..\\..\\") + "\\out.jpg");
                   Console.ReadKey();

               } //using (var img_out = new Bitmap(w, h))     вот тут картинка img_out удаляется методом img_out.Dispose()     

           } //using (var img = new Bitmap("in.jpg"))   вот тут картинка img удаляется методом img.Dispose()     
       } */


            catch (Exception ex) { Console.WriteLine(ex.Message); }

        } //static void Main(string[] args)

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }


}
