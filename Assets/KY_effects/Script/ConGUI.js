#pragma strict
public var mainCamera:Transform;
public var cameraTrs:Transform;
public var rotSpeed:int = 20;
public var effectObj:GameObject[];
public var effectObProj:GameObject[];
public var lightObj:Light;
private var arrayNo:int = 0;

private var nowEffectObj:GameObject;
private var cameraState:String[] = ["Camera move" ,"Camera stop"];
private var cameraRotCon:int = 0;

private var num:int = 0;
private var numBck:int = 0;
private var initPos:Vector3;

private var lightPower:float = 0;
private var lightPowerBck:float = 0;

private var haveProFlg:boolean = false;
private var nonProFX:GameObject;

function visibleBt():boolean{
	for(var tmpObj:GameObject in effectObProj){
		if( effectObj[ arrayNo ].name == tmpObj.name){
			nonProFX = tmpObj;
			return true;
		}
	}
	return false;
}

function Start () {
	initPos = mainCamera.localPosition;
	lightPower = lightObj.intensity;//0;//light.intensity;

	haveProFlg = visibleBt();
}

function Update () {
	if( cameraRotCon == 1)cameraTrs.Rotate(0 ,rotSpeed * Time.deltaTime ,0);


	lightObj.intensity = lightPower;


	if(num > numBck){
		numBck = num;
		mainCamera.localPosition.y += 0.05;
		mainCamera.localPosition.z -= 0.3;
		
	}else if(num < numBck){
		numBck = num;
		mainCamera.localPosition.y -= 0.05;
		mainCamera.localPosition.z += 0.3;
	}else if(num == 0){
		mainCamera.localPosition.y = initPos.y;
		mainCamera.localPosition.z = initPos.z;
	}
	
	if(mainCamera.localPosition.y < initPos.y )mainCamera.localPosition.y = initPos.y;
	if(mainCamera.localPosition.z > initPos.z )mainCamera.localPosition.z = initPos.z;
}

function  OnGUI(){
		
	if (GUI.Button (Rect(0, 0, Screen.width/10, Screen.height/10), "pre")) {//return Rect(20, 0, 30, 30
		arrayNo --;
		if(arrayNo < 0)arrayNo = effectObj.Length -1;
		effectOn();
		
		haveProFlg = visibleBt();
	}
	
	if (GUI.Button (Rect(Screen.width/10, 0, Screen.width/3, Screen.height/10), effectObj[ arrayNo ].name )) {//Rect(50, 0, 200, 30
		effectOn();
	}
	
	if (GUI.Button (Rect(Screen.width/10 + Screen.width/3, 0, Screen.width/10, Screen.height/10), "next")) {//next Rect(250, 0, 30, 30
		arrayNo ++;
		if(arrayNo >= effectObj.Length)arrayNo = 0;
		effectOn();
		
		haveProFlg = visibleBt();
	}
	
	if( haveProFlg ){
		if (GUI.Button (Rect(50, 30, 300, 70), "+Distorsion" )) {
			if(nowEffectObj != null)Destroy( nowEffectObj );
			nowEffectObj = Instantiate( nonProFX );
		}
	}
	
	
	if (GUI.Button (Rect(Screen.width/4 + Screen.width/3, 0, Screen.width/6, Screen.height/10), cameraState[ cameraRotCon ] )) {//Rect(300, 0, 200, 30
		if( cameraRotCon == 1){
			cameraRotCon = 0;
		}else{
			cameraRotCon = 1;
		}
	}
	
	num = GUI.VerticalSlider(Rect(Screen.width/30, Screen.height/4, Screen.width/30, Screen.height/2), num, 0, 20);//Rect(30, 100, 20, 200
	
	lightPower = GUI.VerticalSlider(Rect(Screen.width/30 + Screen.width/25, Screen.height/4, Screen.width/30, Screen.height/2), lightPower, 0, 1.2);//Rect(50, 100, 20, 200
}

function effectOn(){
	if(nowEffectObj != null)Destroy( nowEffectObj );
	nowEffectObj = Instantiate(effectObj[ arrayNo ] );
}