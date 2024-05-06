using System;
using System.Collections.Generic;

namespace TopdownShooter.Pathfinders
{
	/// <summary>
	/// 데이터가 업데이트되면 오름차순으로 정렬됩니다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PriorityQueue<T> where T : IComparable<T>
	{
		#region Property
		/// <summary>
		/// 우선 순위 큐에 들어있는 데이터의 갯수입니다.
		/// </summary>
		public int Count
		{
			get { return heap.Count; }
		}
		#endregion


		#region Field
		/// <summary>
		/// 들어온 데이터를 저장하는 공간입니다.
		/// </summary>
		private List<T> heap = new List<T>();
		#endregion


		#region Method
		/// <summary>
		/// 데이터를 추가합니다. 추가된 데이터는 정렬됩니다.
		/// </summary>
		public void Enqueue(T item)
		{
			heap.Add(item);
			int currentIndex = heap.Count - 1;

			while (currentIndex > 0)
			{
				int parentIndex = (currentIndex - 1) / 2;

				if (heap[currentIndex].CompareTo(heap[parentIndex]) >= 0)
					break;

				Swap(currentIndex, parentIndex);
				currentIndex = parentIndex;
			}
		}

		/// <summary>
		/// 가장 앞에 있는 데이터를 반환합니다.
		/// </summary>
		public T Dequeue()
		{
			if (heap.Count == 0)
				throw new Exception("Queue is empty");

			T frontItem = heap[0];
			int lastIndex = heap.Count - 1;
			heap[0] = heap[lastIndex];
			heap.RemoveAt(lastIndex);

			int currentIndex = 0;

			while (true)
			{
				int leftChildIndex = currentIndex * 2 + 1;
				int rightChildIndex = currentIndex * 2 + 2;

				if (leftChildIndex >= lastIndex)
					break;

				int minIndex = leftChildIndex;

				if (rightChildIndex < lastIndex && heap[rightChildIndex].CompareTo(heap[leftChildIndex]) < 0)
					minIndex = rightChildIndex;

				if (heap[currentIndex].CompareTo(heap[minIndex]) <= 0)
					break;

				Swap(currentIndex, minIndex);
				currentIndex = minIndex;
			}

			return frontItem;
		}

		/// <summary>
		/// 데이터의 위치를 기반으로 스왑합니다.
		/// </summary>
		private void Swap(int i, int j)
		{
			T temp = heap[i];
			heap[i] = heap[j];
			heap[j] = temp;
		}
		#endregion
	}
}
