using System;

namespace Customdelegates.v1
{ 
   public delegate void UnityRefAction<T,T1>(ref T item1,ref T1 item2);
   public delegate void UnityRefAction<T>( ref T item2);


}

namespace Customdelegates.v2
{
   public delegate void UnityRefAction<T0, T1>(ref T0 item1, T1 item2);

}