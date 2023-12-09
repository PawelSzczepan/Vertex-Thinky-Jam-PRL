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
        public DialogGraphView()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            AddThreadStart();
        }

        public void AddThreadStart()
        {
            ThreadStartNode threadStart = new ThreadStartNode
            {
                title = "Thread start"
            };

            threadStart.SetPosition(new Rect(100, 200, 100, 150));

            AddElement(threadStart);
        }
    }
}
