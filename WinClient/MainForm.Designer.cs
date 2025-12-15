namespace WinClient
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
            components = new System.ComponentModel.Container();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btn_Create_ChattingRoom = new Button();
            btn_entryRoom = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            contextMenuStrip2 = new ContextMenuStrip(components);
            lv_rooms = new ListView();
            SuspendLayout();
            // 
            // btn_Create_ChattingRoom
            // 
            btn_Create_ChattingRoom.Location = new Point(11, 23);
            btn_Create_ChattingRoom.Name = "btn_Create_ChattingRoom";
            btn_Create_ChattingRoom.Size = new Size(110, 23);
            btn_Create_ChattingRoom.TabIndex = 6;
            btn_Create_ChattingRoom.Text = "채팅방 만들기";
            btn_Create_ChattingRoom.UseVisualStyleBackColor = true;
            btn_Create_ChattingRoom.Click += btn_Create_ChattingRoom_Click;
            // 
            // btn_entryRoom
            // 
            btn_entryRoom.Location = new Point(11, 53);
            btn_entryRoom.Name = "btn_entryRoom";
            btn_entryRoom.Size = new Size(110, 23);
            btn_entryRoom.TabIndex = 7;
            btn_entryRoom.Text = "채팅방 입장";
            btn_entryRoom.UseVisualStyleBackColor = true;
            btn_entryRoom.Click += btn_entry_room;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(61, 4);
            // 
            // lv_rooms
            // 
            lv_rooms.Location = new Point(12, 82);
            lv_rooms.Name = "lv_rooms";
            lv_rooms.Size = new Size(363, 270);
            lv_rooms.TabIndex = 8;
            lv_rooms.UseCompatibleStateImageBehavior = false;
            lv_rooms.View = View.Details;
            lv_rooms.DoubleClick += lv_rooms_DoubleClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(387, 364);
            Controls.Add(lv_rooms);
            Controls.Add(btn_entryRoom);
            Controls.Add(btn_Create_ChattingRoom);
            Name = "MainForm";
            Text = "MainForm";
            FormClosed += MainForm_Close;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            ResumeLayout(false);
        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btn_Create_ChattingRoom;
        private Button btn_entryRoom;
        private ContextMenuStrip contextMenuStrip1;
        private ContextMenuStrip contextMenuStrip2;
        private ListView lv_rooms;
    }
}
