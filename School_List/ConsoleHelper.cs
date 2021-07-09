using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Models;

namespace School_List
{
    class ConsoleHelper
    {
        private Commands _commands;
        public ConsoleHelper(string connectionString)
        {
            _commands = new Commands(connectionString);
            Instruction();
            Start();
        }

        public void Start()
        {
            ConsoleKeyInfo cki;

            do
            {
                byte command = new byte();
                command = byte.Parse(Console.ReadLine());
                string textPar;
                int numberPar;
                byte bytePar;

                switch (command)
                {
                    case 1:
                        Student student = new Student();

                        Console.Write("Name : ");
                        student.Name = Console.ReadLine();
                        Console.Write("Lastname : ");
                        student.LastName = Console.ReadLine();
                        Console.Write("Personal ID : ");
                        student.PersonalId = int.Parse(Console.ReadLine());
                        Console.Write("Age : ");
                        student.Age = byte.Parse(Console.ReadLine());
                        Console.WriteLine("Gender : 1) F, 2) M");
                        textPar = Console.ReadLine().ToUpper();
                        if (textPar == "M")
                            student.Gender = Gender.Male;
                        if (textPar == "F")
                            student.Gender = Gender.Female;
                        Console.WriteLine();
                        _commands.AddPerson(student);
                        break;

                    case 2:
                        Console.Write("Personal ID : ");
                        numberPar = int.Parse(Console.ReadLine());
                        _commands.RemovePerson(numberPar);
                        Console.WriteLine();
                        break;
                    case 3:
                        Console.Write("Subject name : ");
                        textPar = Console.ReadLine();
                        Console.WriteLine();
                        _commands.AddSubject(textPar);
                        break;
                    case 4:
                        Console.Write("Subject name : ");
                        textPar = Console.ReadLine();
                        Console.Write("Personal ID :");
                        numberPar = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        _commands.AddPersonToSubject(numberPar, textPar);
                        break;
                    case 5:
                        Console.Write("Personal ID : ");
                        numberPar = int.Parse(Console.ReadLine());
                        Console.Write("Subject name : ");
                        textPar = Console.ReadLine();
                        Console.Write("Grade : ");
                        bytePar = byte.Parse(Console.ReadLine());
                        Console.WriteLine();
                        _commands.SetPoint(numberPar, textPar, bytePar);
                        break;
                    case 6:
                        Console.Write("Personal ID : ");
                        numberPar = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        _commands.GetPersonPoint(numberPar);
                        break;
                    case 7:
                        Console.Write("Personal ID : ");
                        numberPar = int.Parse(Console.ReadLine());
                        Console.Write("Subject name : ");
                        textPar = Console.ReadLine();
                        Console.WriteLine();
                        _commands.GetPersonPoint(numberPar, textPar);
                        break;
                    case 8:
                        Console.Write("Subject name : ");
                        textPar = Console.ReadLine();
                        Console.WriteLine();
                        _commands.GetAllPersonsPoints(textPar);
                        break;
                    case 9:
                        Console.WriteLine();
                        _commands.GetAllPersonsPoints();
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue or Esc to exit");
                cki = Console.ReadKey(true);
            } while (cki.Key != ConsoleKey.Escape);
        }

        public void Instruction()
        {
            Console.WriteLine("For closing the application press ESC\n");
            Console.WriteLine("Press 1 for adding a Person");
            Console.WriteLine("Press 2 for removing a Person");
            Console.WriteLine("Press 3 for adding a Subject");
            Console.WriteLine("Press 4 for adding a Person to Subject");
            Console.WriteLine("Press 5 for setting a Grade to a person in a specific subject");
            Console.WriteLine("Press 6 for getting a Person's grades in all subjects");
            Console.WriteLine("Press 7 for getting a Person's grade in a specific subject");
            Console.WriteLine("Press 8 for getting all Persons' grades in a specific subject");
            Console.WriteLine("Press 9 for getting all Persons's grades in all subjects");
            Console.WriteLine();
        }
    }
}
