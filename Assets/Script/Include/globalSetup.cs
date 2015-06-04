using UnityEngine;
using System.Collections;

static public class globalSetup{

    static public float widthRatio = Screen.width / 360;
    static public float heightRatio = Screen.height / 640;
    static public int nowDisplayIndex = 0;
    static public bool isExistSlidePanel = false;
    static public float contentWidth = 360 * widthRatio;
    static public float contentHeight = 520 * heightRatio;
    static public GameObject backBtn;
}
