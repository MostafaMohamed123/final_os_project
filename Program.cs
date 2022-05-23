using System;
using System.IO;
using System.Text;

namespace OS_project2
{
    class Program
    {
        public static Directory currentDirectory;
        public static string currentPath;

        static void Main(string[] args)
        {


            Virtual_Disk.initalize("MyData.txt");
            currentPath = new string(currentDirectory.fileorDirName);
            while (true)
            {
                Console.Write(currentPath.Trim());
                string inputuser = Console.ReadLine();
                if (!inputuser.Contains(" "))
                {
                    if (inputuser.ToLower() == "help")
                    {
                        cmd.help();
                    }
                    else if (inputuser.ToLower() == "quit")
                    {
                        cmd.quit();
                    }
                    else if (inputuser.ToLower() == "cls")
                    {
                        cmd.cls();
                    }
                    else if (inputuser.ToLower() == "md")
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (inputuser.ToLower() == "rd")
                    {
                        Console.WriteLine("The syntax of the command is incorrect.");
                    }
                    else if (inputuser.ToLower() == "cd")
                    {
                        if (currentDirectory.parent != null)
                        {
                            currentDirectory = currentDirectory.parent;
                            currentPath = new string(currentDirectory.fileorDirName);
                        }
                        currentPath = new string(currentDirectory.fileorDirName);
                    } 
                    else if (inputuser.ToLower() == "dir")
                    {
                        cmd.dir();
                    }
                    else
                    {
                        Console.WriteLine($"\"{inputuser}\" is not recognized as an internal or external command, operable program or batch file.");
                    }
                }
                else if (inputuser.Contains(" "))
                {
                    string[] arrInput = inputuser.Split(" ");
                    if (arrInput[0] == "md")
                    {
                        cmd.md(arrInput[1]);
                    }
                    else if (arrInput[0] == "type")
                    {
                        cmd.type(arrInput[1]);
                    }
                    else if (arrInput[0] == "del")
                    {
                        cmd.del(arrInput[1]);
                    }
                    else if (arrInput[0] == "import")
                    {
                        cmd.import(arrInput[1]);
                    }
                    else if (arrInput[0] == "rd")
                    {
                        cmd.rd(arrInput[1]);
                    }
                    else if (arrInput[0] == "cd")
                    {
                        cmd.cd(arrInput[1]);
                    }
                    else if (arrInput[0] == "help")
                    {
                        if (arrInput[1] == "md")
                        {
                            Console.WriteLine("md\t\t Creates a directory");
                        }
                        else if (arrInput[1] == "cd")
                        {
                            Console.WriteLine("cd\t\t Display the name of or change the current directory");
                        }
                        else if (arrInput[1] == "cls")
                        {
                            Console.WriteLine("cls\t\t Clear the screen");
                        }
                        else if (arrInput[1] == "quit")
                        {
                            Console.WriteLine("quit\t\t Quits the CMD.exe program");
                        }
                        else if (arrInput[1] == "rd")
                        {
                            Console.WriteLine("rd\t\t Removes (deletes) a directory.");
                        }
                        else if (arrInput[1] == " ")
                        {
                            cmd.help();
                        }
                    }
                    else if (arrInput[0] == "export")
                    {
                        cmd.export(arrInput[1], arrInput[2]);
                    }
                    else if (arrInput[0] == "rename")
                    {
                        cmd.rename(arrInput[1], arrInput[2]);
                    }

                }

            }
            
            


        }
    }
}
