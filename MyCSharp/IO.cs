namespace My
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;

    /// <summary>
    /// 磁盘文件读写相关函数
    /// </summary>
    public sealed partial class IO
    {

        /// <summary>
        /// 读取文件为Byte数组
        /// </summary>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>结果Byte数组（失败返回空Byte数组）</returns>
        public static byte[] ReadByte(string FilePath)
        {
            try
            {
                FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                byte[] temp = new byte[stream.Length];
                stream.Read(temp, 0, (int)stream.Length);
                stream.Dispose();
                return temp;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// 将Byte数组写入文件（覆盖）
        /// </summary>
        /// <param name="Source">Byte数组</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteByte(byte[] Source, string FilePath)
        {
            try
            {
                FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                stream.Write(Source, 0, Source.Length);
                stream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将Byte数组写入文件（追加）
        /// </summary>
        /// <param name="Source">Byte数组</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool AppendByte(byte[] Source, string FilePath)
        {
            try
            {
                FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                stream.Write(Source, (int)stream.Length, Source.Length);
                stream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 读取文件为字符串（UTF-8）
        /// </summary>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>结果字符串（失败返回空字符串）</returns>
        public static string ReadString(string FilePath)
        {
            try
            {
                StreamReader reader = new StreamReader(FilePath, Encoding.UTF8);
                string temp = reader.ReadToEnd();
                reader.Dispose();
                return temp;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 将字符串写入文件（覆盖，不包含UTF8的BOM头）
        /// </summary>
        /// <param name="Source">字符串</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteString(string Source, string FilePath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(FilePath, false, new UTF8Encoding(false));
                writer.Write(Source);
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串写入文件（追加，不包含UTF8的BOM头）
        /// </summary>
        /// <param name="Source">字符串</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool AppendString(string Source, string FilePath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(FilePath, true, new UTF8Encoding(false));
                writer.Write(Source);
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 读取文件中的一维字符串数组（UTF-8，注意文件的字符串内容为空时，返回空String数组）
        /// </summary>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>结果字符串数组（失败返回空String数组）</returns>
        public static string[] ReadStringArray(string FilePath)
        {
            if (File.Exists(FilePath) == false)
            {
                return new string[0];
            }
            FileInfo info = new FileInfo(FilePath);
            if (info.Length == 0)
            {
                return new string[0];
            }
            try
            {
                StreamReader reader = new StreamReader(FilePath, Encoding.UTF8);
                string temp = reader.ReadToEnd();
                reader.Dispose();
                if (temp.Length == 0)
                {
                    return new string[0];
                }
                string[] result = temp.Replace("\r\n", "\n").Split(new char[] { '\n' });
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i].Replace(@"\\", @"\@");
                    result[i] = result[i].Replace(@"\r", "\r");
                    result[i] = result[i].Replace(@"\n", "\n");
                    result[i] = result[i].Replace(@"\@", @"\");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 将一维字符串数组写入文件（覆盖，不包含UTF8的BOM头，注意数组的字符串内容为空时，不会实际创建或写入文件）
        /// </summary>
        /// <param name="StringArray">字符串数组</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool WriteStringArray(string[] StringArray, string FilePath)
        {
            if (StringArray.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < StringArray.Length; i++)
            {
                StringArray[i] = StringArray[i].Replace(@"\", @"\\");
                StringArray[i] = StringArray[i].Replace("\r", @"\r");
                StringArray[i] = StringArray[i].Replace("\n", @"\n");
            }
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(StringArray[0]);
                for (int j = 1; j < StringArray.Length; j++)
                {
                    builder.Append("\r\n" + StringArray[j]);
                }
                if (builder.Length == 0)
                {
                    return false;
                }
                StreamWriter writer = new StreamWriter(FilePath, false, new UTF8Encoding(false));
                writer.Write(builder);
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将一维字符串数组写入文件（追加，不包含UTF8的BOM头，注意数组的字符串内容为空时，不会实际创建或写入文件）
        /// </summary>
        /// <param name="StringArray">字符串数组</param>
        /// <param name="FilePath">文件路径（可以是相对路径）</param>
        /// <returns>是否写入成功</returns>
        public static bool AppendStringArray(string[] StringArray, string FilePath)
        {
            if (StringArray.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < StringArray.Length; i++)
            {
                StringArray[i] = StringArray[i].Replace(@"\", @"\\");
                StringArray[i] = StringArray[i].Replace("\r", @"\r");
                StringArray[i] = StringArray[i].Replace("\n", @"\n");
            }
            try
            {
                StringBuilder builder = new StringBuilder();
                if (File.Exists(FilePath))
                {
                    FileInfo info = new FileInfo(FilePath);
                    if (info.Length > 0)
                    {
                        builder.Append("\r\n");
                    }
                }
                builder.Append(StringArray[0]);
                for (int j = 1; j < StringArray.Length; j++)
                {
                    builder.Append("\r\n" + StringArray[j]);
                }
                if (builder.Length == 0)
                {
                    return false;
                }
                StreamWriter writer = new StreamWriter(FilePath, true, new UTF8Encoding(false));
                writer.Write(builder);
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 获取指定目录下的全部文件的路径列表
        /// </summary>
        /// <param name="SearchDirectory">要搜索的文件夹路径（默认为程序运行的当前文件夹）</param>
        /// <returns>包含所有文件路径的结果字符串数组（失败返回空String数组）</returns>
        public static string[] ListFile([Optional, DefaultParameterValue(@".\")] string SearchDirectory)
        {
            List<string> FileList = new List<string>();
            List<string> Directory = new List<string>();
            try
            {
                foreach (string temp in System.IO.Directory.GetFiles(SearchDirectory))
                {
                    FileList.Add(temp);
                }
                foreach (string temp in System.IO.Directory.GetDirectories(SearchDirectory))
                {
                    Directory.Add(temp);
                }
                int Index = 0;
                while (Index < Directory.Count)
                {
                    SearchDirectory = Directory[Index];
                    Index++;
                    foreach (string temp in System.IO.Directory.GetFiles(SearchDirectory))
                    {
                        FileList.Add(temp);
                    }
                    foreach (string temp in System.IO.Directory.GetDirectories(SearchDirectory))
                    {
                        Directory.Add(temp);
                    }
                }
                return FileList.ToArray();
            }
            catch (Exception ex)
            {
                return new string[0];
            }
        }



        /// <summary>
        /// 创建快捷方式文件（覆盖）
        /// </summary>
        /// <param name="TargetPath">快捷方式指向的路径（可以是相对路径，如"1.exe"）</param>
        /// <param name="LinkFilePath">快捷方式文件的路径（可以是相对路径，如"1.lnk"）</param>
        /// <param name="Arguments">打开程序的参数（例如"/?"）</param>
        /// <param name="Description">鼠标悬停在快捷方式上的描述</param>
        /// <param name="WorkingDirectory">快捷方式的起始位置（默认设置为快捷方式指向的路径的父目录）</param>
        /// <returns>是否创建成功</returns>
        public static bool WriteLinkFile(string TargetPath, string LinkFilePath, [Optional, DefaultParameterValue("")] string Arguments, [Optional, DefaultParameterValue("")] string Description, [Optional, DefaultParameterValue("")] string WorkingDirectory)
        {
            try
            {
                if (File.Exists(LinkFilePath))
                {
                    File.Delete(LinkFilePath);
                }
                if (!TargetPath.Contains(":"))
                {
                    TargetPath = Directory.GetCurrentDirectory() + @"\" + TargetPath;
                }
                if (WorkingDirectory == "")
                {
                    WorkingDirectory = Directory.GetParent(LinkFilePath).FullName;
                }
                object[] arguments = new object[] { LinkFilePath };
                bool[] copyBack = new bool[] { true };
                object objectValue = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(Interaction.CreateObject("WScript.Shell"), null, "CreateShortcut", arguments, null, null, copyBack));
                NewLateBinding.LateSet(objectValue, null, "TargetPath", new object[] { TargetPath }, null, null);
                NewLateBinding.LateSet(objectValue, null, "IconLocation", new object[] { TargetPath }, null, null);
                NewLateBinding.LateSet(objectValue, null, "Arguments", new object[] { Arguments }, null, null);
                NewLateBinding.LateSet(objectValue, null, "Description", new object[] { Description }, null, null);
                NewLateBinding.LateSet(objectValue, null, "WorkingDirectory", new object[] { WorkingDirectory }, null, null);
                NewLateBinding.LateCall(objectValue, null, "Save", new object[0], null, null, null, true);
                objectValue = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 读取快捷方式指向的路径（获得完整的绝对路径）
        /// </summary>
        /// <param name="LinkFilePath">快捷方式文件的路径（可以是相对路径，如"1.lnk"）</param>
        /// <returns>结果字符串（失败返回空字符串""）</returns>
        public static string ReadLinkFile(string LinkFilePath)
        {
            try
            {
                object[] arguments = new object[] { LinkFilePath };
                bool[] copyBack = new bool[] { true };
                return "" + NewLateBinding.LateGet(RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(Interaction.CreateObject("WScript.Shell", ""), null, "CreateShortcut", arguments, null, null, copyBack)), null, "TargetPath", new object[0], null, null, null);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}