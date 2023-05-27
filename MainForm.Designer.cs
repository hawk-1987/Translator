﻿namespace Translator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstErrors = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mTranslation = new System.Windows.Forms.ToolStripMenuItem();
            this.mPerform = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mTables = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.edtCode = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstErrors
            // 
            this.lstErrors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstErrors.FormattingEnabled = true;
            this.lstErrors.ItemHeight = 15;
            this.lstErrors.Location = new System.Drawing.Point(0, 356);
            this.lstErrors.Name = "lstErrors";
            this.lstErrors.Size = new System.Drawing.Size(800, 94);
            this.lstErrors.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile,
            this.mTranslation});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mOpen,
            this.mSave,
            this.toolStripMenuItem1,
            this.mExit});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(48, 20);
            this.mFile.Text = "Файл";
            // 
            // mOpen
            // 
            this.mOpen.Name = "mOpen";
            this.mOpen.Size = new System.Drawing.Size(133, 22);
            this.mOpen.Text = "Открыть";
            this.mOpen.Click += new System.EventHandler(this.mOpen_Click);
            // 
            // mSave
            // 
            this.mSave.Name = "mSave";
            this.mSave.Size = new System.Drawing.Size(133, 22);
            this.mSave.Text = "Сохранить";
            this.mSave.Click += new System.EventHandler(this.mSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(130, 6);
            // 
            // mExit
            // 
            this.mExit.Name = "mExit";
            this.mExit.Size = new System.Drawing.Size(133, 22);
            this.mExit.Text = "Выход";
            this.mExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // mTranslation
            // 
            this.mTranslation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mPerform,
            this.toolStripMenuItem2,
            this.mTables});
            this.mTranslation.Name = "mTranslation";
            this.mTranslation.Size = new System.Drawing.Size(84, 20);
            this.mTranslation.Text = "Трансляция";
            // 
            // mPerform
            // 
            this.mPerform.Name = "mPerform";
            this.mPerform.Size = new System.Drawing.Size(181, 22);
            this.mPerform.Text = "Выполнить";
            this.mPerform.Click += new System.EventHandler(this.mPerform_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(178, 6);
            // 
            // mTables
            // 
            this.mTables.Name = "mTables";
            this.mTables.Size = new System.Drawing.Size(181, 22);
            this.mTables.Text = "Таблицы символов";
            this.mTables.Click += new System.EventHandler(this.mTables_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Файлы C|*.c";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Файлы C|*.c";
            // 
            // edtCode
            // 
            this.edtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtCode.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.edtCode.Location = new System.Drawing.Point(0, 24);
            this.edtCode.Name = "edtCode";
            this.edtCode.Size = new System.Drawing.Size(800, 332);
            this.edtCode.TabIndex = 3;
            this.edtCode.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.edtCode);
            this.Controls.Add(this.lstErrors);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Транслятор с подмножества языка Си";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lstErrors;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mFile;
        private ToolStripMenuItem mOpen;
        private ToolStripMenuItem mSave;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mExit;
        private ToolStripMenuItem mTranslation;
        private ToolStripMenuItem mPerform;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem mTables;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private RichTextBox edtCode;
    }
}