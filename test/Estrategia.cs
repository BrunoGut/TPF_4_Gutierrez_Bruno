
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{		
		
		public String Consulta1( ArbolGeneral<Planeta> arbol){ //distancia del camino que existe entre el planeta del Bot y la raíz.
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>(); //instancio la cola
			ArbolGeneral<Planeta> arbolAux; //instancio el arbol auxiliar para desencolar el arbol y almacenarlos en el
			int distancia = 0;
			
			c.encolar(arbol); //encolamos el arbol
			while(!c.esVacia()){ //mientras la cola no se vacie
				arbolAux = c.desencolar(); //desencolamos el arbol y lo guardamos en arbolAux
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()){ //si el nodo que esta en el arbolAux es un planeta de la IA
					distancia = arbol.nivel(arbolAux); //llamo al metodo creado en ArbolGeneral para calcular la distancia entre el nodo y la raiz del arbol
				}
				foreach(var hijo in arbolAux.getHijos()){ //recorro y encolo los hijos de arbol aux
					c.encolar(hijo);				
				}				
			}
			return "La distancia del camino que existe entre el planeta del Bot y la raiz es: " + distancia;
		}


		public String Consulta2( ArbolGeneral<Planeta> arbol){ //texto con el listado de los planetas ubicados en todos los descendientes del nodo que contiene al planeta del Bot.
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>(); //cola de arbol general que contiene planetas
			ArbolGeneral<Planeta> arbolAux; //arbol auxiliar para desencolar y guardarlo aca
			string listaPlanetas = "Planetas descendientes del Bot: ";
			bool existe = false; //corte
				
			c.encolar(arbol); //encolo el arbol pasado por parametro
			while(!c.esVacia()){	
				arbolAux = c.desencolar(); //desencolo el arbol en el arbolAux
				if(existe){ //si existe datos, le agrego al string el dato de la raiz y la poblacion
					listaPlanetas = listaPlanetas + arbolAux.getDatoRaiz().Poblacion() + " "; //Poblacion() = Retorna la población actual del planeta.
				}
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()){ //si la raiz del arbolaux es planeta del bot 
					existe = true; 
					while(!c.esVacia()){ //mientras la lista sea distinta de vacia
						c.desencolar(); //voy desencolando la lista
					}
				}
				foreach(var hijo in arbolAux.getHijos()){ //recorro y encolo los hijos de arbol aux
					c.encolar(hijo);
				}
			}
			return "Consulta 2: " + listaPlanetas;
		}


		public String Consulta3( ArbolGeneral<Planeta> arbol){ //retorna en un texto la población total y promedio por cada nivel del árbol.
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>(); //cola de arbol general que contiene planetas
			ArbolGeneral<Planeta> arbolAux; //arbol auxiliar para desencolar la cola(el arbol) y guardarlo en este arbol
			int nivel = 0; //nivel inicial
			int poblacionTotal = 0; //poblacion inicial
			int contador = 0; //contador en cero
			int promedio = 0;
			string textoConsulta = "Consulta 3: ";
			
			c.encolar(arbol); //encolo el arbol pasado por parametro
			c.encolar(null); //encolo un separador
			
			textoConsulta = textoConsulta + "Nivel: " + nivel;  
			textoConsulta = textoConsulta + " | Poblacion total: " + arbol.getDatoRaiz().Poblacion();
			textoConsulta = textoConsulta + " | Promedio: " + arbol.getDatoRaiz().Poblacion();
			
			while(!c.esVacia()){ //mientras la cola no este vacia
				arbolAux = c.desencolar(); //desencolo y guardo en arbolAux
				if(arbolAux != null){
					foreach(var hijo in arbolAux.getHijos()){ //recorro y encolo los hijos de arbol aux
						contador++; //aumento el contador 
						poblacionTotal = poblacionTotal + hijo.getDatoRaiz().Poblacion(); //calculo la poblacion total de cada hijo de arbolAux
						c.encolar(hijo);
					}
					if(arbolAux.getHijos().Count > 0){ //si arbolAux tiene hijos 
						promedio = poblacionTotal / contador; //calculo el promedio
					}
				}
				else{ //si arbol aux == nulo
					if(!c.esVacia()){ //si la cola no esta vacia
						nivel++; //aumento el nivel
						textoConsulta = textoConsulta + "\n                   Nivel: " + nivel;
						textoConsulta = textoConsulta + " | PoblacionTotal: " + poblacionTotal;
						textoConsulta = textoConsulta + " | Promedio: " + promedio;
						c.encolar(null);
						poblacionTotal = 0;
						promedio = 0;
						contador = 0;
					}						
				}
			}
			return textoConsulta;
		}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol) //calcula el movimiento que realizara el bot en el juego
		{
			//Instanciacion de listas con diferentes caminos y estrategias
			List<Planeta> listaAtaque = ejecutaBot(arbol); //le asignamos la estrategia para aracar al bot, mediante ejecutaBot
			List<Planeta> caminoAbot = obtenerCamino(arbol, true); //lista caminoAbot, que obtendra un camino mediante el metodo obternerCamino
			List<Planeta> caminoAjugador = obtenerCamino(arbol, false); //lista botAjugador, similar a la lista anterior, pero representara el camino del jugador, obtenido mediante el metodo obternerCamino
			List<Planeta> botAjugador = calcularCaminoBotAJugador(caminoAbot, caminoAjugador); //lista que almacenara el camino desde el bot hacia el jugador

			if (botAjugador[1].EsPlanetaDelJugador()) //si el elemento en pos 1 de la lista botAjugador es planeta del jugador
			{
				botAjugador = calculoEstrategia(listaAtaque, caminoAjugador); //ajusta la estrategia para buscar un camino del bot al jugador
				listaAtaque = calcularCaminoBotAJugador(caminoAbot, caminoAjugador); //mediante caminoBotAcamino busca un camino desde el bot hacia el jugador
				if (listaAtaque[0].Poblacion() > listaAtaque[1].Poblacion()) //si la poblacion del primer planeta de la listaAtaque es mayor a la poblacion del segundo elemento
				{
					botAjugador = calcularCaminoBotAJugador(caminoAbot, caminoAjugador); //se dirige el camino utilizando el metodo caminoBotAcaminoJugador
				}
			}
			Movimiento movAtaque = new Movimiento(botAjugador[0], botAjugador[1]); //creamos un objeto perteneciente a la clase Movimiento, la cual recibe por parametro
																				//un planeta origen (botJugador[0]) y un planeta destino (botJugador[0])
			return movAtaque; //retornamos el movimiento creado, llamado movAtaque
		}

		private bool calcularCamino(ArbolGeneral<Planeta> arbol, List<Planeta> camino, bool bot) //metodo para encontrar un camino, ya sea para el bot o el jugador
		{
			bool hayCamino = false; //cuando encontremos un camino, devuelve true
			camino.Add(arbol.getDatoRaiz()); //agregamos la raiz al camino
			if ((bot && arbol.getDatoRaiz().EsPlanetaDeLaIA()) || (!bot && arbol.getDatoRaiz().EsPlanetaDelJugador())){ //si bot == true y la raiz del arbol es planeta de la IA, hay camino. O, si bot es false (es decir es el jugador), y la raiz dela arbol pertenece a un planeta del jugador, hay camino.
				hayCamino = true; //encontro un camino
			}
			else
			{
				foreach (var elem in arbol.getHijos()) //recorremos los hijos del arbol de manera recursiva para buscar el hijo planeta del bot
				{
					hayCamino = calcularCamino(elem, camino, bot);
					if (hayCamino){ //si existe camino retorna verdadero
						return true;
					}
					camino.RemoveAt(camino.Count - 1); //sino, eliminamos el ultimo elemento y vuelve un paso atras en el recorrido
				}
			}
			return hayCamino; //retornamos el camino
		}
		
		private List<Planeta> obtenerCamino(ArbolGeneral<Planeta> arbol, bool bot) //metodo para encontrar un camino
		{
			List<Planeta> camino = new List<Planeta>(); //lista camino para guardar el camino

			calcularCamino(arbol, camino, bot); //llamamos a la funcion ObtenerCamino con el arbol, el camino y el bot
			return camino; //retornamos el camino
		}
		
		private List<Planeta> calcularCaminoBotAJugador(List<Planeta> caminoBot, List<Planeta> caminoJugador) //camino desde el bot hacia el jugador, retornando el camino ordenado
		{
			//declaracion de listas para clasificar los planetas
			List<Planeta> planetasBot = new List<Planeta>();
			List<Planeta> planetaNeutral = new List<Planeta>();
			List<Planeta> planetasJugador = new List<Planeta>();
			List<Planeta> ancestro = new List<Planeta>();
			List<Planeta> botAjugador = new List<Planeta>();
			Planeta ancestro1;
			bool existe = false;

			for (int i = 0; i < caminoBot.Count && i < caminoJugador.Count; i++) //recorre las listas caminoBot y caminoJugador, si el planeta[i] de la lista caminoBot y caminoJugdor son iguales, añande ese planeta a la lista de ancestros
			{
				if (caminoBot[i] == caminoJugador[i]) //para encontrar los ancestros
				{
					ancestro.Add(caminoBot[i]);
				}
			}
			ancestro1 = ancestro[ancestro.Count - 1]; //obtiene el ultimo ancestro 
			for (int i = caminoBot.Count - 1; i >= 0; i--) //bucle para construir el camino del bot hacia el jugador
			{
				botAjugador.Add(caminoBot[i]); //agrega cada planeta a la lista botAjugador
				if (caminoBot[i] == ancestro1) //corta cuando encuentra al ancestro
				{
					break;
				}
			}
			//Agrego el planeta al camino hacia el jugador
			foreach(var elem in caminoJugador) //recorre la lista caminoJugador
			{
				if(existe)
				{
					botAjugador.Add(elem); //agrega cada planeta a la lista botAjugador
				}
				if(elem == ancestro1) //si el planeta es == al ancestro1, existe = true;
				{
					existe = true;
				}
			}
			
			foreach (var elem in botAjugador) //clasificacion de planetas
			{
				if (elem.EsPlanetaDeLaIA())
				{
					planetasBot.Add(elem);
				}
				if (elem.EsPlanetaDelJugador())
				{
					planetasJugador.Add(elem);
				}
				if (elem.EsPlanetaNeutral())
				{
					planetaNeutral.Add(elem);
				}
			}
			//limpiamos el camino y lo volvemos a reconstruir ordenadamente, para luego devolverlo
			botAjugador.Clear();
			botAjugador.Add(planetasBot[planetasBot.Count - 1]); //comenzamos nuevamente el recorrido, agregando el ultimo planeta de planetasBot
			foreach (var elem in planetaNeutral) //recorremos los planetas de planeta neutral
			{
				botAjugador.Add(elem); //agregamos los planetas al camino botAjugador
			}
			botAjugador.Add(planetasJugador[0]); //agregamos el primer planeta de planetasJugador al camino botAjugador
			return botAjugador; //retornamos el camino ordenado
		}

		private void listaBot(ArbolGeneral<Planeta> arbol, List<Planeta> listaAtaque) //recorre el arbol y agrega a la listaAtaque los planetas controlados por el bot
		{
			if (arbol.getDatoRaiz().EsPlanetaDeLaIA()){ //si la raiz del arbol es un planeta del bot
				listaAtaque.Add(arbol.getDatoRaiz()); //agrego la raiz a la lista de ataque
			}
			foreach (var elem in arbol.getHijos()) //sino, recorro los hijos del arbol, y hago un llamado recursivo para cada hijo
			{
				listaBot(elem, listaAtaque); //lamado recursivo, pasando por parametro elem(el hijo del arbol que esta recorriendoa actualmente), y la listaAtaque, donde se almacenan los planetas controlados por el bot
			}
		}
		
		private bool EstrategiaBot(ArbolGeneral<Planeta> arbol, List<Planeta> caminoAbot, Planeta ultimo) //estrategia para atacar al planeta con mayor poblacion
		{
			bool hayCamino = false; //corte
			caminoAbot.Add(arbol.getDatoRaiz()); //agrego la raiz del arbol al camino
			if (arbol.getDatoRaiz() == ultimo) //si la raiz del arbol es = ultimo, ya encontro el camino
			{
				hayCamino = true;
			}
			else
			{
				foreach (var elem in arbol.getHijos()) //sino, recorro los hijos del arbol y hago un llamado recursivo con cada uno de ellos
				{
					hayCamino = EstrategiaBot(elem, caminoAbot, ultimo); //llamado recursivo, pasando por parametro a cada hijo del arbol
					if (hayCamino) //si hay un camino, finaliza
					{
						break;
					}
					caminoAbot.RemoveAt(caminoAbot.Count - 1); //si no encontro un camino, removemos el ultimo elemento de la lista caminoAbot
				}
			}
			return hayCamino;
		}
		
		private List<Planeta> ejecutaBot(ArbolGeneral<Planeta> arbol) //retorna el camino con la estrategia de ataque del bot. Utilizamos el metodo EstrategiaBot
		{
			List<Planeta> listaAtaque = new List<Planeta>(); //creamos la lista listaAtaque para almacenar los planetas del bot
			List<Planeta> caminoBot = new List<Planeta>(); //lista camino para guardar el camino del bot
			listaBot(arbol, listaAtaque); //llamamos al metodo listaBot (que recorre y almacena los planetas controlados por el bot)
			Planeta ultimo = listaAtaque[listaAtaque.Count - 1]; //guardo el ultimo planeta de la lista, que es el que tiene mayor poblacion
			EstrategiaBot(arbol, caminoBot, ultimo); //llamo al metodo EstrategiaBot, pasando por parametro el arbol, la lista que almacena el camino del bot, y el planeta con mayor poblacion (ultimo)
			return caminoBot; //retorno el camino hacia el bot
		}
		
		private List<Planeta> calculoEstrategia(List<Planeta> listaAtaque, List<Planeta> caminoAjugador) //retorna un camino desde el bot hacia el jugador utilizando estrategias
		{
			List<Planeta> listaAncestro = new List<Planeta>(); //lista que almacena un ancestro
			List<Planeta> botAjugador = new List<Planeta>(); //lista camino botAjugador
			Planeta ancestro; //ancestro
			bool existe = false;

			for (int i = 0; i < listaAtaque.Count && i < caminoAjugador.Count; i++) //recorremos la lista ataque y caminoAjugador
			{
				if (listaAtaque.Contains(caminoAjugador[i])) //si la lista ataque contiene al elemento que este recorriendo en la lista caminoAjugdor
				{
					listaAncestro.Add(caminoAjugador[i]); //lo agregamos a la lista de ancestro
				}
			}
			ancestro = listaAncestro[listaAncestro.Count - 1]; //asignamos ancestro1 al ultimo elemento de la lista ancestros, porque significa que en este punto los caminos se separan
			for (int i = listaAtaque.Count - 1; i >= 0; i--) //bucle for hasta el primer elemento de la lista de ataque
			{
				botAjugador.Add(listaAtaque[i]); //añadimos al camino botAjugador
				//Si el elemento actual de la lista, es igual al elemento de ancestro
				//estamos en el mismo planeta y corta
				if (listaAtaque[i] == ancestro) //si el elemento que esta recorriendo es = ancestro, significa que ya encontramos el camino y corta
				{
					break; //termina el bucle, ya no hay que seguir buscando caminos porque ya lo encontro
				}
			}
			foreach (var elem in caminoAjugador) //recorremos los elementos del caminoAjugador
			{
				if (existe)
				{
					botAjugador.Add(elem); //agrego los elementos de caminoAjugador a la lista botAjugador
				}
				if (elem == ancestro) //si el elemento que estoy recorriendo actualmente es = ancestro, terminamos el recorrido
				{
					existe = true; //existe es true
				}
			}
			return botAjugador; //retorno el camino botAjugador
		}
	}
}
