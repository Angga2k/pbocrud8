using System.Data;
using Npgsql;

namespace Documents
{
    class Program
    { 
        public static void Main(string[] args) 
        {
            //Select Data Task
            //Task.data();
            //Insert Data Task
            //Task.insData("JAJAN","COKI COKI",false);
            //Update Data Task
            Task.updData("JAJAN","COKI COKI",true,11);
            //Delete Data Task
            //Task.delData(10);
        }
    }

    class Task
    {
        public static void data(){
            DatabaseHelpers db = new DatabaseHelpers();
            string sql = "select * from task";
            var aou = db.getData(sql);
            Console.WriteLine("Data :" + aou);
            for (int i = 0; i < aou.Rows.Count; i++)
            {
                Console.Write("> ");
                Console.Write(aou.Rows[i]["id_task"] + ", ");
                Console.Write(aou.Rows[i]["title"] + ", ");
                Console.Write(aou.Rows[i]["description"] + ", ");
                Console.Write(aou.Rows[i]["status"]);
                Console.WriteLine();
            }
        }
            
        public static void insData(string title, string description, bool status){
            DatabaseHelpers db = new DatabaseHelpers();
            Console.WriteLine($"Insert Data :\n" +
                              $"\tTitle : {title}\n" +
                              $"\tDescription : {description}\n" +
                              $"\tStatus : {status}\n");
            Console.Write("Are you sure [y/n] : \n");
            string ?userChoice = Console.ReadLine();
 
            if (userChoice == "y" || userChoice == "Y")
            {
                string sql = $"insert into task (title, description, status) VALUES ('{title}','{description}','{status}')";
                db.exc(sql);
                Console.WriteLine("Berhasil insert data\n");
                data();
            }
            else
            {
                Console.WriteLine("Gagal!");
            }
            
        }
        
        public static void updData(string title, string description, bool status, int id_task )
        {
            data();
            DatabaseHelpers db = new DatabaseHelpers();
            Console.WriteLine($"\nUpdate Data :\n" +
                              $"\tID Task : {id_task}\n" +
                              $"\tTitle : {title}\n" +
                              $"\tDescription : {description}\n" +
                              $"\tStatus : {status}\n");
            Console.Write("Are you sure [y/n] : ");
            string ?userChoice = Console.ReadLine();
            if (userChoice == "y" || userChoice == "Y")
            {
                var idTask = db.getData($"SELECT * FROM task WHERE id_task = {id_task}");
                if (idTask.Rows.Count == 1)
                { 
                    string sql = $"update task set title='{title}', description='{description}', status='{status}' where id_task={id_task}"; 
                    db.exc(sql); 
                    Console.WriteLine("Berhasil update data\n"); 
                    data();
                }
                else 
                { 
                    Console.WriteLine($"ID {id_task} tidak ada!");
                }
            }
            else
            {
                Console.WriteLine("Gagal!");
            }

        }

        public static void delData(int id_task)
        {
            DatabaseHelpers db = new DatabaseHelpers();
            data();
            Console.Write($"\nAre you sure delete ID Task = {id_task} [y/n] : ");
            string ?userChoice = Console.ReadLine();

            if (userChoice == "y" || userChoice == "Y")
            {
                var idTask = db.getData($"SELECT * FROM task WHERE id_task = {id_task}");
                if (idTask.Rows.Count == 1)
                {
                    string sql = $"delete from task where id_task={id_task}";
                    db.exc(sql);
                    Console.WriteLine("Berhasil hapus data");
                }
                else
                {
                    Console.WriteLine($"ID {id_task} tidak ada!");
                }
            }
            else
            {
                Console.WriteLine("Gagal");
            }
            

        }
    }

    class DatabaseHelpers 
    {
        String connString = $"Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=todolist;CommandTimeout=5";
        NpgsqlConnection? conn;
            
        public void connect()
        {
            if (conn is null)
            {
                conn= new NpgsqlConnection(connString);
            }
            conn.Open();
        }

        public DataTable getData(String sql)
        {
            DataTable table = new DataTable();
            connect();
            try
            {
                if (conn != null)
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);
                    adapter.Fill(table);    
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                conn?.Close();
            }
            return table;
        }

        public void exc(String sql)
        {
            connect();
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {   
                Console.Write(ex);
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}
