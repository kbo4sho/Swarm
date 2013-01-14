using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystem.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNASwarms
{
    public class SwarmsCamera : Camera2D
    {
        private Population _trackingBody;


        public SwarmsCamera(GraphicsDevice graphics) 
            :base(graphics)
        {

        }

        /// <summary>
        /// the body that this camera is currently tracking. 
        /// Null if not tracking any.
        /// </summary>
        public Population TrackingBody
        {
            get { return _trackingBody; }
            set
            {
                _trackingBody = value;
                if (_trackingBody != null)
                {
                    _positionTracking = true;
                }
            }
        }
        public bool EnablePositionTracking
        {
            get { return _positionTracking; }
            set
            {
                if (value && _trackingBody != null)
                {
                    _positionTracking = true;
                }
                else
                {
                    _positionTracking = false;
                }
            }
        }

        public bool EnableRotationTracking
        {
            get { return _rotationTracking; }
            set
            {
                if (value && _trackingBody != null)
                {
                    _rotationTracking = true;
                }
                else
                {
                    _rotationTracking = false;
                }
            }
        }

        public bool EnableTracking
        {
            set
            {
                EnablePositionTracking = value;
                EnableRotationTracking = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_trackingBody != null)
            {
                if (_positionTracking)
                {
                    _targetPosition = new Vector2((float)_trackingBody.get(0).getX(), (float)_trackingBody.get(0).getY());
                    if (_minPosition != _maxPosition)
                    {
                        Vector2.Clamp(ref _targetPosition, ref _minPosition, ref _maxPosition, out _targetPosition);
                    }
                }
                //TODO: Get Rotation
                //if (_rotationTracking)
                //{
                //    _targetRotation = -_trackingBody.get(0). % MathHelper.TwoPi;
                //    if (_minRotation != _maxRotation)
                //    {
                //        _targetRotation = MathHelper.Clamp(_targetRotation, _minRotation, _maxRotation);
                //    }
                //}
            }

            base.Update(gameTime);
        }
    }
}
