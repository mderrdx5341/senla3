using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Passports.Services
{
    /// <summary>
    /// Сервис для обновления списка данных из внешнего источника
    /// </summary>
    internal class DataUpdaterService : IDataUpdaterService
    {
        private readonly IPassportsRepository _passportsRepository;
        private readonly IUpdaterData _updater;
        private DateTime _nextUpdate;
        private Timer _timer;
        public DataUpdaterService(IPassportsRepository passportsRepository, IUpdaterData updater, DateTime nextUpdate)
        {
            _passportsRepository = passportsRepository;
            _nextUpdate = nextUpdate;
            _updater = updater;
        }
        /// <summary>
        /// Запускает обновление данных по указанному времени
        /// </summary>
        public void Start()
        {
            Task.Run(StartTimer);
        }

        private void StartTimer()
        {
            TimerCallback tm = new TimerCallback(StartUpdate);
            _timer = new Timer(tm, null, 0, 2000);
        }

        private void StartUpdate(object o)
        {
            if (isUpdate())
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _nextUpdate = _nextUpdate.AddDays(1);
                Update();
                _timer.Change(0, 2000);
            }
        }

        private bool isUpdate()
        {
            TimeSpan ts = _nextUpdate.Subtract(DateTime.Now);
            return (ts.CompareTo(TimeSpan.Zero) < 0);
        }

        private void Update()
        {
            _updater.Run();
        }
    }
}
