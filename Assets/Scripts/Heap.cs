using System;
using Unity.VisualScripting;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T> {
    T[] items;
    int currentItemCoumt;

    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    public void Add(T item) {
        item.HeapIndex = currentItemCoumt;
        items[currentItemCoumt] = item;
        SortUp(item);
        currentItemCoumt++;
    }

    public T RemoveFirst(){
        T firstItem = items[0];
        currentItemCoumt--;
        items[0] = items[currentItemCoumt];
        items[0]. HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    public int Count {
        get {
            return currentItemCoumt;
        }
    }
    public bool Contains(T item){
        return Equals(items[item.HeapIndex], item);
    }
    
    void SortDown(T item){
        while (true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex;

            if (childIndexLeft < currentItemCoumt){
                swapIndex = childIndexLeft;
                
                if (childIndexRight < currentItemCoumt) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0){
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0){
                    Swap(item, items[swapIndex]);
                }
                else
                    return;
            }
            else
                return;
        }
    }

    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1)/2;
        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
                item.HeapIndex = parentIndex;
                parentIndex = (item.HeapIndex - 1)/2;
            }
            else
                break;
        }
    }

    void Swap(T itemA, T itemB){
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        (itemB.HeapIndex, itemA.HeapIndex) = (itemA.HeapIndex, itemB.HeapIndex);
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex{
        get;
        set;
    }
}
