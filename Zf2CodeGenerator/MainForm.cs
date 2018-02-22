using Zf2CodeGenerator.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Zf2CodeGenerator
{
    public partial class MainForm : Form
    {
        private const string TEMPLATE_ROOT_DIR_NAME = "template";
        private const string TEMPLATE_DIR_NAME_ZF2 = "zf2";

        private string tempRootDirPath = "";
        private string templateRootDirPath = "";
        Dictionary<string, string> placeholdersOfZf2 = new Dictionary<string, string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.initPlaceholderDict();

            this.cmbGenerateType.SelectedIndex = 0;
            string currentDir = Directory.GetCurrentDirectory();
            this.tempRootDirPath = currentDir + "\\temp";
            this.txtProjectPath.Text = currentDir;
            this.templateRootDirPath = currentDir + "\\" + TEMPLATE_ROOT_DIR_NAME;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                 this.txtProjectPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnStartGenerate_Click(object sender, EventArgs e)
        {
            this.setTextControlStatus(false);
            Application.DoEvents();

            bool canGenerateCode = true;

            // 去除目录路径的前后空格
            this.txtProjectPath.Text = this.txtProjectPath.Text.Trim();
            string ppText = this.txtProjectPath.Text;
            int endIndex = ppText.Length;
            for (int i = ppText.Length-1; i >=0 ; i--)
            {
                if (ppText.Substring(i, 1) != "/" && ppText.Substring(i, 1) != @"\")
                {
                    endIndex = i;
                    break;
                }
            }
            if(endIndex!=ppText.Length) // 如果路径的最后含有\或/
            {
                this.txtProjectPath.Text = ppText.Substring(0, endIndex+1);
            }

            // 去除模块、模型和表名的所有空格
            this.txtModule.Text = this.txtModule.Text.Replace(" ", "");
            this.txtModel.Text = this.txtModel.Text.Replace(" ", "");
            this.txtTable.Text = this.txtTable.Text.Replace(" ", "");

            if (string.IsNullOrEmpty(this.txtProjectPath.Text) || Directory.Exists(this.txtProjectPath.Text)==false) // 生成的代码文件存放的目录
            {
                this.showErrorBox("项目路径不存在，请重新指定");
                canGenerateCode = false;
            }

            if (canGenerateCode && string.IsNullOrEmpty(this.txtModule.Text))
            {
                this.showErrorBox("请指定模块名称");
                canGenerateCode = false;
            }

            if (this.txtModel.Enabled && canGenerateCode && string.IsNullOrEmpty(this.txtModel.Text))
            {
                this.showErrorBox("请指定模型名称");
                canGenerateCode = false;
            }

            if (this.txtTable.Enabled && canGenerateCode && string.IsNullOrEmpty(this.txtTable.Text))
            {
                this.showErrorBox("请指定表名称");
                canGenerateCode = false;
            }

            if(canGenerateCode)
            {
                try
                {
                    if (this.cmbGenerateType.SelectedIndex == 0 || this.cmbGenerateType.SelectedIndex == 1 || this.cmbGenerateType.SelectedIndex == 2) // Zend Framework 2
                    {
                        this.generateCode4ZendFramework2();
                    }
                }
                catch(Exception ex)
                {
                    this.showErrorBox("生成代码时出现异常，请重试!\r\n-->"+ ex.Message);
                }
            }

            this.setTextControlStatus(true);
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void initPlaceholderDict()
        {
            this.placeholdersOfZf2.Add("Module", "#{Module}"); // 首字母大写
            this.placeholdersOfZf2.Add("module", "#{module}"); // 全部小写

            this.placeholdersOfZf2.Add("Model", "#{Model}"); // 首字母大写
            this.placeholdersOfZf2.Add("model", "#{model}"); // 全部小写

            this.placeholdersOfZf2.Add("table", "#{table}"); // 全部小写 （在模板文件中，只有全部小写的table标记）

            this.placeholdersOfZf2.Add("Date", "#{Date}"); // 日期 yyyy/MM/dd
            this.placeholdersOfZf2.Add("Time", "#{Time}"); // 时间 HH:mm
        }

        private void setTextControlStatus(bool status)
        {
            bool changeToReadOnly = !status;

            this.txtProjectPath.ReadOnly = changeToReadOnly;
            this.txtModule.ReadOnly = changeToReadOnly;
            this.txtModel.ReadOnly = changeToReadOnly;
            this.txtTable.ReadOnly = changeToReadOnly;
            this.txtTableFields.ReadOnly = changeToReadOnly;
        }

        private void txtModule_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtModule.Text.Length == 1) // 首字母强制大写
            {
                this.txtModule.Text = this.txtModule.Text.ToUpper();
                this.txtModule.Focus();
                this.txtModule.Select(this.txtModule.TextLength, 0);
            }
        }

        private void txtModule_Leave(object sender, EventArgs e)
        {
            if (this.txtModule.Text.Length > 0) // 首字母强制大写
            {
                this.txtModule.Text = this.txtModule.Text.Substring(0, 1).ToUpper() + this.txtModule.Text.Substring(1);
            }
        }

        private void txtModel_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtModel.Text.Length == 1) // 首字母强制大写
            {
                this.txtModel.Text = this.txtModel.Text.ToUpper();
                this.txtModel.Focus();
                this.txtModel.Select(this.txtModel.TextLength, 0);
            }
        }

        private void txtModel_Leave(object sender, EventArgs e)
        {
            if (this.txtModel.Text.Length > 0) // 首字母强制大写
            {
                this.txtModel.Text = this.txtModel.Text.Substring(0, 1).ToUpper() + this.txtModel.Text.Substring(1);
            }
        }

        /// <summary>
        /// 弹出错误提示框
        /// </summary>
        /// <param name="message"></param>
        private void showErrorBox(string message)
        {
            MessageBox.Show(message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 弹出警告提示框
        /// </summary>
        /// <param name="message"></param>
        private void showWarningBox(string message)
        {
            MessageBox.Show(message, "警告提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 切换生成目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enableText4GURD = this.needGenerateGURD();
            this.txtTable.Enabled = enableText4GURD;
            this.txtTableFields.Enabled = enableText4GURD;
            if (this.needGenerateGURD())
            {
                this.label4.Text = "模型名称";
                this.toolTip1.SetToolTip(this.txtModel, "");
            }
            else
            {
                this.label4.Text = "控制器前缀";
                this.toolTip1.SetToolTip(this.txtModel, "注意：不需要输入Controller。(控制名称=前缀Controller)");
            }
        }

        /// <summary>
        /// 是否需要生成GURD
        /// </summary>
        /// <returns></returns>
        private bool needGenerateGURD()
        {
            return (this.cmbGenerateType.SelectedIndex == 0 || this.cmbGenerateType.SelectedIndex == 1);
        }

        /// <summary>
        /// 是否需要生成Controller
        /// </summary>
        /// <returns></returns>
        private bool needGenerateController()
        {
            return (this.cmbGenerateType.SelectedIndex == 0 || this.cmbGenerateType.SelectedIndex == 2);
        }
    }
}
