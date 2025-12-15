namespace WinClient
{
    partial class CreateRoomForm
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
            tb_roomName = new TextBox();
            lb_roomName = new Label();
            tb_password = new TextBox();
            lb_password = new Label();
            btn_createRoom = new Button();
            btn_cancel = new Button();
            SuspendLayout();
            // 
            // tb_roomName
            // 
            tb_roomName.Location = new Point(72, 11);
            tb_roomName.Name = "tb_roomName";
            tb_roomName.PlaceholderText = "name";
            tb_roomName.Size = new Size(239, 23);
            tb_roomName.TabIndex = 0;
            // 
            // lb_roomName
            // 
            lb_roomName.AutoSize = true;
            lb_roomName.Location = new Point(27, 14);
            lb_roomName.Name = "lb_roomName";
            lb_roomName.Size = new Size(31, 15);
            lb_roomName.TabIndex = 1;
            lb_roomName.Text = "이름";
            // 
            // tb_password
            // 
            tb_password.Location = new Point(72, 40);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '*';
            tb_password.PlaceholderText = "password";
            tb_password.Size = new Size(239, 23);
            tb_password.TabIndex = 2;
            // 
            // lb_password
            // 
            lb_password.AutoSize = true;
            lb_password.Location = new Point(11, 43);
            lb_password.Name = "lb_password";
            lb_password.Size = new Size(55, 15);
            lb_password.TabIndex = 3;
            lb_password.Text = "비밀번호";
            // 
            // btn_createRoom
            // 
            btn_createRoom.Location = new Point(155, 69);
            btn_createRoom.Name = "btn_createRoom";
            btn_createRoom.Size = new Size(75, 23);
            btn_createRoom.TabIndex = 4;
            btn_createRoom.Text = "생성";
            btn_createRoom.UseVisualStyleBackColor = true;
            btn_createRoom.Click += btn_Complate_Click;
            // 
            // btn_cancel
            // 
            btn_cancel.Location = new Point(236, 69);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(75, 23);
            btn_cancel.TabIndex = 5;
            btn_cancel.Text = "취소";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += btn_cancel_Click;
            // 
            // CreateRoomForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 104);
            Controls.Add(btn_cancel);
            Controls.Add(btn_createRoom);
            Controls.Add(lb_password);
            Controls.Add(tb_password);
            Controls.Add(lb_roomName);
            Controls.Add(tb_roomName);
            Name = "CreateRoomForm";
            Text = "CreateRoomForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_roomName;
        private Label lb_roomName;
        private TextBox tb_password;
        private Label lb_password;
        private Button btn_createRoom;
        private Button btn_cancel;
    }
}