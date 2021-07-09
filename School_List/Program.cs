using System;
using System.Data;
using School.Repository;
using School.Models;
using System.Linq;
using System.Configuration;
using System.Data.Common;

namespace School_List
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper consoleHelper = new ConsoleHelper(@"server = DESKTOP-V7759FH\SQLEXPRESS; database = School; integrated security=true");
        }
    }
}
