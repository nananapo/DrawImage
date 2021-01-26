## DrawImage

Paintable GUI component for Unity



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



### Other

##### Event Listener

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
    // if you want position on image,you can use ScreenToImagePosition method to convert position
    Debug.Log("(((('ω'))))");
}
```

