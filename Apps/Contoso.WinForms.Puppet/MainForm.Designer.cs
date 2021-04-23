// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Contoso.WinForms.Puppet
{
    partial class MainForm
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.AppCenterTab = new System.Windows.Forms.TabPage();
            this.MiscGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveSizeStorageButton = new System.Windows.Forms.Button();
            this.StorageMaxSizeTextBox = new System.Windows.Forms.TextBox();
            this.StorageMaxSizeLabel = new System.Windows.Forms.Label();
            this.AppCenterEnabled = new System.Windows.Forms.CheckBox();
            this.AnalyticsTab = new System.Windows.Forms.TabPage();
            this.EventBox = new System.Windows.Forms.GroupBox();
            this.TrackEvent = new System.Windows.Forms.Button();
            this.EventProperties = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventName = new System.Windows.Forms.TextBox();
            this.EventNameLabel = new System.Windows.Forms.Label();
            this.AnalyticsEnabled = new System.Windows.Forms.CheckBox();
            this.CrashesTab = new System.Windows.Forms.TabPage();
            this.ErrorAttachmentsBox = new System.Windows.Forms.GroupBox();
            this.SelectFileAttachmentButton = new System.Windows.Forms.Button();
            this.FileAttachmentPathLabel = new System.Windows.Forms.Label();
            this.FileAttachmentLabel = new System.Windows.Forms.Label();
            this.TextAttachmentTextBox = new System.Windows.Forms.TextBox();
            this.TextAttachmentLabel = new System.Windows.Forms.Label();
            this.HandleExceptions = new System.Windows.Forms.CheckBox();
            this.CrashBox = new System.Windows.Forms.GroupBox();
            this.CrashInsideAsyncTask = new System.Windows.Forms.Button();
            this.CrashWithNullReference = new System.Windows.Forms.Button();
            this.CrashWithAggregateException = new System.Windows.Forms.Button();
            this.CrashWithDivisionByZero = new System.Windows.Forms.Button();
            this.CrashWithNonSerializableException = new System.Windows.Forms.Button();
            this.CrashWithTestException = new System.Windows.Forms.Button();
            this.CrashesEnabled = new System.Windows.Forms.CheckBox();
            this.AppCenterAllowNetworkRequest = new System.Windows.Forms.CheckBox();
            this.Tabs.SuspendLayout();
            this.AppCenterTab.SuspendLayout();
            this.MiscGroupBox.SuspendLayout();
            this.AnalyticsTab.SuspendLayout();
            this.EventBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventProperties)).BeginInit();
            this.CrashesTab.SuspendLayout();
            this.ErrorAttachmentsBox.SuspendLayout();
            this.CrashBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.AppCenterTab);
            this.Tabs.Controls.Add(this.AnalyticsTab);
            this.Tabs.Controls.Add(this.CrashesTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(384, 523);
            this.Tabs.TabIndex = 0;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
            // 
            // AppCenterTab
            // 
            this.AppCenterTab.Controls.Add(this.AppCenterAllowNetworkRequest);
            this.AppCenterTab.Controls.Add(this.MiscGroupBox);
            this.AppCenterTab.Controls.Add(this.AppCenterEnabled);
            this.AppCenterTab.Location = new System.Drawing.Point(4, 22);
            this.AppCenterTab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AppCenterTab.Name = "AppCenterTab";
            this.AppCenterTab.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AppCenterTab.Size = new System.Drawing.Size(376, 497);
            this.AppCenterTab.TabIndex = 0;
            this.AppCenterTab.Text = "App Center";
            this.AppCenterTab.UseVisualStyleBackColor = true;
            // 
            // MiscGroupBox
            // 
            this.MiscGroupBox.Controls.Add(this.SaveSizeStorageButton);
            this.MiscGroupBox.Controls.Add(this.StorageMaxSizeTextBox);
            this.MiscGroupBox.Controls.Add(this.StorageMaxSizeLabel);
            this.MiscGroupBox.Location = new System.Drawing.Point(8, 72);
            this.MiscGroupBox.Name = "MiscGroupBox";
            this.MiscGroupBox.Size = new System.Drawing.Size(360, 100);
            this.MiscGroupBox.TabIndex = 2;
            this.MiscGroupBox.TabStop = false;
            this.MiscGroupBox.Text = "Misc";
            // 
            // SaveSizeStorageButton
            // 
            this.SaveSizeStorageButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSizeStorageButton.Location = new System.Drawing.Point(10, 61);
            this.SaveSizeStorageButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SaveSizeStorageButton.Name = "SaveSizeStorageButton";
            this.SaveSizeStorageButton.Size = new System.Drawing.Size(342, 23);
            this.SaveSizeStorageButton.TabIndex = 15;
            this.SaveSizeStorageButton.Text = "Save";
            this.SaveSizeStorageButton.UseVisualStyleBackColor = true;
            this.SaveSizeStorageButton.Click += new System.EventHandler(this.SaveStorageSize_Click);
            // 
            // StorageMaxSizeTextBox
            // 
            this.StorageMaxSizeTextBox.Location = new System.Drawing.Point(103, 17);
            this.StorageMaxSizeTextBox.Name = "StorageMaxSizeTextBox";
            this.StorageMaxSizeTextBox.Size = new System.Drawing.Size(249, 20);
            this.StorageMaxSizeTextBox.TabIndex = 1;
            // 
            // StorageMaxSizeLabel
            // 
            this.StorageMaxSizeLabel.AutoSize = true;
            this.StorageMaxSizeLabel.Location = new System.Drawing.Point(7, 20);
            this.StorageMaxSizeLabel.Name = "StorageMaxSizeLabel";
            this.StorageMaxSizeLabel.Size = new System.Drawing.Size(90, 13);
            this.StorageMaxSizeLabel.TabIndex = 0;
            this.StorageMaxSizeLabel.Text = "Storage Max Size";
            // 
            // AppCenterEnabled
            // 
            this.AppCenterEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppCenterEnabled.Location = new System.Drawing.Point(8, 6);
            this.AppCenterEnabled.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AppCenterEnabled.Name = "AppCenterEnabled";
            this.AppCenterEnabled.Size = new System.Drawing.Size(360, 24);
            this.AppCenterEnabled.TabIndex = 1;
            this.AppCenterEnabled.Text = "App Center Enabled";
            this.AppCenterEnabled.UseVisualStyleBackColor = true;
            this.AppCenterEnabled.CheckedChanged += new System.EventHandler(this.AppCenterEnabled_CheckedChanged);
            // 
            // AnalyticsTab
            // 
            this.AnalyticsTab.Controls.Add(this.EventBox);
            this.AnalyticsTab.Controls.Add(this.AnalyticsEnabled);
            this.AnalyticsTab.Location = new System.Drawing.Point(4, 22);
            this.AnalyticsTab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AnalyticsTab.Name = "AnalyticsTab";
            this.AnalyticsTab.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AnalyticsTab.Size = new System.Drawing.Size(376, 497);
            this.AnalyticsTab.TabIndex = 1;
            this.AnalyticsTab.Text = "Analytics";
            this.AnalyticsTab.UseVisualStyleBackColor = true;
            // 
            // EventBox
            // 
            this.EventBox.Controls.Add(this.TrackEvent);
            this.EventBox.Controls.Add(this.EventProperties);
            this.EventBox.Controls.Add(this.EventName);
            this.EventBox.Controls.Add(this.EventNameLabel);
            this.EventBox.Location = new System.Drawing.Point(8, 36);
            this.EventBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.EventBox.Name = "EventBox";
            this.EventBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.EventBox.Size = new System.Drawing.Size(360, 191);
            this.EventBox.TabIndex = 3;
            this.EventBox.TabStop = false;
            this.EventBox.Text = "Event";
            // 
            // TrackEvent
            // 
            this.TrackEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackEvent.Location = new System.Drawing.Point(10, 162);
            this.TrackEvent.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.TrackEvent.Name = "TrackEvent";
            this.TrackEvent.Size = new System.Drawing.Size(342, 23);
            this.TrackEvent.TabIndex = 14;
            this.TrackEvent.Text = "Track Event";
            this.TrackEvent.UseVisualStyleBackColor = true;
            this.TrackEvent.Click += new System.EventHandler(this.TrackEvent_Click);
            // 
            // EventProperties
            // 
            this.EventProperties.AllowUserToResizeColumns = false;
            this.EventProperties.AllowUserToResizeRows = false;
            this.EventProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EventProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            this.EventProperties.Location = new System.Drawing.Point(10, 44);
            this.EventProperties.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.EventProperties.Name = "EventProperties";
            this.EventProperties.RowHeadersWidth = 82;
            this.EventProperties.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.EventProperties.Size = new System.Drawing.Size(342, 110);
            this.EventProperties.TabIndex = 13;
            // 
            // Key
            // 
            this.Key.HeaderText = "Key";
            this.Key.MaxInputLength = 64;
            this.Key.MinimumWidth = 10;
            this.Key.Name = "Key";
            this.Key.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Key.Width = 200;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.MaxInputLength = 64;
            this.Value.MinimumWidth = 10;
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // EventName
            // 
            this.EventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventName.Location = new System.Drawing.Point(106, 18);
            this.EventName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.EventName.MaxLength = 256;
            this.EventName.Name = "EventName";
            this.EventName.Size = new System.Drawing.Size(246, 20);
            this.EventName.TabIndex = 12;
            // 
            // EventNameLabel
            // 
            this.EventNameLabel.Location = new System.Drawing.Point(6, 16);
            this.EventNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EventNameLabel.Name = "EventNameLabel";
            this.EventNameLabel.Size = new System.Drawing.Size(94, 23);
            this.EventNameLabel.TabIndex = 11;
            this.EventNameLabel.Text = "Event Name";
            this.EventNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AnalyticsEnabled
            // 
            this.AnalyticsEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnalyticsEnabled.Location = new System.Drawing.Point(8, 6);
            this.AnalyticsEnabled.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AnalyticsEnabled.Name = "AnalyticsEnabled";
            this.AnalyticsEnabled.Size = new System.Drawing.Size(360, 24);
            this.AnalyticsEnabled.TabIndex = 2;
            this.AnalyticsEnabled.Text = "Analytics Enabled";
            this.AnalyticsEnabled.UseVisualStyleBackColor = true;
            this.AnalyticsEnabled.CheckedChanged += new System.EventHandler(this.AnalyticsEnabled_CheckedChanged);
            // 
            // CrashesTab
            // 
            this.CrashesTab.Controls.Add(this.ErrorAttachmentsBox);
            this.CrashesTab.Controls.Add(this.HandleExceptions);
            this.CrashesTab.Controls.Add(this.CrashBox);
            this.CrashesTab.Controls.Add(this.CrashesEnabled);
            this.CrashesTab.Location = new System.Drawing.Point(4, 22);
            this.CrashesTab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashesTab.Name = "CrashesTab";
            this.CrashesTab.Size = new System.Drawing.Size(376, 497);
            this.CrashesTab.TabIndex = 2;
            this.CrashesTab.Text = "Crashes";
            this.CrashesTab.UseVisualStyleBackColor = true;
            // 
            // ErrorAttachmentsBox
            // 
            this.ErrorAttachmentsBox.Controls.Add(this.SelectFileAttachmentButton);
            this.ErrorAttachmentsBox.Controls.Add(this.FileAttachmentPathLabel);
            this.ErrorAttachmentsBox.Controls.Add(this.FileAttachmentLabel);
            this.ErrorAttachmentsBox.Controls.Add(this.TextAttachmentTextBox);
            this.ErrorAttachmentsBox.Controls.Add(this.TextAttachmentLabel);
            this.ErrorAttachmentsBox.Location = new System.Drawing.Point(8, 66);
            this.ErrorAttachmentsBox.Name = "ErrorAttachmentsBox";
            this.ErrorAttachmentsBox.Size = new System.Drawing.Size(359, 115);
            this.ErrorAttachmentsBox.TabIndex = 6;
            this.ErrorAttachmentsBox.TabStop = false;
            this.ErrorAttachmentsBox.Text = "Error Attachments";
            // 
            // SelectFileAttachmentButton
            // 
            this.SelectFileAttachmentButton.Location = new System.Drawing.Point(9, 86);
            this.SelectFileAttachmentButton.Name = "SelectFileAttachmentButton";
            this.SelectFileAttachmentButton.Size = new System.Drawing.Size(340, 23);
            this.SelectFileAttachmentButton.TabIndex = 4;
            this.SelectFileAttachmentButton.Text = "Select file attachment";
            this.SelectFileAttachmentButton.UseVisualStyleBackColor = true;
            this.SelectFileAttachmentButton.Click += new System.EventHandler(this.SelectFileAttachmentButton_ClickListener);
            // 
            // FileAttachmentPathLabel
            // 
            this.FileAttachmentPathLabel.AutoSize = true;
            this.FileAttachmentPathLabel.Location = new System.Drawing.Point(92, 64);
            this.FileAttachmentPathLabel.Name = "FileAttachmentPathLabel";
            this.FileAttachmentPathLabel.Size = new System.Drawing.Size(0, 13);
            this.FileAttachmentPathLabel.TabIndex = 3;
            // 
            // FileAttachmentLabel
            // 
            this.FileAttachmentLabel.AutoSize = true;
            this.FileAttachmentLabel.Location = new System.Drawing.Point(6, 64);
            this.FileAttachmentLabel.Name = "FileAttachmentLabel";
            this.FileAttachmentLabel.Size = new System.Drawing.Size(80, 13);
            this.FileAttachmentLabel.TabIndex = 2;
            this.FileAttachmentLabel.Text = "File Attachment";
            // 
            // TextAttachmentTextBox
            // 
            this.TextAttachmentTextBox.Location = new System.Drawing.Point(95, 30);
            this.TextAttachmentTextBox.Name = "TextAttachmentTextBox";
            this.TextAttachmentTextBox.Size = new System.Drawing.Size(254, 20);
            this.TextAttachmentTextBox.TabIndex = 1;
            this.TextAttachmentTextBox.TextChanged += new System.EventHandler(this.TextAttachmentTextBox_TextChanged);
            // 
            // TextAttachmentLabel
            // 
            this.TextAttachmentLabel.AutoSize = true;
            this.TextAttachmentLabel.Location = new System.Drawing.Point(4, 33);
            this.TextAttachmentLabel.Name = "TextAttachmentLabel";
            this.TextAttachmentLabel.Size = new System.Drawing.Size(85, 13);
            this.TextAttachmentLabel.TabIndex = 0;
            this.TextAttachmentLabel.Text = "Text Attachment";
            // 
            // HandleExceptions
            // 
            this.HandleExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HandleExceptions.Location = new System.Drawing.Point(8, 36);
            this.HandleExceptions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.HandleExceptions.Name = "HandleExceptions";
            this.HandleExceptions.Size = new System.Drawing.Size(360, 24);
            this.HandleExceptions.TabIndex = 5;
            this.HandleExceptions.Text = "Handle Exceptions";
            this.HandleExceptions.UseVisualStyleBackColor = true;
            // 
            // CrashBox
            // 
            this.CrashBox.Controls.Add(this.CrashInsideAsyncTask);
            this.CrashBox.Controls.Add(this.CrashWithNullReference);
            this.CrashBox.Controls.Add(this.CrashWithAggregateException);
            this.CrashBox.Controls.Add(this.CrashWithDivisionByZero);
            this.CrashBox.Controls.Add(this.CrashWithNonSerializableException);
            this.CrashBox.Controls.Add(this.CrashWithTestException);
            this.CrashBox.Location = new System.Drawing.Point(7, 197);
            this.CrashBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashBox.Name = "CrashBox";
            this.CrashBox.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashBox.Size = new System.Drawing.Size(360, 204);
            this.CrashBox.TabIndex = 4;
            this.CrashBox.TabStop = false;
            this.CrashBox.Text = "Crashes";
            // 
            // CrashInsideAsyncTask
            // 
            this.CrashInsideAsyncTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashInsideAsyncTask.Location = new System.Drawing.Point(8, 167);
            this.CrashInsideAsyncTask.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashInsideAsyncTask.Name = "CrashInsideAsyncTask";
            this.CrashInsideAsyncTask.Size = new System.Drawing.Size(342, 23);
            this.CrashInsideAsyncTask.TabIndex = 19;
            this.CrashInsideAsyncTask.Text = "Async task crash";
            this.CrashInsideAsyncTask.UseVisualStyleBackColor = true;
            this.CrashInsideAsyncTask.Click += new System.EventHandler(this.CrashInsideAsyncTask_Click);
            // 
            // CrashWithNullReference
            // 
            this.CrashWithNullReference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashWithNullReference.Location = new System.Drawing.Point(8, 138);
            this.CrashWithNullReference.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashWithNullReference.Name = "CrashWithNullReference";
            this.CrashWithNullReference.Size = new System.Drawing.Size(342, 23);
            this.CrashWithNullReference.TabIndex = 18;
            this.CrashWithNullReference.Text = "Crash with null reference";
            this.CrashWithNullReference.UseVisualStyleBackColor = true;
            this.CrashWithNullReference.Click += new System.EventHandler(this.CrashWithNullReference_Click);
            // 
            // CrashWithAggregateException
            // 
            this.CrashWithAggregateException.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashWithAggregateException.Location = new System.Drawing.Point(8, 109);
            this.CrashWithAggregateException.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashWithAggregateException.Name = "CrashWithAggregateException";
            this.CrashWithAggregateException.Size = new System.Drawing.Size(342, 23);
            this.CrashWithAggregateException.TabIndex = 17;
            this.CrashWithAggregateException.Text = "Aggregate Exception";
            this.CrashWithAggregateException.UseVisualStyleBackColor = true;
            this.CrashWithAggregateException.Click += new System.EventHandler(this.CrashWithAggregateException_Click);
            // 
            // CrashWithDivisionByZero
            // 
            this.CrashWithDivisionByZero.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashWithDivisionByZero.Location = new System.Drawing.Point(8, 80);
            this.CrashWithDivisionByZero.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashWithDivisionByZero.Name = "CrashWithDivisionByZero";
            this.CrashWithDivisionByZero.Size = new System.Drawing.Size(342, 23);
            this.CrashWithDivisionByZero.TabIndex = 16;
            this.CrashWithDivisionByZero.Text = "Divide by zero";
            this.CrashWithDivisionByZero.UseVisualStyleBackColor = true;
            this.CrashWithDivisionByZero.Click += new System.EventHandler(this.CrashWithDivisionByZero_Click);
            // 
            // CrashWithNonSerializableException
            // 
            this.CrashWithNonSerializableException.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashWithNonSerializableException.Location = new System.Drawing.Point(8, 51);
            this.CrashWithNonSerializableException.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashWithNonSerializableException.Name = "CrashWithNonSerializableException";
            this.CrashWithNonSerializableException.Size = new System.Drawing.Size(342, 23);
            this.CrashWithNonSerializableException.TabIndex = 15;
            this.CrashWithNonSerializableException.Text = "Generate non serializable Exception";
            this.CrashWithNonSerializableException.UseVisualStyleBackColor = true;
            this.CrashWithNonSerializableException.Click += new System.EventHandler(this.CrashWithNonSerializableException_Click);
            // 
            // CrashWithTestException
            // 
            this.CrashWithTestException.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashWithTestException.Location = new System.Drawing.Point(8, 22);
            this.CrashWithTestException.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashWithTestException.Name = "CrashWithTestException";
            this.CrashWithTestException.Size = new System.Drawing.Size(342, 23);
            this.CrashWithTestException.TabIndex = 14;
            this.CrashWithTestException.Text = "Call Crashes.GenerateTestCrash (debug only)";
            this.CrashWithTestException.UseVisualStyleBackColor = true;
            this.CrashWithTestException.Click += new System.EventHandler(this.CrashWithTestException_Click);
            // 
            // CrashesEnabled
            // 
            this.CrashesEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CrashesEnabled.Location = new System.Drawing.Point(8, 6);
            this.CrashesEnabled.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CrashesEnabled.Name = "CrashesEnabled";
            this.CrashesEnabled.Size = new System.Drawing.Size(360, 24);
            this.CrashesEnabled.TabIndex = 3;
            this.CrashesEnabled.Text = "Crashes Enabled";
            this.CrashesEnabled.UseVisualStyleBackColor = true;
            this.CrashesEnabled.CheckedChanged += new System.EventHandler(this.CrashesEnabled_CheckedChanged);
            // 
            // AppCenterAllowNetworkRequest
            // 
            this.AppCenterAllowNetworkRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppCenterAllowNetworkRequest.Location = new System.Drawing.Point(8, 36);
            this.AppCenterAllowNetworkRequest.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.AppCenterAllowNetworkRequest.Name = "AppCenterAllowNetworkRequest";
            this.AppCenterAllowNetworkRequest.Size = new System.Drawing.Size(360, 24);
            this.AppCenterAllowNetworkRequest.TabIndex = 3;
            this.AppCenterAllowNetworkRequest.Text = "Allow Network Request";
            this.AppCenterAllowNetworkRequest.UseVisualStyleBackColor = true;
            this.AppCenterAllowNetworkRequest.CheckedChanged += new System.EventHandler(this.AppCenterAllowNetworkRequest_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 523);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "App Center Puppet App";
            this.Tabs.ResumeLayout(false);
            this.AppCenterTab.ResumeLayout(false);
            this.MiscGroupBox.ResumeLayout(false);
            this.MiscGroupBox.PerformLayout();
            this.AnalyticsTab.ResumeLayout(false);
            this.EventBox.ResumeLayout(false);
            this.EventBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventProperties)).EndInit();
            this.CrashesTab.ResumeLayout(false);
            this.ErrorAttachmentsBox.ResumeLayout(false);
            this.ErrorAttachmentsBox.PerformLayout();
            this.CrashBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage AppCenterTab;
        private System.Windows.Forms.TabPage AnalyticsTab;
        private System.Windows.Forms.TabPage CrashesTab;
        private System.Windows.Forms.CheckBox AppCenterEnabled;
        private System.Windows.Forms.GroupBox EventBox;
        private System.Windows.Forms.CheckBox AnalyticsEnabled;
        private System.Windows.Forms.TextBox EventName;
        private System.Windows.Forms.Label EventNameLabel;
        private System.Windows.Forms.DataGridView EventProperties;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button TrackEvent;
        private System.Windows.Forms.CheckBox CrashesEnabled;
        private System.Windows.Forms.GroupBox CrashBox;
        private System.Windows.Forms.Button CrashWithTestException;
        private System.Windows.Forms.Button CrashWithNonSerializableException;
        private System.Windows.Forms.Button CrashWithDivisionByZero;
        private System.Windows.Forms.Button CrashWithAggregateException;
        private System.Windows.Forms.Button CrashWithNullReference;
        private System.Windows.Forms.Button CrashInsideAsyncTask;
        private System.Windows.Forms.CheckBox HandleExceptions;
        private System.Windows.Forms.GroupBox ErrorAttachmentsBox;
        private System.Windows.Forms.Label TextAttachmentLabel;
        private System.Windows.Forms.TextBox TextAttachmentTextBox;
        private System.Windows.Forms.Label FileAttachmentPathLabel;
        private System.Windows.Forms.Label FileAttachmentLabel;
        private System.Windows.Forms.Button SelectFileAttachmentButton;
        private System.Windows.Forms.GroupBox MiscGroupBox;
        private System.Windows.Forms.Label StorageMaxSizeLabel;
        private System.Windows.Forms.TextBox StorageMaxSizeTextBox;
        private System.Windows.Forms.Button SaveSizeStorageButton;
        private System.Windows.Forms.CheckBox AppCenterAllowNetworkRequest;
    }
}

