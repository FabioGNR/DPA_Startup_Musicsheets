using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.Memento
{
    public class EditorCaretaker
    {
        private LinkedList<CompositionMemento> _mementoList;
        public LinkedListNode<CompositionMemento> CurrentItem { get; private set; }

        public EditorCaretaker()
        {
            _mementoList = new LinkedList<CompositionMemento>();
        }

        public void Save(CompositionMemento memento)
        {
            if (CurrentItem == null)
            {
                _mementoList.AddFirst(memento);
                CurrentItem = _mementoList.First;
            }
            else
            {
                CurrentItem = CurrentItem.ReplaceNext(memento);
            }
        }

        public CompositionMemento Undo()
        {
            if (!CanUndo)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                CurrentItem = CurrentItem.Previous;
                return CurrentItem.Value;
            }
        }

        public CompositionMemento Redo()
        {
            if (!CanRedo)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                CurrentItem = CurrentItem.Next;
                return CurrentItem.Value;
            }
        }

        public bool CanUndo
        {
            get { return CurrentItem != null && CurrentItem.Previous != null; }
        }

        public bool CanRedo
        {
            get
            {
                return CurrentItem != null && CurrentItem.Next != null;
            }
        }
    }
}
