namespace WinClient
{
    partial class ChattingRoom
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
            tb_message_box = new TextBox();
            btn_send_message = new Button();
            lv_chatting = new ListView();
            lv_members = new ListView();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // tb_message_box
            // 
            tb_message_box.Location = new Point(12, 758);
            tb_message_box.Name = "tb_message_box";
            tb_message_box.Size = new Size(490, 23);
            tb_message_box.TabIndex = 0;
            tb_message_box.KeyDown += tb_message_box_keyDown;
            // 
            // btn_send_message
            // 
            btn_send_message.Location = new Point(511, 757);
            btn_send_message.Name = "btn_send_message";
            btn_send_message.Size = new Size(75, 23);
            btn_send_message.TabIndex = 1;
            btn_send_message.Text = "전송";
            btn_send_message.UseVisualStyleBackColor = true;
            btn_send_message.Click += btn_send_message_click;
            // 
            // lv_chatting
            // 
            lv_chatting.Location = new Point(12, 45);
            lv_chatting.Name = "lv_chatting";
            lv_chatting.Size = new Size(379, 692);
            lv_chatting.TabIndex = 2;
            lv_chatting.UseCompatibleStateImageBehavior = false;
            // 
            // lv_members
            // 
            lv_members.Location = new Point(397, 45);
            lv_members.Name = "lv_members";
            lv_members.Size = new Size(189, 692);
            lv_members.TabIndex = 3;
            lv_members.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 4;
            label1.Text = "채팅방";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(397, 18);
            label2.Name = "label2";
            label2.Size = new Size(71, 15);
            label2.TabIndex = 5;
            label2.Text = "채팅방 멤버";
            // 
            // ChattingRoom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(598, 793);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lv_members);
            Controls.Add(lv_chatting);
            Controls.Add(btn_send_message);
            Controls.Add(tb_message_box);
            Name = "ChattingRoom";
            Text = "ChattingRoom";
            Load += ChattingRoom_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_message_box;
        private Button btn_send_message;
        private ListView lv_chatting;
        private ListView lv_members;
        private Label label1;
        private Label label2;
    }
}