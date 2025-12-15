namespace WinClient
{
    partial class LoginUserControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            tb_userID = new TextBox();
            btn_join = new Button();
            tb_password = new TextBox();
            btn_login = new Button();
            SuspendLayout();
            // 
            // tb_userID
            // 
            tb_userID.Location = new Point(5, 9);
            tb_userID.Margin = new Padding(0);
            tb_userID.Name = "tb_userID";
            tb_userID.PlaceholderText = "UserID";
            tb_userID.Size = new Size(150, 23);
            tb_userID.TabIndex = 4;
            // 
            // btn_join
            // 
            btn_join.Location = new Point(161, 38);
            btn_join.Margin = new Padding(0);
            btn_join.Name = "btn_join";
            btn_join.Size = new Size(75, 23);
            btn_join.TabIndex = 7;
            btn_join.Text = "Join";
            btn_join.UseVisualStyleBackColor = true;
            btn_join.Click += Join_Buttn_Click;
            // 
            // tb_password
            // 
            tb_password.Location = new Point(5, 38);
            tb_password.Margin = new Padding(0);
            tb_password.Name = "tb_password";
            tb_password.PlaceholderText = "Password";
            tb_password.Size = new Size(150, 23);
            tb_password.TabIndex = 5;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(161, 9);
            btn_login.Margin = new Padding(0);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(75, 23);
            btn_login.TabIndex = 6;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += Login_Buttn_Click;
            // 
            // LoginUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tb_userID);
            Controls.Add(btn_join);
            Controls.Add(tb_password);
            Controls.Add(btn_login);
            Name = "LoginUserControl";
            Size = new Size(245, 69);
            Load += LoginPanel_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_userID;
        private Button btn_join;
        private TextBox tb_password;
        private Button btn_login;
    }
}
