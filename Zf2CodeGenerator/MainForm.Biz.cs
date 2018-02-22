using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Zf2CodeGenerator.Utils;
using System.Text.RegularExpressions;

namespace Zf2CodeGenerator
{
    /// <summary>
    /// 主窗体的业务逻辑处理类
    /// </summary>
    partial class MainForm
    {

        /// <summary>
        /// 生成Zend Framework 2项目的代码
        /// </summary>
        private void generateCode4ZendFramework2()
        {
            string templatePathOfZF2 = this.templateRootDirPath + "\\" + TEMPLATE_DIR_NAME_ZF2; // ZF2的模板目录
            if (Directory.Exists(templatePathOfZF2))
            {
                bool needGenerateCode = true;
                string medelFileRelativePath = "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\" + "Model\\" + this.txtModel.Text + ".php";
                string modelFile = this.txtProjectPath.Text + medelFileRelativePath;
                if (File.Exists(modelFile))
                {
                    DialogResult dr = MessageBox.Show("该模型文件已经存在，您确认要强制覆盖所有相关文件吗？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr.Equals(DialogResult.Yes) == false)
                    {
                        needGenerateCode = false;
                    }
                }

                if (needGenerateCode)
                {
                    if (Directory.Exists(this.tempRootDirPath))
                    {
                        FileUtils.ClearDirectory(this.tempRootDirPath); // 清空temp目录
                    }
                    else
                    {
                        Directory.CreateDirectory(this.tempRootDirPath);
                    }

                    // 1. 拷贝所有模板文件及目录结构到临时目录，并修改目录名称、文件名称及文件内容

                    // 拷贝所有模板文件及目录结构到临时目录
                    FileUtils.CopyDirectory(templatePathOfZF2, this.tempRootDirPath);

                    // 修改目录名称
                    FileUtils.ChangeFileName(this.tempRootDirPath, this.placeholdersOfZf2["Module"], this.txtModule.Text, true);
                    FileUtils.ChangeFileName(this.tempRootDirPath, this.placeholdersOfZf2["Model"], this.txtModel.Text, true);

                    // 修改模板文件中的内容
                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["Module"], this.txtModule.Text); // 替换首字母大写的标记
                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["module"], this.txtModule.Text.ToLower()); // 替换全部小写的标记

                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["Model"], this.txtModel.Text);
                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["model"], this.txtModel.Text.ToLower());

                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["Date"], DateTime.Now.ToString("yyyy/MM/dd"));
                    FileUtils.ReplaceAllFilesContent(this.tempRootDirPath, this.placeholdersOfZf2["Time"], DateTime.Now.ToString("HH:mm"));

                    string daoFileDir = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\Dao";
                    FileUtils.ReplaceAllFilesContent(daoFileDir, this.placeholdersOfZf2["table"], this.txtTable.Text);

                    if(this.needGenerateGURD())
                    {
                        // 根据指定的数据库字段生成Model的属性和setter/getter方法
                        this.generateModelPropertyAndMethods(this.tempRootDirPath + medelFileRelativePath);
                    }

                    if (this.needGenerateGURD() == false) // 不需要生成GURD的代码，删除相关目录和文件
                    {
                        this.deleteDirFile4GURD();
                    }

                    if (this.needGenerateController() == false) // 不需要生成Controller的代码，删除相关目录和文件
                    {
                        this.deleteDirFile4Controller();
                    }

                    // 2. 将临时目录中的所有目录和文件拷贝到界面中指定的项目路径中
                    FileUtils.CopyDirectory(this.tempRootDirPath, this.txtProjectPath.Text);

                    // 3. 清空temp目录
                    FileUtils.ClearDirectory(this.tempRootDirPath);

                    // 4. 修改application.config.php：modules节点中增加Module名称
                    this.modifyApplicationConfigFile(this.txtProjectPath.Text, this.txtModule.Text);

                    if (this.needGenerateGURD())
                    {
                        // 修改Module.php文件
                        this.modifyModuleFile(this.txtProjectPath.Text, this.txtModule.Text, this.txtModel.Text);
                    }

                    if (this.needGenerateController())
                    {
                        // 修改module.config.php
                        this.modifyModuleConfigFile(this.txtProjectPath.Text, this.txtModule.Text, this.txtModel.Text);
                    }

