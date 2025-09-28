using System;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        int pipeSpeed = 8;    // Boruların hareket hızı
        int gravity = 5;      // Kuşun düşme hızı
        int score = 0;        // Oyuncu skoru
        int bestScore = 0;    // En yüksek skor
        int birdFrame = 0;    // Kuşun kanat animasyonu
        bool isNight = false; // Tema (gündüz/gece)

        public Form1()
        {
            InitializeComponent();

            // Form ayarları
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(gameKeyDown);
            this.KeyUp += new KeyEventHandler(gameKeyUp);

            // Timer ayarı
            gameTimer.Tick += new EventHandler(gameTimerEvent);
            gameTimer.Start();

            // Restart butonu bağla
            restartButton.Click += new EventHandler(restartButton_Click);

            // İlk başlangıç teması
            this.BackgroundImage = Properties.Resources.background_day;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // Oyun döngüsü
        private void gameTimerEvent(object sender, EventArgs e)
        {
            // Kuş animasyonu
            birdFrame++;
            if (birdFrame > 2) birdFrame = 0;

            switch (birdFrame)
            {
                case 0:
                    flappyBird.Image = Properties.Resources.yellowbird_upflap;
                    break;
                case 1:
                    flappyBird.Image = Properties.Resources.yellowbird_midflap;
                    break;
                case 2:
                    flappyBird.Image = Properties.Resources.yellowbird_downflap;
                    break;
            }

            // Kuşun düşüş / yükselişi
            flappyBird.Top += gravity;

            // Borular hareket etsin
            pipeBottom.Left -= pipeSpeed;
            pipeTop.Left -= pipeSpeed;

            // Skor güncelle
            scoreText.Text = "Score: " + score;

            // Borular ekrandan çıkınca tekrar sağdan gelsin
            if (pipeBottom.Left < -150)
            {
                pipeBottom.Left = 800;
                score++;
            }
            if (pipeTop.Left < -180)
            {
                pipeTop.Left = 950;
                score++;
            }

            // Çarpışma kontrolü
            if (flappyBird.Bounds.IntersectsWith(pipeBottom.Bounds) ||
                flappyBird.Bounds.IntersectsWith(pipeTop.Bounds) ||
                flappyBird.Bounds.IntersectsWith(ground.Bounds))
            {
                endGame();
            }

            // Zorluk ayarı
            if (score > 5) pipeSpeed = 12;
            if (score > 10) pipeSpeed = 15;
            if (score > 20) pipeSpeed = 20;
        }

        // Space basınca kuş zıplasın
        private void gameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                gravity = -10;

            // N tuşu → gece/gündüz
            if (e.KeyCode == Keys.N)
            {
                if (isNight)
                {
                    this.BackgroundImage = Properties.Resources.background_day;
                    isNight = false;
                }
                else
                {
                    this.BackgroundImage = Properties.Resources.background_night;
                    isNight = true;
                }
            }
        }

        // Space bırakılınca kuş düşsün
        private void gameKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                gravity = 5;
        }

        // Oyun bitince
        private void endGame()
        {
            gameTimer.Stop();

            // En yüksek skor kontrolü
            if (score > bestScore)
                bestScore = score;

            scoreText.Text += "   Game Over!";
            bestScoreText.Text = "Best: " + bestScore;

            restartButton.Visible = true;
        }

        // Restart butonu
        private void restartButton_Click(object sender, EventArgs e)
        {
            // Kuşu sıfırla
            flappyBird.Top = 200;
            flappyBird.Left = 50;

            // Boruları sıfırla
            pipeTop.Left = 300;
            pipeBottom.Left = 300;

            // Skor sıfırla
            score = 0;
            scoreText.Text = "Score: 0";

            // Hızı sıfırla
            pipeSpeed = 8;

            // Restart gizle
            restartButton.Visible = false;

            // Timer başlasın
            gameTimer.Start();
        }

        // Designer için boş event (hata engellemek için)
        private void Form1_Load(object sender, EventArgs e)
        {
            // Kullanılmıyor ama Designer hata vermesin
        }
    }
}