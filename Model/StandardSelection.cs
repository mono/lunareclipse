// Selection.cs
//
// Author:
//   Manuel Cerón <ceronman@unicauca.edu.co>
//
// Copyright (c) 2008 Manuel Cerón.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using LunarEclipse.Controller;
using LunarEclipse.Serialization;

namespace LunarEclipse.Model {	
	
	public class StandardSelection: ISelection {
		
		public event EventHandler SelectionChanged;		
		public event MouseEventHandler HandleMouseDown;
		
		public StandardSelection(MoonlightController controller)
		{
			Controller = controller;
			HandleGroups = new Dictionary<UIElement, IHandleGroup>();
			undo = new UndoGroup();
			clipboard = new List<UIElement>();
		}
		
		public void Add(UIElement element)
		{
			if (!HandleGroups.ContainsKey(element) ) {
				IHandleGroup group = FindHandleGroup(element);
				HandleGroups.Add(element, group);
				if (group == null)
					return;
				group.AddToCanvas();
				group.HandleMouseDown += OnHandleMouseDown;
				
				MainElement = element;
				ChangeSelection();
			}
		}
		
		public void Remove(UIElement element)
		{
			IHandleGroup group = HandleGroups[element];
			HandleGroups.Remove(element);
			if (group == null)
				return;
			group.RemoveFromCanvas();
			group.HandleMouseDown -= OnHandleMouseDown;
			ChangeMainElement();
			ChangeSelection();
		}
		
		public void Hide()
		{
			foreach (IHandleGroup group in HandleGroups.Values)
				group.RemoveFromCanvas();
		}
		
		public void Show()
		{
			foreach (IHandleGroup group in HandleGroups.Values)
				group.AddToCanvas();
		}
		
		public void Clear()
		{
			Hide();
			HandleGroups.Clear();
			MainElement = null;
			ChangeSelection();
		}
		
		public void SelectAll()
		{
			Clear();
			
			UIElement[] all = new UIElement[Controller.Canvas.Children.Count];
			Controller.Canvas.Children.CopyTo(all, 0);
			
			foreach (UIElement element in all)
				Add(element);
		}
		
		public IEnumerable<UIElement> Elements {
			get {
				foreach (UIElement element in HandleGroups.Keys)
					yield return element;
			}
		}
		
		public UIElement MainElement
		{
			get { return main_element; }
			protected set {
				main_element = value;
			}
		}
		
		public int Count {
			get { return HandleGroups.Count; }
		}
		
		public bool Contains(UIElement element) {
			return HandleGroups.ContainsKey(element);
		}
		
		public void Update()
		{
			List<UIElement> toRemove = new List<UIElement>();
			foreach (UIElement element in HandleGroups.Keys) {
				if (!Controller.Canvas.Children.Contains(element))
				    toRemove.Add(element);
			}
			foreach (UIElement element in toRemove)
				Remove(element);
			
			// This must be done in order to prevent weird behavior
			// A better way to do this could be to use an event
			// and catch it from ITool
			Controller.CurrentTool.Deactivate();
			Controller.CurrentTool.Activate();
		}
		
		public Rect GetBounds()
		{
			double top = double.MaxValue;
			double bottom = double.MinValue;
			double left = double.MaxValue;
			double right = double.MinValue;
			
			foreach (UIElement element in Elements) {
				IDescriptor descriptor = StandardDescriptor.CreateDescriptor(element);
				Rect bounds = descriptor.GetBounds();
				
				top = Math.Min(top, bounds.Top);
				left = Math.Min(left, bounds.Left);
				right = Math.Max(right, bounds.Right);
				bottom = Math.Max(bottom, bounds.Bottom);
			}
			
			return new Rect(new Point(left, top), new Point(right, bottom));
		}
		
		public void SendToBack()
		{
			int selectionMaxZ = Toolbox.MaxZ(Visuals);
			int minz = Toolbox.MinZ(Controller.Canvas.Children);
			foreach (UIElement element in Elements) {
				int z = (int) element.GetValue(Canvas.ZIndexProperty);
				z -= selectionMaxZ - minz - 1;
				ChangeZ(element, z);
			}
			
		 	PushUndo();
		}
		
		public void SendBackwards()
		{
			foreach (UIElement element in Elements) {
				int z = (int) element.GetValue(Canvas.ZIndexProperty);
				z--;
				ChangeZ(element, z);
			}
			
			PushUndo();
		}
		
		public void BringToFront()
		{
			int selectionMinZ = Toolbox.MinZ(Visuals);
			int maxz = Toolbox.MaxZ(Controller.Canvas.Children);
			foreach (UIElement element in Elements) {
				int z = (int) element.GetValue(Canvas.ZIndexProperty);
				z += maxz - selectionMinZ + 1;
				ChangeZ(element, z);
			}
			
			PushUndo();
		}
		
