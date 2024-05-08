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
		public int Count => _heap.Count;
		#endregion


		#region Field
		/// <summary>
		/// 들어온 데이터를 저장하는 공간입니다.
		/// </summary>
		private List<T> _heap = new List<T>();
		#endregion


		#region Method
		/// <summary>
		/// 데이터를 추가합니다. 추가된 데이터는 정렬됩니다.
		/// </summary>
		public void Enqueue(T item)
		{
			_heap.Add(item);
			//자식의 현재 위치
			int childIndex = _heap.Count - 1;
			while (childIndex > 0)
			{
				//부모의 위치
				int parentIndex = (childIndex - 1) / 2;
				//부모와 자식의 관계가 옳게 된 경우 Break
				if (_heap[childIndex].CompareTo(_heap[parentIndex]) >= 0)
				{
					break;
				}
				//아닌 경우는 스왑을 진행한다.
				Swap(childIndex, parentIndex);
				//위 레벨로 진행
				childIndex = parentIndex;
			}
		}

		/// <summary>
		/// 가장 앞에 있는 데이터를 반환합니다.
		/// </summary>
		public T Dequeue()
		{
			if (_heap.Count == 0)
				throw new Exception("Queue is empty");

			int lastIndex = _heap.Count - 1;
			T frontItem = _heap[0];
			_heap[0] = _heap[lastIndex];
			_heap.RemoveAt(lastIndex);

			--lastIndex;
			int parentIndex = 0;
			//가장 앞의 값을 최대값을 만든다.
			while (true)
			{
				int childIndex = parentIndex * 2 + 1;
				//자식 인덱스 존재하지 않는 경우 Break
				if (childIndex > lastIndex)
				{
					break;
				}
				int rightChild = childIndex + 1;
				//오른쪽 자식이 존재하고 해당 자식이 현재의 자식의 값보다 큰경우 자식 인덱스를 변경해준다.
				if (rightChild <= lastIndex && _heap[rightChild].CompareTo(_heap[childIndex]) < 0)
				{
					childIndex = rightChild;
				}
				//부모와 자식을 비교하였을 때 부모가 자식보다 크거나 같으면 Break
				if (_heap[parentIndex].CompareTo(_heap[childIndex]) <= 0)
				{
					break;
				}
				//스왑
				Swap(parentIndex, childIndex);
				//아래 레벨로 진행
				parentIndex = childIndex;
			}
			return frontItem;

		}

		/// <summary>
		/// 데이터의 위치를 기반으로 스왑합니다.
		/// </summary>
		private void Swap(int i, int j)
		{
			T temp = _heap[i];
			_heap[i] = _heap[j];
			_heap[j] = temp;
		}
		
		/// <summary>
		/// 가장 앞에 있는 데이터를 반환한다.
		/// </summary>
		public T Peek()
		{
			T frontItem = _heap[0];
			return frontItem;
		}

		/// <summary>
		/// 우선순위 큐가 비었는지 확인한다.
		/// </summary>
		public bool IsEmpty()
		{
			return Count == 0;
		}
		#endregion
	}
}
