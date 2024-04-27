using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        // Свойство изменено на ReadOnly
        private Stream _localStream { get; }
        // Добавлено св-во
        private StreamReader _reader { get; }

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            // TODO : Заменить на создание реального стрима для чтения файла! (DONE)
            try
            {
                IsEof = false;
                _localStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
                _reader = new StreamReader(_localStream);
            }
            catch (Exception ex) // Добавлена обработка ошибок при неудачной попытке открытия файла
            {
                IsEof = true;
                Console.WriteLine("Exception: can't open the file or there's no such file or directory exists. Exception message:");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get; // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении (DONE)
            private set;
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar() // Метод модифицирован
        {
            // TODO : Необходимо считать очередной символ из _localStream (DONE)
            char ch = '\0';

            try
            {
                if (IsEof) { throw new NotImplementedException(); }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Exception: end of file or there's no file opened"); // Вызов исключения при попытке прочитать символ после достижения конца файла
                return ch;
            }

            ch = Convert.ToChar(_reader.Read());
            if (_reader.Peek() == -1) { IsEof = true; }

            return ch;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream is null)
            {
                IsEof = true;
                return;
            }

            _localStream.Position = 0;
            IsEof = false;
        }

        /// <summary>
        /// Освобождает ресурсы для работы с файлом.
        /// </summary>
        /// <returns>True, если ресурсы успешно освобождены; иначе - False</returns>
        public bool EnsureFileDisposed() // Добавлена реализация метода
        {
            _localStream.Dispose();
            _reader.Dispose();
            return _localStream == null && _reader == null ? false : true;
        }
    }
}
