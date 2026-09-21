using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace AgendaSQLite
{
    // ###########################################
    // Modelo de la tabla Contactos
    // ###########################################
    [Table("Contactos")]
    public class Contacto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        // Metodo para mostrar el contacto de manera legible
        public override string ToString()
        {
            return $"[{Id}] {Nombre} - Teléfono: {Telefono} - Email: {Email}";
        }
    }

    // ##########################################
    // Creación de Base de Datos y Operaciones CRUD
    // ##########################################
    public class AgendaDatabase
    {
        private SQLiteConnection _db;

        public AgendaDatabase(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Contacto>();
        }

        public void InsertarContacto(Contacto contacto)
        {
            _db.Insert(contacto);
        }

        public List<Contacto> ObtenerTodos()
        {
            return _db.Table<Contacto>().ToList();
        }

        public Contacto BuscarPorNombre(string nombreBuscado)
        {
            // Coincidencias exactas, ignorando mayúsculas y minúsculas
            return _db.Table<Contacto>()
                      .FirstOrDefault(c => c.Nombre.ToLower() == nombreBuscado.ToLower());
        }

        // Actualiza un contacto existente en la base de datos
        public void ActualizarContacto(Contacto contacto)
        {
            _db.Update(contacto);
        }

        public void EliminarContacto(int idContacto)
        {
            _db.Delete<Contacto>(idContacto);
        }
    }

    // ##########################################
    // Programa principal para probar base de datos SQLite
    // ##########################################
    class Program
    {
        static void Main(string[] args)
        {
            // Ruta de la base de datos SQLite
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agenda.db3");
            var db = new SQLiteConnection(dbPath);
            // Inicialización de la base de datos
            db.CreateTable<Contacto>();
            db.DeleteAll<Contacto>();

            Console.WriteLine("###########################");
            Console.WriteLine("### AGENDA DE CONTACTOS ###");
            Console.WriteLine("###########################");
            Console.WriteLine($"\n");
            // 3 contactos diferentes
            if (db.Table<Contacto>().Count() == 0)
            {
                db.Insert(new Contacto { Nombre = "Victoria Gonzalez", Telefono = "299-663355", Email = "victoriag@mail.com" });
                db.Insert(new Contacto { Nombre = "Melani Riquelme", Telefono = "299-651122", Email = "melanir@mail.com" });
                db.Insert(new Contacto { Nombre = "Carlos Palacios", Telefono = "299-111222", Email = "carlosp@mail.com" });
                Console.WriteLine("3 contactos insertados con éxito.\n");
            }

            // lista de todos los contactos guardados
            Console.WriteLine("### LISTADO ###");
            List<Contacto> todosLosContactos = db.Table<Contacto>().ToList();
            foreach (var c in todosLosContactos)
            {
                Console.WriteLine(c.ToString());
            }

            // Buscador de contacto
            Console.WriteLine("### BUSCADOR ###");
            Console.Write("Ingresa el nombre del contacto a buscar: ");

            string nombreABuscar = Console.ReadLine();

            Console.WriteLine($"\nBuscando: '{nombreABuscar}'...");

            // Buscar ignorando mayúsculas/minúsculas
            var resultadoBusqueda = db.Table<Contacto>()
                                      .Where(c => c.Nombre.ToLower().Contains(nombreABuscar.ToLower()))
                                      .FirstOrDefault();

            if (resultadoBusqueda != null)
            {
                Console.WriteLine("Contacto Encontrado: " + resultadoBusqueda.ToString());

                Console.WriteLine("\n¿Desea realizar alguna acción con este contacto?");
                Console.WriteLine("1. Actualizar teléfono o email");
                Console.WriteLine("2. Eliminar contacto");
                Console.WriteLine("3. Salir");
                Console.Write("Ingresa una opción válida: ");

                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    Console.Write($"\nIngresa el nuevo teléfono (actual: {resultadoBusqueda.Telefono}) o presiona Enter para omitir: ");
                    string nuevoTel = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoTel))
                    {
                        resultadoBusqueda.Telefono = nuevoTel;
                    }

                    Console.Write($"Ingresa el nuevo email (actual: {resultadoBusqueda.Email}) o presiona Enter para omitir: ");
                    string nuevoEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoEmail))
                    {
                        resultadoBusqueda.Email = nuevoEmail;
                    }

                    db.Update(resultadoBusqueda);
                    Console.WriteLine("\n Contacto actualizado exitosamente: " + resultadoBusqueda.ToString());
                }
                else if (opcion == "2")
                {
                    db.Delete<Contacto>(resultadoBusqueda.Id);
                    Console.WriteLine($"\n Contacto {resultadoBusqueda.Nombre} fue eliminado correctamente.");

                    Console.WriteLine("### LISTADO ACTUALIZADO ###");
                    var listadoActualizado = db.Table<Contacto>().ToList();

                    if (listadoActualizado.Count == 0)
                    {
                        Console.WriteLine("La agenda no posee ningún contacto.");
                    }
                    else
                    {
                        foreach (var c in listadoActualizado)
                        {
                            Console.WriteLine(c.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nSalir....");
                }
            }
            else
            {
                Console.WriteLine("No se encontró ningún contacto con ese nombre.");
            }
            Console.WriteLine();
        }
    }
}