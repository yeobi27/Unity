using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void OnCantMove<T>(T component)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        // player 를 통해서 음식포인트를 다음 스테이지를 지나도
        // 계속 갱신 받을 수 있음
        food = GameManager.instance.playerFoodPoints;

        // 부모클래스 : MovingObject 의 Start() 호출
        base.Start();
    }

    // OnDisable 게임오브젝트가 비활성화되면 호출
    private void OnDisable()
    {
        // 현재 food 를 GameManager 에 저장
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        // false 이면 return; 
        // 플레이어 동작상태가 아니라면 코드 실행 X
        if(!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("vertical");

        // 수평으로 움직이면 수직 X, 대각선 움직임 제한
        if(horizontal != 0)
        {
            vertical = 0;
        }

        if(horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        // 이동간에 충돌이 있는지 체크하는 구조체변수
        RaycastHit2D hit;

        // 게임오버체크
        CheckIfGameOver();

        // GameManager에 선언해둔 playersTurn 을 false 로 변경
        GameManager.instance.playersTurn = false;
    }

    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);

            //Disable the player object since level is over.
            enabled = false;
        }

        //Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Food")
        {
            //Add pointsPerFood to the players current food total.
            food += pointsPerFood;

            //Update foodText to represent current total and notify player that they gained points
            //foodText.text = "+" + pointsPerFood + " Food: " + food;

            //Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
            //SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            //Disable the food object the player collided with.
            other.gameObject.SetActive(false);
        }

        //Check if the tag of the trigger collided with is Soda.
        else if (other.tag == "Soda")
        {
            //Add pointsPerSoda to players food points total
            food += pointsPerSoda;

            //Update foodText to represent current total and notify player that they gained points
            //foodText.text = "+" + pointsPerSoda + " Food: " + food;

            //Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
            //SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            //Disable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        //Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;

        //Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);

        //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
        animator.SetTrigger("playerChop");
    }

    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
        //and not load all the scene object in the current scene.
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        Application.LoadLevel(Application.loadedLevel);
    }

    //LoseFood is called when an enemy attacks the player.
    //It takes a parameter loss which specifies how many points to lose.
    public void LoseFood(int loss)
    {
        //Set the trigger for the player animator to transition to the playerHit animation.
        animator.SetTrigger("playerHit");

        //Subtract lost food points from the players total.
        food -= loss;

        //Update the food display with the new total.
        //foodText.text = "-" + loss + " Food: " + food;

        //Check to see if game has ended.
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if( food <= 0 )
        {
            GameManager.instance.GameOver();
        }
    }
}
