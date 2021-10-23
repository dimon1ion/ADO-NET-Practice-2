using System;
using System.Data.SqlClient;
using System.Threading;

namespace ADO_NET_Practice_2
{
    class Program
    {
        static void DataReader(string query, SqlConnection connection)
        {
            SqlDataReader sqlDataReader = new SqlCommand(query, connection).ExecuteReader();
            bool first;
            do
            {
                first = true;
                while (sqlDataReader.Read())
                {
                    if (first)
                    {
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            Console.Write(sqlDataReader.GetName(i) + " ");
                        }
                        Console.WriteLine();
                        first = false;
                    }
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        Console.Write(sqlDataReader[i] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            } while (sqlDataReader.NextResult());
            sqlDataReader.Close();
            Console.Write("Нажмите для продолжения..");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Course;Integrated Security=true;";
            string query;

            int write;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Статус: {connection.State}");
                        Console.WriteLine("Выберите действие:" +
                    "\n1. Подключиться к базе данных" +
                    "\n2. Отключиться от базы данных" +
                    "\n3. Далее" +
                    "\n4. Выход");
                        write = Int32.Parse(Console.ReadLine());
                        switch (write)
                        {
                            case 1:
                                if (connection.State == System.Data.ConnectionState.Closed)
                                {
                                    connection.Open();
                                    Console.WriteLine("Connection successful!");
                                    Thread.Sleep(1000);
                                    continue;
                                }
                                Console.WriteLine("There is already a connection");
                                continue;
                            case 2:
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    connection.Close();
                                    Console.WriteLine("The connection was successfully dropped");
                                    Thread.Sleep(1000);
                                    continue;
                                }
                                Console.WriteLine("There was no connection");
                                Thread.Sleep(1000);
                                continue;
                            case 3:
                                if (connection.State == System.Data.ConnectionState.Closed)
                                {
                                    Console.WriteLine("Отсутствует соединение!");
                                    Thread.Sleep(1000);
                                    continue;
                                }
                                break;
                            case 4:
                                return;
                            default:
                                continue;

                        }
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Выберите действие:" +
                                "\n1. Отображение всей информации из таблицы со студентами и оценками." +
                                "\n2. Отображение ФИО всех студентов." +
                                "\n3. Отображение всех средних оценок." +
                                "\n4. Показать ФИО всех студентов с минимальной оценкой, больше, чем указанная." +
                                "\n5. Показать название всех предметов с минимальными средними оценками. Названия предметов должны быть уникальными." + //Я так понял, что среди средних оценок студентов, а не указанная
                                "\n6. Показать минимальную среднюю оценку." +
                                "\n7. Показать максимальную среднюю оценку." +
                                "\n8. Показать количество студентов, у которых минимальная средняя оценка по математике." +
                                "\n9. Показать количество студентов, у которых максимальная средняя оценка по математике." +
                                "\n10. Показать количество студентов в каждой группе." +
                                "\n11. Показать среднюю оценку по группе." +
                                "\n12. Назад");

