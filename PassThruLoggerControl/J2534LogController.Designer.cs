using System;
using System.Windows.Forms;

namespace PassThruLoggerControl
{
    partial class J2534LogController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(J2534LogController));
            this.logsavewindow = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.loggerconnections = new System.Windows.Forms.DataGridView();
            this.logpreview = new System.Windows.Forms.ListBox();
            this.savelog = new System.Windows.Forms.Button();
            this.defaultdriver = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loggerconnections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // logsavewindow
            // 
            this.logsavewindow.DefaultExt = "j2534Log";
            this.logsavewindow.Title = "Save the J2534 Log file";
            this.logsavewindow.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.savelog, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.defaultdriver, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(770, 332);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 3);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.loggerconnections);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logpreview);
            this.splitContainer1.Size = new System.Drawing.Size(764, 292);
            this.splitContainer1.SplitterDistance = 125;
            this.splitContainer1.TabIndex = 7;
            // 
            // loggerconnections
            // 
            this.loggerconnections.AllowUserToAddRows = false;
            this.loggerconnections.AllowUserToDeleteRows = false;
            this.loggerconnections.AllowUserToOrderColumns = true;
            this.loggerconnections.AllowUserToResizeRows = false;
            this.loggerconnections.AutoGenerateColumns = false;
            this.loggerconnections.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.loggerconnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.loggerconnections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.eventCountDataGridViewTextBoxColumn,
            this.driverDataGridViewTextBoxColumn,
            this.clientDataGridViewTextBoxColumn});
            this.loggerconnections.DataSource = this.connectionBindingSource;
            this.loggerconnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggerconnections.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.loggerconnections.Location = new System.Drawing.Point(0, 0);
            this.loggerconnections.MultiSelect = false;
            this.loggerconnections.Name = "loggerconnections";
            this.loggerconnections.ReadOnly = true;
            this.loggerconnections.RowHeadersVisible = false;
            this.loggerconnections.RowTemplate.Height = 24;
            this.loggerconnections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.loggerconnections.Size = new System.Drawing.Size(764, 125);
            this.loggerconnections.TabIndex = 5;
            this.loggerconnections.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.loggerconnections_RowEnter);
            this.loggerconnections.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.loggerconnections_RowLeave);
            // 
            // logpreview
            // 
            this.logpreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logpreview.FormattingEnabled = true;
            this.logpreview.HorizontalScrollbar = true;
            this.logpreview.ItemHeight = 16;
            this.logpreview.Location = new System.Drawing.Point(0, 0);
            this.logpreview.Name = "logpreview";
            this.logpreview.Size = new System.Drawing.Size(764, 163);
            this.logpreview.TabIndex = 0;
            // 
            // savelog
            // 
            this.savelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savelog.Enabled = false;
            this.savelog.Location = new System.Drawing.Point(673, 301);
            this.savelog.Name = "savelog";
            this.savelog.Size = new System.Drawing.Size(94, 28);
            this.savelog.TabIndex = 10;
            this.savelog.Text = "Save Log";
            this.savelog.UseVisualStyleBackColor = true;
            this.savelog.Click += new System.EventHandler(this.savelog_Click);
            // 
            // defaultdriver
            // 
            this.defaultdriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultdriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultdriver.FormattingEnabled = true;
            this.defaultdriver.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.defaultdriver.Location = new System.Drawing.Point(104, 301);
            this.defaultdriver.MinimumSize = new System.Drawing.Size(114, 0);
            this.defaultdriver.Name = "defaultdriver";
            this.defaultdriver.Size = new System.Drawing.Size(563, 24);
            this.defaultdriver.TabIndex = 9;
            this.defaultdriver.SelectedIndexChanged += new System.EventHandler(this.defaultdriver_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 305);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 27);
            this.label1.TabIndex = 8;
            this.label1.Text = "Default Driver";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Width = 40;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 80;
            // 
            // eventCountDataGridViewTextBoxColumn
            // 
            this.eventCountDataGridViewTextBoxColumn.DataPropertyName = "EventCount";
            this.eventCountDataGridViewTextBoxColumn.HeaderText = "EventCount";
            this.eventCountDataGridViewTextBoxColumn.Name = "eventCountDataGridViewTextBoxColumn";
            this.eventCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.eventCountDataGridViewTextBoxColumn.Width = 114;
            // 
            // driverDataGridViewTextBoxColumn
            // 
            this.driverDataGridViewTextBoxColumn.DataPropertyName = "Driver";
            this.driverDataGridViewTextBoxColumn.HeaderText = "Driver";
            this.driverDataGridViewTextBoxColumn.Name = "driverDataGridViewTextBoxColumn";
            this.driverDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverDataGridViewTextBoxColumn.Width = 150;
            // 
            // clientDataGridViewTextBoxColumn
            // 
            this.clientDataGridViewTextBoxColumn.DataPropertyName = "Client";
            this.clientDataGridViewTextBoxColumn.HeaderText = "Client";
            this.clientDataGridViewTextBoxColumn.Name = "clientDataGridViewTextBoxColumn";
            this.clientDataGridViewTextBoxColumn.ReadOnly = true;
            this.clientDataGridViewTextBoxColumn.Width = 150;
            // 
            // connectionBindingSource
            // 
            this.connectionBindingSource.DataSource = typeof(PassThruLoggerControl.ConnectionInfo);
            // 
            // J2534LogController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 332);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "J2534LogController";
            this.Text = "PassThruLoggerControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loggerconnections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.connectionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog logsavewindow;
        private BindingSource connectionBindingSource;
        private TableLayoutPanel tableLayoutPanel1;
        private SplitContainer splitContainer1;
        private DataGridView loggerconnections;
        private DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn eventCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn driverDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn clientDataGridViewTextBoxColumn;
        private ListBox logpreview;
        private Button savelog;
        private ComboBox defaultdriver;
        private Label label1;
    }
}

