using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class KeyMouseReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
    public static Point mapEditMousePos = new Point(-100, -100);
    public static Point mousePos = new Point(-100, -100);
    public static Point LeftClickPos = new Point(-100, -100);
    public static Point RightClickPos = new Point(-100, -100);

    public static Point mapEditLeftClickPos = new Point(-100, -100);
    public static Point mapEditRightClickPos = new Point(-100, -100);
	public static bool KeyPressed(Keys key) {
		return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
	}
	public static bool LeftClick() {
		return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
	}
	public static bool RightClick() {
		return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
	}

	//Should be called at beginning of Update in Game
	public static void Update(Platformer.Camera2D cam) {
		oldKeyState = keyState;
		keyState = Keyboard.GetState();
		oldMouseState = mouseState;
		mouseState = Mouse.GetState();
        mousePos = new Point((int)(KeyMouseReader.mouseState.X), (int)(KeyMouseReader.mouseState.Y));

        LeftClickPos = new Point(-10000, -10000);         //Moves the mouseclick point outside the screen
        if (KeyMouseReader.LeftClick())
            LeftClickPos = mousePos;   //Creates point at mouse location for collision test
        RightClickPos = new Point(-10000, -10000);
        if (KeyMouseReader.RightClick())
            RightClickPos = mousePos;

        mapEditMousePos = new Point((int)(KeyMouseReader.mouseState.X - cam.origin.X + cam.Position.X),
                     (int)(KeyMouseReader.mouseState.Y - cam.origin.Y + cam.Position.Y));
        mapEditLeftClickPos = new Point(-10000, -10000);         //Moves the mouseclick point outside the screen
        if (KeyMouseReader.LeftClick())
            mapEditLeftClickPos = mapEditMousePos;   //Creates point at mouse location for collision test
        mapEditRightClickPos = new Point(-10000, -10000);
        if (KeyMouseReader.RightClick())
            mapEditRightClickPos = mapEditMousePos;

        
	}
}