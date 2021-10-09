using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passports
{
    /// <summary>
    /// Читает строки из потока
    /// </summary>
    internal class CSVStreamReader : IEnumerable, IDisposable
    {
        private readonly LoacalEnumerator _enumerator;

        public CSVStreamReader(Stream stream)
        {
            _enumerator = new LoacalEnumerator(new StreamReader(stream));
        }

        /// <summary>
        /// Удаляет объект
        /// </summary>
        public void Dispose()
        {
            _enumerator.Dispose();
        }
        /// <summary>
        /// Возвращает объект для перебора строк
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _enumerator;
        }
        /// <summary>
        /// Реализация интерфейса IEnumerator
        /// </summary>
        private class LoacalEnumerator : IEnumerator
        {
            private readonly StreamReader _streamReader;
            private string _line;
            public LoacalEnumerator(StreamReader streamReader)
            {
                _streamReader = streamReader;
            }
            /// <summary>
            /// Перемещает курсок к следующей записи
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                _line = _streamReader.ReadLine();

                if (_line != null)
                {
                    return true;
                }
                Reset();
                return false;
            }
            /// <summary>
            /// Возвращает курсор к первой записи
            /// </summary>
            public void Reset()
            {
                //_streamReader.DiscardBufferedData();
                _streamReader.BaseStream.Position = 0;
            }
            /// <summary>
            /// Возвращает текущую запись
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return _line.Split(";");
                }
            }
            /// <summary>
            /// Удаляет объект
            /// </summary>
            public void Dispose()
            {
                _streamReader.Dispose();
            }
        }
    }
}
