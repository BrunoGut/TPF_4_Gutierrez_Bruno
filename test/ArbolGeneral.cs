using System;
using System.Collections.Generic;

namespace DeepSpace
{
	public class ArbolGeneral<T>
	{
		
		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato) {
			this.dato = dato;
		}
	
		public T getDatoRaiz() {
			return this.dato;
		}
	
		public List<ArbolGeneral<T>> getHijos() {
			return hijos;
		}
	
		public void agregarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Add(hijo);
		}
	
		public void eliminarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Remove(hijo);
		}
	
		public bool esHoja() {
			return this.getHijos().Count == 0;
		}
	
		public int altura() {
			return 0;
		}
	
		
		public int nivel(T dato) {
			return 0;
		}
		
		public int nivel(ArbolGeneral<T> valor){
			Cola<ArbolGeneral<T>> c = new Cola<ArbolGeneral<T>>(); //cola de arboles
			ArbolGeneral<T> arbolAux; //arbol auxiliar
			int nivel = 0;
			bool fin = false; //corte
			
			c.encolar(this); //encolo el arbol
			c.encolar(null); //encolo un separador
			
			while(!c.esVacia() && fin == false){
				arbolAux = c.desencolar(); //desencolo un nodo de cola y lo pongo en el arbol aux
				if(arbolAux != null){ //si no es un separador
					if(arbolAux.Equals(valor)){ //si el nodo que esta en arbol aux = valor, finaliza
						fin = true;
					}
					foreach(var hijo in arbolAux.hijos) //recorro y encolo los hijos de arbol aux
						c.encolar(hijo);
				}
				else{
					if(arbolAux == null){
						nivel++;
						c.encolar(null); //separador
					}
				}
				
			}
			return nivel;
		}
	}
}