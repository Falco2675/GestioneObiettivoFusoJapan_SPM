using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreTurni : IGestoreTurni
    {

        private TimeSpan INIZIOPRIMOTURNO = new TimeSpan(6, 0, 0);
        private TimeSpan FINEPRIMOTURNO = new TimeSpan(13, 59, 59);
        private TimeSpan INIZIOSECONDOTURNO = new TimeSpan(14, 0, 0);
        private TimeSpan FINESECONDOTURNO = new TimeSpan(21, 59, 59);
        private TimeSpan INIZIOTERZOTURNO = new TimeSpan(22, 0, 0);
        private TimeSpan FINETERZOTURNO = new TimeSpan(05, 59, 59);


        private readonly Timer _timerOrario;
        private DateTime _orario;

        #region PROPRIETA'
        public DateTime Orario
        {
            get { return _orario; }
            private set
            {
                _orario = value;
                ControllaTurno();
            }
        }

        private TurnoEnum _turno;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;

        public TurnoEnum Turno
        {
            get { return _turno; }
            private set
            {
                if (_turno == value) return;
                _turno = value;
                _gestoreConfigurazione.ResettaTurno();
            }
        }



        

        #endregion


        #region EVENTI
        public event EventHandler<string> OrarioChanged;
        #endregion


        #region CTOR
        public GestoreTurni
            (
                IGestoreConfigurazione gestoreConfigurazione
            )
        {
            _gestoreConfigurazione = gestoreConfigurazione;

            _timerOrario = new Timer((o) => { Orario = DateTime.Now; }, null, 500, 1000);

        }
        #endregion

        #region Metodi
        private void ControllaTurno()
        {
            if(_orario.TimeOfDay>=INIZIOPRIMOTURNO && _orario.TimeOfDay <= FINEPRIMOTURNO)
            {
                Turno = TurnoEnum.PrimoTurno;
                return;
            }
            if (_orario.TimeOfDay >= INIZIOSECONDOTURNO && _orario.TimeOfDay <= FINESECONDOTURNO)
            {
                Turno = TurnoEnum.SecondoTurno;
                return;
            }
            Turno = TurnoEnum.TerzoTurno;
        }

        #endregion

    }
}
