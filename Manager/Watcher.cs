using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    internal class Watcher
    {
        const string FILE_NAME = "test.txt";

        /// <summary>
        /// Список активных потоков
        /// </summary>
        private static List<Thread> _threads { get; set; } = new List<Thread>();

        /// <summary>
        /// Запуск прослушивание классов 
        /// </summary>
        public async void StartWatching()
        {
            try
            {
                //Ожидание 10 секунд
                await Task.Delay(10000);
                //Запуск dll с продолжительностью дейсвия 10 секунд
                CreateNewWatchThread("ClassLibrary1.dll", "ClassLibrary1.Class1", "StartWatch" , 10);             
                //Ожидание 10 секунд
                await Task.Delay(10000);
                CreateNewWatchThread("ClassLibrary2.dll", "ClassLibrary2.Class2", "StartWatch");
                //Ожидание 10 секунд
                await Task.Delay(10000);
                CreateNewWatchThread("ClassLibrary3.dll", "ClassLibrary3.Class3", "StartWatch");

            }
            catch (Exception ex)
            {
                //Создание лога с ошибкой
                Logger.CreateLog(FILE_NAME, ex.Message, Logger.TypeLog.Error);

                //Перезапускаем метод
                StartWatching();
            }
        }

        /// <summary>
        /// Создание нового потока для прослушивания с подключением dll
        /// </summary>
        /// <param name="nameDLL">название DLL</param>
        /// <param name="nameClass">название класса</param>
        /// <param name="nameMethod">название метода, который будет запущен</param>
        private void CreateNewWatchThread(string nameDLL, string nameClass, string nameMethod = "StartWatch", int countSeconds = 20)
        {
            //Создание лога на подключение потока
            Logger.CreateLog(FILE_NAME, string.Format("Запуск модуля {0}", nameDLL), Logger.TypeLog.Start);
            Thread myNewThread = new Thread(() => AddDLL(nameDLL, nameClass));
            myNewThread.Start();
            _threads.Add(myNewThread);

            //Запуск таймера с временм дейсвия countSeconds
            System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(countSeconds).TotalMilliseconds);
            timer.Elapsed += (sender, e) =>
            {
                _threads.Remove(myNewThread);
                //Прерывание потока
                myNewThread.Interrupt();

                //Создание лога на отключение потока
                Logger.CreateLog(FILE_NAME, string.Format("Модуль успешно завершился {0}", nameDLL), Logger.TypeLog.Close);
                timer.Stop();
            };
            timer.Start();
        }

        /// <summary>
        /// Добавить динамически библиотеку класса в систему и запустить метод StartWatch
        /// </summary>
        /// <param name="nameDLL">название DLL</param>
        /// <param name="nameClass">название класса</param>
        /// <param name="nameMethod">название метода, который будет запущен</param>
        private void AddDLL(string nameDLL, string nameClass, string nameMethod = "StartWatch") 
        {
            Assembly assembly = Assembly.LoadFile(string.Format("{0}\\{1}", Environment.CurrentDirectory, nameDLL));
            Object obj = assembly.CreateInstance(nameClass);
            Type type = assembly.GetType(nameClass);
            MethodInfo methodInfo = type.GetMethod(nameMethod);

            methodInfo.Invoke(obj, null);
        }

        /// <summary>
        /// Освобождаем потоки, после освобождения класса (если они есть)
        /// </summary>
        ~Watcher()
        {
            foreach(var thread in _threads)
            {
                thread.Interrupt();
            }
        }
    }
}
