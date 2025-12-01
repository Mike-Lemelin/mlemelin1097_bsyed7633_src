namespace control_panel
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
            connectBtn = new Button();
            disconnectBtn = new Button();
            label2 = new Label();
            topicList = new ListView();
            label1 = new Label();
            label3 = new Label();
            deviceCombo = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            selectedTopicText = new Label();
            subscribeBtn = new Button();
            unsubBtn = new Button();
            deviceTopicText = new Label();
            SuspendLayout();
            // 
            // connectBtn
            // 
            connectBtn.Location = new Point(758, 526);
            connectBtn.Name = "connectBtn";
            connectBtn.Size = new Size(112, 34);
            connectBtn.TabIndex = 1;
            connectBtn.Text = "Connect";
            connectBtn.UseVisualStyleBackColor = true;
            connectBtn.Click += connectBtn_Click;
            // 
            // disconnectBtn
            // 
            disconnectBtn.Location = new Point(876, 525);
            disconnectBtn.Name = "disconnectBtn";
            disconnectBtn.Size = new Size(112, 34);
            disconnectBtn.TabIndex = 2;
            disconnectBtn.Text = "Disconnect";
            disconnectBtn.UseVisualStyleBackColor = true;
            disconnectBtn.Click += disconnectBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(671, 17);
            label2.Name = "label2";
            label2.Size = new Size(0, 25);
            label2.TabIndex = 5;
            // 
            // topicList
            // 
            topicList.Location = new Point(12, 55);
            topicList.Name = "topicList";
            topicList.Size = new Size(973, 220);
            topicList.TabIndex = 0;
            topicList.UseCompatibleStateImageBehavior = false;
            topicList.SelectedIndexChanged += topicList_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(186, 35);
            label1.TabIndex = 6;
            label1.Text = "Topic Selection";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 308);
            label3.Name = "label3";
            label3.Size = new Size(320, 35);
            label3.TabIndex = 7;
            label3.Text = "Subscription Configuration";
            // 
            // deviceCombo
            // 
            deviceCombo.FormattingEnabled = true;
            deviceCombo.Location = new Point(197, 364);
            deviceCombo.Name = "deviceCombo";
            deviceCombo.Size = new Size(182, 33);
            deviceCombo.TabIndex = 8;
            deviceCombo.SelectedIndexChanged += deviceCombo_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 367);
            label4.Name = "label4";
            label4.Size = new Size(131, 25);
            label4.TabIndex = 9;
            label4.Text = "Select a device:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 412);
            label5.Name = "label5";
            label5.Size = new Size(160, 25);
            label5.TabIndex = 10;
            label5.Text = "Subscription topic:";
            // 
            // selectedTopicText
            // 
            selectedTopicText.AutoSize = true;
            selectedTopicText.Location = new Point(197, 412);
            selectedTopicText.Name = "selectedTopicText";
            selectedTopicText.Size = new Size(218, 25);
            selectedTopicText.TabIndex = 11;
            selectedTopicText.Text = "Select a topic from the list";
            selectedTopicText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // subscribeBtn
            // 
            subscribeBtn.Location = new Point(12, 456);
            subscribeBtn.Name = "subscribeBtn";
            subscribeBtn.Size = new Size(112, 34);
            subscribeBtn.TabIndex = 12;
            subscribeBtn.Text = "Subscribe";
            subscribeBtn.UseVisualStyleBackColor = true;
            subscribeBtn.Click += subscribeBtn_Click;
            // 
            // unsubBtn
            // 
            unsubBtn.Location = new Point(130, 456);
            unsubBtn.Name = "unsubBtn";
            unsubBtn.Size = new Size(120, 34);
            unsubBtn.TabIndex = 13;
            unsubBtn.Text = "Unsubscribe";
            unsubBtn.UseVisualStyleBackColor = true;
            unsubBtn.Click += unsubBtn_Click;
            // 
            // deviceTopicText
            // 
            deviceTopicText.AutoSize = true;
            deviceTopicText.Location = new Point(394, 367);
            deviceTopicText.Name = "deviceTopicText";
            deviceTopicText.Size = new Size(0, 25);
            deviceTopicText.TabIndex = 14;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(997, 572);
            Controls.Add(deviceTopicText);
            Controls.Add(unsubBtn);
            Controls.Add(subscribeBtn);
            Controls.Add(selectedTopicText);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(deviceCombo);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(topicList);
            Controls.Add(label2);
            Controls.Add(disconnectBtn);
            Controls.Add(connectBtn);
            Name = "MainForm";
            Text = "MQTT Control Panel";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button connectBtn;
        private Button disconnectBtn;
        private Label label2;
        private ListView topicList;
        private Label label1;
        private Label label3;
        private ComboBox deviceCombo;
        private Label label4;
        private Label label5;
        private Label selectedTopicText;
        private Button subscribeBtn;
        private Button unsubBtn;
        private Label deviceTopicText;
    }
}