                    MessageBox.Show("代码已成功生成", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                this.showErrorBox(this.cmbGenerateType.Text + "的模板文件不存在");
            }
        }

        /// <summary>
        /// 生成模型的字段和setter/getter方法
        /// </summary>
        /// <param name="modelFilePath">模型文件全路径</param>
        private void generateModelPropertyAndMethods(string modelFilePath)
        {
            string[] inputFields = this.txtTableFields.Lines;

            if (inputFields.Length > 0)
            {
                List<string> propertyLineList = new List<string>();
                List<string> methodLineList = new List<string>();

                foreach (string tempField in inputFields)
                {
                    string field = StringUtils.Capitalize(tempField).Replace(" ", "").Replace("_", "");
                    if (string.IsNullOrEmpty(field)) { continue; }
                    
                    string property = field.Substring(0, 1).ToLower() + field.Substring(1);
                    // property
                    propertyLineList.Add("\tprotected $" + property + ";");

                    // id的setter/getter已在模板中固定提供。根据当前的框架设计，就算没有提供id字段，也需要有getId和setId两个方法
                    if (field.ToLower()!="id")
                    {
                        // setter
                        methodLineList.Add("\tpublic function set" + field + "($" + property + ")");
                        methodLineList.Add("\t{");
                        methodLineList.Add("\t\t$this->" + property + " = $" + property + ";");
                        methodLineList.Add("\t\treturn $this;");
                        methodLineList.Add("\t}");
                        // getter
                        methodLineList.Add("\tpublic function get" + field + "()");
                        methodLineList.Add("\t{");
                        methodLineList.Add("\t\treturn $this->" + property + ";");
                        methodLineList.Add("\t}");
                        methodLineList.Add(""); // 每个属性的setter/getter方法之间间隔一个空行
                    }
                }

                List<string> newModelLineList = new List<string>();
                string[] oldModelLines = File.ReadAllLines(modelFilePath, new UTF8Encoding(false));
                foreach(string line in oldModelLines)
                {
                    if(line.Contains("#{properties}"))
                    {
                        foreach(string property in propertyLineList)
                        {
                            newModelLineList.Add(property);
                        }
                    }
                    else if(line.Contains("#{methods}"))
                    {
                        foreach (string method in methodLineList)
                        {
                            newModelLineList.Add(method);
                        }
                    }
                    else
                    {
                        newModelLineList.Add(line);
                    }
                }

                string[] newModelLines = new string[newModelLineList.Count];
                newModelLineList.CopyTo(newModelLines);
                File.WriteAllLines(modelFilePath, newModelLines, new UTF8Encoding(false)); // 生成不带BOM的UTF8文件，不能使用 Encoding.UTF8
            }
        }

        /// <summary>
        /// 修改应用配置文件：application.config.php
        /// </summary>
        /// <param name="rootDirPath">工程根目录</param>
        /// <param name="moduleName">模块名称</param>
        private void modifyApplicationConfigFile(string rootDirPath, string moduleName)
        {
            string filePath = rootDirPath + "\\config\\application.config.php";

            // 判断指定的项目目录中是否存在该配置文件，如果存在则修改，不存在则创建该文件
            if (File.Exists(filePath))
            {
                // 在application.config.php文件中查找是否存在当前module的配置，不存在则添加进去
                string fileText = File.ReadAllText(filePath, new UTF8Encoding(false));
                int length = fileText.Length;

                Regex r = new Regex(@"'modules'[\s]*=>[\s]*array[\s]*\([\s\S]*\'" + moduleName + @"\'"); // 定义一个Regex对象实例
                Match m = r.Match(fileText); // 在字符串中匹配
                if (m.Success==false)
                {
                    r = new Regex(@"'modules'[\s]*=>[\s]*array[\s]*\("); // 定义一个Regex对象实例
                    m = r.Match(fileText); // 在字符串中匹配
                    if (m.Success)
                    {
                        int startIndex = m.Index;
                        string insertText = "'" + moduleName + "'";
                        string newFileText = getNewFileText(fileText, startIndex, insertText);

                        if (fileText.Equals(newFileText) == false)
                        {
                            File.WriteAllText(filePath, newFileText, new UTF8Encoding(false));
                        }
                    }
                }
                // 修改完配置文件后，删除掉dist文件（该文件仅在配置文件不存在时保留下来，以便后续手动拷贝到配置文件中去）
                File.Delete(filePath + ".dist");
            }
        }

        /// <summary>
        /// 修改Module.php文件
        /// </summary>
        /// <param name="rootDirPath">工程根目录</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="modelName">模型名称</param>
        private void modifyModuleFile(string rootDirPath, string moduleName, string modelName)
        {
            string filePath = rootDirPath + "\\module\\" + moduleName + "\\Module.php";
            if (File.Exists(filePath))
            {
                string fileText = File.ReadAllText(filePath, new UTF8Encoding(false));
                int length = fileText.Length;

                // 判断getServiceConfig方法是否存在，如果不存在则自动添加到文件中去
                Regex r = new Regex(@"function[\s]+getServiceConfig[\s]*\("); // 定义一个Regex对象实例
                Match m = r.Match(fileText); // 在字符串中匹配
                if (m.Success == false)
                {
                    string getServiceConfigMethod = System.Environment.NewLine + "    public function getServiceConfig(){"
                           + System.Environment.NewLine + "        return array("
                           + System.Environment.NewLine + "            'factories'=>array("
                           + System.Environment.NewLine + "            ),"
                           + System.Environment.NewLine + "        );"
                           + System.Environment.NewLine + "    }"
                           + System.Environment.NewLine;

                     for (int i = fileText.Length - 1; i > 0; i--)
                    {
                        string curString = fileText.Substring(i, 1);
                        if (curString == "}") // 最后一个大括弧
                        {
                            if (i > 0)
                            {
                                fileText = fileText.Insert(i - 1, getServiceConfigMethod);
                                File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
                            }
                            break;
                        }
                    }
                }

                string newFileText = null;
                r = new Regex("'" + moduleName + @"\\Service\\" + modelName + @"Service'[\s]*=>[\s]*function[\s]*\(\$sm\)"); // 定义一个Regex对象实例
                m = r.Match(fileText); // 在字符串中匹配
                if (m.Success == false)
                {
                    r = new Regex(@"'factories'[\s]*=>[\s]*array[\s]*\("); // 定义一个Regex对象实例
                    m = r.Match(fileText); // 在字符串中匹配
                    if (m.Success)
                    {
                        int startIndex = m.Index;
                        string insertText = File.ReadAllText(filePath + ".dist");
                        newFileText = getNewFileText(fileText, startIndex, insertText, true);

                        if (fileText.Equals(newFileText) == false)
                        {
                            File.WriteAllText(filePath, newFileText, new UTF8Encoding(false));
                        }
                        else
                        {
                            newFileText = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(newFileText) == false) // 文件已被修改
                {
                    fileText = newFileText;
                }
                length = fileText.Length;
                r = new Regex(@"function[\s]+_getDefaultDbSettings[\s]*\("); // 定义一个Regex对象实例
                m = r.Match(fileText); // 在字符串中匹配
                if (m.Success == false)
                {
                    string dbSettingMethod = File.ReadAllText(filePath + ".dist2");
                    for (int i = length - 1; i > 0; i--)
                    {
                        string curString = fileText.Substring(i, 1);
                        if (curString == "}") // 最后一个大括弧
                        {
                            if (i > 0)
                            {
                                fileText = fileText.Insert(i - 1, dbSettingMethod);
                                File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
                            }
                            break;
                        }
                    }
                }

                // 修改完配置文件后，删除掉dist文件（该文件仅在配置文件不存在时保留下来，以便后续手动拷贝到配置文件中去）
                File.Delete(filePath + ".dist");
                File.Delete(filePath + ".dist2");
            }
        }

        /// <summary>
        /// 修改模块配置文件：module.config.php
        /// </summary>
        /// <param name="rootDirPath">工程根目录</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="modelName">模型名称</param>
        private void modifyModuleConfigFile(string rootDirPath, string moduleName, string modelName)
        {
            string filePath = rootDirPath + "\\module\\" + moduleName + "\\config\\module.config.php";
            if (File.Exists(filePath))
            {
                string fileText = File.ReadAllText(filePath, new UTF8Encoding(false));
                int length = fileText.Length;

                string newFileText = null;
                Regex r = new Regex("'" + moduleName.ToLower() + @"'[\s]*=>[\s]*array[\s]*\("); // 定义一个Regex对象实例
                Match m = r.Match(fileText); // 在字符串中匹配
                if (m.Success == false)
                {
                    r = new Regex(@"'routes'[\s]*=>[\s]*array[\s]*\("); // 定义一个Regex对象实例
                    m = r.Match(fileText); // 在字符串中匹配
                    if (m.Success)
                    {
                        int startIndex = m.Index;
                        string insertText = File.ReadAllText(filePath + ".dist");
                        newFileText = getNewFileText(fileText, startIndex, insertText, true);

                        if (fileText.Equals(newFileText) == false)
                        {
                            File.WriteAllText(filePath, newFileText, new UTF8Encoding(false));
                        }
                        else
                        {
                            newFileText = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(newFileText) == false) // 文件已被修改
                {
                    fileText = newFileText;
                }
                length = fileText.Length;
                r = new Regex("'" + moduleName + @"\\Controller\\" + modelName + @"'[\s]*=>[\s]*'" + moduleName + @"\\Controller\\" + modelName + "Controller'"); // 定义一个Regex对象实例
                m = r.Match(fileText); // 在字符串中匹配
                if (m.Success == false)
                {
                    r = new Regex(@"'controllers'[\s]*=>[\s]*array[\s]*\([\s]*'invokables'[\s]*=>[\s]*array[\s]*\("); // 定义一个Regex对象实例
                    m = r.Match(fileText); // 在字符串中匹配
                    if (m.Success)
                    {
                        int startIndex = m.Index;
                        string insertText = "            '" + moduleName + "\\Controller\\" + modelName + "' => '" + moduleName + "\\Controller\\" + modelName + "Controller',";
                        newFileText = getNewFileText(fileText, startIndex, insertText, true, 1);

                        if (fileText.Equals(newFileText) == false)
                        {
                            File.WriteAllText(filePath, newFileText, new UTF8Encoding(false));
                        }
                    }
                }

                // 修改完配置文件后，删除掉dist文件（该文件仅在配置文件不存在时保留下来，以便后续手动拷贝到配置文件中去）
                File.Delete(filePath + ".dist");
            }
        }

        /// <summary>
        /// 查找对应的右括弧(小括弧)在文件中的索引
        /// </summary>
        /// <param name="fileText">文件文本</param>
        /// <param name="startIndex">开始查找的位置索引</param>
        /// <param name="textLength">文件文本总长度</param>
        /// <param name="numOfSkipLeftBracket">忽略几个左括弧</param>
        /// <returns></returns>
        private int getRightBracketIndex(string fileText, int startIndex, int textLength, int numOfSkipLeftBracket = 0)
        {
            string leftBracket = "(";
            string rightBracket = ")";

            int numOfLeftBracket = 0 - numOfSkipLeftBracket;
            int rightBracketIndex = -1;

            for (int i = startIndex + 1; i < textLength; i++)
            {
                string curString = fileText.Substring(i, 1);
                if (leftBracket.Equals(curString))
                {
                    numOfLeftBracket++;
                    continue;
                }
                if (rightBracket.Equals(curString))
                {
                    if (numOfLeftBracket == 1)
                    {
                        rightBracketIndex = i;
                        break;
                    }
                    else
                    {
                        numOfLeftBracket--;
                    }
                }
            }
            return rightBracketIndex;
        }

        /// <summary>
        /// 取得要写入的文件的新文本内容
        /// </summary>
        /// <param name="fileText">原文件文本内容</param>
        /// <param name="startIndex">要包含新增文本的节的起始位置索引</param>
        /// <param name="insertText">待插入的文本内容</param>
        /// <param name="startAtNewLine">插入的文本前是否需要换行，默认不换行</param>
        /// <returns></returns>
        private string getNewFileText(string fileText, int startIndex, string insertText, bool startAtNewLine = false, int numOfSkipLeftBracket = 0)
        {
            string newFileText = null;
            int rightBracketIndex = getRightBracketIndex(fileText, startIndex, fileText.Length, numOfSkipLeftBracket);
            if (fileText.Substring(startIndex, rightBracketIndex - startIndex).Contains(insertText) == false)
            {
                int destIndex = -1;
                string lastString = "";
                for (int i = rightBracketIndex - 1; i > startIndex; i--)
                {
                    string curString = fileText.Substring(i, 1);
                    if (StringUtils.isNotBlank(curString))
                    {
                        destIndex = i + 1;
                        lastString = curString;
                        break;
                    }
                }
                if (destIndex != -1)
                {
                    if (startAtNewLine)
                    {
                        insertText = insertText.Insert(0, "\n");
                    }
                    // 逗号在换行之前。前面已经有逗号或是左括号，则不需要添加逗号
                    if (lastString.Equals(",")==false && lastString.Equals("(") == false)
                    {
                        insertText = insertText.Insert(0, ",");
                    }
                    newFileText = fileText.Insert(destIndex, insertText);
                }
            }
            return string.IsNullOrEmpty(newFileText) ? fileText : newFileText;
        }

        /// <summary>
        /// 删除GURD相关的目录和文件
        /// </summary>
        private void deleteDirFile4GURD()
        {
            string daoFileDir = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\Dao";
            string modelFileDir = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\Model";
            string serviceFileDir = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\Service";
            string moduleFileDist = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\Module.php.dist";
            string moduleFileDist2 = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\Module.php.dist2";
            if (Directory.Exists(daoFileDir))
            {
                Directory.Delete(daoFileDir, true);
            }
            if (Directory.Exists(modelFileDir))
            {
                Directory.Delete(modelFileDir, true);
            }
            if (Directory.Exists(serviceFileDir))
            {
                Directory.Delete(serviceFileDir, true);
            }
            if (File.Exists(moduleFileDist))
            {
                File.Delete(moduleFileDist);
            }
            if (File.Exists(moduleFileDist2))
            {
                File.Delete(moduleFileDist2);
            }
        }

        /// <summary>
        /// 删除Controller相关的目录和文件
        /// </summary>
        private void deleteDirFile4Controller()
        {
            string controllerFileDir = this.tempRootDirPath + "\\module\\" + this.txtModule.Text + "\\src\\" + this.txtModule.Text + "\\Controller";
            if (Directory.Exists(controllerFileDir))
            {
                Directory.Delete(controllerFileDir, true);
            }
        }


    }
}
