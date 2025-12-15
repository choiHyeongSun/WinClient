namespace WinClient
{
    partial class UserInfoUserControl
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
            btn_logout = new Button();
            lb_nicknameField = new Label();
            lb_nickname = new Label();
            SuspendLayout();
            // 
            // btn_logout
            // 
            btn_logout.Location = new Point(8, 35);
            btn_logout.Name = "btn_logout";
            btn_logout.Size = new Size(57, 26);
            btn_logout.TabIndex = 5;
            btn_logout.Text = "Logout";
            btn_logout.UseVisualStyleBackColor = true;
            btn_logout.Click += Logout_Buttn_Click;
            // 
            // lb_nicknameField
            // 
            lb_nicknameField.Location = new Point(71, 10);
            lb_nicknameField.MaximumSize = new Size(170, 45);
            lb_nicknameField.MinimumSize = new Size(170, 45);
            lb_nicknameField.Name = "lb_nicknameField";
            lb_nicknameField.Size = new Size(170, 45);
            lb_nicknameField.TabIndex = 4;
            lb_nicknameField.Text = "UserNickname";
            // 
            // lb_nickname
            // 
            lb_nickname.AutoSize = true;
            lb_nickname.Location = new Point(8, 10);
            lb_nickname.Name = "lb_nickname";
            lb_nickname.Size = new Size(66, 15);
            lb_nickname.TabIndex = 3;
            lb_nickname.Text = "Nicname : ";
            // 
            // UserInfoUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btn_logout);
            Controls.Add(lb_nicknameField);
            Controls.Add(lb_nickname);
            Name = "UserInfoUserControl";
            Size = new Size(248, 70);
            Load += UserInfoUserControl_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_logout;
        private Label lb_nicknameField;
        private Label lb_nickname;
    }
}
