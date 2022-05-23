using System;
using System.IO;

namespace OS_project2
{
    class cmd
    {
        public static void cls()
        {
            Console.Clear();
        }

        public static void help()
        {
            Console.WriteLine("cls\t\t Clear the screen.");
            Console.WriteLine("quit\t\t Quits the CMD.exe program.");
            Console.WriteLine("help\t\t Provides help information for windows command.");
            Console.WriteLine("md\t\t Creates a directory.");
            Console.WriteLine("cd\t\t Display the name of or change the current directory.");
            Console.WriteLine("rd\t\t Removes (deletes) a directory.");
            Console.WriteLine("dir\t\t Displays a list of files and subdirectories in a directory.");
            Console.WriteLine("import\t\t Add a file from your PC to your Virtual Disk.");
            Console.WriteLine("export\t\t Add a file from your Virtual Disk to your PC.");
            Console.WriteLine("type\t\t Displays the contents of a text file or files.");
            Console.WriteLine("rename\t\t Change the name of a directory or file.");
            Console.WriteLine("del\t\t Removes (deletes) a file.");

            
        }

        public static void quit()
        {
            System.Environment.Exit(1);
        }

        public static void md(string name)
        {
            if (Program.currentDirectory.searchDirectory(name) == -1)
            {
                Directory_Entry newdirectory = new Directory_Entry(name, 0x10, 0, 0);
                Program.currentDirectory.directoryTable.Add(newdirectory);
                Program.currentDirectory.writeDirectory();
                if (Program.currentDirectory.parent != null)
                {
                    Program.currentDirectory.parent.updateContent(Program.currentDirectory.parent);
                    Program.currentDirectory.parent.writeDirectory();
                }
            }
            else
            {
                Console.WriteLine("A subdirectory or file " + name + " already exists.");
            }
        }