                            write = Int32.Parse(Console.ReadLine());
                            switch (write)
                            {
                                case 1:
                                    query = "SELECT Groups.GroupName, Students.FirstName, Students.LastName, Subjects.Name, Marks.Mark" +
                                        " FROM Groups, Students, Subjects, Marks " +
                                        " WHERE Marks.StudentID = Students.Id AND Marks.SubjectID = Subjects.Id AND Students.GroupId = Groups.Id";
                                    DataReader(query, connection);
                                    continue;
                                case 2:
                                    query = "SELECT Students.FirstName, Students.LastName FROM Students";
                                    DataReader(query, connection);
                                    continue;
                                case 3:
                                    query = "SELECT Students.FirstName, Students.LastName, AVG(Marks.Mark)[AVG Mark] FROM Students, Marks" +
                                        " WHERE Marks.StudentID = Students.Id" +
                                        " Group BY Students.FirstName, Students.LastName" +
                                        " ORDER BY Students.FirstName, Students.LastName";
                                    DataReader(query, connection);
                                    continue;
                                case 4:
                                    Console.Write("Укажите оценку => ");
                                    write = Int32.Parse(Console.ReadLine());
                                    query = "SELECT Students.FirstName, Students.LastName, MIN(Marks.Mark)[Min Mark]" +
                                        $" FROM Students, Marks WHERE Marks.StudentID = Students.Id AND Marks.Mark < {write}" +
                                        " GROUP BY Students.FirstName, Students.LastName" +
                                        " ORDER BY Students.FirstName, Students.LastName";
                                    DataReader(query, connection);
                                    continue;
                                case 5:
                                    query = "SELECT AvgMarks.Name, MIN(AvgMarks.Mark)[Min AVG Mark]" +
                                            " FROM(SELECT Subjects.Name, AVG(Marks.Mark)[Mark] FROM Marks, Subjects, Students" +
                                                " WHERE Marks.SubjectID = Subjects.Id AND Marks.StudentID = Students.Id" +
                                                " GROUP BY Students.FirstName, Students.LastName, Subjects.Name) as AvgMarks" +
                                            " GROUP BY AvgMarks.Name";
                                    DataReader(query, connection);
                                    continue;
                                case 6:
                                    query = "SELECT MIN(AvgMarks.Mark)[Min AVG Mark]" +
                                            " FROM(SELECT AVG(Marks.Mark)[Mark] FROM Students, Marks" +
                                               " WHERE Marks.StudentID = Students.Id" +
                                               " GROUP BY Students.FirstName, Students.LastName) as AvgMarks";
                                    DataReader(query, connection);
                                    continue;
                                case 7:
                                    query = "SELECT MAX(AvgMarks.Mark)[Max AVG Mark]" +
                                            " FROM(SELECT AVG(Marks.Mark)[Mark] FROM Students, Marks" +
                                               " WHERE Marks.StudentID = Students.Id" +
                                               " GROUP BY Students.FirstName, Students.LastName) as AvgMarks";
                                    DataReader(query, connection);
                                    continue;
                                case 8:
                                    query = "SELECT COUNT(AvgMarks.Mark)[Count of students]" +
                                                " FROM(SELECT AVG(Marks.Mark)[Mark] FROM Students, Subjects, Marks" +
                                                " WHERE Subjects.Name = 'Math' AND Marks.StudentID = Students.Id AND Marks.SubjectID = Subjects.Id" +
                                                " GROUP BY Students.FirstName, Students.LastName) as AvgMarks" +
                                            " WHERE AvgMarks.Mark = (SELECT MIN(marks.mark) FROM(SELECT AVG(Marks.Mark)[mark] FROM Marks, Students, Subjects" +
                                                                    " WHERE Subjects.Name = 'Math' AND Marks.StudentID = Students.Id AND Marks.SubjectID = Subjects.Id" +
                                                                    " GROUP BY Students.FirstName, Students.LastName) as marks)";
                                    DataReader(query, connection);
                                    continue;
                                case 9:
                                    query = "SELECT COUNT(AvgMarks.Mark)[Count of students]" +
                                               " FROM(SELECT AVG(Marks.Mark)[Mark] FROM Students, Subjects, Marks" +
                                               " WHERE Subjects.Name = 'Math' AND Marks.StudentID = Students.Id AND Marks.SubjectID = Subjects.Id" +
                                               " GROUP BY Students.FirstName, Students.LastName) as AvgMarks" +
                                           " WHERE AvgMarks.Mark = (SELECT MAX(marks.mark) FROM(SELECT AVG(Marks.Mark)[mark] FROM Marks, Students, Subjects" +
                                                                   " WHERE Subjects.Name = 'Math' AND Marks.StudentID = Students.Id AND Marks.SubjectID = Subjects.Id" +
                                                                   " GROUP BY Students.FirstName, Students.LastName) as marks)";
                                    DataReader(query, connection);
                                    continue;
                                case 10:
                                    query = "SELECT Groups.GroupName, COUNT(Students.Id)[Count of students] FROM Groups, Students" +
                                            " WHERE Students.GroupId = Groups.Id" +
                                            " GROUP BY Groups.GroupName";
                                    DataReader(query, connection);
                                    continue;
                                case 11:
                                    query = "SELECT Groups.GroupName, AVG(Marks.Mark)[Avg Mark] FROM Students, Groups, Marks" +
                                            " WHERE Students.GroupId = Groups.Id AND Marks.StudentID = Students.Id" +
                                            " GROUP BY Groups.GroupName";
                                    DataReader(query, connection);
                                    continue;
                                case 12:
                                    break;
                                default:
                                    continue;
                            }
                            break;
                        }
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error!");
            }
        }


    }
}
