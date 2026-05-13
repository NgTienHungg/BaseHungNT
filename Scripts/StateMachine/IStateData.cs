namespace HungNT
{
    /// <summary>
    /// Marker interface cho data truyền vào state khi chuyển trạng thái.
    /// <code>
    /// public class MoveData : IStateData { public Vector3 Target; }
    /// </code>
    /// </summary>
    public interface IStateData
    {
    }

    /// <summary>
    /// Generic wrapper khi chỉ cần truyền 1 giá trị đơn giản.
    /// <code>
    /// fsm.ChangeState&lt;MoveState&gt;(new StateData&lt;Vector3&gt;(target));
    /// </code>
    /// </summary>
    public class StateData<T> : IStateData
    {
        public T Data;

        public StateData(T data)
        {
            Data = data;
        }
    }
}
