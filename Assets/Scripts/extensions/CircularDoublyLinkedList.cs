﻿using System.Collections;
using System.Collections.Generic;

namespace extensions
{
    public class CircularDoublyLinkedList<T> : IEnumerable<T>  // кольцевой двусвязный список
    {
        DoublyNode<T> head; // головной/первый элемент
        int count;  // количество элементов в списке
 
        // добавление элемента
        public void Add(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);
 
            if (head == null)
            {
                head = node;
                head.Next = node;
                head.Previous = node;
            }
            else
            {
                node.Previous = head.Previous;
                node.Next = head;
                head.Previous.Next = node;
                head.Previous = node;
            }
            count++;
        }
        // удаление элемента
        public bool Remove(T data)
        {
            DoublyNode<T> current = head;
 
            DoublyNode<T> removedItem = null;
            if (count == 0) return false;
 
            // поиск удаляемого узла
            do
            {
                if (current.Data.Equals(data))
                {
                    removedItem = current;
                    break;
                }
                current = current.Next;
            }
            while (current!=head);
 
            if (removedItem != null)
            {
                // если удаляется единственный элемент списка
                if (count==1)
                    head = null;
                else
                {
                    // если удаляется первый элемент
                    if(removedItem==head)
                    {
                        head = head.Next;
                    }
                    removedItem.Previous.Next = removedItem.Next;
                    removedItem.Next.Previous = removedItem.Previous;
                }
                count--;
                return true;
            }
            return false;
        }
        
        public int Count { get { return count; } }

        public bool IsEmpty { get { return count == 0; } }
 
        public void Clear()
        {
            head = null;
            count = 0;
        }
 
        public bool Contains(T data)
        {
            DoublyNode<T> current = head;
            if (current == null) return false;
            do
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            while (current != head);
            return false;
        }
 
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
 
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            DoublyNode<T> current = head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }
    }
}