using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Learning;
using SFML.Graphics;
using SFML.Window;

namespace SFMLL
{
    class Program : Game
    {
        static string backgroundTexture = LoadTexture("background.png");
        static string playerTexture = LoadTexture("player.png");
        static string foodTexture = LoadTexture("food.png");

        static string meowSound = LoadSound("meow_sound.wav");
        static string crashSound = LoadSound("cat_crash_sound.wav");
        static string bgMusic = LoadMusic("bg_music.wav");

        static float playerX = 300;
        static float playerY = 220;
        static int playerSize = 56;

        static float playerSpeed = 150;
        static int playerDirection = 1; // Автоматическое движение игрока

        static int playerScore = 0;

        static float foodX;
        static float foodY;
        static int foodSize = 32;

        static void PlayerMove()
        {
            if (GetKey(Keyboard.Key.W) == true) playerDirection = 0; // Автоматическое движение игрока
            if (GetKey(Keyboard.Key.S) == true) playerDirection = 1;
            if (GetKey(Keyboard.Key.A) == true) playerDirection = 2;
            if (GetKey(Keyboard.Key.D) == true) playerDirection = 3;

            if(playerDirection == 0) playerY -= playerSpeed * DeltaTime; // Задание с помощью клавиш напраавления автоматического движения
            if(playerDirection == 1) playerY += playerSpeed * DeltaTime;
            if(playerDirection == 2) playerX -= playerSpeed * DeltaTime;
            if(playerDirection == 3) playerX += playerSpeed * DeltaTime;

            // if (GetKey(Keyboard.Key.W) == true) playerY -= playerSpeed * DeltaTime; // Ручное управление игроком
            // if (GetKey(Keyboard.Key.S) == true) playerY += playerSpeed * DeltaTime;
            // if (GetKey(Keyboard.Key.A) == true) playerX -= playerSpeed * DeltaTime;
            // if (GetKey(Keyboard.Key.D) == true) playerX += playerSpeed * DeltaTime;
        }

        static void DrawPlayer()
        {
           if (playerDirection == 0) DrawSprite(playerTexture, playerX, playerY, 64, 64, playerSize, playerSize); // Задание формы игрока
           if (playerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
           if (playerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
           if (playerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
        }

        static void Main()
        {
            InitWindow(800, 600, "Meow");

            SetFont("comic.ttf");

            Random rnd = new Random();

            foodX = rnd.Next(0, 800 - foodSize);
            foodY = rnd.Next(200, 600 - foodSize);

            bool isLose = false;

            PlayMusic(bgMusic, 10);

            while (true)
            {
                // 1.Расчет
                DispatchEvents();

                if (isLose == false)
                {
                    PlayerMove();

                    if (playerX + playerSize > foodX && playerX < foodX + foodSize // Логика столкновения (коллизии)
                        && playerY + playerSize > foodY && playerY < foodY + foodSize)
                    {
                        foodX = rnd.Next(0, 800 - foodSize);
                        foodY = rnd.Next(200, 600 - foodSize);

                        playerScore += 1; // Счетчик очков
                        playerSpeed += 10;

                        PlaySound(meowSound, 15);
                    }

                    if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 150) // Условия поражения
                    {
                        isLose = true;

                        PlaySound(crashSound, 15);
                    }

                }


                // Игровая логика

                // 2.Очистка буфера и окна
                ClearWindow(100, 100, 100); // Если отключить очистку буфера (удалить/закоментировать ClearWindow) то получится эффект роста фигуры

                // 3.Отрисовка буфера на окне

                DrawSprite(backgroundTexture, 0, 0);


                if (isLose == true) // Уведомить игрока о проигрыше
                {
                    SetFillColor(50, 50, 50);
                    DrawText(300, 300, "Ты съел стену! хахаха", 24);
                    SetFillColor(50, 50, 50);
                    DrawText(240, 350, "Найдём другую еду??? Дави на \"R\"", 24);

                    if (GetKeyDown(Keyboard.Key.R) == true) // Логика Рестарта
                    {
                        isLose = false;
                        playerX = 300;
                        playerY = 220;
                        playerSpeed = 150;
                        playerDirection = 1;
                        playerScore = 0;
                    }    
                }

                // Вызов методов отрисовки объектов

                DrawPlayer();

                DrawSprite(foodTexture, foodX, foodY);

                SetFillColor(70, 70, 70);
                DrawText(20, 8, "Съедено корма: " + playerScore.ToString(), 18);

                // Console.SetCursorPosition(0, 0);
                // Console.Write("                ");
                // Console.SetCursorPosition(0, 0);
                // Console.Write("Score: " + playerScore);

                DisplayWindow();

                // Ожидание
                Delay(1);
            }

        }
    }
}
