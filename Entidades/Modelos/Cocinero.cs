using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;



namespace Entidades.Modelos
{  
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoIngreso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new ()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private Task tarea;
        private T menu;

        public event DelegadoNuevoIngreso  OnIngreso;
        public event DelegadoDemoraAtencion OnDemora;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();                   
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void IniciarIngreso()
        {
            if (!this.cancellation.IsCancellationRequested)
            {
                this.tarea = Task.Run(NotificarNuevoIngreso);
                this.cantPedidosFinalizados++;     
                DataBaseManager.GuardarTicket(this.Nombre, this.menu);
                Thread.Sleep(1000);
            }
        }

        private void NotificarNuevoIngreso()
        {
            if(this.OnIngreso is not null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.EsperarProximoIngreso();                
            }
            else
            {
                this.OnIngreso.Invoke(this.menu);
            }           
        }
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            if(this.OnDemora is not null)
            {
                Thread.Sleep(2000);
                this.demoraPreparacionTotal += tiempoEspera;
            }
            else
            {
                this.OnDemora.Invoke(this.demoraPreparacionTotal);
            }          

        }
    }
}
