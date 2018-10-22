using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FclConsoleApp.FileOperation
{
    /// <summary>
    /// https://www.cnblogs.com/wangshenhe/archive/2012/05/09/2490438.html
    /// 创建文件会出现文件被访问，以至于无法删除以及编辑。建议用上using。 
    /// using (File.Create(@"D:\TestDir\TestFile.txt"));
    /// </summary>
    public class FileStudy
    {
        public static string CurrDirectoryPath;
        static FileStudy()
        {
            CurrDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "FileDirectory");
            if (!Directory.Exists(CurrDirectoryPath))
            {
                Directory.CreateDirectory(CurrDirectoryPath);
            }
        }

        public void DirectoryOperation()
        {
            //Directory.Delete(dirPath); //删除空目录，否则需捕获指定异常处理
            //Directory.Delete(dirPath, true);//删除该目录以及其所有内容

            CreteDirStructure();
            DeleteDirectoryContent(Path.Combine(CurrDirectoryPath, "TestDir"));
        }

        public void ReadFileOperation()
        {
            //读取文件内容
            // 一、直接使用File类
            //1.public static string ReadAllText(string path);
            //2.public static string[] ReadAllLines(string path);
            //3.public static IEnumerable<string> ReadLines(string path);
            //4.public static byte[] ReadAllBytes(string path);
            //以上获得内容是一样的，只是返回类型不同罢了，根据自己需要调用。
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string readFilePath = Path.Combine(CurrDirectoryPath, "ReadFileDir");
            string file1 = Path.Combine(readFilePath, "1.txt");
            string file2 = Path.Combine(readFilePath, "2.docx");

            string read1 = File.ReadAllText(file1);
            //Console.WriteLine(read1);
            string[] read2 = File.ReadAllLines(file1);
            IEnumerable<string> read3 = File.ReadLines(file1);
            byte[] read4 = File.ReadAllBytes(file1);

            //ReadAllLines
            //Console.WriteLine("--{0}", "ReadAllLines");
            //List<string> list = new List<string>(File.ReadAllLines(file1));
            //list.ForEach(str => Console.WriteLine(str));

            //StreamReader
            using (StreamReader sr = new StreamReader(file1))
            {
                //方法一：从流的当前位置到末尾读取流
                //string fileContent = sr.ReadToEnd();
                //Console.WriteLine(fileContent);

                //方法二：一行行读取直至为NULL
                string fileContent = string.Empty;
                string strLine = string.Empty;
                while (strLine != null)
                {
                    strLine = sr.ReadLine();
                    fileContent += strLine + "\r\n";
                }
                Console.WriteLine(fileContent);
            }
        }

        public void WriteFileOperation()
        {
            string readFilePath = Path.Combine(CurrDirectoryPath, "ReadFileDir");
            string file1 = Path.Combine(readFilePath, "Write1.txt");
            string file2 = Path.Combine(readFilePath, "1.txt");

            //WriteAllLines 如果文件存在覆盖不存在新建并写入
            //File.WriteAllLines(file1, new string[] { "11111", "22222", "3333" }); 
            //File.WriteAllLines(file2, new string[] { "11111", "22222", "3333" });

            File.WriteAllText(file1, "11111\r\n22222\r\n3333\r\n");
        }

        public void FileAttributeOperation()
        {
            //只读与系统属性，删除时会提示拒绝访问

            string dirPath = Path.Combine(CurrDirectoryPath, "TestDir");
            string filePath = Path.Combine(CurrDirectoryPath, "TestDir", "File2.txt");

            Console.WriteLine("目录属性");
            Console.WriteLine(File.GetAttributes(dirPath));
            Console.WriteLine("文件属性");
            Console.WriteLine(File.GetAttributes(filePath));

            //设置属性
            File.SetAttributes(filePath, FileAttributes.ReadOnly); //设置只读属性
            Console.WriteLine(File.GetAttributes(filePath)); //ReadOnly
            File.SetAttributes(filePath, FileAttributes.Normal);
            Console.WriteLine(File.GetAttributes(filePath));

            FileInfo fi = new FileInfo(filePath);
            fi.Attributes = FileAttributes.Archive;
            Console.WriteLine(fi.Attributes);

        }

        public void GetPath()
        {
            CreteDirStructure();
            string dirPath = Path.Combine(CurrDirectoryPath, "TestDir");
            string filePath = Path.Combine(CurrDirectoryPath, "TestDir", "File2.txt");

            Console.WriteLine("<<<<<<<<<<<{0}>>>>>>>>>>", "文件路径");
            //获得当前路径
            Console.WriteLine(Environment.CurrentDirectory);
            //文件或文件夹所在目录
            Console.WriteLine(Path.GetDirectoryName(filePath));     //D:\TestDir
            Console.WriteLine(Path.GetDirectoryName(dirPath));      //D:\
                                                                    //文件扩展名
            Console.WriteLine(Path.GetExtension(filePath));         //.txt
                                                                    //文件名
            Console.WriteLine(Path.GetFileName(filePath));          //TestFile.txt
            Console.WriteLine(Path.GetFileName(dirPath));           //TestDir
            Console.WriteLine(Path.GetFileNameWithoutExtension(filePath)); //TestFile
                                                                           //绝对路径
            Console.WriteLine(Path.GetFullPath(filePath));          //D:\TestDir\TestFile.txt
            Console.WriteLine(Path.GetFullPath(dirPath));           //D:\TestDir  
                                                                    //更改扩展名
            Console.WriteLine(Path.ChangeExtension(filePath, ".jpg"));//D:\TestDir\TestFile.jpg
                                                                      //根目录
            Console.WriteLine(Path.GetPathRoot(dirPath));           //D:\      
                                                                    //生成路径
            Console.WriteLine(Path.Combine(new string[] { @"D:\", "BaseDir", "SubDir", "TestFile.txt" })); //D:\BaseDir\SubDir\TestFile.txt
                                                                                                           //生成随即文件夹名或文件名
            Console.WriteLine(Path.GetRandomFileName());
            //创建磁盘上唯一命名的零字节的临时文件并返回该文件的完整路径
            Console.WriteLine(Path.GetTempFileName());
            //返回当前系统的临时文件夹的路径
            Console.WriteLine(Path.GetTempPath());
            //文件名中无效字符
            Console.WriteLine(Path.GetInvalidFileNameChars());
            //路径中无效字符
            Console.WriteLine(Path.GetInvalidPathChars());
        }


        public void MoveFolderOperation()
        {
            //创建一个目录层级结构
            CreteDirStructure();

            string sourcePath = Path.Combine(CurrDirectoryPath, "TestDir");
            string root = Path.GetPathRoot(sourcePath);
            string destPath = Path.Combine("D:\\", sourcePath.TrimStart(root.ToCharArray()));
            //string destPath = Path.Combine(root, "DestDir");
            MoveFolder(sourcePath, destPath);
        }

        public void CopyFolderOperation()
        {
            //创建一个目录层级结构
            CreteDirStructure();

            string sourcePath = Path.Combine(CurrDirectoryPath, "TestDir");
            string root = Path.GetPathRoot(sourcePath);
            string destPath = Path.Combine("D:\\", sourcePath.TrimStart(root.ToCharArray()));
            //string destPath = Path.Combine(root, "DestDir");
            CopyFolder(sourcePath, destPath);
        }

        /// <summary>
        ///  移动文件夹中的所有文件夹与文件到另一个文件夹
        ///  如果是在同一个盘符中移动，则直接调用Directory.Move的方法即可！跨盘符则使用下面递归的方法！
        /// </summary>
        private void MoveFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    #region 创建目录
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Dest Dir Crete Fail" + ex.Message);
                    }
                    #endregion
                }

                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(sourceFile =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(sourceFile) });
                    //覆盖模式
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(sourceFile, destFile);
                });

                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(sourceDir =>
                {
                    string destDir = Path.Combine(destPath, Path.GetFileName(sourceDir));
                    //Directory.Move必须要在同一个根目录下移动才有效，不能在不同卷中移动。
                    //判断源和目标是否在一个盘符下
                    if (Path.GetPathRoot(sourcePath).ToUpper() == Path.GetPathRoot(destPath).ToUpper())
                    {
                        Directory.Move(sourceDir, destDir);
                    }
                    else
                    {
                        //不在一个盘符下 采用递归方法实现 
                        MoveFolder(sourceDir, destDir);
                    }
                });

                //当前源移动完成，尝试删除当前目录 
                if (Directory.GetFileSystemEntries(sourcePath).Length == 0)
                {
                    try
                    {
                        Directory.Delete(sourcePath);
                    }
                    catch (Exception ex)
                    {

                    }                   
                }
            }
            else
            {
                throw new DirectoryNotFoundException("Source Dir Not Exist");
            }
        }

        /// <summary>
        /// 复制文件夹中的所有文件夹与文件到另一个文件夹
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        private void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    #region 创建目标目录
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Dest Dir Create Fail!{ex.Message}");
                    }
                    #endregion
                }

                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(sourceFile =>
                {
                    string destFile = Path.Combine(destPath, Path.GetFileName(sourceFile));
                    File.Copy(sourceFile, destFile, true); //目标文件有相同的覆盖
                });

                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(sourceDir => {
                    string destDir = Path.Combine(destPath, Path.GetFileName(sourceDir));
                    CopyFolder(sourceDir, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("Source Dir Not Found!");
            }
        }

        public void FileOperation()
        {

        }

        private void CreteDirStructure()
        {
            if (Directory.Exists(Path.Combine(CurrDirectoryPath, "TestDir")))
            {
                Directory.Delete(Path.Combine(CurrDirectoryPath, "TestDir"), true);
            }
            Directory.CreateDirectory(Path.Combine(CurrDirectoryPath, "TestDir"));
            using (File.Create(Path.Combine(CurrDirectoryPath, "TestDir", "File2.txt"))) ;
            Directory.CreateDirectory(Path.Combine(CurrDirectoryPath, "TestDir", "SubTestDir"));
            using (File.Create(Path.Combine(CurrDirectoryPath, "TestDir", "SubTestDir", "File1.txt"))) ;
        }

        /// <summary>
        /// 删除指定目录下所有内容：方法二--找到所有文件和子文件夹删除
        /// </summary>
        /// <param name="dirPath"></param>
        private void DeleteDirectoryContent(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                //Directory.GetFileSystemEntries 指定目录下所有子目录及文件（递归查找的）
                foreach (string file in Directory.GetFileSystemEntries(dirPath))
                {
                    if (Directory.Exists(file))
                    {
                        Directory.Delete(file, true); //删除目录及子目录内容
                        //Directory.Delete(file); //如果目录不为空，则异常
                    }
                    else if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }


    }
}
