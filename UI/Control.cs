﻿using ClassicUO.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassicUO.UI
{
    public interface IControl
    {
        Point Position { get; set; }
        Rectangle Bounds { get; set; }
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        bool IsFocused { get; set; }
        bool MouseIsOver { get; set; }
        bool IsMovable { get; set; }
        IReadOnlyList<IControl> Children { get; }
        bool CanDrawNow { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        IControl Parent { get; set; }


        event EventHandler<MouseEventArgs> MouseButton, MouseMove, MouseEnter, MouseLeft;
        event EventHandler<MouseWheelEventArgs> MouseWheel;
        event EventHandler<KeyboardEventArgs> Keyboard;

        void AddChild(in IControl child);
        void RemoveChild(in IControl child);
        void SetFocus();
        void RemoveFocus();
        void Clear();

        void MoveTo(in int x, in int Y);
    }


    public abstract class Control : IDisposable
    {


        protected Control(in Control parent = null)
        {
            Parent = parent;
            _children = new List<Control>();
            IsEnabled = true;
            IsVisible = true;
        }

        protected Control(in Control parent, in int x, in int y) : this(parent)
        {
            X = x;
            Y = y;
        }

        protected Control(in Control parent, in int x, in int y, in int width, in int height) : this(parent, x, y)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Point Location
        {
            get => _bounds.Location;
            set => _bounds.Location = value;
        }

        public Rectangle Bounds
        {
            get => _bounds;
            set => _bounds = value;
        }

        public bool IsDisposed { get; private set; }

        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFocused { get; protected set; }
        public bool MouseIsOver { get; protected set; }
        public bool CanMove { get; set; }
        public bool CanCloseWithRightClick { get; set; }
        public bool CanCloseWithEsc { get; set; }
        public bool IsEditable { get; set; }

        public IReadOnlyList<Control> Children => _children;

        internal bool CanDragNow { get; set; }

        public int Width
        {
            get => _bounds.Width;
            set => _bounds.Width = value;
        }

        public int Height
        {
            get => _bounds.Height;
            set => _bounds.Height = value;
        }

        public int X
        {
            get => _bounds.X;
            set => _bounds.X = value;
        }

        public int Y
        {
            get => _bounds.Y;
            set => _bounds.Y = value;
        }

        public Control Parent
        {
            get => _parent;
            set
            {
                if (value == null)
                    _parent?._children.Remove(this);
                else
                    value._children.Add(this);

                _parent = value;

            }
        }


        internal void SetFocused()
        {
            IsFocused = true;
        }

        internal void RemoveFocus()
        {
            IsFocused = false;
        }


        public event EventHandler<MouseEventArgs> MouseButton, MouseMove, MouseEnter, MouseLeft;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;
        public event EventHandler<KeyboardEventArgs> Keyboard;


        public Control[] HitTest(in Point position)
        {
            List<Control> results = new List<Control>();

            bool inbouds = Bounds.Contains(position);

            if (inbouds)
            {
                results.Insert(0, this);
                foreach (var c in Children)
                {
                    var cl = c.HitTest(position);
                    if (cl != null)
                    {
                        for (int i = cl.Length - 1; i >= 0; i--)
                            results.Insert(0, cl[i]);
                    }
                }
            }

            return results.Count == 0 ? null : results.ToArray();
        }


        public void AddChildren(in Control c)
        {
            c.Parent = this;
            _children.Add(c);
        }

        public void RemoveChildren(in Control c)
        {
            c.Parent = null;
            _children.Remove(c);
        }

        public void Clear()
        {
            _children.ForEach(s => s.Parent = null);
            _children.Clear();
        }

        //public virtual void Draw(GameTime time, SpriteBatch spriteBatch)
        //{
        //    _children.ForEach(s => s.Draw(time, spriteBatch));
        //}

        //public virtual void Update(GameTime time)
        //{
        //    _children.ForEach(s => s.Update(time));
        //}

        public void MoveOf(in int offsetX, in int offsetY)
        {
            if (CanMove)
            {
                Console.WriteLine("OFFSET: {0},{1}", offsetX, offsetY);

                if (Parent != null)
                {
                    if (X + offsetX > Parent.Width || Y + offsetY > Parent.Height || X + offsetX < Parent.X || Y + offsetY < Parent.Y)
                        return;
                }


                X += offsetX;
                Y += offsetY;

                foreach (Control c in Children)
                    c.MoveOf(offsetX, offsetY);
            }
        }

        public T[] GetControls<T>() where T : Control => Children.OfType<T>().ToArray();


        public virtual void OnMouseButton(in MouseEventArgs e)
        {
            MouseButton?.Invoke(this, e);
        }

        public virtual void OnMouseEnter(in MouseEventArgs e)
        {
            MouseIsOver = true;
            MouseEnter?.Invoke(this, e);
        }

        public virtual void OnMouseLeft(in MouseEventArgs e)
        {
            MouseIsOver = false;
            MouseLeft?.Invoke(this, e);
        }

        public virtual void OnMouseMove(in MouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        public virtual void OnMouseWheel(in MouseWheelEventArgs e)
        {
            MouseWheel?.Invoke(this, e);
        }

        public virtual void OnKeyboard(in KeyboardEventArgs e)
        {
            Keyboard?.Invoke(this, e);
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
        }
    }
}