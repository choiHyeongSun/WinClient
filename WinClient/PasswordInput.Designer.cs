namespace WinClient
{
    partial class PasswordInput
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
            tb_password = new TextBox();
            btn_compare = new Button();
            btn_cancel = new Button();
            SuspendLayout();
            // 
            // tb_password
            // 
            tb_password.Location = new Point(12, 12);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '*';
            tb_password.PlaceholderText = "password";
            tb_password.Size = new Size(223, 23);
            tb_password.TabIndex = 0;
            tb_password.UseSystemPasswordChar = true;
            // 
            // btn_compare
            // 
            btn_compare.Location = new Point(79, 41);
            btn_compare.Name = "btn_compare";
            btn_compare.Size = new Size(75, 23);
            btn_compare.TabIndex = 1;
            btn_compare.Text = "확인";
            btn_compare.UseVisualStyleBackColor = true;
            btn_compare.Click += btn_compare_click;
            // 
            // btn_cancel
            // 
            btn_cancel.Location = new Point(160, 41);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(75, 23);
            btn_cancel.TabIndex = 2;
            btn_cancel.Text = "취소";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += btn_cancel_click;
            // 
            // PasswordInput
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(249, 73);
            Controls.Add(btn_cancel);
            Controls.Add(btn_compare);
            Controls.Add(tb_password);
            Name = "PasswordInput";
            Text = "password";
            Load += PasswordInput_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_password;
        private Button btn_compare;
        private Button btn_cancel;
    }
}