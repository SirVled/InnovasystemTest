namespace ClassLibrary2
{
    public class Class2
    {
        const string FILE_NAME = "test.txt";

        /// <summary>
        /// Запуск записи в файл информации по классу
        /// </summary>
        public void StartWatch()
        {
            int seconds = 0;
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    File.AppendAllText(FILE_NAME, string.Format("{0} - {1} ({2} sec)\n", DateTime.Now, nameof(Class2), ++seconds));
                }
            }
            catch
            {

            }
        }
    }
}