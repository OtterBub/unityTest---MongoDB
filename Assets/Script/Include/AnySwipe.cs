using UnityEngine;
using System.Collections;

public class AnySwipe : MonoBehaviour{

	public int		minLenght = 50;			//the minimum length to be consider an swipe 

	[Range(0.0f, 1f)]
	public float	relevance = 0.9f;		//amount of relevance to take into account for checking
											//if an swipe is swiped according to the desired direction

	#if UNITY_EDITOR
	public bool		touchEnabled;			//useful if you would like to use touch input from the editor if using UNTIY REMOTE
	#endif

	private Vector2	touchStart;				//swipe starting position
	private	Vector2	touchEnd;				//swipe ending position
	private Vector2	dir;					//direction of swipe
	private bool	isInit;					//determines if swipes can be check against
	private bool	reset;
	private bool	isTouchDown;
	

    //150602 정유훈 추가
    //해당 스크립트가 들어있는 게임 오브젝트 내부에서만 스와이프 판정을 가린다.
    //private RectTransform rectTran;


	//Get current swipe direction
	public Vector2	Dir{
		
		get{
			
			return dir;
		}
	}

	#if UNITY_EDITOR
	void OnEnable(){

		if(touchEnabled)
			Debug.Log ("AnySwipe : Tounch Input Enabled");
	}
	#endif
	//-------------------------

    //void Start()
    //{
    //    rectTran = gameObject.GetComponent<RectTransform>();
    //}

	//-------------------------
	public bool CheckForSwipe (Vector2 swipe) {

		if(isInit){

		
			//compare current swipe to desired swipe direction
			float dot = Vector2.Dot(swipe, dir);

			//round to 2 decimal places
			dot = Mathf.Round(dot * 100)/100;


			//check if current swipe is within match to desired swipe direction
			if(dot >= relevance){

				return true;
			}


		}


		#if UNITY_EDITOR
		if(!isInit){
			
			//display error
			Debug.LogError("AnySwipe, Must call IsSwiped(), before you can use CheckForSwipe()");
		}
		#endif


		return false;



	}
	//-------------------------



    //bool isPointContain(Vector2 pos)
    //{
    //    //현재 이벤트 시스템의 PointerEnter 객체가 이 스크립트 게임오브젝트의 자식 객체인가?
    //    Transform temp = null;
    //    if (globalSetup.pointerDownObj)
    //    {
    //        temp = globalSetup.pointerDownObj.transform;
    //    }
    //    bool isChild = false;
    //    while (temp)
    //    {
    //        Debug.Log(temp.gameObject);
    //        if (temp.gameObject == gameObject)
    //        {
    //            isChild = true;
    //            break;
    //        }
    //        temp = temp.transform.parent;
    //    }
    //    if(isChild)
    //    {
    //        if (rectTran.rect.Contains(pos))
    //        {
    //            Debug.Log("true");
    //            return true;
    //        }
    //    }
    //    return false;
    //}

	//-------------------------

	public bool IsSwiped(){

		#if UNITY_EDITOR

		if(!touchEnabled){

			if(!isTouchDown){

				if(Input.GetMouseButtonDown(0)){
                    //Vector2 coordTransPos = new Vector2(Input.mousePosition.x + Screen.width * -0.5f, Input.mousePosition.y + Screen.height * -0.5f);
                    isInit = true;
                    isTouchDown = true;
                    touchStart = Input.mousePosition;

				}
			}
			else{

				if(Input.GetMouseButtonUp(0)){


					isTouchDown = false;

					touchEnd = Input.mousePosition;
							
					dir = touchEnd - touchStart;

					reset = true;	

					//check lenght
					if(dir.sqrMagnitude >= minLenght * minLenght){
								
						dir.Normalize();


						return true;
								
					}

		
				}
			}


		}
		else{

			if(Input.touchCount > 0){
				
				
				Touch touch = Input.GetTouch(0);
				
				if(!isTouchDown){
                    isInit = true;

                    isTouchDown = true;

                    touchStart = touch.position;
					
				}
				else{
					
					touchEnd = touch.position;
					
				}
				

			}
			else{
				
				
				if(isTouchDown){
					
					isTouchDown = false;

					dir = touchEnd - touchStart;

					reset = true;

					//check lenght
					if(dir.sqrMagnitude >= (minLenght * minLenght)){
						
						dir.Normalize();
						

						return true;
						
					}



				}

			}

		}
		#elif UNITY_WEBPLAYER || UNITY_STANDALONE

		if(!isTouchDown){
			
			if(Input.GetMouseButtonDown(0)){			
				isInit = true;
				isTouchDown = true;
				touchStart = Input.mousePosition;
				
			}
		}
		else{
			
			if(Input.GetMouseButtonUp(0)){
				
				
				isTouchDown = false;
				
				touchEnd = Input.mousePosition;
				
				dir = touchEnd - touchStart;
				
				reset = true;	
				
				//check lenght
				if(dir.sqrMagnitude >= minLenght * minLenght){
					
					dir.Normalize();
					
					
					return true;
					
				}
				
				
			}
		}



#elif UNITY_IOS || UNITY_ANDROID

		if(Input.touchCount > 0){
			
			
			Touch touch = Input.GetTouch(0);
			
			if(!isTouchDown){
				isInit = true;

				isTouchDown = true;
				
				touchStart = touch.position;
				
			}
			else{
				
				touchEnd = touch.position;
				
			}
			
			
		}
		else{
			
			
			if(isTouchDown){
				
				isTouchDown = false;
				
				dir = touchEnd - touchStart;

				reset = true; 

				//check lenght
				if(dir.sqrMagnitude >= minLenght * minLenght){
					
					dir.Normalize();
					
					
					return true;
					
				}
				
				
				
			}
		}

#endif

        if (reset){

			reset = false;
			isInit = false;
		}
		return false;
	}



}
