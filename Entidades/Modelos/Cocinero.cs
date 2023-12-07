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
        private Mozo<T> mozo;
        private T pedidoEnPreparacion;
        private Queue<T> pedidos;

        public event DelegadoNuevoIngreso  OnPedido;
        public event DelegadoDemoraAtencion OnDemora;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();

            
        }

        public Queue<T> Pedidos { get { return pedidos; } }

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
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();
                    this.mozo.EmpezarATrabajar = false;
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void EmpezarACocinar()
        {
            tarea = Task.Run(() =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    if (pedidos.Count > 0)
                    {
                        pedidoEnPreparacion = pedidos.Dequeue();
                        OnPedido?.Invoke(pedidoEnPreparacion);
                        EsperarProximoIngreso();
                        cantPedidosFinalizados++;
                        FileManager.Guardar("ingresa", "entra.txt", true);
                        DataBaseManager.GuardarTicket<T>(nombre, pedidoEnPreparacion);
                    }
                }
            }, cancellation.Token);
        }

        private void NotificarNuevoIngreso()
        {
            if(OnIngreso != null)
            {
                menu = new T();
                menu.IniciarPreparacion();
                OnIngreso.Invoke(menu);                            
            }                  
        }
        private void EsperarProximoIngreso()
        {

            if(OnDemora != null)
            {
                int tiempoEspera = 0;
                while (!menu.Estado && !cancellation.IsCancellationRequested)
                {
                    OnDemora.Invoke(tiempoEspera);

                    Thread.Sleep(1000);

                    tiempoEspera += 1;
                }
                demoraPreparacionTotal += tiempoEspera;
            }
        }

        private void TomarNuevoPedido(T pedido)
        {
            if (this.OnPedido != null)
            {
                pedidos.Enqueue(pedido);
            }
        }
    }
}
