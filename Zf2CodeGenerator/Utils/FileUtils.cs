using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zf2CodeGenerator.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sources">源路径</param>
        /// <param name="dest">新路径</param>
        public static void CopyDirectory(string source, string dest)
        {
            DirectoryInfo dinfo = new DirectoryInfo(source);
            //注，这里面传的是路径，并不是文件，所以不能包含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                //目标路径destName = 目标文件夹路径 + 原文件夹下的子文件(或文件夹)名字                
                String destName = Path.Combine(dest, f.Name);
                if (f is FileInfo)
                {
                    //如果是文件就复制       
                    File.Copy(f.FullName, destName, true);//true代表可以覆盖同名文件                     
                }
                else
                {
                    //如果是文件夹就创建文件夹然后复制然后递归复制              
                    Directory.CreateDirectory(destName);
                    CopyDirectory(f.FullName, destName);
                }
            }
        }

        /// <summary>
        /// 清空目录，即删除目录下的所有子目录及文件
        /// </summary>
        /// <param name="sources">目录路径</param>
        public static void ClearDirectory(string source)
        {
            DirectoryInfo dinfo = new DirectoryInfo(source);
            //注，这里面传的是路径，并不是文件，所以不能包含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                if (f is FileInfo)
                {
                    File.Delete(f.FullName);
                }
                else
                {
                    Directory.Delete(f.FullName, true);
                }
            }
        }

        /// <summary>
        /// 修改目录下的所有目录和文件的名称
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <param name="replace"></param>
        public static void ChangeFileName(string dir, string pattern, string replace, bool forceChange=false)
        {
            DirectoryInfo dinfo = new DirectoryInfo(dir);
            //注，这里面传的是路径，并不是文件，所以不能包含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                string newFullName = dinfo.FullName+"\\"+f.Name.Replace(pattern, replace);
                if (f is FileInfo) // 如果是文件
                {
                    if (f.FullName.Contains(pattern))
                    {
                        if (forceChange && File.Exists(newFullName))
                        {
                            File.Delete(newFullName);
                        }
                        File.Move(f.FullName, newFullName);
                    }
                }
                else
                {
                    if (f.FullName.Contains(pattern))
                    {
                        if (forceChange && Directory.Exists(newFullName))
                        {
                            Directory.Delete(newFullName, true);
                        }
                        Directory.Move(f.FullName, newFullName);
                    }

                    ChangeFileName(newFullName, pattern, replace, forceChange); // 递归修改名称
                }
            }
        }

        /// <summary>
        /// 替换目录下的所有文件的内容
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <param name="replace"></param>
        public static void ReplaceAllFilesContent(string dir, string pattern, string replace)
        {
            DirectoryInfo dinfo = new DirectoryInfo(dir);
            //注，这里面传的是路径，并不是文件，所以不能保含带后缀的文件                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                if (f is FileInfo) // 如果是文件
                {
                    ReplaceFileContent(f.FullName, pattern, replace);
                }
                else
                {
                    ReplaceAllFilesContent(f.FullName, pattern, replace); // 递归修改文件内容
                }
            }
        }


        /// <summary>
        /// 替换文件的内容
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        /// <param name="replace"></param>
        public static void ReplaceFileContent(string file, string pattern, string replace)
        {
            if(File.Exists(file))
            {
                string[] oldContents = File.ReadAllLines(file, new UTF8Encoding(false));
                if (oldContents != null)
                {
                    string[] newContents = new string[oldContents.Length];
                    for (int i = 0; i < oldContents.Length; i++)
                    {
                        newContents[i] = oldContents[i].Replace(pattern, replace);
                    }

                    File.WriteAllLines(file, newContents, new UTF8Encoding(false)); // 生成不带BOM的UTF8文件，不能使用 Encoding.UTF8
                }
            }
        }
    }
}
