using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game2048;
using System;

public class GameController : MonoBehaviour
{
    public Button buttonStart;
    public Text textMessage;

    Model model = new Model(4);

    private Vector3 fp;   //Первая позиция касания
    private Vector3 lp;   //Последняя позиция касания
    private float dragDistance;  //Минимальная дистанция для определения свайпа
    private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке

    // Start is called before the first frame update
    void Start()
    {
        textMessage.text = "Welcome to 2048!";
        
        dragDistance = Screen.height * 20 / 100; //dragDistance это 20% высоты экрана
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Touch touch in Input.touches)  //используем цикл для отслеживания больше одного свайпа
        { //должны быть закоментированы, если вы используете списки 
          /*if (touch.phase == TouchPhase.Began) //проверяем первое касание
          {
              fp = touch.position;
              lp = touch.position;

          }*/

            if (touch.phase == TouchPhase.Moved) //добавляем касания в список, как только они определены
            {
                touchPositions.Add(touch.position);
            }

            if (touch.phase == TouchPhase.Ended) //проверяем, если палец убирается с экрана
            {
                //lp = touch.position;  //последняя позиция касания. закоментируйте если используете списки
                fp = touchPositions[0]; //получаем первую позицию касания из списка касаний
                lp = touchPositions[touchPositions.Count - 1]; //позиция последнего касания

                //проверяем дистанцию перемещения больше чем 20% высоты экрана
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//это перемещение
                 //проверяем, перемещение было вертикальным или горизонтальным 
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //Если горизонтальное движение больше, чем вертикальное движение ...
                        if ((lp.x > fp.x))  //Если движение было вправо
                        {   //Свайп вправо
                            Debug.Log("Right Swipe");
                            model.Right();
                        }
                        else
                        {   //Свайп влево
                            Debug.Log("Left Swipe");
                            model.Left();
                        }
                    }
                    else
                    {   //Если вертикальное движение больше, чнм горизонтальное движение
                        if (lp.y > fp.y)  //Если движение вверх
                        {   //Свайп вверх
                            Debug.Log("Up Swipe");
                            model.Up();
                        }
                        else
                        {   //Свайп вниз
                            Debug.Log("Down Swipe");
                            model.Down();
                        }
                    }
                }
                Show();
                if (model.IsGameOver())
                    textMessage.text = "Game over!";
            }
            else
            {   //Это ответвление, как расстояние перемещения составляет менее 20% от высоты экрана

            }
            Show();
        }


        //if (Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Moved)
        //{

        //    Vector2 touchDeltPosition = Input.GetTouch(0).deltaPosition;

        //    float sx = touchDeltPosition.x;
        //    float sy = touchDeltPosition.y;

        //    if (Math.Abs(sx) > Math.Abs(sy))//left or right
        //    {

        //        if (sx < 0)
        //            model.Left();
        //        else
        //            model.Right();
        //    }
        //    else//up or down
        //    {
        //        if (sy < 0)
        //            model.Up();
        //        else
        //            model.Down();
        //    }
        //    Show();
        //    if (model.IsGameOver())
        //        textMessage.text = "Game over!";

        //}
        //Show();
    }
    public void ClickButtonStart()
    {
        textMessage.text = "Let's play!";
        model.Start();
        Show();
    }
    void Show()
    {
        for (int x = 0; x < model.size; x++)
            for (int y = 0; y < model.size; y++)
                ShowButtonText("b" + x + y, model.GetMap(x, y));
    }

    private void ShowButtonText(string name, int number)
    {
        var button = GameObject.Find(name);
        var text = button.GetComponentInChildren<Text>();
        text.text = number == 0 ? " " : number.ToString();
    }
}
