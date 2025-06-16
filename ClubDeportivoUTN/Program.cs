using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace ClubDeportivoUTN
{
    //enunm
    public enum CategoriaSocio { Infantil, Juvenil, Adulto }
    public enum PuestoEmpleado { Entrenador, Administrativo, Mantenimiento }
    public enum TipoInstalacion { CanchaFutbol, CanchaTenis, Pileta, Gimnasio }

    //interfaz
    public interface IPagable
    {
        double CalcularPago();
    }

    //clases
    public class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public Persona() { }

        public Persona(string nombre, string apellido, string dni, DateTime fechaNacimiento)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FechaNacimiento = fechaNacimiento;
        }

        public override string ToString()
        {
            return $"{Nombre} {Apellido} - DNI: {Dni}";
        }
    }

    public class Socio : Persona, IPagable
    {
        public CategoriaSocio Categoria { get; set; }
        public double CuotaMensual { get; set; }

        public Socio() { }

        public Socio(string nombre, string apellido, string dni, DateTime fechaNacimiento, CategoriaSocio categoria, double cuota)
            : base(nombre, apellido, dni, fechaNacimiento)
        {
            Categoria = categoria;
            CuotaMensual = cuota;
        }

        public double CalcularPago() => CuotaMensual;

        public override string ToString()
        {
            return base.ToString() + $" | Categoría: {Categoria} | Cuota: ${CuotaMensual}";
        }
    }

    public class Empleado : Persona, IPagable
    {
        public PuestoEmpleado Puesto { get; set; }
        public double Sueldo { get; set; }

        public Empleado() { }

        public Empleado(string nombre, string apellido, string dni, DateTime fechaNacimiento, PuestoEmpleado puesto, double sueldo)
            : base(nombre, apellido, dni, fechaNacimiento)
        {
            Puesto = puesto;
            Sueldo = sueldo;
        }

        public double CalcularPago() => Sueldo;

        public override string ToString()
        {
            return base.ToString() + $" | Puesto: {Puesto} | Sueldo: ${Sueldo}";
        }
    }

    public class Actividad
    {
        public string Nombre { get; set; }
        public string Dias { get; set; }
        public string Horario { get; set; }
        public List<Socio> SociosInscriptos { get; set; } = new List<Socio>();

        public Actividad() { }

        public Actividad(string nombre, string dias, string horario)
        {
            Nombre = nombre;
            Dias = dias;
            Horario = horario;
        }

        public void InscribirSocio(Socio socio) => SociosInscriptos.Add(socio);

        public override string ToString() => $"{Nombre} - {Dias} - {Horario} - Inscriptos: {SociosInscriptos.Count}";
    }

    public class Instalacion
    {
        public string Nombre { get; set; }
        public TipoInstalacion Tipo { get; set; }
        public Actividad ActividadAsignada { get; set; }

        public Instalacion() { }

        public Instalacion(string nombre, TipoInstalacion tipo, Actividad actividad)
        {
            Nombre = nombre;
            Tipo = tipo;
            ActividadAsignada = actividad;
        }

        public override string ToString() => $"{Nombre} - {Tipo} - Actividad: {ActividadAsignada?.Nombre}";
    }

    public class Club
    {
        public List<Socio> Socios { get; set; } = new List<Socio>();
        public List<Empleado> Empleados { get; set; } = new List<Empleado>();
        public List<Actividad> Actividades { get; set; } = new List<Actividad>();
        public List<Instalacion> Instalaciones { get; set; } = new List<Instalacion>();

        public void GuardarDatos()
        {
            File.WriteAllText("socios.json", JsonSerializer.Serialize(Socios));
            File.WriteAllText("empleados.json", JsonSerializer.Serialize(Empleados));
            File.WriteAllText("actividades.json", JsonSerializer.Serialize(Actividades));
            File.WriteAllText("instalaciones.json", JsonSerializer.Serialize(Instalaciones));
        }

        public void CargarDatos()
        {
            if (File.Exists("socios.json")) Socios = JsonSerializer.Deserialize<List<Socio>>(File.ReadAllText("socios.json"));
            if (File.Exists("empleados.json")) Empleados = JsonSerializer.Deserialize<List<Empleado>>(File.ReadAllText("empleados.json"));
            if (File.Exists("actividades.json")) Actividades = JsonSerializer.Deserialize<List<Actividad>>(File.ReadAllText("actividades.json"));
            if (File.Exists("instalaciones.json")) Instalaciones = JsonSerializer.Deserialize<List<Instalacion>>(File.ReadAllText("instalaciones.json"));
        }
    }

    
    class Program
    {
        static Club club = new Club();

        static void Main(string[] args)
        {
            club.CargarDatos();
            int opcion;

            do
            {
                Console.WriteLine("\n Menú Principal ");
                Console.WriteLine("1. Agregar Socio");
                Console.WriteLine("2. Agregar Empleado");
                Console.WriteLine("3. Listar Socios");
                Console.WriteLine("4. Listar Empleados");
                Console.WriteLine("5. Agregar Actividad");
                Console.WriteLine("6. Agregar Instalación");
                Console.WriteLine("7. Guardar y Salir");
                Console.Write("Seleccione una opción: ");

                opcion = int.Parse(Console.ReadLine());
                Console.Clear();

                switch (opcion)
                {
                    case 1:
                        Console.Write("Nombre: "); string n = Console.ReadLine();
                        Console.Write("Apellido: "); string a = Console.ReadLine();
                        Console.Write("DNI: "); string d = Console.ReadLine();
                        Console.Write("Fecha Nacimiento (yyyy-mm-dd): "); DateTime f = DateTime.Parse(Console.ReadLine());
                        Console.Write("Categoría (0:Infantil, 1:Juvenil, 2:Adulto): "); CategoriaSocio c = (CategoriaSocio)int.Parse(Console.ReadLine());
                        Console.Write("Cuota mensual: "); double q = double.Parse(Console.ReadLine());
                        club.Socios.Add(new Socio(n, a, d, f, c, q));
                        break;
                    case 2:
                        Console.Write("Nombre: "); n = Console.ReadLine();
                        Console.Write("Apellido: "); a = Console.ReadLine();
                        Console.Write("DNI: "); d = Console.ReadLine();
                        Console.Write("Fecha Nacimiento (yyyy-mm-dd): "); f = DateTime.Parse(Console.ReadLine());
                        Console.Write("Puesto (0:Entrenador, 1:Administrativo, 2:Mantenimiento): "); PuestoEmpleado p = (PuestoEmpleado)int.Parse(Console.ReadLine());
                        Console.Write("Sueldo: "); double s = double.Parse(Console.ReadLine());
                        club.Empleados.Add(new Empleado(n, a, d, f, p, s));
                        break;
                    case 3:
                        Console.WriteLine("\n Socios ");
                        club.Socios.ForEach(Console.WriteLine);
                        break;
                    case 4:
                        Console.WriteLine("\n Empleados ");
                        club.Empleados.ForEach(Console.WriteLine);
                        break;
                    case 5:
                        Console.Write("Nombre de actividad: "); string act = Console.ReadLine();
                        Console.Write("Días: "); string dias = Console.ReadLine();
                        Console.Write("Horario: "); string horario = Console.ReadLine();
                        club.Actividades.Add(new Actividad(act, dias, horario));
                        break;
                    case 6:
                        Console.Write("Nombre instalación: "); string nom = Console.ReadLine();
                        Console.Write("Tipo (0:Cancha de futbol, 1:Cancha de tenis, 2:Pileta, 3:Gimnasio): "); TipoInstalacion tipo = (TipoInstalacion)int.Parse(Console.ReadLine());
                        Console.WriteLine("Seleccione actividad por número:");
                        for (int i = 0; i < club.Actividades.Count; i++)
                            Console.WriteLine($"{i}: {club.Actividades[i].Nombre}");
                        int index = int.Parse(Console.ReadLine());
                        club.Instalaciones.Add(new Instalacion(nom, tipo, club.Actividades[index]));
                        break;
                    case 7:
                        club.GuardarDatos();
                        Console.WriteLine("Datos guardados.");
                        break;
                }
            } while (opcion != 7);
        }
    }
}
