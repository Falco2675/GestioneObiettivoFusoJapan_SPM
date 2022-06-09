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


        private DateTime orario;

        #region PROPRIETA'

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
                TurnoChanged?.Invoke(this, null);
            }
        }

        public string Turno_string
        {
            get
            {
                string _turno="";

                switch (Turno)
                {
                    case TurnoEnum.PrimoTurno:
                        _turno = "1° Turno";
                        break;
                    case TurnoEnum.SecondoTurno:
                        _turno = "2° Turno";
                        break;
                    case TurnoEnum.TerzoTurno:
                        _turno = "3° Turno";
                        break;
                }
                return _turno;

            }
        }

        #endregion


        #region EVENTI
        public event EventHandler TurnoChanged;
        #endregion


        #region CTOR
        public GestoreTurni
            (
                IGestoreConfigurazione gestoreConfigurazione
            )
        {
            _gestoreConfigurazione = gestoreConfigurazione;

        }
        #endregion

        #region Metodi
        public void ControllaTurno()
        {
            DateTime orario = DateTime.Now;

            if(orario.TimeOfDay>=INIZIOPRIMOTURNO && orario.TimeOfDay <= FINEPRIMOTURNO)
            {
                Turno = TurnoEnum.PrimoTurno;
                return;
            }
            if (orario.TimeOfDay >= INIZIOSECONDOTURNO && orario.TimeOfDay <= FINESECONDOTURNO)
            {
                Turno = TurnoEnum.SecondoTurno;
                return;
            }
            Turno = TurnoEnum.TerzoTurno;
        }

        #endregion

    }
}
