using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class DialogGraphView : GraphView
    {
        private static Vector2 firstNodePos = new Vector2(100, 200);
        private static Vector2 nodeSize = new Vector2(100, 150);

        public DialogGraphView()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));

            AddThreadStart(firstNodePos);
        }

        public void AddThreadStart(Vector2 position)
        {
            ThreadStartNode threadStart = new ThreadStartNode
            {
                title = "Thread start"
            };

            threadStart.SetPosition(new Rect(position, nodeSize));

            AddElement(threadStart);
        }


        private void CreateContextMenu(ContextualMenuPopulateEvent e)
        {
            e.menu.ClearItems();
            e.menu.AppendAction("Add Thread Start", (DropdownMenuAction a) =>
            {
                Vector2 mousePos = a.eventInfo.localMousePosition;
                AddThreadStart(mousePos);
            });
        }
    }
}
