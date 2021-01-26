## DrawImage

Paintable GUI component for Unity

![DrawUI-SampleScene-PC_-Mac-_-Linux-Standalone-Unity-2019 3 5f1-_DX11_-2021-01-26-23-41-50_Trim(1)](https://user-images.githubusercontent.com/26675945/105862516-3133a980-6033-11eb-8579-3544ce320d7d.gif)

### How to use

1. Add this repository to your Assets folder.
2. Attach DrawImage.cs to GUI Object.
3. Done!



### Setting

| variable         | type  | description                  |
| ---------------- | ----- | ---------------------------- |
| imageWIdth       | int   | resolution width             |
| imageHeight      | int   | resolution height            |
| brushColor       | Color | color of brush               |
| brushWidth       | int   | width of brush               |
| resetOnTouchUp   | bool  | clear canvas when touch up   |
| resetOnTouchDown | bool  | clear canvas when touch down |


### Event Listener

You can register event listener if you want to do something when DrawImage component is touched. 

* OnTouchDown
* OnTouchUp
* OnDrag

```c#
void OnStart()
{
    var drawImage = GetComponent<KanaStudio.DrawImage>()
    drawImage.OnDrag += OnDrag;
}

// Called when you drag drawImage
// Lower right of (Vector2Int)position is (0,0)
void OnDrag(Vector2Int position)
{
    // Note that position is screen coordinate
    // if you want position on image,you can use DrawImage.ScreenToImagePosition method to convert position
    Debug.Log("(((('ω'))))");
}
```

