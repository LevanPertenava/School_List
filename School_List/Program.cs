using System;
using System.Data;
using School.Repository;
using School.Models;
using System.Linq;

namespace School_List
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands commands = new Commands(@"server = DESKTOP-V7759FH\SQLEXPRESS; database = School; integrated security=true");

            var dataTable = commands.GetAllPersonsPoints();
            var group = from table in dataTable.Rows.OfType<DataRow>()
                        group table by table[0] into g
                        select new { Subject = g.Key, Data = g };
            foreach (var item in group)
            {
                Console.WriteLine(item.Subject);
                foreach (var row in item.Data)
                {
                    for (int i = 1; i < row.ItemArray.Length; i++)
                    {
                        Console.Write(" " + row[i] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
