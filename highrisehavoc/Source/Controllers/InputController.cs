using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace highrisehavoc.Source.Controllers
{
    internal class InputController
    {
        public event Action<Vector2> OnTap;
        public event Action<Vector2> OnDoubleTap;
        public event Action<Vector2, Vector2> OnDrag;
        public event Action<Vector2> OnRelease;

        public bool isHoldingArmy = false;
        // enumerate for army type
        public enum DeployType
        {
            Soldier,
            AASoldier,
            Obstacle,
            Mine
        }

        public DeployType deployType;

        public Vector2? lastKnownDragPosition;
        public void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation touchLocation in touchCollection)
            {
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    OnTap?.Invoke(touchLocation.Position);

                    // double tap logic
                    if (touchLocation.TryGetPreviousLocation(out TouchLocation prevTouch) &&
                        touchLocation.State == TouchLocationState.Pressed)
                    {
                        if (touchLocation.Position == prevTouch.Position)
                        {
                            OnDoubleTap?.Invoke(touchLocation.Position);
                        }
                    }
                }
                else if (touchLocation.State == TouchLocationState.Moved)
                {
                    // drag and hold logic
                    if (touchLocation.TryGetPreviousLocation(out TouchLocation prevTouch2))
                    {
                        if (touchLocation.Position != prevTouch2.Position)
                        {
                            OnDrag?.Invoke(touchLocation.Position, prevTouch2.Position);
                            lastKnownDragPosition = touchLocation.Position;
                        }
                    }
                }
                else if (touchLocation.State == TouchLocationState.Released)
                {
                    // drag and release logic
                    OnRelease?.Invoke(touchLocation.Position);
                }
            }
        }


        public bool IsTapNearObject(Vector2 tapPosition, Rectangle objectRectangle)
        {
            return objectRectangle.Contains(tapPosition);
        }
    }
}