		public void BringForwards()
		{
			foreach (UIElement element in Elements) {
				int z = (int) element.GetValue(Canvas.ZIndexProperty);
				z++;
				ChangeZ(element, z);
			}
			
			PushUndo();
		}
		
		public void AlignLeft()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.X = mainBounds.Left;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void AlignHorizontalCenter()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.X = mainBounds.Left + mainBounds.Width/2 - bounds.Width/2;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void AlignRight()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.X = mainBounds.Right - bounds.Width;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void AlignTop()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.Y = mainBounds.Top;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void AlignVerticalCenter()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.Y = mainBounds.Top + mainBounds.Height/2 - bounds.Height/2;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void AlignBottom()
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(MainElement);
			Rect mainBounds = descriptor.GetBounds();
			
			foreach (UIElement element in Elements) {
				descriptor = StandardDescriptor.CreateDescriptor(element, undo);
				Rect bounds = descriptor.GetBounds();
				bounds.Y = mainBounds.Bottom - bounds.Height;
				descriptor.SetBounds(bounds);
			}
			
			PushUndo();
		}
		
		public void DeleteFromCanvas()
		{
			foreach (UIElement element in Elements) {
				RemoveElementFromCanvas(element);
			}
			
			Clear();
			PushUndo();
		}
		
		public void Copy()
		{
			Controller.CurrentTool.Deactivate();
			clipboard.Clear();
			foreach (UIElement element in HandleGroups.Keys) {
				DependencyObject clone = Serializer.Clone(Controller.Canvas, element);
				clipboard.Add(clone as UIElement);
			}
			Controller.CurrentTool.Activate();
		}
		
		public void Cut()
		{
			Controller.CurrentTool.Deactivate();
			clipboard.Clear();
			foreach (UIElement element in HandleGroups.Keys) {
				clipboard.Add(element);
				RemoveElementFromCanvas(element);
			}
			Clear();
			Controller.CurrentTool.Activate();
		 	PushUndo();
		}
			
		public void Paste()
		{
			if (clipboard.Count == 0)
				return;
			
			Controller.CurrentTool.Deactivate();
			
			UIElement[] copy = new UIElement[clipboard.Count];
			clipboard.CopyTo(copy);
			
			Clear();
			clipboard.Clear();
			foreach (UIElement element in copy) {
				AddElementToCanvas(element);
				Add(element);
				DependencyObject clone = Serializer.Clone(Controller.Canvas, element);
				clipboard.Add(clone as UIElement);
			}
			
			Controller.CurrentTool.Activate();
			PushUndo();
		}
		
		public void Clone()
		{
			Copy();
			Paste();
		}
		
		protected Dictionary<UIElement, IHandleGroup> HandleGroups {
			get { return handle_groups; }
			set { handle_groups = value; }
		}

		protected MoonlightController Controller {
			get { return controller; }
			set { controller = value; }
		}
		
		protected void OnHandleMouseDown(object sender, MouseEventArgs args)
		{
			if (HandleMouseDown != null)
				HandleMouseDown(sender, args);
		}
				
		private IHandleGroup FindHandleGroup(UIElement element)
		{
			return StandardDescriptor.CreateHandleGroup(Controller, element);
		}
		
		private void ChangeSelection()
		{
			if (SelectionChanged != null)
				SelectionChanged(this, new EventArgs());
		}
		
		private void ChangeMainElement()
		{
			foreach (UIElement element in handle_groups.Keys) {
				if (element != MainElement) {
					MainElement = element;
					return;
				}
			}
			MainElement = null;
		}
		
		private UIElementCollection Visuals {
			get {
				UIElementCollection collection = new UIElementCollection();
				
				foreach (UIElement element in handle_groups.Keys)
					collection.Add(element);
				
				return collection;
			}
		}
		
		private void PushUndo ()
		{
			Controller.UndoEngine.PushUndo(undo);
			undo = new UndoGroup();
		}
		
		private void ChangeZ (UIElement element, int z)
		{
			IDescriptor descriptor = StandardDescriptor.CreateDescriptor(element, undo);
			descriptor.ChangeProperty(element, Canvas.ZIndexProperty, z);
		}
		
		private void RemoveElementFromCanvas(UIElement element)
		{
			Controller.Canvas.Children.Remove(element);
			undo.Add(new UndoRemoveObject(Controller.Canvas.Children, element));
		}
		
		private void AddElementToCanvas(UIElement element)
		{
			Controller.Canvas.Children.Add(element);
			undo.Add(new UndoAddObject(Controller.Canvas.Children, element));
		}
		
		private MoonlightController controller;
		private Dictionary<UIElement, IHandleGroup> handle_groups;
		private UIElement main_element;
		private List<UIElement> clipboard;
		private UndoGroup undo;
		
		private const double PastePositionOffset = 10.0;
	}
}
