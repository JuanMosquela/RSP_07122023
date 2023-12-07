using Entidades.Enumerados;
using System.Diagnostics.CodeAnalysis;

namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension 
    {

        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double aumento = costoInicial;
            foreach (int ingrediente in ingredientes)
            {
                double porcentual = (ingrediente / 100) * costoInicial;
                aumento += porcentual;
            }

            return aumento;
        }
        public static List<EIngrediente> IngredientesAleatorios(this Random rand) 
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>() 
            { 
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.JAMON,
                EIngrediente.HUEVO
            };

            int randomNum = rand.Next(1, ingredientes.Count + 1);

            return ingredientes.Take(randomNum).ToList();         
        }
    }
}
