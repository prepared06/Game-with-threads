using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ocLab5
{
    public partial class Form1 : Form
    {
        static Mutex mutexObj;//объект мьютекс   
        //потоки
        Thread FORM;
        Thread COLOR;
        Thread LOCATION;
        //переменная для управления поездок по экрану
        int recLoc = 1;
        Bitmap bmp;//файл картинки
        Graphics graph;
        //цвета кистей
        Pen pen_red;//
        Pen pen_blue;//
        Pen pen_green;
        //переменные координаты
        int x1, y1, w,h;
        public Form1()
        {
            InitializeComponent();
            x1 = y1 = 78;
            w = 80;
            h = 20;
            bmp = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height); //создаем новую картинку

                graph = Graphics.FromImage(bmp);
                pen_red = new Pen(Color.Red);
                pen_blue = new Pen(Color.Blue);
                pen_green = new Pen(Color.Green); 
                graph.DrawRectangle(pen_red, x1, y1, w, h);
                pictureBox1.Image = bmp;


        }
        private void Change_Form()//меняем форму, путём скрытие других фигур и открытие нужной
        {
            while (true)
            {
                mutexObj.WaitOne();//приостанавливает выполнение потока до тех пор, пока не будет получен мьютекс mutexObj   

                graph.Clear(Color.LightGray);//очищаем экран
                graph.DrawEllipse(pen_red, x1, y1, w, h);//рисуем элипс
                pictureBox1.Image = bmp;//выводим картинку
                Thread.Sleep(100);//задержка

                graph.Clear(Color.LightGray);//очищаем экран
                graph.DrawEllipse(pen_red, x1+30, y1, h, h);//рисуем круг
                pictureBox1.Image = bmp;//выводим картинку 
                Thread.Sleep(100);//задержка

                graph.Clear(Color.LightGray);//очищаем экран
                graph.DrawRectangle(pen_red, x1, y1, w, h);//рисуем прямоугольник
                pictureBox1.Image = bmp;//выводим картинку
                Thread.Sleep(100);//задержка

                mutexObj.ReleaseMutex();//После выполнения всех действий, когда мьютекс больше не нужен, поток освобождает его с помощью метода mutexObj.ReleaseMutex()
                
            }
        }
        private void Change_Color()//меняем цвет сразу всем
        {
            
                while (true)
                {
                    mutexObj.WaitOne();//приостанавливает выполнение потока до тех пор, пока не будет получен мьютекс mutexObj

                    graph.DrawRectangle(pen_blue, x1, y1, w, h);//рисуем прямоугольник синего цвета
                    pictureBox1.Image = bmp;//выводим
                    Thread.Sleep(100);//задержка

                    graph.DrawRectangle(pen_green, x1, y1, w, h);//рисуем прямоугольник зеленого цвета
                    pictureBox1.Image = bmp;//выводим
                    Thread.Sleep(100);//задержка

                    graph.DrawRectangle(pen_red, x1, y1, w, h);//рисуем прямоугольник красного цвета
                    pictureBox1.Image = bmp;//выводим
                    Thread.Sleep(100);//задержка

                    mutexObj.ReleaseMutex();//После выполнения всех действий, когда мьютекс больше не нужен, поток освобождает его с помощью метода mutexObj.ReleaseMutex()
                }
            
        }
        private void Change_Location()//меняем локацию фигуры
        {
            while (true)
            {
                mutexObj.WaitOne();//приостанавливает выполнение потока до тех пор, пока не будет получен мьютекс mutexObj
                if (y1 == 1 | y1 == 180)//усливое изменения полярности движение
                {
                    recLoc *= -1;
                }
                y1 += recLoc;//инкремент 

                graph.Clear(Color.LightGray);
                graph.DrawRectangle(pen_red, x1, y1, w, h);
                pictureBox1.Image = bmp;
                Thread.Sleep(100);

                mutexObj.ReleaseMutex();//После выполнения всех действий, когда мьютекс больше не нужен, поток освобождает его с помощью метода mutexObj.ReleaseMutex()
            }
        }
        //свойство изменения checkBox цвета
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true & checkBox2.Checked == false & checkBox3.Checked == false)//условие создания мьютекса(баганутое место, нельзя выключать первонажатую галочку)
            {
                mutexObj = new Mutex();//инициализация объекта мьютекса
            }
            if (checkBox1.Checked)//если активировали
            {
                COLOR = new Thread(Change_Color);//инициализация потока
                COLOR.Start();//запуск потока
            }
            else//если деактивировали
            {
                COLOR.Abort();//останавливаем поток
            }
        }
        //свойство изменения checkBox формы
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false & checkBox2.Checked == true & checkBox3.Checked == false)
            {
                mutexObj = new Mutex();
            }
            if (checkBox2.Checked)
            {
                FORM = new Thread(Change_Form);
                FORM.Start();
            }
            else
            {
                FORM.Abort();
            }
        }
        //свойство изменения checkBox координаты
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false & checkBox2.Checked == false & checkBox3.Checked == true)
            {
                mutexObj = new Mutex();
            }
            if (checkBox3.Checked)
            {
                LOCATION = new Thread(Change_Location);
                LOCATION.Start();
            }
            else
            {
                LOCATION.Abort();
            }
        }



    }
}
