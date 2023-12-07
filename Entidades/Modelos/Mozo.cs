using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DeleadoNuevoPedido<T>(T menu);
    public class Mozo<T> where T : IComestible, new()
    {
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;

        public event DeleadoNuevoPedido<T> OnPedido;

        public bool EmpezarATrabajar
        {
            get
            {
                return tarea != null &&
                       (tarea.Status == TaskStatus.Running ||
                        tarea.Status == TaskStatus.WaitingToRun ||
                        tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && (tarea == null ||
                              tarea.Status != TaskStatus.Running ||
                              tarea.Status != TaskStatus.WaitingToRun ||
                              tarea.Status != TaskStatus.WaitingForActivation))
                {
                  
                    cancellation = new CancellationTokenSource();
                    this.TomarPedidos();
                   
                }
                else
                {                    
                    cancellation?.Cancel();
                }
            }
        }

        private void NotificarNuevoPedido()
        {                     
            if (OnPedido != null)
            {               
                T newMenu = new T();
                menu.IniciarPreparacion();
                OnPedido.Invoke(newMenu);
            }
        }

        private void TomarPedidos()
        {
            while (!cancellation.IsCancellationRequested)
            {
                NotificarNuevoPedido();
                Thread.Sleep(5000);
            
                if (OnPedido != null)
                {                
                    menu = new T();
                    OnPedido.Invoke(menu);
                }
            }
        }



    }
}
