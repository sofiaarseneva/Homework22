using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Параллельное программирование!\n\n");
            try
            {
                Console.WriteLine("Введите размерность массива: ");
                int n = Convert.ToInt32(Console.ReadLine());
                //создание и заполнение массива
                Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
                Task<int[]> task1 = new Task<int[]>(func1, n);
                //получение суммы чисел массива
                Func<Task<int[]>, int[]> getSum = new Func<Task<int[]>, int[]>(GetSumArray);
                Task<int[]> task2 = task1.ContinueWith<int[]>(getSum);
                //получение максимального значения
                Func<Task<int[]>, int[]> getMax = new Func<Task<int[]>, int[]>(GetMaxNumber);
                Task<int[]> task3 = task2.ContinueWith<int[]>(getMax);
                //вывод массива для удобства проверки
                Action<Task<int[]>> printArray = new Action<Task<int[]>>(PrintArray);
                Task task4 = task3.ContinueWith(printArray);

                task1.Start();
                task4.Wait();
                Console.WriteLine("Конец выполнения задачи!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка!" + ex.Message);
            }
            Console.ReadKey();
        }

        static int[] GetArray(object a)
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 50);
            }
            return array;
        }
        static int[] GetSumArray(Task<int[]> task)
        {
            int sum = 0;
            int[] array = task.Result;
            Console.WriteLine("\nНачало выполнения поиска суммы элементов!\n");
            for (int i = 0; i < array.Count(); i++)
            {
                sum += array[i];
            }
            Console.WriteLine($"Сумма элементов массива равна {sum}");
            Console.WriteLine("\nКонец выполнения поиска суммы элементов!\n"); 
            Thread.Sleep(500);
            return array;
        }
        static int[] GetMaxNumber(Task<int[]> task)
        {
            int[] array = task.Result;
            int max = array[0];
            Console.WriteLine("Начало выполнения поиска максимального элемента массива!\n");
            for (int i = 0; i < array.Count(); i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                }
            }
            Console.WriteLine($"Максимальный элемент массива равен {max}");
            Console.WriteLine("\nКонец выполнения поиска максимального элемента массива!\n"); 
            Thread.Sleep(500);
            return array;
        }
        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            Console.WriteLine("Начало печати массива!\n");
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i]} ");
                Thread.Sleep(100);
            }
            Console.WriteLine("\nКонец печати массива!\n");
        }
    }
}
