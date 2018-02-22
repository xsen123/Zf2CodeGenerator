namespace Zf2CodeGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbGenerateType = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.btnStartGenerate = new System.Windows.Forms.Button();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTableFields = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目路径";
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Location = new System.Drawing.Point(66, 41);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(506, 21);
            this.txtProjectPath.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "模块名称";
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(66, 71);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(92, 21);
            this.txtModule.TabIndex = 3;
            this.txtModule.Text = "Application";
            this.txtModule.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtModule_KeyUp);
            this.txtModule.Leave += new System.EventHandler(this.txtModule_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "生成目标";
            // 
            // cmbGenerateType
            // 
            this.cmbGenerateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenerateType.FormattingEnabled = true;
            this.cmbGenerateType.Items.AddRange(new object[] {
            "Zend Framework 2 - All",
            "Zend Framework 2 - GURD",
            "Zend Framework 2 - Controller"});
            this.cmbGenerateType.Location = new System.Drawing.Point(66, 12);
            this.cmbGenerateType.Name = "cmbGenerateType";
            this.cmbGenerateType.Size = new System.Drawing.Size(219, 20);
            this.cmbGenerateType.TabIndex = 1;
            this.cmbGenerateType.SelectedIndexChanged += new System.EventHandler(this.cmbGenerateType_SelectedIndexChanged);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(578, 39);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(57, 25);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "选择...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "模型名称";
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(234, 71);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(92, 21);
            this.txtModel.TabIndex = 4;
            this.txtModel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtModel_KeyUp);
            this.txtModel.Leave += new System.EventHandler(this.txtModel_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(342, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "数据库表名";
            // 
            // txtTable
            // 
            this.txtTable.Location = new System.Drawing.Point(420, 71);
            this.txtTable.Name = "txtTable";
            this.txtTable.Size = new System.Drawing.Size(92, 21);
            this.txtTable.TabIndex = 5;
            // 
            // btnStartGenerate
            // 
            this.btnStartGenerate.Location = new System.Drawing.Point(211, 422);
            this.btnStartGenerate.Name = "btnStartGenerate";
            this.btnStartGenerate.Size = new System.Drawing.Size(93, 33);
            this.btnStartGenerate.TabIndex = 7;
            this.btnStartGenerate.Text = "生成代码";
            this.btnStartGenerate.UseVisualStyleBackColor = true;
            this.btnStartGenerate.Click += new System.EventHandler(this.btnStartGenerate_Click);
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.Location = new System.Drawing.Point(340, 422);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(93, 33);
            this.btnCloseForm.TabIndex = 8;
            this.btnCloseForm.Text = "关闭窗口";
            this.btnCloseForm.UseVisualStyleBackColor = true;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "数据库字段列表";
            // 
            // txtTableFields
            // 
            this.txtTableFields.Location = new System.Drawing.Point(12, 128);
            this.txtTableFields.Multiline = true;
            this.txtTableFields.Name = "txtTableFields";
            this.txtTableFields.Size = new System.Drawing.Size(623, 287);
            this.txtTableFields.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(104, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(515, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "(每行录入一个字段，字段内有多个单词以下划线或空格分隔，例如： id、name、created_time)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 460);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCloseForm);
            this.Controls.Add(this.btnStartGenerate);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.cmbGenerateType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableFields);
            this.Controls.Add(this.txtModule);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProjectPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zf2CG [v0.1.6]";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjectPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbGenerateType;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.Button btnStartGenerate;
        private System.Windows.Forms.Button btnCloseForm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTableFields;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

