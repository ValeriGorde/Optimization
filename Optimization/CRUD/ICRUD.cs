using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace Optimization.CRUD
{
    internal interface ICRUD<T> where T: class
    {
        /// <summary>
        /// добавление элемента в БД
        /// </summary>
        /// <param name="item"></param>
        public void Create(T item);

        /// <summary>
        /// Просмотр элемента по id
        /// </summary>
        /// <param name="item"></param>
        public bool Read(int id);

        /// <summary>
        /// Обновление данных
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item);

        /// <summary>
        /// Удаление по id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id);
    }
}
