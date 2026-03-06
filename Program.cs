using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using MySqlConnector;

class Program
{
    static async Task Main()
    {
        var connectionString =
    "server=localhost;port=3306;Database=universidad;user=root;Password=;SslMode=None";

        await using var conn = new MySqlConnection(connectionString);

        try
        {
            await conn.OpenAsync();
            Console.WriteLine("Conexión exitosa a la base de datos.");
            var insertSql = "INSERT INTO alumnos (id, nombre, fecha_de_nacimiento, carrera, fecha_ingreso, genero, direccion) VALUES (@id, @nombre, @fecha_de_nacimiento, @carrera, @fecha_ingreso, @genero, @direccion)";
            await using (var insertCmd = new MySqlCommand(insertSql, conn))
            {
                insertCmd.Parameters.AddWithValue("@id", 8);
                insertCmd.Parameters.AddWithValue("@nombre", "Ana Lopez");
                insertCmd.Parameters.AddWithValue("@fecha_de_nacimiento", "2000-01-01");
                insertCmd.Parameters.AddWithValue("@carrera", "Ingeniería en Sistemas");
                insertCmd.Parameters.AddWithValue("@fecha_ingreso", "2020-09-01");
                insertCmd.Parameters.AddWithValue("@genero", "F");
                insertCmd.Parameters.AddWithValue("@direccion", "Calle 123, Ciudad");

                int rows = await insertCmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Filas insertadas: {rows}");
            }
            var selectSql = "SELECT id, nombre, carrera FROM alumnos WHERE carrera = @carrera";
            await using (var selectCmd = new MySqlCommand(selectSql, conn))
            {
                selectCmd.Parameters.AddWithValue("@carrera", "Ingeniería en Sistemas");


                await using var reader = await selectCmd.ExecuteReaderAsync();
                Console.WriteLine("Resultados: ");
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("id");
                    string nombre = reader.GetString("nombre");
                    string grupo = reader.GetString("carrera");
                    Console.WriteLine($"ID: {id}, Nombre: {nombre}, Carrera: {grupo}");
                }

            }


        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Error MySQL: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error General: " + ex.Message);
        }
    }

}
