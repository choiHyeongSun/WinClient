namespace WinClient
{
    partial class JoinForm
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
            tb_userID = new TextBox();
            tb_password = new TextBox();
            tb_nickname = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btn_cancel = new Button();
            btn_complate = new Button();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tb_userID
            // 
            tb_userID.Location = new Point(65, 20);
            tb_userID.Name = "tb_userID";
            tb_userID.Size = new Size(215, 23);
            tb_userID.TabIndex = 0;
            // 
            // tb_password
            // 
            tb_password.Location = new Point(65, 49);
            tb_password.Name = "tb_password";
            tb_password.Size = new Size(215, 23);
            tb_password.TabIndex = 1;
            // 
            // tb_nickname
            // 
            tb_nickname.Location = new Point(65, 78);
            tb_nickname.Name = "tb_nickname";
            tb_nickname.Size = new Size(215, 23);
            tb_nickname.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 28);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 3;
            label1.Text = "아이디";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 52);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 4;
            label2.Text = "비밀번호";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 78);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 5;
            label3.Text = "닉네임";
            // 
            // btn_cancel
            // 
            btn_cancel.Location = new Point(241, 138);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(75, 23);
            btn_cancel.TabIndex = 6;
            btn_cancel.Text = "Cancel";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += Cancel_Button_Click;
            // 
            // btn_complate
            // 
            btn_complate.Location = new Point(157, 138);
            btn_complate.Name = "btn_complate";
            btn_complate.Size = new Size(75, 23);
            btn_complate.TabIndex = 7;
            btn_complate.Text = "OK";
            btn_complate.UseVisualStyleBackColor = true;
            btn_complate.Click += Complate_Button_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(tb_userID);
            groupBox1.Controls.Add(tb_password);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(tb_nickname);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(304, 120);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Join";
            // 
            // JoinForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 173);
            Controls.Add(groupBox1);
            Controls.Add(btn_complate);
            Controls.Add(btn_cancel);
            Name = "JoinForm";
            Text = "JoinForm";
            FormClosed += JoinForm_FormClosed;
            Load += JoinForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox tb_userID;
        private TextBox tb_password;
        private TextBox tb_nickname;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btn_cancel;
        private Button btn_complate;
        private GroupBox groupBox1;
    }
}