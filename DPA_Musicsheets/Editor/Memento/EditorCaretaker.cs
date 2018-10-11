using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.Memento
{
    class EditorCaretaker
    {
        private LinkedList<CompositionMemento> _mementoList;
        private LinkedListNode<CompositionMemento> _currentItem;

        public EditorCaretaker()
        {
            _mementoList = new LinkedList<CompositionMemento>();
        }

        public void Save(CompositionMemento memento)
        {
            if (_currentItem == null)
            {
                _mementoList.AddFirst(memento);
                _currentItem = _mementoList.First;
            }
            else
            {
                _currentItem = _currentItem.ReplaceNext(memento);
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
                _currentItem = _currentItem.Previous;
                return _currentItem.Value;
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
                _currentItem = _currentItem.Next;
                return _currentItem.Value;
            }
        }

        public bool CanUndo
        {
            get { return _currentItem != null && _currentItem.Previous != null; }
        }

        public bool CanRedo
        {
            get
            {
                return _currentItem != null && _currentItem.Next != null;
            }
        }
    }
}