        public static void rd(string name)
        {
            int index = Program.currentDirectory.searchDirectory(name);
            if (index != -1)
            {
                int firstCluster = Program.currentDirectory.directoryTable[index].fileFirstCluster;
                Directory d1 = new Directory(name, 0x10, firstCluster, 0, Program.currentDirectory);
                d1.deleteDirectory();
                Program.currentPath = new string(Program.currentDirectory.fileorDirName).Trim();
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
        }

        public static void cd(string name)
        {
            int index = Program.currentDirectory.searchDirectory(name);

            if (index != -1)
            {
                int firstCluster = Program.currentDirectory.directoryTable[index].fileFirstCluster;
                Directory d1 = new Directory(name, 0x10, firstCluster, 0, Program.currentDirectory);
                Program.currentPath = new string(Program.currentDirectory.fileorDirName).Trim() + "\\" + new string(d1.fileorDirName).Trim();
                Program.currentDirectory.writeDirectory();
                Program.currentDirectory.readDirectory();
                Program.currentDirectory = d1;

            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
        }



        public static void dir()
        {
            int fileCount = 0;
            int directoryCount = 0;
            int sizeCount = 0;
            Console.WriteLine("Directory of " + new string(Program.currentDirectory.fileorDirName));
            for (int i = 0; i < Program.currentDirectory.directoryTable.Count; i++)
            {
                if (Program.currentDirectory.directoryTable[i].filaAttribute == 0x0)
                {
                    Console.WriteLine("\t\t" + Program.currentDirectory.directoryTable[i].fileSize + "\t" + new string(Program.currentDirectory.directoryTable[i].fileorDirName));
                    fileCount++;
                    sizeCount += Program.currentDirectory.directoryTable[i].fileSize;
                }
                else
                {
                    Console.WriteLine("\t\t" + "<DIR>" + "\t" + new string(Program.currentDirectory.directoryTable[i].fileorDirName));
                    directoryCount++;
                }
            }
            Console.WriteLine("\t\t" + fileCount + " File(s)" + "\t" + sizeCount + " bytes");
            Console.WriteLine("\t\t" + directoryCount + " Dir(s)" + "\t" + Fat_Table.GetFreeSpace() + " bytes free");
        }

        public static void import(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                int name_start = filePath.LastIndexOf("\\");
                string filename = filePath.Substring(name_start + 1);
                int index = Program.currentDirectory.searchFile(filename);
                int firstcluster;
                if (index == -1)
                {
                    if (content.Length > 0)
                    {
                        firstcluster = Fat_Table.Getavaliableblock();

                    }
                    else 
                    {
                        firstcluster = 0;
                    }
                    File_Entry fe = new File_Entry(filename, 0x0, firstcluster, content.Length, content, Program.currentDirectory);
                    fe.writeFileContent();
                    Directory_Entry d = new Directory_Entry(filename, 0x0, firstcluster, content.Length);
                    Program.currentDirectory.directoryTable.Add(d);
                    Program.currentDirectory.writeDirectory();
                }
                else
                {
                    Console.WriteLine("This File is already exist");
                }
            }
            else
            {
                Console.WriteLine("This file is not exist");
            }
        }

        public static void type(string Name)
        {
            int index = Program.currentDirectory.searchFile(Name);
            if (index != -1)
            {
                if (Program.currentDirectory.directoryTable[index].filaAttribute == 0x0)
                {
                    int FirstCluster = Program.currentDirectory.directoryTable[index].fileFirstCluster;
                    int FileSize = Program.currentDirectory.directoryTable[index].fileSize;
                    string Content = string.Empty;
                    File_Entry file = new File_Entry(Name, 0x0, FirstCluster, FileSize,Content , Program.currentDirectory);
                    file.readFileContent();
                    Console.WriteLine(file.content);
                }
            }
            else if (Program.currentDirectory.searchDirectory(Name) != -1)
            { Console.WriteLine("it's folder not file"); }
            else if (index == -1 && index == -1)
            { Console.WriteLine("file Not Been created"); }
        }

        public static void export(string source, string dest)
        {
            int name_start = source.LastIndexOf(".");
            string filename = source.Substring(name_start + 1);
            if (filename == "txt")
            {
                int index = Program.currentDirectory.searchFile(source);
                if (index != -1)
                {
                    if (System.IO.Directory.Exists(dest))
                    {
                        int cluster = Program.currentDirectory.directoryTable[index].fileFirstCluster;
                        int size = Program.currentDirectory.directoryTable[index].fileSize;
                        string content = null;
                        File_Entry f = new File_Entry(source,0x0,cluster,size,content,Program.currentDirectory);
                        f.readFileContent();
                        StreamWriter st = new StreamWriter(dest + "\\" + source);
                        st.Write(f.content);
                        st.Flush();
                        st.Close();
                    }
                    else
                    {
                        Console.WriteLine("The system can not find the path");
                    }
                }
                else
                {
                    Console.WriteLine("This file is not exist");
                }
            }
        }

        public static void rename(string oldName, string newName)
        {
            int oldIndex = Program.currentDirectory.searchFile(oldName);
            if (oldIndex == -1) oldIndex = Program.currentDirectory.searchDirectory(oldName);
            if (oldIndex != -1)
            {
                int newIndex = Program.currentDirectory.searchFile(new string(newName));

                if (newIndex == -1)
                {
                    Directory_Entry d1 = new Directory_Entry(newName, Program.currentDirectory.directoryTable[oldIndex].filaAttribute, Program.currentDirectory.directoryTable[oldIndex].fileFirstCluster, Program.currentDirectory.directoryTable[oldIndex].fileSize);

                    Program.currentDirectory.directoryTable.RemoveAt(oldIndex);
                    Program.currentDirectory.directoryTable.Insert(oldIndex, d1);
                    Program.currentDirectory.writeDirectory();

                }
                else
                {
                    Console.WriteLine("new name is exist write another name");
                }
            }
            else
            {
                Console.WriteLine("no file or folder with this name");
            }
        }

        public static void del(string fileName)
        {
            int index = Program.currentDirectory.searchFile(fileName);
            if (index != -1)
            {
                if (Program.currentDirectory.directoryTable[index].filaAttribute == 0x0)
                {
                    int cluster = Program.currentDirectory.directoryTable[index].fileFirstCluster;
                    int size = Program.currentDirectory.directoryTable[index].fileSize;
                    File_Entry f = new File_Entry(fileName, 0x0, cluster, size,null , Program.currentDirectory);
                    f.deleteFile();
                }
                else
                {
                    Console.WriteLine("no file with this name");
                }
            }
        }

        public static void copy(string source, string dest)
        {
            int index = Program.currentDirectory.searchDirectory(source);
            if (index != -1)
            {
                int name_start = dest.LastIndexOf("\\");
                string filename = dest.Substring(name_start + 1);
                int index2 = Program.currentDirectory.searchDirectory(filename);
                if (index2 != -1)
                { 

                }
            }
        }

    }
}
